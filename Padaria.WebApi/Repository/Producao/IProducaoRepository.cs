using System;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Producao;

public interface IProducaoRepository
{
    Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoDiaria(DateTime data,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoParametro(DateTime data, DateTime data2,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ListarProducaoPorEstadoAsync(string status, DateTime data, DateTime data2,int skip = 0, int take = 30, CancellationToken c = default);
    Task<string> AdicionarAsync(ProducaoModel producao);
    Task<string> AtualizarAsync(ProducaoModel producao);
    Task<bool> RemoverAsync(int id);
}
