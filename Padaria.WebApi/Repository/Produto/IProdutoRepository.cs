using System;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Produto;

public interface IProdutoRepository
{
    Task<string> CadastrarProdutoAsync(Post_Produto_DTO produto);
    Task<string> AtualizarProdutoAsync(Post_Produto_DTO produto);
    Task<string> DeletarProdutoAsync(ProdutoModel produto);
    Task<Get_Produto_DTO?> PesquisarProdutoAsync(int id);
    Task<ProdutoModel?> PesquisarProduto(int id);
    Task<ProdutoModel?> PesquisarProdutoByName(string nome);
    Task<IEnumerable<Get_Produto_DTO>> PesquisarProdutosAsync();
}
