package pt.upskill.iet.auctionhouse.Retrofit;

import pt.upskill.iet.auctionhouse.Dtos.StatusDto;
import com.google.gson.*;

import java.lang.reflect.Type;

public class StatusDtoDeserializer implements JsonDeserializer<StatusDto> {

    @Override
    public StatusDto deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        int code = json.getAsInt();
        return StatusDto.values()[code]; // Converte 0 -> Available, 1 -> AtAuction, 2 -> Sold
    }
}
