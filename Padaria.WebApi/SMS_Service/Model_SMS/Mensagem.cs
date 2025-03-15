using System;
using System.Text.Json.Serialization;

namespace Padaria.WebApi.SMS_Service.Model_SMS;

public class Mensagem
{
    [JsonPropertyName("accao")]
    public string Accao { get; set; } = "enviar_sms";

    [JsonPropertyName("chave_entidade")]
    public string ChaveEntidade { get; set; } = "5H6gd62Se5Es5Ke6f6T5FcYd2sJ";  

    [JsonPropertyName("destinatario")]
    public string Destinatario { get; set; } = string.Empty;

    [JsonPropertyName("descricao_sms")]
    public string DescricaoSms { get; set; } = string.Empty;
}
