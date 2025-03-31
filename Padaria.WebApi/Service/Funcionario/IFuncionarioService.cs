using System;
using Padaria.Share.Funcionario.DTO;
using Padaria.WebApi.Models;
using Padaria.WebApi.SMS_Service.Model_SMS;

namespace Padaria.WebApi.Service.Funcionario;

public interface IFuncionarioService
{
    // Create
    Task<string> AdicionarFuncionarioAsync(Post_Func_DTO funcionario);

    // Read
    Task<IEnumerable<Get_Func_DTO>> ObterTodosFuncionariosAsync();
    Task<IEnumerable<Get_Func_DTO>> ObterFuncionariosPorCategoriaAsync(int idCategoria);
    Task<IEnumerable<Get_Categ_Func_DTO>> ObterCategoriasAsync();

    // Update
    Task<string> AtualizarFuncionarioAsync(Post_Func_DTO funcionario);
    Task<bool> AtualizarSenhaFuncionarioAsync(int id, string novaSenha);
    Task<bool> AtualizarEstadoFuncionarioAsync(int id, string novoEstado);
    Task<string> AtualizarAvatarFuncionarioAsync(Update_Avatar_DTO avatar);

    // login
    Task<Get_Func_DTO?> AutenticarFuncionarioAsync(Login_Func_DTO login);

}
