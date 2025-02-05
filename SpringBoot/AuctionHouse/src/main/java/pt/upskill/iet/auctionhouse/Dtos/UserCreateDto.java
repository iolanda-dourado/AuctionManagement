package pt.upskill.iet.auctionhouse.Dtos;

import lombok.Getter;
import lombok.Setter;
import pt.upskill.iet.auctionhouse.Models.Bid;

import java.util.List;

@Getter
@Setter
public class UserCreateDto {
    private String name;
    private String nif;
}
