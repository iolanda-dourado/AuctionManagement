package pt.upskill.iet.auctionhouse.Models;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
public class AuctionBid {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private long itemId;
    private long userId;
    private double price;

    @ManyToOne
    @JoinColumn(name = "auction_id", nullable = false)
    private Auction auction; // Campo que mapeia a relação
}
