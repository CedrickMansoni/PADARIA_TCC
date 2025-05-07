using System;

namespace Padaria.Share.Pedido.DTO;

public class Post_Pedido_DTO
{
    public int Id { get; set; }

    public int IdProduto { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    public DateTime DataEncomenda { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoTotal { get; set; }

    public string EstadoEncomenda { get; set; } = string.Empty;
}
