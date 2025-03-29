using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.DNS_App;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Produto;

public class ProdutoRepository(AppDataContext context) : IProdutoRepository
{
    private readonly AppDataContext _context = context;
    public async Task<string> AtualizarProdutoAsync(Post_Produto_DTO produto)
    {
        try
        {
            var produtoAtualizar = await _context.TabelaProdutoModel.FindAsync(produto.Id);
            if (produtoAtualizar == null)
            {
                return "Produto não encontrado";
            }
            produtoAtualizar.Nome = produto.Nome;
            produtoAtualizar.Preco = produto.Preco;
            produtoAtualizar.Descricao = produto.Descricao;
            produtoAtualizar.IdCategoria = produto.IdCategoria;
            produtoAtualizar.Imagem = produto.Imagem.FileName;
            produtoAtualizar.IdFuncionario = produto.IdFuncionario;
            return await _context.SaveChangesAsync() > 0 ? "Produto actualizado com sucesso" : "Erro ao actualizar produto";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> CadastrarProdutoAsync(Post_Produto_DTO produto)
    {
        try
        {
            var novoProduto = new ProdutoModel
            {
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                IdCategoria = produto.IdCategoria,
                Imagem = produto.Imagem.FileName,
                IdFuncionario = produto.IdFuncionario,
                Unidade = produto.Unidade,
            };
            await _context.TabelaProdutoModel.AddAsync(novoProduto);
            return await _context.SaveChangesAsync() > 0 ? "Produto cadastrado com sucesso" : "Erro ao cadastrar produto";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> DeletarProdutoAsync(ProdutoModel produto)
    {
        try
        {
            var produtoDeletar = await _context.TabelaProdutoModel.FindAsync(produto.Id);
            if (produtoDeletar == null)
            {
                return "Produto não encontrado";
            }
            _context.TabelaProdutoModel.Remove(produtoDeletar);
            return await _context.SaveChangesAsync() > 0 ? "Produto deletado com sucesso" : "Erro ao deletar produto";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<ProdutoModel?> PesquisarProduto(int id)
    {
        return await _context.TabelaProdutoModel.FindAsync(id);
    }

    public async Task<ProdutoModel?> PesquisarProdutoByName(string nome)
    {
        return await _context.TabelaProdutoModel.FirstOrDefaultAsync(p => p.Nome == nome);
    }


    public async Task<Get_Produto_DTO?> PesquisarProdutoAsync(int id)
    {
        var query = from produto in _context.TabelaProdutoModel
                    join categoria in _context.TabelaCategoriaProdutoModel on produto.IdCategoria equals categoria.Id
                    where produto.Id == id
                    select new Get_Produto_DTO
                    {
                        Id = produto.Id,
                        Nome = produto.Nome,
                        Preco = produto.Preco,
                        Descricao = produto.Descricao,
                        Categoria = categoria.Descricao,
                        Unidade = produto.Unidade,
                        Imagem = $"{My_DNS.App_DNS}images/produtos/{produto.Imagem}"
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Get_Produto_DTO>> PesquisarProdutosAsync()
    {
        var query = from produto in _context.TabelaProdutoModel
                    join categoria in _context.TabelaCategoriaProdutoModel on produto.IdCategoria equals categoria.Id
                    select new Get_Produto_DTO
                    {
                        Id = produto.Id,
                        Nome = produto.Nome,
                        Preco = produto.Preco,
                        Descricao = produto.Descricao,
                        Categoria = categoria.Descricao,
                        Unidade = produto.Unidade,
                        Imagem = $"{My_DNS.App_DNS}images/produtos/{produto.Imagem}"
                    };
        return await query.ToListAsync();
    }
}
