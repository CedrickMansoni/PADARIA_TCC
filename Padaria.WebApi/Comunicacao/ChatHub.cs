using System;
using Microsoft.AspNetCore.SignalR;

namespace Padaria.WebApi.Comunicacao;

public class ChatHub : Hub
{
    public async Task NotificarUsuario()
    {
        await Clients.All.SendAsync("notificacao");
    }
}
