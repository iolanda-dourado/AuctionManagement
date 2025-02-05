package pt.upskill.iet.auctionhouse.Services.Implementation.ConsumeAPI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
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

    @GetMapping("/item-by-id/{id}")
    public ItemDto getItemById(@PathVariable long id) throws NotFoundException {
        return auctionHouseService.getItemById(id);
    }

    @PatchMapping("/update-item-status/id/{id}/status/{status}")
    public StatusDto updateItemStatus(@PathVariable long id, @PathVariable StatusDto status) throws NotFoundException {
        return auctionHouseService.updateItemStatus(id, status);
    }
}
