package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import pt.upskill.iet.auctionhouse.Models.Auction;

public interface AuctionRepository extends JpaRepository<Auction, Long> {
}
