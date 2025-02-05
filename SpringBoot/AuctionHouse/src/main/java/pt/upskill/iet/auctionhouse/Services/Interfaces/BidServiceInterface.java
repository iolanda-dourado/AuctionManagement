package pt.upskill.iet.auctionhouse.Services.Interfaces;

import org.springframework.data.domain.Page;
import pt.upskill.iet.auctionhouse.Dtos.BidDto;

public interface BidServiceInterface {

    BidDto addBid(BidDto BidDto) throws Exception;

    Page<BidDto> getAllBids(int page, int size);

    BidDto getBidById(long id) throws Exception;

    BidDto updateBid (long id, BidDto BidDto) throws Exception;

    BidDto deleteBid (long id) throws Exception;
}
