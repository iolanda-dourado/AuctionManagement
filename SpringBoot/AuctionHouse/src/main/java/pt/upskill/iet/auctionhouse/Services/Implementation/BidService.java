package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.BidDto;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;
import pt.upskill.iet.auctionhouse.Models.User;
import pt.upskill.iet.auctionhouse.Repositories.AuctionRepository;
import pt.upskill.iet.auctionhouse.Repositories.BidRepository;
import pt.upskill.iet.auctionhouse.Repositories.UserRepository;
import pt.upskill.iet.auctionhouse.Retrofit.AuctionHouseService;
import pt.upskill.iet.auctionhouse.Services.Interfaces.BidServiceInterface;

import java.util.Optional;

@Service
public class BidService implements BidServiceInterface {

    @Autowired
    BidRepository bidRepository;
    @Autowired
    AuctionRepository auctionRepository;
    @Autowired
    UserRepository userRepository;
    @Autowired
    AuctionHouseService auctionHouseService;


    // -------- ADD BID --------
    @Override
    public BidDto addBid(BidDto bidDto) throws Exception {
        // Buscar o leilão pelo ID
        Auction auction = auctionRepository.findById(bidDto.getAuctionId())
                .orElseThrow(() -> new NotFoundException("Auction not found"));

        // Buscar o utilizador que fez o lance
        User user = userRepository.findById(bidDto.getUserId())
                .orElseThrow(() -> new NotFoundException("User not found"));

        // Buscar os detalhes do item associado ao leilão
        ItemDto itemDto = auctionHouseService.getItemById(auction.getItemId());

        if (itemDto == null) {
            throw new NotFoundException("Item not found in external API");
        }

        // Criar o Bid e associá-lo ao leilão e ao utilizador
        Bid bid = new Bid();
        bid.setAuction(auction);
        bid.setUser(user);
        bid.setPrice(bidDto.getPrice());

        // Adicionar o bid à lista de bids do utilizador e do leilão
        user.getBids().add(bid);
        auction.getBids().add(bid);

        // Salvar o lance no banco de dados
        bid = bidRepository.save(bid);

        return BidDto.fromBidToDto(bid);
    }


    // -------- GET ALL BIDS --------
    @Override
    public Page<BidDto> getAllBids(int page, int size) {
        return this.bidRepository.findAll(PageRequest.of(page, size)).map(BidDto::fromBidToDto);
    }


    // -------- GET BID BY ID --------
    @Override
    public BidDto getBidById(long id) throws Exception {
        return this.bidRepository.findById(id).map(BidDto::fromBidToDto)
                .orElseThrow(() -> new NotFoundException("Bid not found"));
    }


    // -------- UPDATE BID --------
    @Override
    public BidDto updateBid(long id, BidDto bidDto) throws Exception {
        Optional<Bid> optionalBid = this.bidRepository.findById(id);
        Optional<User> user = userRepository.findById(bidDto.getUserId());
        Optional<Auction> auction = auctionRepository.findById(bidDto.getAuctionId());
        if (auction.isEmpty()) {
            throw new NotFoundException("Auction not found");
        }

        Optional<ItemDto> itemDto = Optional.ofNullable(auctionHouseService.getItemById(auction.get().getItemId()));
        if (optionalBid.isEmpty()) {
            throw new InvalidOperationException("Bid not found");
        }
        if (user.isEmpty()) {
            throw new NotFoundException("User not found");
        }
        if (itemDto.isEmpty()) {
            throw new NotFoundException("User not found");
        }

        Bid bid = bidDto.fromDtoToBid(auctionRepository, userRepository);
        bid.setUser(user.get());
        bid.setPrice(bidDto.getPrice());
        bid.setAuction(bid.getAuction());
        bid = this.bidRepository.save(bid);

        return BidDto.fromBidToDto(bid);
    }


    // -------- DELETE BID --------
    @Override
    public BidDto deleteBid(long id) throws Exception {
        Optional<Bid> optionalBid = this.bidRepository.findById(id);

        if (optionalBid.isEmpty()) {
            throw new NotFoundException("Bid not found");
        }

        this.bidRepository.deleteById(id);
        return BidDto.fromBidToDto(optionalBid.get());
    }
}
