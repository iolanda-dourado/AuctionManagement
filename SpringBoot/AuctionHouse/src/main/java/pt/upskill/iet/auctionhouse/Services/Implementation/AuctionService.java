package pt.upskill.iet.auctionhouse.Services.Implementation;

import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import pt.upskill.iet.auctionhouse.Dtos.*;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AuctionHouseService;
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
    AuctionHouseService auctionHouseService;


    // -------- ADD AUCTION --------
    @Override
    public AuctionDto addAuction(AuctionCreateDto auctionCreateDto) throws Exception {
        // Validação de data
        if (auctionCreateDto.getFinalDate().isBefore(auctionCreateDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        // Verifica se o item não foi encontrado
        ItemDto itemDto = this.auctionHouseService.getItemById(auctionCreateDto.getItemId());
        if (itemDto == null) {
            throw new NotFoundException("Item not found");
        }
        // Verifica se o status do item é Sold e não deixa criar o leilão
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore it cannot be auctioned.");
        } else if (itemDto.getStatus() == StatusDto.AtAuction) {
            throw new InvalidOperationException("The item is already at auction, therefore it cannot be auctioned.");
        }
        auctionHouseService.updateItemStatus(itemDto.getId(), StatusDto.AtAuction);


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
    public Page<AuctionDto> getAllAuctions(int page, int size) {
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
    @Override
    public AuctionDto updateAuction(long id, AuctionUpdateDto auctionUpdateDto) throws Exception {
        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);

        if (optionalAuction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        if (auctionUpdateDto.getFinalDate().isBefore(auctionUpdateDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        // Se o status do item for Sold, não permite atualizar
        ItemDto itemDto = auctionHouseService.getItemById(optionalAuction.get().getItemId());
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore the auction cannot be updated.");
        }

        // Na atualização só deixa atualizar as datas e o estado
        Auction auction = optionalAuction.get();
        auction.setItemId(optionalAuction.get().getItemId());
        auction.setInitialDate(auctionUpdateDto.getInitialDate());
        auction.setFinalDate(auctionUpdateDto.getFinalDate());
        auction.setFinalPrice(optionalAuction.get().getFinalPrice());
        auction.setActive(auctionUpdateDto.isActive());

        auction = this.auctionRepository.save(auction);

        return AuctionDto.fromAuctionToDto(auction);
    }


    // -------- DELETE AUCTION --------
    @Override
    public AuctionDto deleteAuction(long id) throws Exception {
        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);

        if (optionalAuction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        ItemDto itemDto = this.auctionHouseService.getItemById(optionalAuction.get().getItemId());
        // Não deixa deletar se o item tiver status Sold
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore the auction cannot be deleted.");
        }

        // Não deixar deletar se o leilão já tiver licitações
        if (!optionalAuction.get().getBids().isEmpty()) {
            throw new InvalidOperationException("The auction has bids already, therefore it cannot be deleted.");
        }

        // Atualiza o status do Item para Available antes de deletar
        auctionHouseService.updateItemStatus(itemDto.getId(), StatusDto.Available);

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
                    auctionHouseService.updateItemStatus(auction.getItemId(), StatusDto.Available);
                    System.out.printf("CheckDate... -> Auction with id %d has no bids and its item with id %d is now available\n", auction.getId(), auction.getItemId());
                }
            }
        }

        return auctionsWithBids;
    }
}
