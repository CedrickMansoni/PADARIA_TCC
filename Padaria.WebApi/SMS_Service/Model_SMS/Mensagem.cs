using System;
using System.Text.Json.Serialization;

namespace Padaria.WebApi.SMS_Service.Model_SMS;

public class Mensagem
{
    [JsonPropertyName("api_key_app")]
    public string ApiKeyApp { get; set; } = "prdcbd2d2f67db368f04b70e649fe";

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("message_body")]
    public string MessageBody { get; set; } = string.Empty;
}
