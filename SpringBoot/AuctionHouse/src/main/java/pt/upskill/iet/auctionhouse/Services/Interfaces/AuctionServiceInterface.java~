package pt.upskill.iet.auctionhouse.Services.Interfaces;

import org.springframework.data.domain.Page;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;

public interface AuctionServiceInterface {

    AuctionDto addAuction(AuctionDto auctionDto) throws Exception;

    Page<AuctionDto> getAllAuctions(int page, int size);

    AuctionDto getAuctionById(long id) throws Exception;

    AuctionDto updateAuction (long id, AuctionDto auctionDto) throws Exception;

    AuctionDto deleteAuction (long id) throws Exception;
}
