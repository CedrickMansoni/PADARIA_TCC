using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;
[Table("t_13_capacidade")]
public class CapacidadeProducaoModel
{
    [Column("id")]
    public int Id { get; set; }
    [Column("id_produto")]
    public int IdProduto { get; set; }
    [Column("quantidade")]
    public int QuantidadeSolicitada { get; set; }
    [Column("quantidade_maxima")]
    public int QuantidadeMaxima { get; set; }
    [Column("data_producao")]
    public DateTime Data { get; set; }  
}
