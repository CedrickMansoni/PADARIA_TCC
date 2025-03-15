using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_02_funcionario")]

public class FuncionarioModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("nome_completo")]
    public string NomeCompleto { get; set; } = string.Empty;

    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    [Column("avatar")]
    public string Avatar { get; set; } = string.Empty;

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
