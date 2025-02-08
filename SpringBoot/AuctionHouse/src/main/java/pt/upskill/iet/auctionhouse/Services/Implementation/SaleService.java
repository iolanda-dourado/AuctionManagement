package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AHItemService;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AHSaleService;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

@Service
public class SaleService {

    private final AHItemService auctionHouseItemService;
    private final AHSaleService auctionHouseSaleService;
    private final AuctionRepository auctionRepository;

    @Autowired
    public SaleService(AHItemService auctionHouseItemService, AHSaleService auctionHouseSaleService, AuctionRepository auctionRepository) {
        this.auctionHouseItemService = auctionHouseItemService;
        this.auctionHouseSaleService = auctionHouseSaleService;
        this.auctionRepository = auctionRepository;
    }


    public List<Auction> checkDateAndTurnAuctionsIntoInactive()  throws Exception {
        List<Auction> auctionsWithBids = new ArrayList<>();
        LocalDate today = LocalDate.now();

        // Confere se algum leilão está com a data final de hoje. Caso esteja, muda o estado para inativo e adiciona esse leilão à lista de leilões
        for (Auction auction : this.auctionRepository.findAll()) {
            if (!auction.isActive()) {
                continue;
            }
            if (auction.getFinalDate().equals(today)) {
                auction.setActive(false);
                this.auctionRepository.save(auction);

                // Confere se, dentre os leilões que estão fechados, há leilões que tenham lances. Caso haja, esse leilão é adicionado à lista de leilões
                if (!auction.getBids().isEmpty()) {
                    auctionsWithBids.add(auction);
                    System.out.printf("CheckDate... -> Auction with id %d has bids and has been added to the list of auctions with bids\n", auction.getId());
                } else {
                    auctionHouseItemService.updateItemStatus(auction.getItemId(), StatusDto.Available);
                    System.out.printf("CheckDate... -> Auction with id %d has no bids and its item with id %d is now available\n", auction.getId(), auction.getItemId());
                }
            }
        }

        return auctionsWithBids;
    }


    public Bid getBidWithGreaterPrice() throws Exception {
        List<Auction> auctionsWithBids = this.checkDateAndTurnAuctionsIntoInactive();
        Bid bidWithGreaterPrice = null;

        // Procura o maior lance entre os leilões ativos
        for (Auction auction : auctionsWithBids) {
            for (Bid bid : auction.getBids()) {
                if (bidWithGreaterPrice == null || bid.getPrice() > bidWithGreaterPrice.getPrice()) {
                    bidWithGreaterPrice = bid;
                    System.out.printf("getBidWithGreaterPrice -> Bid with id %d has the greater price found\n", bid.getId());
                }
            }
        }

        if (bidWithGreaterPrice == null) {
            return null;
        }

        ItemDto itemDto = auctionHouseItemService.getItemById(bidWithGreaterPrice.getItemId());
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("getBidWithGreaterPrice -> The item is already sold, therefore the sale cannot continue.");
        }

        return bidWithGreaterPrice;
    }


    @Transactional
    public void addSaleWithHighestBid() throws Exception {
        Bid bidWithGreaterPrice = this.getBidWithGreaterPrice();

        if (bidWithGreaterPrice == null) {
            System.out.println("addSalesWithHighestBid -> No bid with greater price found.");
            return;
        }

        Auction auction = bidWithGreaterPrice.getAuction();
        System.out.println("addSalesWithHighestBid -> Processing sale for auction ID: " + auction.getId());

        // Tenta adicionar a venda
        this.auctionHouseSaleService.addSale(bidWithGreaterPrice.getPrice(), auction.getItemId());
        System.out.println("addSalesWithHighestBid -> Sale added successfully!");

        // Atualiza preço final no leilão
        auction.setFinalPrice(bidWithGreaterPrice.getPrice());
        this.auctionRepository.save(auction);
        System.out.println("addSalesWithHighestBid -> Auction final price updated!");

        // ** Não é preciso atualizar status do item para Sold porque o C# já faz isso quando adiciona uma venda
//        this.auctionHouseService.updateItemStatus(auction.getItemId(), StatusDto.Sold);
//        System.out.println("addSalesWithHighestBid -> Item with id " + auction.getItemId() + " was updated to SOLD.");
    }
}
