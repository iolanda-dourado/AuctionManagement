package pt.upskill.iet.auctionhouse.Dtos;

import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;
import org.springframework.hateoas.RepresentationModel;
import pt.upskill.iet.auctionhouse.Models.AuctionBid;
import pt.upskill.iet.auctionhouse.Models.User;

@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
public class UserDto extends RepresentationModel<UserDto> {

    private long id;
    private String name;
    private String nif;

    @JsonIgnore
    private AuctionBid bids;

    // Método que converte de User para DTO
    public static UserDto fromUserToDto(User user) {
        return new UserDto(user.getId(), user.getName(), user.getNif(), user.getBids());
    }
}
