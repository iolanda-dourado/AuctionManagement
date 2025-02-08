package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import pt.upskill.iet.auctionhouse.Models.Auction;

public interface AuctionRepository extends JpaRepository<Auction, Long> {

    @Query("SELECT a FROM Auction a WHERE a.active = true")
    Page<Auction> findActiveAuctions(Pageable pageable);

    @Query("SELECT a FROM Auction a WHERE a.active = false")
    Page<Auction> findInactiveAuctions(Pageable pageable);

    @Query("SELECT a FROM Auction a WHERE SIZE(a.bids) > 0")
    Page<Auction> findAuctionsWithBids(Pageable pageable);

    @Query("SELECT a FROM Auction a WHERE SIZE(a.bids) = 0")
    Page<Auction> findAuctionsWithoutBids(Pageable pageable);

    @Query("SELECT a FROM Auction a WHERE a.finalPrice >= :price")
    Page<Auction> findAuctionsAbovePrice(double price, Pageable pageable);
}
