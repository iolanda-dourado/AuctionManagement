package pt.upskill.iet.auctionhouse.Retrofit;

import okhttp3.OkHttpClient;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.ItemDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Exceptions.NotFoundException;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import java.io.IOException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.Collections;
import java.util.List;

@Service
public class AuctionHouseService {

    String API_BASE_URL = "https://localhost:7053/api/";
    private AuctionHouseAPI auctionHouseAPI;

    public AuctionHouseService() {
        OkHttpClient unsafeHttpClient = createUnsafeOkHttpClient();

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(API_BASE_URL)
                .client(unsafeHttpClient) // Usa cliente com SSL desabilitado
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


    public StatusDto updateItemStatus(long id, StatusDto status) throws NotFoundException {
        try {
            Response<StatusDto> response = auctionHouseAPI.updateItemStatus(id, status).execute();
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

    private OkHttpClient createUnsafeOkHttpClient() {
        try {
            // Cria um trustManager que não valida certificados
            TrustManager[] trustAllCerts = new TrustManager[]{
                    new X509TrustManager() {
                        public void checkClientTrusted(X509Certificate[] chain, String authType) throws CertificateException {
                        }

                        public void checkServerTrusted(X509Certificate[] chain, String authType) throws CertificateException {
                        }

                        public X509Certificate[] getAcceptedIssuers() {
                            return new X509Certificate[0];
                        }
                    }
            };

            // Cria um SSL context com o trustManager
            SSLContext sslContext = SSLContext.getInstance("SSL");
            sslContext.init(null, trustAllCerts, new java.security.SecureRandom());
            SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();

            // Configura o cliente OkHttp para ignorar SSL
            OkHttpClient.Builder builder = new OkHttpClient.Builder();
            builder.sslSocketFactory(sslSocketFactory, (X509TrustManager) trustAllCerts[0]);
            builder.hostnameVerifier((hostname, session) -> true);

            return builder.build();
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}