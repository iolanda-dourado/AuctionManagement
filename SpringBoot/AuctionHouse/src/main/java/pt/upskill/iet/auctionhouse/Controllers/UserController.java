package pt.upskill.iet.auctionhouse.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.hateoas.CollectionModel;
import org.springframework.hateoas.Link;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.AuctionDto;
import pt.upskill.iet.auctionhouse.Dtos.UserCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidNifException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.User;
import pt.upskill.iet.auctionhouse.Services.Implementation.UserService;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.linkTo;
import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.methodOn;

@RestController
@RequestMapping("api/v1/user/")
public class UserController {

    @Autowired
    private UserService userService;

    // -------- ADD USER --------
    // http://localhost:8080/api/v1/user/add-user
    @PostMapping("add-user")
    public ResponseEntity<UserDto> addUser(@RequestBody UserCreateDto userCreateDto) {
        try {
            UserDto userDto = this.userService.addUser(userCreateDto);

            userDto.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("users"));

            return new ResponseEntity<UserDto>(userDto, HttpStatus.CREATED);
        } catch (InvalidOperationException | InvalidNifException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }

    }


    // -------- GET USERS --------
    // http://localhost:8080/api/v1/user/get-users
    @GetMapping("get-users")
    public CollectionModel<UserDto> getUsers(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<UserDto> usersDto = this.userService.getAllUsers(_page, _size);
        return createPagedResponse(usersDto, _page, _size);
    }


    // -------- GET USER BY ID --------
    // http://localhost:8080/api/v1/user/get-user/1
    @GetMapping("get-user/{id}")
    public ResponseEntity<UserDto> getUserById(@PathVariable("id") long id) {
        try {
            UserDto userDto = this.userService.getUserById(id);

            userDto.add(linkTo(methodOn(UserController.class).getUserById(id)).withSelfRel());
            userDto.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("Users"));
            return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- UPDATE USER --------
    // http://localhost:8080/api/ex1/user/update-user/1
//    @PutMapping("update-user/{id}")
//    public ResponseEntity<UserDto> updateUser(@RequestParam long id, @RequestBody UserDto user) {
//
//        UserDto existingUser = this.userService.getUserById(id);
//        if (existingUser == null) {
//            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
//        }
//
//        UserDto updatedUser = this.userService.updateUser(id, user);
//        updatedUser.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("users"));
//        return new ResponseEntity<>(updatedUser, HttpStatus.OK);
//    }


    @PatchMapping("update-user-name/{id}/{newName}")
    public ResponseEntity<UserDto> patchUserName(@PathVariable long id, @PathVariable String newName) {
        try {
            UserDto user = this.userService.patchUserName(id, newName);
            return new ResponseEntity<UserDto>(user, HttpStatus.OK);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }


    // -------- DELETE USER --------
    // http://localhost:8080/api/ex1/user/delete-user/1
    @DeleteMapping("delete-user/{id}")
    public ResponseEntity<UserDto> deleteUser(@PathVariable long id) {
        try {
            UserDto deletedUser = userService.deleteUser(id);

            return new ResponseEntity<>(deletedUser, HttpStatus.NO_CONTENT);
        } catch (NotFoundException e) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        } catch (InvalidOperationException e) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }



    // ----------- EXTRA ENDPOINTS -----------

    // -------- GET USERS WITH BIDS --------
    // http://localhost:8080/api/v1/user/get-users-with-bids
    @GetMapping("get-users-with-bids")
    public CollectionModel<UserDto> getUsersWithBids(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<UserDto> usersDto = this.userService.getUsersWithBids(_page, _size);

        return createPagedResponse(usersDto, _page, _size);
    }


    // -------- GET USERS WITHOUT BIDS --------
    // http://localhost:8080/api/v1/user/get-users-without-bids
    @GetMapping("get-users-without-bids")
    public CollectionModel<UserDto> getUsersWithoutBids(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<UserDto> usersDto = this.userService.getUsersWithoutBids(_page, _size);

        return createPagedResponse(usersDto, _page, _size);
    }



    private CollectionModel<UserDto> createPagedResponse(Page<UserDto> usersDto, int page, int size) {
        usersDto = usersDto.map(auction -> auction.add(linkTo(methodOn(AuctionController.class).getAuctionById(auction.getId())).withSelfRel()));

        Link selfLink = linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page), Optional.of(size))).withSelfRel();
        List<Link> links = new ArrayList<>();
        links.add(selfLink);

        if (!usersDto.isLast()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page + 1), Optional.of(size))).withRel("next"));
        }
        if (!usersDto.isFirst()) {
            links.add(linkTo(methodOn(AuctionController.class).getAuctions(Optional.of(page - 1), Optional.of(size))).withRel("previous"));
        }

        return CollectionModel.of(usersDto, links);
    }
}
