using System;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Producao;

namespace Padaria.WebApi.Service.Producao;

public class ProducaoService(IProducaoRepository repository) : IProducaoService
{
    private readonly IProducaoRepository _repository = repository;
    public async Task<string> AdicionarAsync(Post_Producao_DTO producao)
    {
        
        if (producao == null) return "Producao não pode ser nula";
        if (producao.Produto == 0) return "Selecione o Produto que está sendo produzido";
        if (producao.Quantidade <= 0) return "Quantidade deve ser maior que zero";
        if (producao.DataProducao == default) return "Data não pode ser nula ou vazia";
        if (producao.Padeiro == 0) return "Informe o seu id";
        
        return await _repository.AdicionarAsync(new ProducaoModel{
            IdProduto = producao.Produto,
            Quantidade = producao.Quantidade,
            DataProducao = DateTime.SpecifyKind(producao.DataProducao, DateTimeKind.Utc),
            EstadoProducao = producao.Estado,
            IdPadeiro = producao.Padeiro
        });
    }

    public async Task<string> AtualizarAsync(Post_Producao_DTO producao)
    {
        if (producao == null) return "Producao não pode ser nula";
        if (producao.Produto == 0) return "Selecione o Produto que está sendo actualizado";
        if (producao.Quantidade <= 0) return "Quantidade deve ser maior que zero";
        if (producao.DataProducao == default) return "Data não pode ser nula ou vazia";

         return await _repository.AtualizarAsync(new ProducaoModel{
            IdProduto = producao.Produto,
            Quantidade = producao.Quantidade,
            DataProducao = producao.DataProducao,
            EstadoProducao = producao.Estado,
            IdPadeiro = producao.Padeiro
        });
    }

    public async Task<IEnumerable<Get_Producao_DTO>?> ListarProducaoPorData(DateTime data, int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarProducaoDiaria(data,skip, take, c);        
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ObterPorStatusAsync(string status, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarProducaoPorEstadoAsync(status, data, data2,skip, take, c);
    }

    public async Task<IEnumerable<Get_Producao_DTO>> ListarProducao(int skip = 0, int take = 30, CancellationToken c = default)
    {
        return await _repository.ListarProducao(skip, take, c);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _repository.RemoverAsync(id);
    }
}
