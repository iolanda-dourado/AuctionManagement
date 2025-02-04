package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import pt.upskill.iet.auctionhouse.Models.AuctionBid;

public interface BidRepository extends JpaRepository<AuctionBid, Long> {
}
