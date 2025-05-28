using System;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Producao;

namespace Padaria.WebApi.Service.Producao;

public class ProducaoService(IProducaoRepository repository) : IProducaoService
{
    private readonly IProducaoRepository _repository = repository;
    public async Task<string> AdicionarAsync(IEnumerable<Post_Producao_DTO> producao)
    {
        string response = string.Empty;
        if (producao.Count() == 0) return "Producao não pode ser vazia";
        if (producao.Any(p => p.Produto == 0)) return "Selecione o Produto que está sendo produzido";
        if (producao.Any(p => p.Quantidade <= 0)) return "Quantidade deve ser maior que zero";


        int count = producao.Count();
        foreach (var item in producao)
        {
            if (count == 0) break;
            response = await _repository.AdicionarAsync(new ProducaoModel
            {
                IdProduto = item.Produto,
                Quantidade = item.Quantidade,
                DataProducao = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.Now), DateTimeKind.Utc).ToString("yyyy-MM-dd"),
                EstadoProducao = item.Cliente == 0 ? "Pendente" : "Pendente por falta de pagamento",
                IdFuncionario = item.Cliente != 0 ? (int?)null : item.Funcionario,
                IdCliente = item.Cliente == 0 ? (int?)null : item.Cliente,
                Telefone = item.Telefone
            });
            var response2 = await _repository.AdicionarSolicitacao(item.Produto, item.Quantidade);
            count -= 1;
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase))
            {
                break;
            }
        }
        if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase))
        {
            return "Solicitacao nao foi enviada na totalidade";
        }

        return response;
    }

    public async Task<string> AtualizarAsync(Put_Producao_DTO producao)
    {
        if (producao == null) return "Producao não pode ser nula";
        if (producao.Produto == 0) return "Selecione o Produto que está sendo actualizado";
        if (producao.Quantidade <= 0) return "Quantidade deve ser maior que zero";
        if (producao.DataProducao == default) return "Data não pode ser nula ou vazia";

        return await _repository.AtualizarAsync(new ProducaoModel
        {
            IdProduto = producao.Produto,
            Quantidade = producao.Quantidade,
            DataProducao = producao.DataProducao.ToString("yyyy-MM-dd"),
            EstadoProducao = producao.Estado,
            IdFuncionario = producao.Padeiro
        });
    }

    public async Task<string> AtualizarEstadoAsync(Put_PedidoState_DTO producao)
    {
        if (producao == null) return "Producao não pode ser nula";

        return await _repository.AtualizarEstadoAsync(producao);
    }

    public async Task<IEnumerable<Get_Producao_DTO>?> ListarProducaoPorData(DateTime data, int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarProducaoDiaria(data, skip, take, c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ObterPorStatusAsync(string status, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarProducaoPorEstadoAsync(status, data, data2, skip, take, c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 60, CancellationToken c = default)
    {
        return await _repository.ListarProducao(skip, take, c);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _repository.RemoverAsync(id);
    }

    public async Task<string> AdicionarCapacidadeProducaoAsync(Post_Capacidade_Producao capacidadeProducao)
    {
        if (capacidadeProducao == null) return "Capacidade de produção não pode ser nula";
        if (capacidadeProducao.Produto == 0) return "Selecione o Produto que está sendo produzido";
        if (capacidadeProducao.QuantidadeMaxima <= 0) return "Quantidade máxima deve ser maior que zero";
        if (capacidadeProducao.DataProducao == default) return "Data não pode ser nula ou vazia";

        return await _repository.AdicionarCapacidadeProducaoAsync(new CapacidadeProducaoModel
        {
            IdProduto = capacidadeProducao.Produto,
            QuantidadeMaxima = capacidadeProducao.QuantidadeMaxima,
            Data = DateTime.SpecifyKind(Convert.ToDateTime(capacidadeProducao.DataProducao), DateTimeKind.Utc)
        });
    }

    public async Task<string> AtualizarCapacidadeProducaoAsync(Put_Capacidade_Producao capacidadeProducao)
    {
        if (capacidadeProducao == null) return "Capacidade de produção não pode ser nula";
        if (capacidadeProducao.QuantidadeMaxima <= 0) return "Quantidade máxima deve ser maior que zero";
        

        return await _repository.AtualizarCapacidadeProducaoAsync(new CapacidadeProducaoModel
        {
            IdProduto = capacidadeProducao.Id,
            QuantidadeMaxima = capacidadeProducao.QuantidadeMaxima,
        });
    }

    public async Task<bool> RemoverCapacidadeProducaoAsync(int id)
    {
        return await _repository.RemoverCapacidadeProducaoAsync(id);
    }

    public async Task<IEnumerable<Get_Capacidade_Producao>> ListarCapacidadeProducao(int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarCapacidadeProducao(skip, take, c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoCliente(int idCliente, int skip = 0, int take = 60, CancellationToken c = default)
    {
        return await _repository.ListarProducaoCliente(idCliente, skip, take, c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducaoClientePagamento(int idCliente, int skip = 0, int take = 60, CancellationToken c = default)
    {
        return await _repository.ListarProducaoClientePagamento(idCliente, skip, take, c);
    }
}
