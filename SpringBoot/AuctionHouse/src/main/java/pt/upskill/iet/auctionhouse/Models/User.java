package pt.upskill.iet.auctionhouse.Models;

import jakarta.persistence.*;
import lombok.*;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Entity
@Table(name = "auction_user")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE)
    private long id;

    @Column(length = 100)
    private String name;

    @Column(length = 9)
    private String nif;

    @ManyToOne
    @JoinColumn(name = "bids_id")
    private AuctionBid bids;

    public User(String name, String nif, AuctionBid bids) {
        this.name = name;
        this.nif = nif;
        this.bids = bids;
    }


    // Método que converte de DTO para User
    public static User fromDtoToUser(UserDto userDto) {
        return new User(userDto.getId(), userDto.getName(), userDto.getNif(), userDto.getBids());
    }
}
