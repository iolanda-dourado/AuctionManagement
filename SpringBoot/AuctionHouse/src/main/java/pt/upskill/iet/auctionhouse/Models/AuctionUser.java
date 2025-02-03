package pt.upskill.iet.auctionhouse.Models;

import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
public class AuctionUser {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Column(length = 100)
    private String name;

    @Column(length = 9)
    private String nif;

    @ManyToOne
    @JoinColumn(name = "bids_id")
    private Bid bids;
}
