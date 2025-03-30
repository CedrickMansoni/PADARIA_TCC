using System;
using Padaria.Share.Producao.DTO;

namespace Padaria.WebApi.Service.Producao;

public interface IProducaoService
{
    Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>?> ListarProducaoPorData(DateTime data,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ObterPorStatusAsync(string status, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default);
    Task<string> AdicionarAsync(Post_Producao_DTO producao);
    Task<string> AtualizarAsync(Post_Producao_DTO producao);
    Task<bool> RemoverAsync(int id);
}
