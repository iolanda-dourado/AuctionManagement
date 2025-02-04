package pt.upskill.iet.auctionhouse.Dtos;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ItemDto {

    private long id;
    private String name;
    private double price;
    private int status;
    private long categoryId;
    private String categoryName;

    public ItemDto(long id, String name, double price, int status, long categoryId, String categoryName) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.status = status;
        this.categoryId = categoryId;
        this.categoryName = categoryName;
    }
}
