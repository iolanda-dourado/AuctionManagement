package pt.upskill.iet.auctionhouse.Services.Interfaces;

import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import org.springframework.data.domain.Page;

public interface UserServiceInterface {

    UserDto addUser(UserDto userDto);

    Page<UserDto> getAllUsers(int page, int size);

    UserDto getUserById(long id);

    UserDto updateUser (long id, UserDto userDto);

    UserDto deleteUser (long id);
}