using System;

namespace Padaria.Share.Producao.DTO;

public class Put_Capacidade_Producao
{
    public int Id { get; set; }
    public int Produto { get; set; }
    public int QuantidadeMaxima { get; set; }
    public DateTime DataProducao { get; set; }  
}
