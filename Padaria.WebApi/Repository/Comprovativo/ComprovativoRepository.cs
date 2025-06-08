using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Comprovativos.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Comprovativo;

public class ComprovativoRepository(AppDataContext context) : IComprovativoRepository
{
    private readonly AppDataContext context = context;

    public async Task<ComprovativoResponse?> DownloadComprativo(int id)
    {
        var c = await context.TabelaComprovativo.FirstOrDefaultAsync(x => x.IdProducao == id);
        if (c is null) return new ComprovativoResponse { Comprovativo = "Erro: o documento nao existe na base de dados" };

        var telefone = await context.TabelaTelefoneModel.FirstOrDefaultAsync(x => x.IdCliente == c.IdCliente);
        if (telefone is null) return null;
        return new ComprovativoResponse
        {
            Comprovativo = c.Comprovativo,
            Telefone = telefone.NumeroTelefone
        };
    }

    public async Task<string> SalvarComprovativo(ComprovativoModels comprovativo)
    {
        var c = await context.TabelaComprovativo.FirstOrDefaultAsync(x => x.Comprovativo == comprovativo.Comprovativo);
        if (c is not null) return "Este comprovativo ja foi submetido.";
        await context.TabelaComprovativo.AddAsync(comprovativo);
        return await context.SaveChangesAsync() > 0 ?
            "Comprovativo submetido com sucesso" :
            "Erro: Nao foi possivel submeter o comprovativo"; 
    }
}
