package pt.upskill.iet.auctionhouse.Models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
public class Bid {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;
//
//    private long itemId;
    private double price;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user; // Relacionamento correto

    @ManyToOne
    @JoinColumn(name = "auction_id", nullable = false)
    @JsonIgnore
    private Auction auction;

    public Bid(double price, User user, Auction auction) {
        this.price = price;
        this.user = user;
        this.auction = auction;
    }
}
