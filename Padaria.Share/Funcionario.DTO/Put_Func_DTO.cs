using System;

namespace Padaria.Share.Funcionario.DTO;

public class Put_Func_DTO
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string SenhaFuncionario { get; set; } = string.Empty;
    public string AvatarFuncionario { get; set; } = string.Empty;
    public string EstadoFuncionario { get; set; } = string.Empty;
}
