using System;
using Padaria.WebApi.SMS_Service.Model_SMS;

namespace Padaria.WebApi.SMS_Service.Service;

public interface ISMS_enviar
{
     Task<MensagemResponse?> EnviarSMS(EnviarMensagem mensagem);
}
