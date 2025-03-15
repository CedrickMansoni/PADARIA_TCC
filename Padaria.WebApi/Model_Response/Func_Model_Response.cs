using System;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Model_Response;

public class Func_Model_Add
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string SenhaFuncionario { get; set; } = string.Empty;
    public string AvatarFuncionario { get; set; } = string.Empty;
    public string EstadoFuncionario { get; set; } = string.Empty;
}
