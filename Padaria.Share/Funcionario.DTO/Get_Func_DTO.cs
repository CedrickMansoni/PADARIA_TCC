using System;

namespace Padaria.Share.Funcionario.DTO;

public class Get_Func_DTO
{
    public int Id { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string NomeFuncionario { get; set; } = string.Empty;
    public string TelefoneFuncionario { get; set; } = string.Empty;
    public string AvatarFuncionario { get; set; } = string.Empty;
    public string EstadoFuncionario { get; set; } = string.Empty;
    public bool Estado {get;set;}
    public string Mensagem { get; set; } = string.Empty;
}
