using System;

namespace Padaria.Share.Pedido.DTO;

public class Put_Pedido_DTO
{
     public int Id { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoTotal { get; set; }

    public string EstadoEncomenda { get; set; } = string.Empty;
}
