using System;
using Padaria.Share.Funcionario.DTO;
using Padaria.WebApi.Model_Response;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Funcionario;

public interface IFuncionarioRepository
{
    // Create
    Task<string> AdicionarFuncionarioAsync(Post_Func_DTO funcionario);

    // Read
    Task<Get_Func_DTO?> ObterFuncionarioPorIdAsync(int id);
    Task<IEnumerable<Get_Func_DTO>> ObterTodosFuncionariosAsync();
    Task<IEnumerable<Get_Func_DTO>> ObterFuncionariosPorCategoriaAsync(int idCategoria);
    Task<bool> FuncionarioExisteAsync(int id);
    Task<string> ObterCategoriaPorIdAsync(int id);
    Task<IEnumerable<CategoriaFuncionarioModel>> ObterCategoriasAsync();

    // Update
    Task<string> AtualizarFuncionarioAsync(Post_Func_DTO funcionario);
    Task<bool> AtualizarSenhaFuncionarioAsync(int id, string novaSenha);
    Task<bool> AtualizarEstadoFuncionarioAsync(int id, string novoEstado);
    Task<string> AtualizarAvatarFuncionarioAsync(int id, string novoAvatar);

    // login
    Task<Get_Func_DTO?> AutenticarFuncionarioAsync(Login_Func_DTO login);
    Task<int> ContarFuncionarios();
}
