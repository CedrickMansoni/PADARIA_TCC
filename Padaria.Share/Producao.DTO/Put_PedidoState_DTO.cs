using System;

namespace Padaria.Share.Producao.DTO;

public class Put_PedidoState_DTO
{
    public int IdPedido { get; set; }
    public string Estado { get; set; } = string.Empty;

}
