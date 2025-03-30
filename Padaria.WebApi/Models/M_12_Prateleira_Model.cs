using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_12_prateleira")]
public class PrateleiraModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }
    [Column("quantidade")]
    public int Quantidade { get; set; }
}
