using System;
using Padaria.Share.Produto.DTO;

namespace Padaria.WebApi.Service.Produto;

public interface IProdutoService
{
    Task<string> CadastrarProdutoAsync(Post_Produto_DTO produto);
    Task<string> AtualizarProdutoAsync(Post_Produto_DTO produto);
    Task<string> DeletarProdutoAsync(int id);
    Task<Get_Produto_DTO?> PesquisarProdutoAsync(int id);
    Task<IEnumerable<Get_Produto_DTO>?> PesquisarProdutosAsync();
}
