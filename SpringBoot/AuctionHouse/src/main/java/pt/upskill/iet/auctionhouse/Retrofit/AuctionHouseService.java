package pt.upskill.iet.auctionhouse.Retrofit;

import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import java.io.IOException;
import java.util.Collections;
import java.util.List;

@Service
public class AuctionHouseService {

    String API_BASE_URL = "https://localhost:7053/api/";
    private AuctionHouseAPI auctionHouseAPI;

    public AuctionHouseService() {
//        OkHttpClient unsafeHttpClient = createUnsafeOkHttpClient();

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(API_BASE_URL)
//                .client(unsafeHttpClient) // Usa cliente com SSL desabilitado
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        this.auctionHouseAPI = retrofit.create(AuctionHouseAPI.class);
    }

    public List<ItemDto> getAvailableItems() {
        try {
            Response<List<ItemDto>> response = auctionHouseAPI.getAvailableItems().execute();
            if (response.isSuccessful()) {
                return response.body();
            }
        } catch (IOException e) {
            throw new RuntimeException("Error fetching items: " + e.getMessage());
        }

        return Collections.emptyList();
    }

    public ItemDto getItemById(long id) throws NotFoundException {
        try {
            Response<ItemDto> response = auctionHouseAPI.getItemById(id).execute();
            if (response.isSuccessful()) {
                return response.body();
            } else {
                // Lançar exceção específica para item não encontrado
                throw new NotFoundException("Item não encontrado com ID: " + id);
            }
        } catch (IOException e) {
            // Capturar exceções de rede ou I/O
            throw new RuntimeException("Erro ao buscar item: " + e.getMessage());
        }
    }
}