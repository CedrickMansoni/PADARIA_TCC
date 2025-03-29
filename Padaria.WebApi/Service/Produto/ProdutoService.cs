using System;
using System.Linq.Expressions;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Repository.Produto;
using Padaria.WebApi.SalvarArquivos;

namespace Padaria.WebApi.Service.Produto;

public class ProdutoService(IProdutoRepository repository, ICategoriaService service, IConfiguration configuration, IArquivoService arquivoService) : IProdutoService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IProdutoRepository _repository = repository;
    private readonly ICategoriaService _service = service;
    private readonly IArquivoService arquivoService = arquivoService;
    public async Task<string> AtualizarProdutoAsync(Post_Produto_DTO produto)
    {
        if (produto.Id == 0) return "Id do produto não pode ser 0";
        if (string.IsNullOrEmpty(produto.Nome)) return "Nome do produto não pode ser nulo";
        if (produto.Preco == 0) return "Preço do produto não pode ser 0";
        if (string.IsNullOrEmpty(produto.Descricao)) return "Descrição do produto não pode ser nula";
        if (produto.IdCategoria == 0) return "Id da categoria do produto não pode ser 0";
        if (produto.Imagem.Length == 0) return "Imagem do produto não pode ser nula";
        if (produto.IdFuncionario == 0) return "Id do funcionário do produto não pode ser 0";
        return await _repository.AtualizarProdutoAsync(produto);
    }

    public async Task<string> CadastrarProdutoAsync(Post_Produto_DTO produto)
    {
        string storagePath;
        storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
        _configuration["VPSStoragePath:ProdutionStoragePath"]! :
        _configuration["StoragePath:LocalStoragePath"]!;

        if (string.IsNullOrEmpty(produto.Nome)) return "Nome do produto não pode ser nulo";
        if (produto.Preco == 0) return "Preço do produto não pode ser 0";
        if (string.IsNullOrEmpty(produto.Descricao)) return "Descrição do produto não pode ser nula";
        if (produto.IdCategoria == 0) return "Id da categoria do produto não pode ser 0";
        if (produto.Imagem.Length == 0) return "Imagem do produto não pode ser nula";
        if (produto.IdFuncionario == 0) return "Id do funcionário do produto não pode ser 0";
        if (produto.Imagem == null || produto.Imagem.Length == 0) return "A imagem do produto é obrigatória.";
        if(await _repository.PesquisarProdutoByName(produto.Nome) != null) return "Produto já cadastrado";

        var response = await _repository.CadastrarProdutoAsync(produto);
        if(response.Contains("Erro")) return response;

        // Garantir que o diretório existe
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }
        var documentos = new List<IFormFile>();
        documentos.Add(produto.Imagem);
        // Salvar a imagem no caminho configurado
       foreach (var documento in documentos)
        {
            await arquivoService.SalvarArquivoAsync(documento, storagePath, "produtos");
        }

        return response;
    }

    public async Task<string> DeletarProdutoAsync(int id)
    {
        if (id == 0) return "Id do produto não pode ser 0";
        var produto = await _repository.PesquisarProduto(id);
        if (produto == null) return "Produto não encontrado";

        return await _repository.DeletarProdutoAsync(produto);
    }

    public async Task<Get_Produto_DTO?> PesquisarProdutoAsync(int id)
    {
        return await _repository.PesquisarProdutoAsync(id);
    }

    public async Task<IEnumerable<Get_Produto_DTO>?> PesquisarProdutosAsync()
    {
        return await _repository.PesquisarProdutosAsync();
    }
}
