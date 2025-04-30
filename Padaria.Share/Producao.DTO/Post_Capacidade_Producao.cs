using System;

namespace Padaria.Share.Producao.DTO;

public class Post_Capacidade_Producao
{
    public int Produto { get; set; }
    public int QuantidadeMaxima { get; set; }
    public DateTime DataProducao { get; set; }  
}
