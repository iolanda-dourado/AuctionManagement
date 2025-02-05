package pt.upskill.iet.auctionhouse.Dtos;

import pt.upskill.iet.auctionhouse.Models.Bid;

import java.time.LocalDate;

public class AuctionUpdateDto {
    private long id;

    private long itemId;

    private LocalDate initialDate;

    private LocalDate finalDate;

    private boolean active;
}
