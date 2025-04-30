using System;
using Padaria.Share.Producao.DTO;

namespace Padaria.WebApi.Service.Producao;

public interface IProducaoService
{
    Task<string> AdicionarCapacidadeProducaoAsync(Post_Capacidade_Producao capacidadeProducao);
    Task<string> AtualizarCapacidadeProducaoAsync(Put_Capacidade_Producao capacidadeProducao);
    Task<bool> RemoverCapacidadeProducaoAsync(int id);
    Task<IEnumerable<Get_Capacidade_Producao>> ListarCapacidadeProducao(int skip = 0, int take = 30, CancellationToken c = default);
    // «««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««

    Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>?> ListarProducaoPorData(DateTime data,int skip = 0, int take = 30, CancellationToken c = default);
    Task<IEnumerable<Get_Producao_DTO>> ObterPorStatusAsync(string status, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default);
    Task<string> AdicionarAsync(IEnumerable<Post_Producao_DTO> producao);
    Task<string> AtualizarAsync(Put_Producao_DTO producao);
    Task<bool> RemoverAsync(int id);
}
