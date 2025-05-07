using System;

namespace Padaria.Share.Pedido.DTO;

public class Get_Pedido_DTO
{
    public int Id { get; set; }

    public int IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;

    public string NumeroFactura { get; set; } = string.Empty;

    public DateTime DataEncomenda { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoTotal { get; set; }

    public string EstadoPedido { get; set; } = string.Empty;
}
