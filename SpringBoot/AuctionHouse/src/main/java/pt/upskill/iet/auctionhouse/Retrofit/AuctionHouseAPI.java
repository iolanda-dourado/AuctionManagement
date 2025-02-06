package pt.upskill.iet.auctionhouse.Retrofit;

import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.Path;

import java.util.List;

public interface AuctionHouseAPI {

    @GET("Items")
    Call<List<ItemDto>> getItems();

    @GET("Items/available-items")
    Call<List<ItemDto>> getAvailableItems();

    @GET("Items/{id}")
    Call<ItemDto> getItemById(@Path("id") long id);

    @PATCH("Items/update-item-status/itemid/{id}/status/{status}")
    Call<Void> updateItemStatus( // Altere para Void
                                 @Path("id") long id,
                                 @Path("status") int status
    );
}
