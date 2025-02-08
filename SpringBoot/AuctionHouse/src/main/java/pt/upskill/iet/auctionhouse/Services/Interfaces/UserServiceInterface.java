package pt.upskill.iet.auctionhouse.Services.Interfaces;

import pt.upskill.iet.auctionhouse.Dtos.UserCreateDto;
import pt.upskill.iet.auctionhouse.Dtos.UserDto;
import org.springframework.data.domain.Page;

public interface UserServiceInterface {

    UserDto addUser(UserCreateDto userCreateDto) throws Exception;

    Page<UserDto> getAllUsers(int page, int size);

    UserDto getUserById(long id) throws Exception;

//    UserDto updateUser (long id, UserDto userDto);

    UserDto patchUserName(long id, String name) throws Exception;

    UserDto deleteUser (long id) throws Exception ;

    // Extra endpoints
    Page<UserDto> getUsersWithBids(int page, int size);

    Page<UserDto> getUsersWithoutBids(int page, int size);
}