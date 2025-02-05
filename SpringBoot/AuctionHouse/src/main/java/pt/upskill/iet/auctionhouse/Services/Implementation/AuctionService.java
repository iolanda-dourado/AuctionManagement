package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidPriceException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Retrofit.AuctionHouseService;
import pt.upskill.iet.auctionhouse.Services.Interfaces.AuctionServiceInterface;

import java.util.Optional;

@Service
public class AuctionService implements AuctionServiceInterface {

    @Autowired
    AuctionRepository auctionRepository;
    @Autowired
    AuctionHouseService auctionHouseService;


    // -------- ADD AUCTION --------
    @Override
    public AuctionDto addAuction(AuctionDto auctionDto) throws Exception {
        // Validação de data
        if (auctionDto.getFinalDate().isBefore(auctionDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        ItemDto itemDto = this.auctionHouseService.getItemById(auctionDto.getItemId());
        // Verifica se o item não foi encontrado
        if (itemDto == null) {
            throw new NotFoundException("Item not found");
        }
        // Validação de preço
        if (auctionDto.getFinalPrice() < itemDto.getPrice()) {
            throw new InvalidPriceException("The final price must be greater than the item's price");
        }
        if (itemDto.getStatus() == StatusDto.Sold) {
            throw new InvalidOperationException("The item is already sold, therefore it cannot be auctioned.");
        }
        auctionHouseService.updateItemStatus(itemDto.getId(), StatusDto.AtAuction);


        // Criando a entidade Auction
        Auction auction = new Auction();
        auction.setItemId(auctionDto.getItemId());
        auction.setInitialDate(auctionDto.getInitialDate());
        auction.setFinalDate(auctionDto.getFinalDate());
        auction.setFinalPrice(auctionDto.getFinalPrice());
        auction.setActive(auctionDto.isActive());

        // Salvando no repositório
        auction = this.auctionRepository.save(auction);

        // Convertendo para DTO e retornando
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
    public AuctionDto updateAuction(long id, AuctionDto auctionDto) throws Exception {
        Optional<Auction> optionalAuction = this.auctionRepository.findById(id);

        if (optionalAuction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        if (auctionDto.getFinalDate().isBefore(auctionDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        Auction auction = optionalAuction.get();
        auction.setItemId(auctionDto.getItemId());
        auction.setInitialDate(auctionDto.getInitialDate());
        auction.setFinalDate(auctionDto.getFinalDate());
        auction.setFinalPrice(auctionDto.getFinalPrice());
        auction.setActive(auctionDto.isActive());

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
        // Faz update ao estado do item para Available
        if (itemDto.getStatus() != StatusDto.Sold) {
            auctionHouseService.updateItemStatus(itemDto.getId(), StatusDto.Available);

        // Caso o item tenha sido vendido, lançar exceção para impedir que o leilão seja eliminado
        } else {
            throw new InvalidOperationException("The item is already sold, therefore the Auction cannot be deleted.");
        }

        this.auctionRepository.deleteById(id);
        return AuctionDto.fromAuctionToDto(optionalAuction.get());
    }
}
