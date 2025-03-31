using System;
using System.Text.Json.Serialization;

namespace Padaria.WebApi.SMS_Service.Model_SMS;

public class EnviarMensagem
{
    [JsonPropertyName("message")]
    public required Mensagem Mensagem { get; set; }
}
