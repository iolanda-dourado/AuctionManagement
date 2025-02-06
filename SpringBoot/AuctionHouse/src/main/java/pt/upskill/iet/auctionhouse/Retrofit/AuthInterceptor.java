package pt.upskill.iet.auctionhouse.Retrofit;

import okhttp3.Interceptor;
import okhttp3.Request;
import okhttp3.Response;
import java.io.IOException;

public class AuthInterceptor implements Interceptor {

    private String authToken;

    public AuthInterceptor(String token) {
        this.authToken = token;
    }

    @Override
    public Response intercept(Chain chain) throws IOException {
        Request original = chain.request();

        // Adiciona o token ao cabeçalho de autorização
        Request.Builder requestBuilder = original.newBuilder()
                .header("Authorization", "Bearer " + authToken)
                .method(original.method(), original.body());

        Request request = requestBuilder.build();
        return chain.proceed(request);
    }
}