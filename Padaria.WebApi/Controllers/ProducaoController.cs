using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Service.Producao;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoController(IProducaoService service) : ControllerBase
    {
        private readonly IProducaoService service = service;

        [HttpPost, Route("/adicionar/producao")]
        public async Task<IActionResult> AdicionarProducao(Post_Producao_DTO producao)
        {
            var response = await service.AdicionarAsync(producao);
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPut, Route("/editar/producao")]
        public async Task<IActionResult> EditarProducao(Post_Producao_DTO producao)
        {
            var response = await service.AtualizarAsync(producao);
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpGet, Route("/listar/producao/data/{data}")]
        public async Task<IActionResult> ListarProducaoPorData(DateTime data, int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ListarProducaoPorData(data, skip, take, c));
        }

        [HttpGet, Route("/listar/producao/estado/{estado}")]
        public async Task<IActionResult> ListarProducaoPorEstado(string estado, DateTime data, DateTime data2,int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ObterPorStatusAsync(estado, data, data2,skip, take, c));
        }
        [HttpGet, Route("/listar/producao")]
        public async Task<IActionResult> ListarProducaoAsync(int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ListarProducao(skip, take, c));
        }
    }
}
