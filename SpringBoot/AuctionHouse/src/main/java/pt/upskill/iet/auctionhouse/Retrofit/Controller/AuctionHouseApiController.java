package pt.upskill.iet.auctionhouse.Retrofit.Controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import pt.upskill.iet.auctionhouse.Retrofit.Service.AHItemService;

import java.util.List;

@RestController
@RequestMapping("api/v1/auction/")
public class AuctionHouseApiController {

    @Autowired
    AHItemService AHItemService;

    @GetMapping("items")
    public List<ItemDto> getItems() {
        return AHItemService.getItems();
    }

    @GetMapping("/available-items")
    public List<ItemDto> getAvailableItems() {
        return AHItemService.getAvailableItems();
    }

    @GetMapping("/item-by-id/{id}")
    public ItemDto getItemById(@PathVariable long id) throws NotFoundException {
        return AHItemService.getItemById(id);
    }

//    @PatchMapping("/update-item-status/id/{id}/status/{status}")
//    public ResponseEntity<Void> updateItemStatus(
//            @PathVariable long id,
//            @PathVariable StatusDto status
//    ) {
//        try {
//            auctionHouseService.updateItemStatus(id, status);
//            return ResponseEntity.ok().build();
//        } catch (NotFoundException e) {
//            return ResponseEntity.notFound().build();
//        } catch (Exception e) {
//            return ResponseEntity.internalServerError().build();
//        }
//    }

//    @PostMapping("/add-sale")
//    public SaleDto addSale(@RequestBody SaleDto saleDto) {
//        return auctionHouseService.addSale(saleDto.getPrice(), saleDto.getItemId());
//    }
}
