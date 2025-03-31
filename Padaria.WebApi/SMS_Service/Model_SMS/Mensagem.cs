using System;
using System.Text.Json.Serialization;

namespace Padaria.WebApi.SMS_Service.Model_SMS;

public class Mensagem
{
    [JsonPropertyName("api_key_app")]
    public string ApiKeyApp { get; set; } = "prdd2daddfaab1ce424ecaa405340";

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("message_body")]
    public string MessageBody { get; set; } = string.Empty;
}
