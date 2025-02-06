package pt.upskill.iet.auctionhouse.Services.Implementation.ConsumeAPI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
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

    @GetMapping("items")
    public List<ItemDto> getItems() {
        return auctionHouseService.getItems();
    }

    @GetMapping("/available-items")
    public List<ItemDto> getAvailableItems() {
        return auctionHouseService.getAvailableItems();
    }

    @GetMapping("/item-by-id/{id}")
    public ItemDto getItemById(@PathVariable long id) throws NotFoundException {
        return auctionHouseService.getItemById(id);
    }

    @PatchMapping("/update-item-status/id/{id}/status/{status}")
    public ResponseEntity<Void> updateItemStatus(
            @PathVariable long id,
            @PathVariable StatusDto status
    ) {
        try {
            auctionHouseService.updateItemStatus(id, status);
            return ResponseEntity.ok().build(); // Retorna 200 OK sem corpo
        } catch (NotFoundException e) {
            return ResponseEntity.notFound().build(); // Retorna 404 Not Found
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build(); // Retorna 500
        }
    }
}
