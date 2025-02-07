package pt.upskill.iet.auctionhouse.Retrofit.Service;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import org.springframework.stereotype.Service;
import pt.upskill.iet.auctionhouse.Dtos.SaleDto;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import pt.upskill.iet.auctionhouse.Retrofit.Interfaces.AuctionHouseApiInterface;
import pt.upskill.iet.auctionhouse.Retrofit.StatusDtoDeserializer;
import pt.upskill.iet.auctionhouse.Retrofit.StatusDtoSerializer;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import java.io.IOException;
import java.security.cert.X509Certificate;

@Service
public class AHSaleService {

    private static final String API_BASE_URL = "https://localhost:7053/api/";
    private AuctionHouseApiInterface auctionHouseApiInterface;

    public AHSaleService() {
        // Cria um cliente OkHttp que ignora SSL e adiciona o token no cabeçalho
        OkHttpClient unsafeHttpClient = createUnsafeOkHttpClient("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBZG1pbiIsImp0aSI6ImZjZTViYzc5LTdjYzAtNDMxMS05ZDUwLTRkZDA2M2QwYTc0OSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzQ0MDM2OTIwLCJpc3MiOiJBSElzc3VlciJ9.hgmOjMo6fvmjOFQOzj6d-kLv4Q0hCiwjsgOFCafnGS4");

        // Configura o Gson com o deserializer personalizado
        Gson gson = new GsonBuilder()
                .registerTypeAdapter(StatusDto.class, new StatusDtoDeserializer())
                .registerTypeAdapter(StatusDto.class, new StatusDtoSerializer())
                .create();

        // Configura o Retrofit
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(API_BASE_URL)
                .client(unsafeHttpClient) // Usa o cliente que ignora SSL
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();

        this.auctionHouseApiInterface = retrofit.create(AuctionHouseApiInterface.class);
    }


    public SaleDto addSale(double price, long itemId) {
        SaleDto saleDto = new SaleDto();
        saleDto.setPrice(price);
        saleDto.setItemId(itemId);

        try {
            Response<SaleDto> response = auctionHouseApiInterface.addSale(saleDto).execute();
            if (response.isSuccessful()) {
                return response.body();
            } else {
                // Logar o erro
                System.err.println("Erro ao adicionar venda: " + response.code() + " - " + response.message());
                if (response.errorBody() != null) {
                    System.err.println("Detalhes do erro: " + response.errorBody().string());
                }

            }
        } catch (IOException e) {
            throw new RuntimeException("Erro ao criar venda: " + e.getMessage());
        }
        return null;
    }

    private OkHttpClient createUnsafeOkHttpClient(String token) {
        try {
            TrustManager[] trustAllCertificates = new TrustManager[]{
                    new X509TrustManager() {
                        @Override
                        public void checkClientTrusted(X509Certificate[] chain, String authType) {}

                        @Override
                        public void checkServerTrusted(X509Certificate[] chain, String authType) {}

                        @Override
                        public X509Certificate[] getAcceptedIssuers() {
                            return new X509Certificate[0];
                        }
                    }
            };

            SSLContext sslContext = SSLContext.getInstance("TLS");
            sslContext.init(null, trustAllCertificates, new java.security.SecureRandom());

            SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();

            return new OkHttpClient.Builder()
                    .sslSocketFactory(sslSocketFactory, (X509TrustManager) trustAllCertificates[0])
                    .hostnameVerifier((hostname, session) -> true) // Ignora verificação do hostname
                    .addInterceptor(chain -> {
                        Request original = chain.request();
                        Request.Builder requestBuilder = original.newBuilder()
                                .header("Authorization", "Bearer " + token) // Adiciona o token no cabeçalho
                                .method(original.method(), original.body());
                        return chain.proceed(requestBuilder.build());
                    })
                    .build();
        } catch (Exception e) {
            throw new RuntimeException("Falha ao criar um cliente HTTP inseguro", e);
        }
    }
}
