using System;

namespace Padaria.Share.Producao.DTO;

public class Put_Producao_DTO
{
    public int Produto { get; set; }

    public int Quantidade { get; set; }

    public DateTime DataProducao { get; set; }

    public int Padeiro { get; set; }

    public string Estado { get; set; } = string.Empty; 
}
