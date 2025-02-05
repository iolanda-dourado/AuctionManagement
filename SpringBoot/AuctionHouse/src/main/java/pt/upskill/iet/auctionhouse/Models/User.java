package pt.upskill.iet.auctionhouse.Models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;

import java.util.ArrayList;
import java.util.List;

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

    @Column(length = 100, nullable = false)
    private String name;

    @Column(length = 9, nullable = false, unique = true)
    private String nif;

    @OneToMany(mappedBy = "user", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    @JsonIgnore
    private List<Bid> bids = new ArrayList<>(); // Relacionamento correto

    public User(String name, String nif) {
        this.name = name;
        this.nif = nif;
        this.bids = new ArrayList<>();
    }

    public User(String name, String nif, List<Bid> bids) {
        this.name = name;
        this.nif = nif;
        this.bids = bids;
    }

    public static User fromDtoToUser(UserDto userDto) {
        return new User(userDto.getId(), userDto.getName(), userDto.getNif(), userDto.getBids());
    }
}
