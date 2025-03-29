using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Produto;

public class CategoriaRepository(AppDataContext context) : ICategoriaRepository
{
    private readonly AppDataContext _context = context;

    public async Task<string> DeleteCategoria(CategoriaProdutoModel categoria)
    {
        _context.TabelaCategoriaProdutoModel.Remove(categoria);
        return await _context.SaveChangesAsync() > 0 ? "Categoria deletada com sucesso" : "Erro ao deletar categoria";
    }

    public async Task<CategoriaProdutoModel?> GetCategoria(int id)
    {
        return await _context.TabelaCategoriaProdutoModel.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CategoriaProdutoModel?> GetCategoria(string categoria)
    {
        return await _context.TabelaCategoriaProdutoModel.FirstOrDefaultAsync(x => x.Descricao == categoria);
    }

    public async Task<IEnumerable<CategoriaProdutoModel>> GetCategorias()
    {
        return await _context.TabelaCategoriaProdutoModel.ToListAsync();
    }

    public async Task<string> PostCategoria(CategoriaProdutoModel categoria)
    {
        await _context.TabelaCategoriaProdutoModel.AddAsync(categoria);
        return await _context.SaveChangesAsync() > 0 ? "Categoria cadastrada com sucesso" : "Erro ao cadastrar categoria";
    }

    public async Task<string> PutCategoria(CategoriaProdutoModel categoria)
    {
        _context.TabelaCategoriaProdutoModel.Update(categoria);
        return await _context.SaveChangesAsync() > 0 ? "Categoria atualizada com sucesso" : "Erro ao atualizar categoria";
    }
}
