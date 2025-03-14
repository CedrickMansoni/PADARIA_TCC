using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_08_factura")]
public class FacturaModel
{
    [Column("numero_factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("valor_total")]
    public decimal ValorTotal { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;

    [Column("comprovativo")]
    public string Comprovativo { get; set; } = string.Empty;
}
