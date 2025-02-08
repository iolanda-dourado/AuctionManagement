package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import pt.upskill.iet.auctionhouse.Models.User;


public interface UserRepository extends JpaRepository<User, Long> {
    @Query("SELECT u FROM User u WHERE SIZE(u.bids) > 0")
    Page<User> findUsersWithBids(Pageable pageable);

    @Query("SELECT u FROM User u WHERE SIZE(u.bids) = 0")
    Page<User> findUsersWithoutBids(Pageable pageable);
}
