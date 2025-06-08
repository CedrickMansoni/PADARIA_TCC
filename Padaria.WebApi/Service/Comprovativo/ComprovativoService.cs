using System;
using System.Data.SqlTypes;
using Padaria.Share.Comprovativos.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Comprovativo;
using Padaria.WebApi.SalvarArquivos;
using Padaria.WebApi.Service.Producao;

namespace Padaria.WebApi.Service.Comprovativo;

public class ComprovativoService(IComprovativoRepository repository, IConfiguration configuration, IArquivoService arquivoService, IProducaoService service) : IComprovativoService
{
    private readonly IComprovativoRepository repository = repository;
    private readonly IConfiguration _configuration = configuration;
    private readonly IArquivoService arquivoService = arquivoService;
    private readonly IProducaoService service = service;

    public async Task<ComprovativoResponse?> DownloadComprativo(int id)
    {
        if (id < 1) return new ComprovativoResponse{ Comprovativo = "Erro: Informe o identificador do comprovativo"};
        return await repository.DownloadComprativo(id);
    }

    public async Task<string> SalvarComprovativo(ComprovativoDTO comprovativo)
    {
        string storagePath;
        storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
        _configuration["VPSStoragePath:ProdutionStoragePath"]! :
        _configuration["StoragePath:LocalStoragePath"]!;

        if (comprovativo.Arquivo == null || comprovativo.Arquivo.Length == 0) return "O comprovativo do pagamento é obrigatória.";

        var producao = await service.ListarProducaoCliente(comprovativo.IdCliente);
        var p = producao.OrderByDescending(x => x.Id).FirstOrDefault();
        var c = new ComprovativoModels
        {
            IdCliente = comprovativo.IdCliente,
            IdProducao = p!.Id,
            Estado = "Recebido",
            DataEnvio = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            Comprovativo = comprovativo.Arquivo.FileName
        };
        var response = await repository.SalvarComprovativo(c);
        if(response.Contains("Erro")) return response;

        //storagePath += $"/Comprovativos/{comprovativo.Telefone}";

        // Garantir que o diretório existe
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }
        var documentos = new List<IFormFile>
        {
            comprovativo.Arquivo
        };
        // Salvar a imagem no caminho configurado
       foreach (var documento in documentos)
        {
            await arquivoService.SalvarArquivoAsync(documento, storagePath, $"Comprovativos/{comprovativo.Telefone}");
        }

        return response;
    }
}
