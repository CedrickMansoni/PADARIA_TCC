using System;

namespace Padaria.Share.Funcionario.DTO;

public class Put_Password_DTO
{
    public int UserId {get; set;}
    public string NewPassword {get; set;} = string.Empty;
}
