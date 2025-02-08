package pt.upskill.iet.auctionhouse.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.hateoas.CollectionModel;
import org.springframework.hateoas.Link;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.BidCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.BidDto;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidPriceException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Services.Implementation.BidService;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.linkTo;
import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.methodOn;

@RestController
@RequestMapping("api/v1/bid/")
public class BidController {

    @Autowired
    private BidService bidService;

    // -------- ADD BID --------
    // http://localhost:8080/api/v1/bid/add-bid
    @PostMapping("add-bid")
    public ResponseEntity<BidDto> addBid(@RequestBody BidCreateDto bidCreateDto) {
        try {
            BidDto bid = this.bidService.addBid(bidCreateDto);

            bid.add(linkTo(methodOn(BidController.class).getBids(Optional.of(1), Optional.of(10))).withRel("bids"));

            return new ResponseEntity<>(bid, HttpStatus.CREATED); // Use HttpStatus.CREATED para a criação
        } catch (InvalidPriceException | InvalidOperationException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }

    // -------- GET BIDS --------
    // http://localhost:8080/api/v1/bid/get-bids
    @GetMapping("get-bids")
    public CollectionModel<BidDto> getBids(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<BidDto> bidsDto = this.bidService.getAllBids(_page, _size);

        return createPagedResponse(bidsDto, _page, _size);
    }


    // -------- GET BID BY ID --------
    // http://localhost:8080/api/v1/bid/get-bid/1
    @GetMapping("get-bid/{id}")
    public ResponseEntity<BidDto> getBidById(@PathVariable("id") long id) {
        try {
            BidDto bidDto = this.bidService.getBidById(id);

            bidDto.add(linkTo(methodOn(BidController.class).getBidById(id)).withSelfRel());
            bidDto.add(linkTo(methodOn(BidController.class).getBids(Optional.of(1), Optional.of(10))).withRel("Bids"));
            return new ResponseEntity<>(bidDto, HttpStatus.OK);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- UPDATE BID --------
    // http://localhost:8080/api/v1/bid/update-bid/1
//    @PutMapping("update-bid/{id}")
//    public ResponseEntity<BidDto> updateBid(@PathVariable long id, @RequestBody BidDto bidDto) {
//        try {
//            BidDto updatedBid = this.bidService.updateBid(id, bidDto);
//            updatedBid.add(linkTo(methodOn(BidController.class).getBids(Optional.of(1), Optional.of(10))).withRel("bids"));
//            return new ResponseEntity<>(updatedBid, HttpStatus.OK);
//        } catch (NotFoundException e) {
//            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
//        } catch (Exception e) {
//            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
//        }
//    }


    // -------- DELETE BID --------
    // http://localhost:8080/api/v1/bid/delete-bid/1
    @DeleteMapping("delete-bid/{id}")
    public ResponseEntity<Void> deleteBid(@PathVariable long id) {
        try {
            bidService.deleteBid(id);
            return new ResponseEntity<>(HttpStatus.NO_CONTENT); // Retorna NO_CONTENT sem o corpo
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (InvalidOperationException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }




    // -------- EXTRA ENDPOINTS --------

    // http://localhost:8080/api/v1/bid/get-bids-by-user/1
    @GetMapping("get-bids-by-user/{id}")
    public CollectionModel<BidDto> getBidsByUser(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size, @PathVariable("id") long id) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<BidDto> bidsDto = this.bidService.getBidsByUserId(id, _page, _size);

        return createPagedResponse(bidsDto, _page, _size);
    }



    // http://localhost:8080/api/v1/bid/get-bids-by-auction/1
    @GetMapping("get-bids-by-auction/{id}")
    public CollectionModel<BidDto> getBidsByAuction(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size, @PathVariable("id") long id) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<BidDto> bidsDto = this.bidService.getBidsByAuctionId(id, _page, _size);

        return createPagedResponse(bidsDto, _page, _size);
    }


    private CollectionModel<BidDto> createPagedResponse(Page<BidDto> bidsDto, int page, int size) {
        bidsDto = bidsDto.map(auction -> auction.add(linkTo(methodOn(AuctionController.class).getAuctionById(auction.getId())).withSelfRel()));

        Link selfLink = linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page), Optional.of(size))).withSelfRel();
        List<Link> links = new ArrayList<>();
        links.add(selfLink);

        if (!bidsDto.isLast()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page + 1), Optional.of(size))).withRel("next"));
        }
        if (!bidsDto.isFirst()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page - 1), Optional.of(size))).withRel("previous"));
        }

        return CollectionModel.of(bidsDto, links);
    }

}
