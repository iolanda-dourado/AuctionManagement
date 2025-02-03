package pt.upskill.iet.auctionhouse.Models;

import jakarta.persistence.*;
import lombok.*;

import java.util.Date;

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

    @ManyToOne
    @JoinColumn(name = "bid_id")
    private Bid bid;
}
