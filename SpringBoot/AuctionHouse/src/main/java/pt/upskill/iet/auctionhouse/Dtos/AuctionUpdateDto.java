package pt.upskill.iet.auctionhouse.Dtos;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDate;

@Getter
@Setter
@AllArgsConstructor
public class AuctionUpdateDto {
    private long id;

    private LocalDate initialDate;

    private LocalDate finalDate;

    private boolean active;
}
