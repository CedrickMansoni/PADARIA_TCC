using System;
using Padaria.Share.Pedido.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Pedido;

public interface IPedidoRepository
{
    Task<IEnumerable<Get_Pedido_DTO>> GetAllAsync();
    Task<string> CreateAsync(EncomendaModel pedido);
    Task<string> UpdateAsync(EncomendaModel pedido);
    Task<string> DeleteAsync(int id);
}
