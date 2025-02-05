package pt.upskill.iet.auctionhouse.Dtos;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@AllArgsConstructor
public class BidCreateDto {

    private double price;
    private long userId;
    private long auctionId;
}
