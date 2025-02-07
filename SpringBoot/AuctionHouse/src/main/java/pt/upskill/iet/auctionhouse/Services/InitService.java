package pt.upskill.iet.auctionhouse.Services;

import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import pt.upskill.iet.auctionhouse.Services.Implementation.SaleService;

@Component

public class InitService {
    private final SaleService saleService;

    @Autowired
    public InitService(SaleService saleService) {
        this.saleService = saleService;
    }

    @PostConstruct
    public void init() {
        try {
            saleService.addSaleWithHighestBid();
        } catch (Exception e) {
            System.err.println("Error adding sales in initialization: " + e.getMessage());
        }
    }
}
