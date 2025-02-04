package pt.upskill.iet.auctionhouse.Services.Implementation.ConsumeAPI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Retrofit.AuctionHouseService;

import java.util.List;

@RestController
@RequestMapping("api/v1/auction/")
public class ConsumeDotNetApiService {

    @Autowired
    AuctionHouseService auctionHouseService;

    @GetMapping("/available-items")
    public List<ItemDto> getAvailableItems() {
        return auctionHouseService.getAvailableItems();
    }
}
