using System;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Produto;

public interface ICategoriaRepository
{
    Task<IEnumerable<CategoriaProdutoModel>> GetCategorias();
    Task<CategoriaProdutoModel?> GetCategoria(int id);
    Task<CategoriaProdutoModel?> GetCategoria(string categoria);
    Task<string> PostCategoria(CategoriaProdutoModel categoria);
    Task<string> PutCategoria(CategoriaProdutoModel categoria);
    Task<string> DeleteCategoria(CategoriaProdutoModel categoria);
}
