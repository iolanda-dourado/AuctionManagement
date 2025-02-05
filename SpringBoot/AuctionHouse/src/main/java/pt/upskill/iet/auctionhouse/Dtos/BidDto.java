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

@Getter
@Setter
@AllArgsConstructor
public class BidDto extends RepresentationModel<BidDto> {

    private long id;
    private long userId;
    private double price;
    private long auctionId;

    // Método que converte de Bid para DTO
    public static BidDto fromBidToDto(Bid bid) {
        return new BidDto(
                bid.getId(),
                bid.getUser().getId(), // Corrigido: pegar o ID do User
                bid.getPrice(),
                bid.getAuction().getId()
        );
    }

    // Método que converte de DTO para Bid
    public Bid fromDtoToBid(AuctionRepository auctionRepository, UserRepository userRepository) {
        Auction auction = auctionRepository.findById(auctionId)
                .orElseThrow(() -> new IllegalArgumentException("Auction not found with ID: " + auctionId));

        User user = userRepository.findById(userId)
                .orElseThrow(() -> new IllegalArgumentException("User not found with ID: " + userId));

        return new Bid(price, user, auction);
    }
}
