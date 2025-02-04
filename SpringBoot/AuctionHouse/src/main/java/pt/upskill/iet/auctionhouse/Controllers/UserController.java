package pt.upskill.iet.auctionhouse.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.hateoas.CollectionModel;
import org.springframework.hateoas.Link;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
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
    public ResponseEntity<UserDto> addUser(@RequestBody UserDto userDto) {
        UserDto user = this.userService.addUser(userDto);

        if (user == null) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        }

        user.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("users"));

        return new ResponseEntity<UserDto>(user, HttpStatus.OK);
    }


    // -------- GET USERS --------
    // http://localhost:8080/api/v1/user/get-users
    @GetMapping("get-users")
    public CollectionModel<UserDto> getUsers(@RequestParam Optional<Integer> page, @RequestParam Optional<Integer> size) {
        int _page = page.orElse(0);
        int _size = size.orElse(10);

        Page<UserDto> userDtos = this.userService.getAllUsers(_page, _size);
        userDtos = userDtos.map((UserDto user) -> user.add(linkTo(methodOn(UserController.class).getUserById(user.getId())).withSelfRel()));
        Link link = linkTo(methodOn(UserController.class).getUsers(Optional.of(_page), Optional.of(_size))).withSelfRel();

        List<Link> links = new ArrayList<>();
        links.add(link);
        if (!userDtos.isLast()) {
            Link _link = linkTo(methodOn(UserController.class).getUsers(Optional.of(_page + 1), size)).withRel("next");
            links.add(_link);
        }
        if (!userDtos.isFirst()) {
            Link _link = linkTo(methodOn(UserController.class).getUsers(Optional.of(_page - 1), size)).withRel("previous");
            links.add(_link);
        }
        return CollectionModel.of(userDtos, links);
    }


    // -------- GET USER BY ID --------
    // http://localhost:8080/api/v1/user/get-user/1
    @GetMapping("get-user/{id}")
    public ResponseEntity<UserDto> getUserById(@PathVariable("id") long id) {
        UserDto userDto = this.userService.getUserById(id);
        if (userDto == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }

        userDto.add(linkTo(methodOn(UserController.class).getUserById(id)).withSelfRel());
        userDto.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("Users"));
        return new ResponseEntity<UserDto>(userDto, HttpStatus.OK);
    }


    // -------- UPDATE USER --------
    // http://localhost:8080/api/ex1/user/update-user/1
    @PutMapping("update-user/{id}")
    public ResponseEntity<UserDto> updateUser(@RequestParam long id, @RequestBody UserDto user) {

        UserDto existingUser = this.userService.getUserById(id);
        if (existingUser == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }

        UserDto updatedUser = this.userService.updateUser(id, user);
        updatedUser.add(linkTo(methodOn(UserController.class).getUsers(Optional.of(1), Optional.of(10))).withRel("users"));
        return new ResponseEntity<>(updatedUser, HttpStatus.OK);
    }


    // -------- DELETE USER --------
    // http://localhost:8080/api/ex1/user/delete-user/1
    @DeleteMapping("delete-user/{id}")
    public ResponseEntity<UserDto> deleteUser(@PathVariable long id) {
        UserDto deletedUser = userService.deleteUser(id);

        if (deletedUser == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(deletedUser, HttpStatus.OK);
    }
}
