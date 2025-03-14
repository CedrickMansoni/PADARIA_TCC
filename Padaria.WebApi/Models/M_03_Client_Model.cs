using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_03_cliente")]
public class ClienteModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    [Column("nif")]
    public string Nif { get; set; } = string.Empty;

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
