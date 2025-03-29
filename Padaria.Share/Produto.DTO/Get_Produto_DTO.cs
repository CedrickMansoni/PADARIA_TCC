using System;

namespace Padaria.Share.Produto.DTO;

public class Get_Produto_DTO
{
    public int Id { get; set; }

    public string Categoria { get; set; }  = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public int Unidade { get; set; }

    public decimal Preco { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public string Imagem { get; set; } = string.Empty;
}
