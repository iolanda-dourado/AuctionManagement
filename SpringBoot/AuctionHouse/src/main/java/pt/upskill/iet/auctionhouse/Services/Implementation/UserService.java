package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.BidDto;
import pt.upskill.iet.auctionhouse.Dtos.UserCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidNifException;
import pt.upskill.iet.auctionhouse.Exceptions.InvalidOperationException;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Models.Auction;
import pt.upskill.iet.auctionhouse.Models.Bid;
import pt.upskill.iet.auctionhouse.Models.User;
import pt.upskill.iet.auctionhouse.Repositories.UserRepository;
import pt.upskill.iet.auctionhouse.Services.Interfaces.UserServiceInterface;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class UserService implements UserServiceInterface {

    @Autowired
    UserRepository userRepository;


    // -------- ADD USER --------
    @Override
    public UserDto addUser(UserCreateDto userCreateDto) throws Exception {
        List<Bid> bids = new ArrayList<>();
        User user = new User(userCreateDto.getName(), userCreateDto.getNif(), bids);

        // Confere se o nif tem exatamente 9 dígitos. Caso não tenha, não permite a criação
        if (user.getNif().length() != 9) {
            throw new InvalidNifException("NIF must have exactly 9 digits");
        }


        // Confere se algum dos users já têm o nif do user a ser criado, se já existir, não permite a criação
        for (User u : this.userRepository.findAll()) {
            if (u.getNif().equals(user.getNif())) {
                throw new InvalidOperationException("User with NIF " + user.getNif() + " already exists");
            }
        }

        user = this.userRepository.save(user);

        return UserDto.fromUserToDto(user);
    }


    // -------- GET ALL USERS --------
    @Override
    public Page<UserDto> getAllUsers(int page, int size) {
        return this.userRepository.findAll(PageRequest.of(page, size)).map(UserDto::fromUserToDto);
    }


    // -------- GET USER BY ID --------
    @Override
    public UserDto getUserById(long id) throws Exception {
        return this.userRepository.findById(id).map(UserDto::fromUserToDto).orElseThrow(() -> new NotFoundException("User not found"));
    }


    // -------- UPDATE USER --------
//    @Override
//    public UserDto updateUser(long id, UserDto userDto) {
//        Optional<User> optionalUser = this.userRepository.findById(id);
//
//        if (optionalUser.isEmpty()) {
//            return null;
//        }
//
//        User user = optionalUser.get();
//        user.setName(userDto.getName());
//        user.setNif(userDto.getNif());
//        user.setBids(userDto.getBids());
//        user = this.userRepository.save(user);
//
//        return UserDto.fromUserToDto(user);
//    }


    @Override
    public UserDto patchUserName(long id, String name) throws Exception {
        User user = this.userRepository.findById(id).orElseThrow(() -> new NotFoundException("User not found"));

        user.setName(name);
        user = this.userRepository.save(user);

        return UserDto.fromUserToDto(user);
    }


    // -------- DELETE USER --------
    @Override
    public UserDto deleteUser(long id) throws Exception {
        User user = this.userRepository.findById(id).orElseThrow(() -> new NotFoundException("User not found"));

        // Não permite a deleção caso o user já tenha dado bids
        if (!user.getBids().isEmpty()) {
            throw new InvalidOperationException("User has bids, cannot be deleted.");
        }

        this.userRepository.deleteById(id);
        return UserDto.fromUserToDto(user);
    }


    // Extra endpoints
    @Override
    public Page<UserDto> getUsersWithBids(int page, int size) {
        return this.userRepository.findUsersWithBids(PageRequest.of(page, size)).map(UserDto::fromUserToDto);
    }


    @Override
    public Page<UserDto> getUsersWithoutBids(int page, int size) {
        return this.userRepository.findUsersWithoutBids(PageRequest.of(page, size)).map(UserDto::fromUserToDto);
    }
}
