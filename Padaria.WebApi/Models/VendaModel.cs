using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_10_venda")]
public class VendaModel
{
    [Column("numero_factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Column("id_caixa")]
    public int IdCaixa { get; set; }

    [Column("data_venda")]
    public DateTime DataVenda { get; set; }

    [Column("tipo_venda")]
    public string TipoVenda { get; set; } = string.Empty;

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
