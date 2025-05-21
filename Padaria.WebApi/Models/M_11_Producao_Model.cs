using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_11_producao")]
public class ProducaoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("data_producao")]
    public string DataProducao { get; set; }  = string.Empty; 

    [Column("id_funcionario")]
    public int? IdFuncionario { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("status")]
    public string EstadoProducao { get; set; } = string.Empty;    
}

