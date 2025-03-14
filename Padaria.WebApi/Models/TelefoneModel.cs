using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_04_telefone")]
public class TelefoneModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_funcionario")]
    public int? IdFuncionario { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("telefone")]
    public string NumeroTelefone { get; set; } = string.Empty;
}
