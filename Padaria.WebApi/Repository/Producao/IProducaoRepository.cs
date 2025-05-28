using System;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Producao;

public interface IProducaoRepository
{
    Task<string> AdicionarCapacidadeProducaoAsync(CapacidadeProducaoModel capacidadeProducao);
    Task<string> AtualizarCapacidadeProducaoAsync(CapacidadeProducaoModel capacidadeProducao);
    Task<bool> RemoverCapacidadeProducaoAsync(int id);
    Task<IEnumerable<Get_Capacidade_Producao>> ListarCapacidadeProducao(int skip = 0, int take = 30, CancellationToken c = default);
    // «««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««
    Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoCliente(int idCliente, int skip = 0, int take = 60, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoClientePagamento(int idCliente, int skip = 0, int take = 60, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoDiaria(DateTime data,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoParametro(DateTime data, DateTime data2,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoPorEstadoAsync(string status, DateTime data, DateTime data2,int skip = 0, int take = 30, CancellationToken c = default);
    Task<string> AdicionarAsync(ProducaoModel producao);
    Task<string> AtualizarAsync(ProducaoModel producao);
    Task<string> AtualizarEstadoAsync(Put_PedidoState_DTO producao);
    Task<bool> RemoverAsync(int id);
    Task<string> AdicionarSolicitacao(int id, int q);
}
