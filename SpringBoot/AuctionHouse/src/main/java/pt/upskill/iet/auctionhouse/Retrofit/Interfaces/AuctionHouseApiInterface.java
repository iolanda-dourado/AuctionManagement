package pt.upskill.iet.auctionhouse.Retrofit.Interfaces;

import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.SaleDto;
import retrofit2.Call;
import retrofit2.http.*;

import java.util.List;

public interface AuctionHouseApiInterface {

    @GET("Items")
    Call<List<ItemDto>> getItems();

    @GET("Items/available-items")
    Call<List<ItemDto>> getAvailableItems();

    @GET("Items/{id}")
    Call<ItemDto> getItemById(@Path("id") long id);

    @PATCH("Items/update-item-status/itemid/{id}/status/{status}")
    Call<Void> updateItemStatus(@Path("id") long id, @Path("status") int status);

    @POST("Sales")
    Call<SaleDto> addSale(@Body SaleDto saleDto);
}
