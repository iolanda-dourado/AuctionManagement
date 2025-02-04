package pt.upskill.iet.auctionhouse.Repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import pt.upskill.iet.auctionhouse.Models.User;

public interface UserRepository extends JpaRepository<User, Long> {
}
