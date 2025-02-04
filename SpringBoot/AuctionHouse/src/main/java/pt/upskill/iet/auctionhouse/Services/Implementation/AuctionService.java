package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
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
        // Log para verificar os dados recebidos
        System.out.println("Dados do leilão: " + auctionDto);
        Auction auction = new Auction(
                auctionDto.getItemId(),
                auctionDto.getInitialDate(),
                auctionDto.getFinalDate(),
                auctionDto.getFinalPrice(),
                auctionDto.isActive(),
                auctionDto.getBids()
        );

        if (auctionDto.getFinalDate().before(auctionDto.getInitialDate())) {
            throw new InvalidDateException("The final date must be greater than the initial date");
        }

        ItemDto itemDto = this.auctionHouseService.getItemById(auctionDto.getItemId());

        // Verifica se o item não foi encontrado
        if (itemDto == null) {
            throw new NotFoundException("Item not found");
        }

        if (auctionDto.getFinalPrice() < itemDto.getPrice()) {
            throw new InvalidPriceException("The final price must be greater than the item price");
        }

        auction = this.auctionRepository.save(auction);

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

        if (auctionDto.getFinalDate().before(auctionDto.getInitialDate())) {
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

        this.auctionRepository.deleteById(id);
        return AuctionDto.fromAuctionToDto(optionalAuction.get());
    }
}
