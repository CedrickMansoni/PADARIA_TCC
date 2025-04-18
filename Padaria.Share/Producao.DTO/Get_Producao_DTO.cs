using System;

namespace Padaria.Share.Producao.DTO;

public class Get_Producao_DTO
{
    public int Id { get; set; }
    public string Produto { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public DateTime DataProducao { get; set; }

    public string Padeiro { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}
