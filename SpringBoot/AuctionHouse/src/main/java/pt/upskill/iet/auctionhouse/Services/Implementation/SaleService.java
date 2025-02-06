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
import pt.upskill.iet.auctionhouse.Retrofit.Service.AuctionHouseService;

import java.util.List;

@Service
public class SaleService {

    private final AuctionHouseService auctionHouseService;
    private final AuctionService auctionService;
    private final AuctionRepository auctionRepository;

    @Autowired
    public SaleService(AuctionHouseService auctionHouseService, AuctionService auctionService, AuctionRepository auctionRepository) {
        this.auctionHouseService = auctionHouseService;
        this.auctionService = auctionService;
        this.auctionRepository = auctionRepository;
    }


    public Bid getBidWithGreaterPrice() throws Exception {
        List<Auction> auctionsWithBids = this.auctionService.checkDateAndTurnAuctionsIntoInactive();
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

        ItemDto itemDto = auctionHouseService.getItemById(bidWithGreaterPrice.getItemId());
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
        this.auctionHouseService.addSale(bidWithGreaterPrice.getPrice(), auction.getItemId());
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
