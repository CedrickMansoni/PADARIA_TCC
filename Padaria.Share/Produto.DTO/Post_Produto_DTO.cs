using System;
using Microsoft.AspNetCore.Http;

namespace Padaria.Share.Produto.DTO;

public class Post_Produto_DTO
{
    public int Id { get; set; }

    public int IdCategoria { get; set; }

    public int IdFuncionario { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int Unidade { get; set; }

    public decimal Preco { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public IFormFile Imagem { get; set; } = default!;
}
