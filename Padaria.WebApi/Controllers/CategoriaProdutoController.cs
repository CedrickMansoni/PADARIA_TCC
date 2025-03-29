using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Produto.DTO;
using Padaria.WebApi.Service.Produto;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaProdutoController(ICategoriaService serviceCateg) : ControllerBase
    {
         private readonly ICategoriaService _serviceCateg = serviceCateg;

        [HttpGet, Route("/pesquisar/categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var response = await _serviceCateg.GetCategorias();
            return response is not null ? Ok(response) : StatusCode(StatusCodes.Status204NoContent);
            
        }

        [HttpGet, Route("/pesquisar/categoria{id}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
             var response = await _serviceCateg.GetCategoria(id);
            return response is not null ? Ok(response) : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost, Route("/cadastrar/categoria")]
        public async Task<IActionResult> PostCategoria(Post_Categ_Produto_DTO categoria)
        {
            var response = await _serviceCateg.PostCategoria(categoria);
            return response.Contains("sucesso") ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPut("/editar/categoria")]
        public async Task<IActionResult> PutCategoria(Post_Categ_Produto_DTO categoria)
        {
            var response = await _serviceCateg.PutCategoria(categoria);
            return response.Contains("sucesso") ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpDelete("/deletar/categoria{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var response = await _serviceCateg.DeleteCategoria(id);
            return response.Contains("sucesso") ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
