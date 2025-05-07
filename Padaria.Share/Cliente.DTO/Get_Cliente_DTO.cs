using System;

namespace Padaria.Share.Cliente.DTO;

public class Get_Cliente_DTO
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Nif { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;
}
