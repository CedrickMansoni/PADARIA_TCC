using System;

namespace Padaria.Share.Cliente.DTO;

public class ClienteReponse
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
