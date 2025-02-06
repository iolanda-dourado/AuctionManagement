package pt.upskill.iet.auctionhouse.Retrofit.Controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.SaleDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AuctionHouseService;
import retrofit2.Call;
import retrofit2.http.POST;

import java.util.List;

@RestController
@RequestMapping("api/v1/auction/")
public class AuctionHouseApiController {

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
            return ResponseEntity.ok().build();
        } catch (NotFoundException e) {
            return ResponseEntity.notFound().build();
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    @PostMapping("/add-sale")
    public SaleDto addSale(@RequestBody SaleDto saleDto) {
        return auctionHouseService.addSale(saleDto.getPrice(), saleDto.getItemId());
    }
}
