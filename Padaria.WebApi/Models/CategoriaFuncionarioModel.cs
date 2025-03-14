using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_01_categoria")]
public class CategoriaFuncionarioModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("descricao")]
    public string Descricao { get; set; } = string.Empty;
}
