using System;
using Padaria.Share.Pedido.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Pedido;

namespace Padaria.WebApi.Service.Pedido;

public class PedidoService(IPedidoRepository repository) : IPedidoService
{
    private readonly IPedidoRepository _repository = repository;

    public async Task<string> CreateAsync(Post_Pedido_DTO pedido)
    {
        if (pedido == null)
        {
            throw new ArgumentNullException(nameof(pedido), "Pedido nao pode ser nula");
        }
        if (pedido.Quantidade <= 0)
        {
            throw new ArgumentException("Quantidade nao pode ser menor que 1", nameof(pedido.Quantidade));
        }
        var model = new EncomendaModel
        {
            Id = pedido.Id,
            IdProduto = pedido.IdProduto,
            NumeroFactura = pedido.NumeroFactura,
            DataEncomenda = DateTime.SpecifyKind(pedido.DataEncomenda, DateTimeKind.Utc),
            Quantidade = pedido.Quantidade,
            PrecoTotal = pedido.PrecoTotal,
            EstadoPedido = "Pendente"
        };

        return await _repository.CreateAsync(model);
    }

    public async Task<string> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id nao pode ser menor que 1", nameof(id));
        }
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Get_Pedido_DTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<string> UpdateAsync(Put_Pedido_DTO pedido)
    {
        if (pedido == null)
        {
            throw new ArgumentNullException(nameof(pedido), "Pedido nao pode ser nula");
        }
        if (pedido.Quantidade <= 0)
        {
            throw new ArgumentException("Quantidade nao pode ser menor que 1", nameof(pedido.Quantidade));
        }
        var model = new EncomendaModel
        {
            Id = pedido.Id,
            Quantidade = pedido.Quantidade,
            PrecoTotal = pedido.PrecoTotal,
            EstadoPedido = pedido.EstadoEncomenda
        };

        return await _repository.UpdateAsync(model);
    }
}
