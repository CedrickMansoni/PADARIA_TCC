using System;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Produto;

namespace Padaria.WebApi.Service.Produto;

public class CategoriaService(ICategoriaRepository repository) : ICategoriaService
{
    private readonly ICategoriaRepository _repository = repository;
    public async Task<string> DeleteCategoria(int id)
    {
        if (id == 0) return "Informe a categoria";
        var cat = await _repository.GetCategoria(id);
        if (cat == null) return "A categoria que pretende eliminar nao existe na base de dados";
        return await _repository.DeleteCategoria(cat);
    }

    public async Task<Get_Categ_Produto_DTO?> GetCategoria(int id)
    {
        var cat = await _repository.GetCategoria(id);
        if (cat == null) return null;
        return new Get_Categ_Produto_DTO
        {
            Id = cat.Id,
            Categoria = cat.Descricao
        };
    }

    public async Task<IEnumerable<Get_Categ_Produto_DTO>?> GetCategorias()
    {
        var categorias = await _repository.GetCategorias();
        return categorias.Select(c => new Get_Categ_Produto_DTO
        {
            Id = c.Id,
            Categoria = c.Descricao
        });
    }

    public async Task<string> PostCategoria(Post_Categ_Produto_DTO categoria)
    {
        if (string.IsNullOrEmpty(categoria.Categoria)) return "Informe a categoria";
        var cat = await _repository.GetCategoria(categoria.Categoria);
        if (cat != null) return "A categoria que pretende adicionar ja existe na base de dados";
        return await _repository.PostCategoria(new CategoriaProdutoModel
        {
            Descricao = categoria.Categoria
        });
    }

    public async Task<string> PutCategoria(Post_Categ_Produto_DTO categoria)
    {
        if (string.IsNullOrEmpty(categoria.Categoria)) return "Informe a categoria";
        var cat = await _repository.GetCategoria(categoria.Categoria);
        if (cat == null) return "A categoria que pretende atualizar nao existe na base de dados";
        return await _repository.PutCategoria(new CategoriaProdutoModel
        {
            Id = cat.Id,
            Descricao = categoria.Categoria
        });
    }
}
