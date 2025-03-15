using System;
using Microsoft.AspNetCore.Http;

namespace Padaria.Share.Funcionario.DTO;

public class Update_Avatar_DTO
{
    public int Id {get; set;}
    public IFormFile? Ficheiro{get; set;} 
}
