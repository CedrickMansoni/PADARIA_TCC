using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Funcionario.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Model_Response;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Funcionario;

public class FuncionarioRepository(AppDataContext context) : IFuncionarioRepository
{
    private readonly AppDataContext _context = context;

    // Create
    public async Task<string> AdicionarFuncionarioAsync(Post_Func_DTO dados)
    {
        using var transacao = await _context.Database.BeginTransactionAsync();
        try
        {
            var funcionario = new FuncionarioModel
            {
                IdCategoria = dados.IdCategoria,
                NomeCompleto = dados.NomeFuncionario,
                Senha = dados.SenhaFuncionario,
                Avatar = dados.AvatarFuncionario,
                Estado = dados.EstadoFuncionario

            };
            // Adicionar o funcionário
            await _context.TabelaFuncionarioModel.AddAsync(funcionario);
            await _context.SaveChangesAsync();

            // Adicionar o telefone associado ao funcionário
            if (funcionario.Id < 1) throw new Exception("Erro: Não foi possível salvar os dados do funcionário");

            var telefoneModel = new TelefoneModel
            {
                IdFuncionario = funcionario.Id,
                NumeroTelefone = dados.TelefoneFuncionario,
            };
            await _context.TabelaTelefoneModel.AddAsync(telefoneModel);

            if (await _context.SaveChangesAsync() == 0) throw new Exception("Erro: Não foi possível salvar os dados do funcionário");

            await transacao.CommitAsync();

            return $"Funcionário {dados.NomeFuncionario}, foi cadastrado com sucesso";
        }
        catch
        {
            await transacao.RollbackAsync();
            return $"Erro: Não foi possível cadastrar o funcionário: {dados.NomeFuncionario}";
        }
    }

    // Read
    public async Task<Get_Func_DTO?> ObterFuncionarioPorIdAsync(int id)
    {
        var query = from funcionario in _context.TabelaFuncionarioModel
                    join telefone in _context.TabelaTelefoneModel on funcionario.Id equals telefone.IdFuncionario
                    join categoria in _context.TabelaCategoriaFuncionarioModel on funcionario.IdCategoria equals categoria.Id
                    where funcionario.Id == id
                    select new Get_Func_DTO
                    {
                        Id = funcionario.Id,
                        NomeFuncionario = funcionario.NomeCompleto,
                        TelefoneFuncionario = telefone.NumeroTelefone,
                        Categoria = categoria.Descricao,
                        EstadoFuncionario = funcionario.Estado,
                        AvatarFuncionario = funcionario.Avatar,
                        Mensagem = "Sucesso"
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Get_Func_DTO>> ObterTodosFuncionariosAsync()
    {
        var query = from funcionario in _context.TabelaFuncionarioModel
                    join telefone in _context.TabelaTelefoneModel on funcionario.Id equals telefone.IdFuncionario
                    join categoria in _context.TabelaCategoriaFuncionarioModel on funcionario.IdCategoria equals categoria.Id

                    select new Get_Func_DTO
                    {
                        Id = funcionario.Id,
                        NomeFuncionario = funcionario.NomeCompleto,
                        TelefoneFuncionario = telefone.NumeroTelefone,
                        Categoria = categoria.Descricao,
                        EstadoFuncionario = funcionario.Estado,
                        AvatarFuncionario = funcionario.Avatar,
                        Mensagem = "Sucesso"
                    };
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Get_Func_DTO>> ObterFuncionariosPorCategoriaAsync(int idCategoria)
    {
        var query = from funcionario in _context.TabelaFuncionarioModel
                    join telefone in _context.TabelaTelefoneModel on funcionario.Id equals telefone.IdFuncionario
                    join categoria in _context.TabelaCategoriaFuncionarioModel on funcionario.IdCategoria equals categoria.Id
                    where funcionario.IdCategoria == idCategoria
                    select new Get_Func_DTO
                    {
                        Id = funcionario.Id,
                        NomeFuncionario = funcionario.NomeCompleto,
                        TelefoneFuncionario = telefone.NumeroTelefone,
                        Categoria = categoria.Descricao,
                        EstadoFuncionario = funcionario.Estado,
                        AvatarFuncionario = funcionario.Avatar,
                        Mensagem = "Sucesso"
                    };
        return await query.ToListAsync();
    }

    public async Task<bool> FuncionarioExisteAsync(int id)
    {
        return await _context.TabelaFuncionarioModel
            .AnyAsync(f => f.Id == id);
    }

    // Update
    public async Task<string> AtualizarFuncionarioAsync(Post_Func_DTO f)
    {
        using var transacao = await _context.Database.BeginTransactionAsync();
        try
        {
            var funcionario = new FuncionarioModel
            {
                IdCategoria = f.IdCategoria,
                NomeCompleto = f.NomeFuncionario,

            };
            // Actualizar funcionário
            _context.TabelaFuncionarioModel.Update(funcionario);

            var telefone = new TelefoneModel
            {
                IdFuncionario = funcionario.Id,
                NumeroTelefone = f.TelefoneFuncionario,
            };
            // Actualizar ou adicionar telefone
            if (!string.IsNullOrEmpty(telefone.NumeroTelefone))
            {
                var telefoneExistente = await _context.TabelaTelefoneModel
                    .FirstOrDefaultAsync(t => t.IdFuncionario == funcionario.Id);

                if (telefoneExistente != null)
                {
                    telefoneExistente.NumeroTelefone = telefone.NumeroTelefone;
                    _context.TabelaTelefoneModel.Update(telefoneExistente);
                }
                else
                {
                    var novoTelefone = new TelefoneModel
                    {
                        IdFuncionario = funcionario.Id,
                        NumeroTelefone = telefone.NumeroTelefone
                    };
                    await _context.TabelaTelefoneModel.AddAsync(novoTelefone);
                }
            }

            await _context.SaveChangesAsync();
            return "Dados actualizados com sucesso";
        }
        catch
        {
            await transacao.RollbackAsync();
            return $"Erro: Não foi possível editar os dados do funcionário {f.NomeFuncionario}";
        }
    }

    public async Task<bool> AtualizarSenhaFuncionarioAsync(int id, string novaSenha)
    {
        var funcionario = await _context.TabelaFuncionarioModel.FindAsync(id);
        if (funcionario == null)
            return false;

        funcionario.Senha = novaSenha;
        _context.TabelaFuncionarioModel.Update(funcionario);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AtualizarEstadoFuncionarioAsync(int id, string novoEstado)
    {
        var funcionario = await _context.TabelaFuncionarioModel.FindAsync(id);
        if (funcionario == null)
            return false;

        funcionario.Estado = novoEstado;
        _context.TabelaFuncionarioModel.Update(funcionario);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<string> AtualizarAvatarFuncionarioAsync(int id, string novoAvatar)
    {
        var funcionario = await _context.TabelaFuncionarioModel.FindAsync(id);
        if (funcionario == null)
            return "Não existe nenhum funcionário com este id";

        funcionario.Avatar = novoAvatar;
        _context.TabelaFuncionarioModel.Update(funcionario);
        return await _context.SaveChangesAsync() > 0 ? "Foto de perfil editada com sucesso" : "Erro: Não conseguimos editar a sua foto de perfil";
    }

    // Login
    public async Task<Get_Func_DTO?> AutenticarFuncionarioAsync(Login_Func_DTO login)
    {
       var query = from funcionario in _context.TabelaFuncionarioModel
                    join telefone in _context.TabelaTelefoneModel on funcionario.Id equals telefone.IdFuncionario
                    join categoria in _context.TabelaCategoriaFuncionarioModel on funcionario.IdCategoria equals categoria.Id
                    where funcionario.Senha == login.SenhaFuncionario && telefone.NumeroTelefone == login.TelefoneFuncionario
                    select new Get_Func_DTO
                    {
                        Id = funcionario.Id,
                        NomeFuncionario = funcionario.NomeCompleto,
                        TelefoneFuncionario = telefone.NumeroTelefone,
                        Categoria = categoria.Descricao,
                        EstadoFuncionario = funcionario.Estado,
                        AvatarFuncionario = funcionario.Avatar,
                        Mensagem = "Sucesso"
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> ContarFuncionarios()
    {
        return await _context.TabelaFuncionarioModel.CountAsync();
    }

    public async Task<string> ObterCategoriaPorIdAsync(int id)
    {
        var categoria = await _context.TabelaCategoriaFuncionarioModel.FirstOrDefaultAsync(x => x.Id == id);
        if(categoria is null) return string.Empty;
        return categoria.Descricao;
    }

    public async Task<IEnumerable<CategoriaFuncionarioModel>> ObterCategoriasAsync()
    {
        return await _context.TabelaCategoriaFuncionarioModel.Where(x => x.Id <= 4).ToListAsync();        
    }
}
