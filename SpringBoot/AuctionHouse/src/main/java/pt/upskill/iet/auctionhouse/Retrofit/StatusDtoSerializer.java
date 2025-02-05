package pt.upskill.iet.auctionhouse.Retrofit;

import com.google.gson.JsonElement;
import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;
import pt.upskill.iet.auctionhouse.Dtos.StatusDto;

import java.lang.reflect.Type;

public class StatusDtoSerializer implements JsonSerializer<StatusDto> {

    @Override
    public JsonElement serialize(StatusDto src, Type typeOfSrc, JsonSerializationContext context) {
        return new JsonPrimitive(src.ordinal()); // Converte para 0, 1, 2...
    }
}
