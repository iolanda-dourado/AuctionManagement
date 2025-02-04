package pt.upskill.iet.auctionhouse.Models;

import jakarta.persistence.*;
import lombok.*;

import java.util.Date;
import java.util.List;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
public class Auction {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Column(nullable = false)
    private long itemId;

    private Date initialDate;

    private Date finalDate;

    private double finalPrice;

    private boolean active;

    @OneToMany(mappedBy = "auction", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<AuctionBid> bids; // Um leilão pode ter vários lances
}
