package pt.upskill.iet.auctionhouse.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.hateoas.CollectionModel;
import org.springframework.hateoas.Link;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.AuctionCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidDateException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidPriceException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Services.Implementation.AuctionService;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.linkTo;
import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.methodOn;

@RestController
@RequestMapping("api/v1/auction/")
public class AuctionController {

    @Autowired
    private AuctionService auctionService;


    //     -------- ADD AUCTION --------
//     http://localhost:8080/api/v1/auction/add-auction
    @PostMapping("add-auction")
    public ResponseEntity<AuctionDto> addAuction(@RequestBody AuctionCreateDto auctionCreateDto) {
        try {
            AuctionDto createdAuction = this.auctionService.addAuction(auctionCreateDto);

            // Adiciona links HATEOAS
            createdAuction.add(linkTo(methodOn(AuctionController.class).getAuctionById(createdAuction.getId())).withSelfRel());
            createdAuction.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(1), Optional.of(10))).withRel("Auctions"));

            return new ResponseEntity<>(createdAuction, HttpStatus.CREATED);
        } catch (InvalidDateException | InvalidPriceException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- GET AUCTIONS --------
    // http://localhost:8080/api/v1/auction/get-auctions
    @GetMapping("get-auctions")
    public CollectionModel<AuctionDto> getAuctions(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        try {
            auctionsDto = this.auctionService.getAllAuctions(_page, _size);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }

        return createPagedResponse(auctionsDto, _page, _size);
    }


    // -------- GET AUCTION BY ID --------
    // http://localhost:8080/api/v1/auction/get-auction/1
    @GetMapping("get-auction/{id}")
    public ResponseEntity<AuctionDto> getAuctionById(@PathVariable("id") long id) {
        try {
            AuctionDto auctionDto = this.auctionService.getAuctionById(id);

            auctionDto.add(linkTo(methodOn(AuctionController.class).getAuctionById(id)).withSelfRel());
            auctionDto.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(1), Optional.of(10))).withRel("Auctions"));
            return new ResponseEntity<AuctionDto>(auctionDto, HttpStatus.OK);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- UPDATE AUCTION --------
    // http://localhost:8080/api/ex1/auction/update-auction/1
//    @PutMapping("update-auction/{id}")
//    public ResponseEntity<AuctionDto> updateAuction(@RequestParam long id, @RequestBody AuctionUpdateDto auctionUpdateDtoDto) {
//        try {
//            AuctionDto updatedAuction = this.auctionService.updateAuction(id, auctionUpdateDtoDto);
//            updatedAuction.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(1), Optional.of(10))).withRel("auctions"));
//            return new ResponseEntity<>(updatedAuction, HttpStatus.OK);
//        } catch (NotFoundException e) {
//            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
//        } catch (InvalidDateException | InvalidOperationException e) {
//            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
//        } catch (Exception e) {
//            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
//        }
//    }


    // -------- DELETE AUCTION --------
    // http://localhost:8080/api/ex1/auction/delete-auction/1
    @DeleteMapping("delete-auction/{id}")
    public ResponseEntity<AuctionDto> deleteAuction(@PathVariable long id) {
        try {
            AuctionDto deletedAuction = auctionService.deleteAuction(id);

            return new ResponseEntity<>(deletedAuction, HttpStatus.NO_CONTENT);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (InvalidOperationException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- EXTRA ENDPOINTS --------

    // http://localhost:8080/api/v1/auction/get-active-auctions
    @GetMapping("get-active-auctions")
    public CollectionModel<AuctionDto> getActiveAuctions(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        auctionsDto = this.auctionService.getActiveAuctions(_page, _size);

        return createPagedResponse(auctionsDto, _page, _size);
    }


    // http://localhost:8080/api/v1/auction/get-inactive-auctions
    @GetMapping("get-inactive-auctions")
    public CollectionModel<AuctionDto> getInactiveAuctions(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        auctionsDto = this.auctionService.getInactiveAuctions(_page, _size);

        return createPagedResponse(auctionsDto, _page, _size);
    }


    // http://localhost:8080/api/v1/auction/get-auctions-with-bids
    @GetMapping("get-auctions-with-bids")
    public CollectionModel<AuctionDto> getAuctionsWithBids(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        auctionsDto = this.auctionService.getAuctionsWithBids(_page, _size);

        return createPagedResponse(auctionsDto, _page, _size);
    }


    // http://localhost:8080/api/v1/auction/get-auctions-without-bids
    @GetMapping("get-auctions-without-bids")
    public CollectionModel<AuctionDto> getAuctionsWithoutBids(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        auctionsDto = this.auctionService.getAuctionsWithoutBids(_page, _size);

        return createPagedResponse(auctionsDto, _page, _size);
    }



    // http://localhost:8080/api/v1/auction/get-auctions-above-price/1000
    @GetMapping("get-auctions-above-price/{price}")
    public CollectionModel<AuctionDto> getAuctionsAbovePrice(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size, @PathVariable("price") double price) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<AuctionDto> auctionsDto = null;
        auctionsDto = this.auctionService.getAuctionsAbovePrice(price, _page, _size);

        return createPagedResponse(auctionsDto, _page, _size);
    }


    private CollectionModel<AuctionDto> createPagedResponse(Page<AuctionDto> auctionsDto, int page, int size) {
        auctionsDto = auctionsDto.map(auction -> auction.add(linkTo(methodOn(AuctionController.class).getAuctionById(auction.getId())).withSelfRel()));

        Link selfLink = linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page), Optional.of(size))).withSelfRel();
        List<Link> links = new ArrayList<>();
        links.add(selfLink);

        if (!auctionsDto.isLast()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page + 1), Optional.of(size))).withRel("next"));
        }
        if (!auctionsDto.isFirst()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page - 1), Optional.of(size))).withRel("previous"));
        }

        return CollectionModel.of(auctionsDto, links);
    }


//    @PatchMapping("update-auction-status/{id}")
//    public ResponseEntity<AuctionDto> updateAuctionStatus(@PathVariable("id") long id, @RequestParam boolean isActive) {
//        try {
//            AuctionDto updatedAuction = this.auctionService.updateAuctionStatus(id, isActive);
//            return new ResponseEntity<>(updatedAuction, HttpStatus.OK);
//        } catch (NotFoundException e) {
//            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
//        } catch (Exception e) {
//            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
//        }
//    }
}
