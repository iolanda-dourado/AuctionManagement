package pt.upskill.iet.auctionhouse.Dtos;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.springframework.hateoas.RepresentationModel;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class AuctionDto extends RepresentationModel<AuctionDto> {
    private long id;

    private long itemId;

    private LocalDate initialDate;

    private LocalDate finalDate;

    private double finalPrice;

    private boolean active;

    private List<Bid> bids;

    // Método que converte de User para DTO
    public static AuctionDto fromAuctionToDto(Auction auction) {
        return new AuctionDto(auction.getId(), auction.getItemId(), auction.getInitialDate(), auction.getFinalDate(), auction.getFinalPrice(), auction.isActive(), auction.getBids());
    }
}
