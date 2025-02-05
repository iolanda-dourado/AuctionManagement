package pt.upskill.iet.auctionhouse.Dtos;

import pt.upskill.iet.auctionhouse.Models.Bid;

import java.util.List;

public class UserCreateDto {
    private long id;
    private String name;
    private String nif;
    private List<Bid> bids;
}
