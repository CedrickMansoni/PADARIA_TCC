using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_06_produto")]
public class ProdutoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("unidade")]
    public int Unidade { get; set; }

    [Column("preco")]
    public decimal Preco { get; set; }

    [Column("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [Column("imagem")]
    public string Imagem { get; set; } = string.Empty;
}
