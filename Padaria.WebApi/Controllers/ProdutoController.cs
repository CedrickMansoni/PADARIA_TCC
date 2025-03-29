using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Service.Produto;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController(IProdutoService service) : ControllerBase
    {
        private readonly IProdutoService _service = service;

        [HttpGet, Route("/listar/produtos")]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _service.PesquisarProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet, Route("/listar/produto/{id}")]
        public async Task<IActionResult> ListarProduto(int id)
        {
            var produto = await _service.PesquisarProdutoAsync(id);
            return Ok(produto);
        }

        [HttpPost, Route("/cadastrar/produto")]
        public async Task<IActionResult> CadastrarProduto()
        {
            // Verificar se a requisição possui conteúdo multimídia
            if (!Request.HasFormContentType)
            {
                return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
            }

            var form = await Request.ReadFormAsync();
            var produto = new Post_Produto_DTO
            {
                IdCategoria = int.TryParse(form["idCategoria"], out int idCategoria) ? idCategoria : 0,
                IdFuncionario = Convert.ToInt32(form["idFuncionario"]),
                Nome = form["nome"]!,
                Unidade = Convert.ToInt32(form["unidade"])!,
                Preco = Convert.ToDecimal(form["preco"]),
                Descricao = form["descricao"]!,                
                Imagem = form.Files.GetFile("imagem")!,                
            };
            var produtoCadastrado = await _service.CadastrarProdutoAsync(produto);

            return produtoCadastrado.Contains("sucesso") ? Ok(produtoCadastrado) : BadRequest(produtoCadastrado);
        }

        [HttpPut, Route("/atualizar/produto")]
        public async Task<IActionResult> AtualizarProduto([FromBody] Post_Produto_DTO produto)
        {
            var produtoAtualizado = await _service.AtualizarProdutoAsync(produto);
            return produtoAtualizado.Contains("sucesso") ? Ok(produtoAtualizado) : BadRequest(produtoAtualizado);
        }

        [HttpDelete, Route("/deletar/produto/{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            var produtoDeletado = await _service.DeletarProdutoAsync(id);
            return produtoDeletado.Contains("sucesso") ? Ok(produtoDeletado) : BadRequest(produtoDeletado);
        }

    }
}
