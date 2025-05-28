using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Producao;

public class ProducaoRepository(AppDataContext context) : IProducaoRepository
{
    private readonly AppDataContext _context = context;

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default)
    {
        var hoje = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.UtcNow), DateTimeKind.Utc); // ou DateTime.Now.Date dependendo do fuso
        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join cliente in _context.TabelaClienteModel on producao.IdCliente equals cliente.Id
                    join telefone in _context.TabelaTelefoneModel on cliente.Id equals telefone.IdCliente 
                     where producao.DataProducao == hoje.ToString("yyyy-MM-dd")
                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        Produto = produto.Nome,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),
                        ClienteNome = cliente.Nome,
                        Telefone = telefone.NumeroTelefone,
                        Preco = produto.Preco,
                        PrecoTotal = producao.Quantidade * produto.Preco,
                        Status = !producao.EstadoProducao.Contains("pagamento") ? false : true
                    };
        return await query.Skip(skip).Take(take).OrderByDescending(i => i.Id).ToListAsync(c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoDiaria(DateTime data, int skip = 0, int take = 30, CancellationToken c = default)
    {
        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join padeiro in _context.TabelaFuncionarioModel on producao.IdFuncionario equals padeiro.Id
                    where
                    producao.DataProducao == data.ToString("yyyy-MM-dd") &&
                    producao.EstadoProducao != "Pendente por falta de pagamento"
                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        IdProduto = produto.Id,
                        Produto = produto.Nome,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),
                        Padeiro = padeiro.NomeCompleto
                    };
        return await query.Skip(skip).Take(take).ToListAsync(c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoParametro(DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default)
    {
        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join padeiro in _context.TabelaFuncionarioModel on producao.IdFuncionario equals padeiro.Id
                    where
                    producao.DataProducao == data.ToString("yyyy-MM-dd") &&
                    producao.DataProducao == data2.ToString("yyyy-MM-dd")
                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        Produto = produto.Nome,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),
                        Padeiro = padeiro.NomeCompleto
                    };
        return await query.Skip(skip).Take(take).ToListAsync(c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoPorEstadoAsync(string status, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default)
    {
        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join padeiro in _context.TabelaFuncionarioModel on producao.IdFuncionario equals padeiro.Id
                    where producao.EstadoProducao == status &&
                    producao.DataProducao == data.ToString("yyyy-MM-dd") &&
                    producao.DataProducao == data2.ToString("yyyy-MM-dd")
                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        Produto = produto.Nome,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),
                        Padeiro = padeiro.NomeCompleto
                    };
        return await query.Skip(skip).Take(take).ToListAsync(c);
    }

    public async Task<string> AdicionarAsync(ProducaoModel producao)
    {
        var q = await _context.TabelaCapacidade.OrderByDescending(i => i.Id).FirstOrDefaultAsync(x => x.IdProduto == producao.IdProduto);
        if (q is not null)
        {
            if (q.QuantidadeMaxima < producao.Quantidade)
            {
                return "A quantidade solicitada nao pode ser inferior a quantidade maxima";
            }
        }

        await _context.TabelaProducaoModel.AddAsync(producao);
        return await _context.SaveChangesAsync() > 0 ?
            "Produção adicionada com sucesso!" :
            "Erro ao adicionar produção.";
    }

    public async Task<string> AtualizarAsync(ProducaoModel producao)
    {
        var p = await _context.TabelaProducaoModel.FindAsync(producao.Id);
        if (p == null)
            return "Produção não encontrada.";
        p.IdProduto = producao.IdProduto;
        p.Quantidade = producao.Quantidade;
        p.DataProducao = producao.DataProducao;
        p.EstadoProducao = producao.EstadoProducao;
        _context.TabelaProducaoModel.Update(p);
        return await _context.SaveChangesAsync() > 0 ?
            "Produção actualizada com sucesso!" :
            "Erro ao actualizar produção.";
    }

    public async Task<string> AtualizarEstadoAsync(Put_PedidoState_DTO producao)
    {
        var p = await _context.TabelaProducaoModel.FindAsync(producao.IdPedido);
        if (p == null)
            return "Produção não encontrada.";
        p.EstadoProducao = producao.Estado;
        return await _context.SaveChangesAsync() > 0 ?
            "Produção actualizada com sucesso!" :
            "Erro ao actualizar produção.";
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var producao = await _context.TabelaProducaoModel.FindAsync(id);
        if (producao == null)
            return false;

        _context.TabelaProducaoModel.Remove(producao);
        return true;
    }

    public async Task<string> AdicionarCapacidadeProducaoAsync(CapacidadeProducaoModel capacidadeProducao)
    {
        await _context.TabelaCapacidade.AddAsync(capacidadeProducao);
        return await _context.SaveChangesAsync() > 0 ?
            "Capacidade de produção adicionada com sucesso!" :
            "Erro ao adicionar capacidade de produção.";
    }

    public async Task<string> AtualizarCapacidadeProducaoAsync(CapacidadeProducaoModel capacidadeProducao)
    {
        var p = await _context.TabelaCapacidade.FindAsync(capacidadeProducao.Id);
        if (p == null)
            return "Capacidade de produção não encontrada.";
        p.IdProduto = capacidadeProducao.IdProduto;
        p.QuantidadeMaxima += capacidadeProducao.QuantidadeMaxima;
        return await _context.SaveChangesAsync() > 0 ?
            "Capacidade de produção actualizada com sucesso!" :
            "Erro ao actualizar capacidade de produção.";
    }

    public async Task<bool> RemoverCapacidadeProducaoAsync(int id)
    {
        var capacidade = await _context.TabelaCapacidade.FindAsync(id);
        if (capacidade == null)
            return false;

        _context.TabelaCapacidade.Remove(capacidade);
        return true;
    }

    public async Task<IEnumerable<Get_Capacidade_Producao>> ListarCapacidadeProducao(int skip = 0, int take = 30, CancellationToken c = default)
    {
        var dataAtual = DateTime.UtcNow.Date; // ou DateTime.Now.Date dependendo do fuso
        var hoje = DateTime.SpecifyKind(Convert.ToDateTime(dataAtual), DateTimeKind.Utc);
        var query = from capacidade in _context.TabelaCapacidade
                    join produto in _context.TabelaProdutoModel on capacidade.IdProduto equals produto.Id
                    where
                    capacidade.Data.Date == hoje
                    select new Get_Capacidade_Producao
                    {
                        Id = capacidade.Id,
                        IdProduto = produto.Id,
                        Produto = produto.Nome,
                        QuantidadeMaxima = capacidade.QuantidadeMaxima,
                        QuantidadeSolicitada = capacidade.QuantidadeSolicitada,
                        Data = capacidade.Data
                    };
        return await query.Skip(skip).Take(take).ToListAsync(c);
    }

    public async Task<string> AdicionarSolicitacao(int id, int q)
    {
        var p = await _context.TabelaCapacidade.OrderByDescending(i => i.Id).FirstOrDefaultAsync(i => i.IdProduto == id);
        if (p == null)
            return "Capacidade de produção não encontrada.";
        p.QuantidadeMaxima = p.QuantidadeMaxima - q;
        p.QuantidadeSolicitada = p.QuantidadeSolicitada + q;
        return await _context.SaveChangesAsync() > 0 ?
            "Capacidade de produção actualizada com sucesso!" :
            "Erro ao actualizar capacidade de produção.";
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoCliente(int idCliente, int skip = 0, int take = 60, CancellationToken c = default)
    {
        var hoje = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.UtcNow), DateTimeKind.Utc); // ou DateTime.Now.Date dependendo do fuso

        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join cliente in _context.TabelaClienteModel on producao.IdCliente equals cliente.Id
                    where cliente.Id == idCliente &&
                    producao.DataProducao == hoje.ToString("yyyy-MM-dd") &&
                    producao.EstadoProducao.Contains("Pendente") || producao.EstadoProducao.Contains("Em") || producao.EstadoProducao.Contains("Concluído")
                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        IdProduto = produto.Id,
                        Produto = produto.Nome,
                        Preco = produto.Preco,
                        PrecoTotal = produto.Preco * producao.Quantidade,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),

                    };
        return await query.Skip(skip).Take(take).ToListAsync(c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoClientePagamento(int idCliente, int skip = 0, int take = 60, CancellationToken c = default)
    {
        var dataAtual = DateTime.Now.Date.ToString("yyyy-MM-dd"); // ou DateTime.Now.Date dependendo do fuso
        var query = from producao in _context.TabelaProducaoModel
                    join produto in _context.TabelaProdutoModel on producao.IdProduto equals produto.Id
                    join cliente in _context.TabelaClienteModel on producao.IdCliente equals cliente.Id

                    where
                    cliente.Id == idCliente &&
                    producao.DataProducao == dataAtual &&
                    producao.EstadoProducao != "Pendente por falta de pagamento"

                    select new Get_Producao_DTO
                    {
                        Id = producao.Id,
                        IdProduto = produto.Id,
                        Produto = produto.Nome,
                        Preco = produto.Preco,
                        PrecoTotal = produto.Preco * producao.Quantidade,
                        Quantidade = producao.Quantidade,
                        Estado = producao.EstadoProducao,
                        DataProducao = Convert.ToDateTime(producao.DataProducao),

                    };

        return await query.Skip(skip).Take(take).ToListAsync(c);

    }
}
