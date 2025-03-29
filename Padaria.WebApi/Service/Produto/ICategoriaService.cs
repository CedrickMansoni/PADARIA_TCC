using System;
using Padaria.Share.Produto.DTO;

namespace Padaria.WebApi.Service.Produto;

public interface ICategoriaService
{
    Task<IEnumerable<Get_Categ_Produto_DTO>?> GetCategorias();
    Task<Get_Categ_Produto_DTO?> GetCategoria(int id);
    Task<string> PostCategoria(Post_Categ_Produto_DTO categoria);
    Task<string> PutCategoria(Post_Categ_Produto_DTO categoria);
    Task<string> DeleteCategoria(int id);
}
