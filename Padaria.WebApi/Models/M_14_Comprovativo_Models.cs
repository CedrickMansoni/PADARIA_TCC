using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padaria.WebApi.Models;

[Table("t_14_comprovativo")]
public class ComprovativoModels
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("id_producao")]
    public int IdProducao { get; set; }

    [Column("data_envio")]
    public DateTime DataEnvio { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;

    [Column("comprovativo")]
    public string Comprovativo { get; set; } = string.Empty;

}
