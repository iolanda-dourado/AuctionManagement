package pt.upskill.iet.auctionhouse.Dtos;

import lombok.Getter;
import lombok.Setter;

import java.time.LocalDate;

@Getter
@Setter
public class AuctionCreateDto {

    private long itemId;

    private LocalDate initialDate;

    private LocalDate finalDate;
}
