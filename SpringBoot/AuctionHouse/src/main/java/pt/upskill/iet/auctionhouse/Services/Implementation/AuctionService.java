package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.*;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AHItemService;
import pt.upskill.iet.auctionhouse.Services.Interfaces.AuctionServiceInterface;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class AuctionService implements AuctionServiceInterface {

    @Autowired
    AuctionRepository auctionRepository;
    @Autowired
    AHItemService AHItemService;
    @Autowired
    SaleService saleService;


    // -------- ADD AUCTION --------
    @Override
    public AuctionDto addAuction(AuctionCreateDto auctionCreateDto) throws Exception {
        // Validação de data
        if (auctionCreateDto.getFinalDate().isBefore(auctionCreateDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        // Verifica se o item não foi encontrado
        ItemDto itemDto = this.AHItemService.getItemById(auctionCreateDto.getItemId());
        if (itemDto == null) {
            throw new NotFoundException("Item not found");
        }
        // Verifica se o status do item é Sold e não deixa criar o leilão
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore it cannot be auctioned.");
        } else if (itemDto.getStatus() == StatusDto.AtAuction) {
            throw new InvalidOperationException("The item is already at auction, therefore it cannot be auctioned.");
        }
        AHItemService.updateItemStatus(itemDto.getId(), StatusDto.AtAuction);


        Auction auction = new Auction();
        auction.setItemId(auctionCreateDto.getItemId());
        auction.setInitialDate(auctionCreateDto.getInitialDate());
        auction.setFinalDate(auctionCreateDto.getFinalDate());
        auction.setFinalPrice(0);
        auction.setActive(true);

        // Salvar no repositório
        auction = this.auctionRepository.save(auction);

        // Converter para DTO e retornar
        return AuctionDto.fromAuctionToDto(auction);
    }


    // -------- GET ALL AUCTIONS --------
    @Override
    public Page<AuctionDto> getAllAuctions(int page, int size) throws Exception {
        saleService.addSaleWithHighestBid();
        return this.auctionRepository.findAll(PageRequest.of(page, size)).map(AuctionDto::fromAuctionToDto);
    }


    // -------- GET AUCTION BY ID --------
    @Override
    public AuctionDto getAuctionById(long id) throws Exception {
        if (this.auctionRepository.findById(id).isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        return this.auctionRepository.findById(id).map(AuctionDto::fromAuctionToDto).orElse(null);
    }


    // -------- UPDATE AUCTION --------
//    @Override
//    public AuctionDto updateAuction(long id, AuctionUpdateDto auctionUpdateDto) throws Exception {
//        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);
//
//        if (optionalAuction.isEmpty()) {
//            throw new NotFoundException("Auction not found");
//        }
//
//        if (auctionUpdateDto.getFinalDate().isBefore(auctionUpdateDto.getInitialDate())) {
//            throw new InvalidDateException("The final date must be greater than the initial date");
//        }
//
//        // Se o status do item for Sold, não permite atualizar
//        ItemDto itemDto = auctionHouseService.getItemById(optionalAuction.get().getItemId());
//        if (itemDto.getStatus() == StatusDto.Sold) {
//            throw new InvalidOperationException("The item is already sold, therefore the auction cannot be updated.");
//        }
//
//        // Na atualização só deixa atualizar as datas e o estado
//        Auction auction = optionalAuction.get();
//        auction.setItemId(optionalAuction.get().getItemId());
//        auction.setInitialDate(auctionUpdateDto.getInitialDate());
//        auction.setFinalDate(auctionUpdateDto.getFinalDate());
//        auction.setFinalPrice(optionalAuction.get().getFinalPrice());
//        auction.setActive(auctionUpdateDto.isActive());
//
//        auction = this.auctionRepository.save(auction);
//
//        return AuctionDto.fromAuctionToDto(auction);
//    }


    // -------- DELETE AUCTION --------
    @Override
    public AuctionDto deleteAuction(long id) throws Exception {
        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);

        if (optionalAuction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        ItemDto itemDto = this.AHItemService.getItemById(optionalAuction.get().getItemId());
        // Não deixa deletar se o item tiver status Sold
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore the auction cannot be deleted.");
        }

        // Não deixar deletar se o leilão já tiver licitações
        if (!optionalAuction.get().getBids().isEmpty()) {
            throw new InvalidOperationException("The auction has bids already, therefore it cannot be deleted.");
        }

        // Atualiza o status do Item para Available antes de deletar
        AHItemService.updateItemStatus(itemDto.getId(), StatusDto.Available);

        this.auctionRepository.deleteById(id);
        return AuctionDto.fromAuctionToDto(optionalAuction.get());
    }


    public AuctionDto updateAuctionStatus(long id, boolean isActive) throws Exception {
        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);
        if (optionalAuction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        Auction auction = optionalAuction.get();
        auction.setActive(isActive);
        auction = this.auctionRepository.save(auction);
        return AuctionDto.fromAuctionToDto(auction);
    }
}
