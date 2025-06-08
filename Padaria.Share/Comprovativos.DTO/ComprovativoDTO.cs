using System;
using Microsoft.AspNetCore.Http;

namespace Padaria.Share.Comprovativos.DTO;

public class ComprovativoDTO
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdProducao { get; set; }
    public DateTime DataEnvio { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Comprovativo { get; set; } = string.Empty;
    public IFormFile Arquivo { get; set; } = default!;

}
