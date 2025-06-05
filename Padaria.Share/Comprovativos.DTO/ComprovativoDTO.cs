using System;

namespace Padaria.Share.Comprovativos.DTO;

public class ComprovativoDTO
{
    public int Id { get; set; } 
    public int IdCliente { get; set; } 
    public int IdProducao { get; set; } 
    public DateTime DataEnvio { get; set; } 
    public string Estado { get; set; } = string.Empty; 
    public string Comprovativo { get; set; } = string.Empty;
}
