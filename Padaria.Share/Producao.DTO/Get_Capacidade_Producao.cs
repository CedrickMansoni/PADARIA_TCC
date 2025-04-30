using System;

namespace Padaria.Share.Producao.DTO;

public class Get_Capacidade_Producao
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int QuantidadeSolicitada { get; set; }
    public int QuantidadeMaxima { get; set; }
    public DateTime Data { get; set; }  
}
