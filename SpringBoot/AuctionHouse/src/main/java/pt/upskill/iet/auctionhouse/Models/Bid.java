package pt.upskill.iet.auctionhouse.Models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDate;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
public class Bid {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private long itemId;

    private double price;

    private LocalDate bidDate;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user; // Relacionamento correto

    @ManyToOne
    @JoinColumn(name = "auction_id", nullable = false)
    @JsonIgnore
    private Auction auction;

}
