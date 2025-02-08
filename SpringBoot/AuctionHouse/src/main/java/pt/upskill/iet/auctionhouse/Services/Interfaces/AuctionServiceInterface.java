package pt.upskill.iet.auctionhouse.Services.Interfaces;

import org.springframework.data.domain.Page;
import pt.upskill.iet.auctionhouse.Dtos.AuctionCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;

public interface AuctionServiceInterface {

    AuctionDto addAuction(AuctionCreateDto auctionDto) throws Exception;

    Page<AuctionDto> getAllAuctions(int page, int size) throws Exception;

    AuctionDto getAuctionById(long id) throws Exception;

//    AuctionDto updateAuction (long id, AuctionUpdateDto auctionUpdateDto) throws Exception;

    AuctionDto deleteAuction (long id) throws Exception;

    Page<AuctionDto> getActiveAuctions(int page, int size);

    Page<AuctionDto> getInactiveAuctions(int page, int size);

    Page<AuctionDto> getAuctionsWithBids(int page, int size);

    Page<AuctionDto> getAuctionsWithoutBids(int page, int size);

    Page<AuctionDto> getAuctionsAbovePrice(double price, int page, int size);
}
