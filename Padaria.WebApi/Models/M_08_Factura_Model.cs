using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_08_factura")]
public class FacturaModel
{
    [Key]
    [Column("numero_factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("valor_total")]
    public decimal ValorTotal { get; set; }

    [Column("pagamento_cash")]
    public decimal PagamentoCash { get; set; }

    [Column("pagamento_cartao")]
    public decimal PagamentoCartao { get; set; }

    [Column("pagamento_transferencia")]
    public decimal PagamentoTransferencia { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;

    [Column("comprovativo")]
    public string Comprovativo { get; set; } = string.Empty;
}
