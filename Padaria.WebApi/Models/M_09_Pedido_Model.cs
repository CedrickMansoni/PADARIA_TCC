using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

 [Table("t_09_pedido")]
public class EncomendaModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }

    [Column("numero_factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Column("data_encomenda")]
    public DateTime DataEncomenda { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("preco_total")]
    public decimal PrecoTotal { get; set; }

    [Column("estado_encomenda")]
    public string EstadoEncomenda { get; set; } = string.Empty;
}
