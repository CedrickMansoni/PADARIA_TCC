using System;

namespace Padaria.Share.Funcionario.DTO;

public class Put_UserState_DTO
{
    public int UserId {get; set;}
    public string EstadoFuncionario {get; set;} = string.Empty;
}
