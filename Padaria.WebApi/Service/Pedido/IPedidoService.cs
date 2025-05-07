using System;
using Padaria.Share.Pedido.DTO;

namespace Padaria.WebApi.Service.Pedido;

public interface IPedidoService
{
    Task<IEnumerable<Get_Pedido_DTO>> GetAllAsync();
    Task<string> CreateAsync(Post_Pedido_DTO pedido);
    Task<string> UpdateAsync(Put_Pedido_DTO pedido);
    Task<string> DeleteAsync(int id);
}
