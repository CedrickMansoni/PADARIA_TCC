using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_07_caixa")]
public class CaixaModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }

    [Column("data_abertura")]
    public DateTime DataAbertura { get; set; }

    [Column("valor_inicial")]
    public decimal ValorInicial { get; set; }

    [Column("valor_final")]
    public decimal? ValorFinal { get; set; }

    [Column("data_fecho")]
    public DateTime? DataFecho { get; set; }
}