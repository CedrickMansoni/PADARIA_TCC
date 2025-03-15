using System;

namespace Padaria.Share.Funcionario.DTO;

public class Post_Func_DTO
{
    public int IdCategoria { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string SenhaFuncionario { get; set; } = string.Empty;
    public string TelefoneFuncionario { get; set; } = string.Empty;
    public string AvatarFuncionario { get; set; } = string.Empty;
    public string EstadoFuncionario { get; set; } = string.Empty;
}
