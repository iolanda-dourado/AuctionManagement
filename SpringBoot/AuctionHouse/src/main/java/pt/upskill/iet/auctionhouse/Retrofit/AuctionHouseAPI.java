package pt.upskill.iet.auctionhouse.Retrofit;

import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import retrofit2.Call;
import retrofit2.http.GET;

import java.util.List;

public interface AuctionHouseAPI {

    @GET("Items/available-items")
    Call<List<ItemDto>> getAvailableItems();
}
