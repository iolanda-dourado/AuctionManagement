package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import pt.upskill.iet.auctionhouse.Models.Bid;

public interface BidRepository extends JpaRepository<Bid, Long> {

    // Buscar lances por usuário (User ID)
    @Query("SELECT b FROM Bid b WHERE b.user.id = :userId")
    Page<Bid> findBidsByUserId(Long userId, Pageable pageable);

    // Buscar lances por leilão (Auction ID)
    @Query("SELECT b FROM Bid b WHERE b.auction.id = :auctionId")
    Page<Bid> findBidsByAuctionId(Long auctionId, Pageable pageable);
}