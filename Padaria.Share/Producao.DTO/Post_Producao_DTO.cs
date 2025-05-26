using System;

namespace Padaria.Share.Producao.DTO;

public class Post_Producao_DTO
{
    public int Produto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int? Funcionario { get; set; }
    public int? Cliente { get; set; }
    public string? Telefone { get; set; } = string.Empty;
    public int Limite { get; set; }

}
