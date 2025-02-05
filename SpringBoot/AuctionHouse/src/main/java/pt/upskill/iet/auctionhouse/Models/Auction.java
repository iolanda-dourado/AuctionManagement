package pt.upskill.iet.auctionhouse.Models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
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

    private LocalDate initialDate;
    private LocalDate finalDate;
    private double finalPrice;
    private boolean active;

    @OneToMany(mappedBy = "auction", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    @JsonIgnore
    private List<Bid> bids = new ArrayList<>();

    public Auction(long itemId, LocalDate initialDate, LocalDate finalDate, double finalPrice, boolean active, List<Bid> bids) {
        this.itemId = itemId;
        this.initialDate = initialDate;
        this.finalDate = finalDate;
        this.finalPrice = finalPrice;
        this.active = active;
        this.bids = new ArrayList<>();
    }

    public Auction(long itemId, LocalDate initialDate, LocalDate finalDate, double finalPrice, boolean active) {
        this.itemId = itemId;
        this.initialDate = initialDate;
        this.finalDate = finalDate;
        this.finalPrice = finalPrice;
        this.active = active;
        this.bids = new ArrayList<>();
    }
}
