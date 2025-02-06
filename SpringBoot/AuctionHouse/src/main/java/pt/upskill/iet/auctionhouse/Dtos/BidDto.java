package pt.upskill.iet.auctionhouse.Dtos;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import org.springframework.hateoas.RepresentationModel;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;
import pt.upskill.iet.auctionhouse.Models.User;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Repositories.UserRepository;

import java.time.LocalDate;

@Getter
@Setter
@AllArgsConstructor
public class BidDto extends RepresentationModel<BidDto> {

    private long id;
    private long userId;
    private long itemId;
    private double price;
    private LocalDate bidDate;
    private long auctionId;

    // Método que converte de Bid para DTO
    public static BidDto fromBidToDto(Bid bid) {
        return new BidDto(
                bid.getId(),
                bid.getUser().getId(),
                bid.getItemId(),
                bid.getPrice(),
                bid.getBidDate(),
                bid.getAuction().getId()
        );
    }


    // Método que converte de DTO para Bid
    public Bid fromDtoToBid(AuctionRepository auctionRepository, UserRepository userRepository) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new IllegalArgumentException("User not found with ID: " + userId));

        Auction auction = auctionRepository.findById(auctionId)
                .orElseThrow(() -> new IllegalArgumentException("Auction not found with ID: " + auctionId));

        return new Bid(0, 0, price, LocalDate.now(), user, auction); // 0 para ID e itemId
    }
}
