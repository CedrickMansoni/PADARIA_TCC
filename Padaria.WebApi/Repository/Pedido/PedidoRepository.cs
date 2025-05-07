using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Pedido.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Pedido;

public class PedidoRepository(AppDataContext context) : IPedidoRepository
{
    private readonly AppDataContext _context  = context;

    public async Task<string> CreateAsync(EncomendaModel pedido)
    {
        await _context.TabelaEncomendaModel.AddAsync(pedido);
        return await _context.SaveChangesAsync() > 0 ? "Encomenda criada com sucesso!" : "Erro ao criar encomenda!";
    }

    public async Task<string> DeleteAsync(int id)
    {
        var pedido = _context.TabelaEncomendaModel.Find(id);
        if (pedido == null)
        {
            return "Encomenda não encontrada!";
        }
        _context.TabelaEncomendaModel.Remove(pedido);
        return await _context.SaveChangesAsync() > 0 ? "Encomenda eliminada com sucesso!" : "Erro ao eliminar encomenda!";
    }

    public async Task<IEnumerable<Get_Pedido_DTO>> GetAllAsync()
    {
         var dataAtual = DateTime.UtcNow.Date; // ou DateTime.Now.Date dependendo do fuso
        var hoje = DateTime.SpecifyKind(Convert.ToDateTime(dataAtual), DateTimeKind.Utc);

        var pedidos = from p in _context.TabelaEncomendaModel
            join pr in _context.TabelaProdutoModel on p.IdProduto equals pr.Id
            join f in _context.TabelaFacturaModel on p.NumeroFactura equals f.NumeroFactura
            join c in _context.TabelaClienteModel on f.IdCliente equals c.Id
            where p.DataEncomenda.Date == hoje
            select new Get_Pedido_DTO
            {
                Id = p.Id,
                IdProduto = p.IdProduto,
                NomeProduto = pr.Nome,
                NumeroFactura = p.NumeroFactura,
                DataEncomenda = p.DataEncomenda,
                Quantidade = p.Quantidade,
                PrecoTotal = p.PrecoTotal,
                EstadoPedido = p.EstadoPedido
            };
        return await pedidos.ToListAsync();
    }

 
    public async Task<string> UpdateAsync(EncomendaModel pedido)
    {
        var p = _context.TabelaEncomendaModel.Find(pedido.Id);
        if (p == null)
        {
            return "Encomenda não encontrada!";
        }
        p.IdProduto = pedido.IdProduto;
        p.NumeroFactura = pedido.NumeroFactura;
        p.DataEncomenda = pedido.DataEncomenda;
        p.Quantidade = pedido.Quantidade;
        p.PrecoTotal = pedido.PrecoTotal;
        p.EstadoPedido = pedido.EstadoPedido;

        return await _context.SaveChangesAsync() > 0 ? "Encomenda actualizada com sucesso!" : "Erro ao actualizar encomenda!";
    }
}
