package pt.upskill.iet.auctionhouse.Services.Implementation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import pt.upskill.iet.auctionhouse.Models.User;
import pt.upskill.iet.auctionhouse.Repositories.UserRepository;
import pt.upskill.iet.auctionhouse.Services.Interfaces.UserServiceInterface;

import java.util.Optional;

@Service
public class UserService implements UserServiceInterface {

    @Autowired
    UserRepository userRepository;


    // -------- ADD USER --------
    @Override
    public UserDto addUser(UserDto userDto) {
        User user = new User(userDto.getName(), userDto.getNif(), userDto.getBids());

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
    public UserDto getUserById(long id) {
        return this.userRepository.findById(id).map(UserDto::fromUserToDto).orElse(null);
    }


    // -------- UPDATE USER --------
    @Override
    public UserDto updateUser(long id, UserDto userDto) {
        Optional<User> optionalUser = this.userRepository.findById(id);

        if (optionalUser.isEmpty()) {
            return null;
        }

        User user = optionalUser.get();
        user.setName(userDto.getName());
        user.setNif(userDto.getNif());
        user.setBids(userDto.getBids());
        user = this.userRepository.save(user);

        return UserDto.fromUserToDto(user);
    }


    // -------- DELETE USER --------
    @Override
    public UserDto deleteUser(long id) {
        Optional<User> optionalUser = this.userRepository.findById(id);

        if (optionalUser.isEmpty()) {
            return null;
        }

        this.userRepository.deleteById(id);
        return UserDto.fromUserToDto(optionalUser.get());
    }
}
