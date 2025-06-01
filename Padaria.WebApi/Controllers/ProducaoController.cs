using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Padaria.Share.Producao.DTO;
using Padaria.WebApi.Comunicacao;
using Padaria.WebApi.Service.Producao;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoController(IProducaoService service, IHubContext<ChatHub> hubContext) : ControllerBase
    {
        private IHubContext<ChatHub> _hubContext = hubContext;
        private readonly IProducaoService service = service;

        [HttpPost, Route("/adicionar/producao")]
        public async Task<IActionResult> AdicionarProducao(IEnumerable<Post_Producao_DTO> producao)
        {
            var response = await service.AdicionarAsync(producao);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPut, Route("/editar/producao")]
        public async Task<IActionResult> EditarProducao(Put_Producao_DTO producao)
        {
            var response = await service.AtualizarAsync(producao);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPut, Route("/editar/estado/pedido")]
        public async Task<IActionResult> EditarProducao(Put_PedidoState_DTO producao)
        {
            var response = await service.AtualizarEstadoAsync(producao);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpGet, Route("/listar/producao/data/{data}")]
        public async Task<IActionResult> ListarProducaoPorData(DateTime data, int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ListarProducaoPorData(data, skip, take, c));
        }

        [HttpGet, Route("/listar/producao/estado/{estado}")]
        public async Task<IActionResult> ListarProducaoPorEstado(string estado, DateTime data, DateTime data2, int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ObterPorStatusAsync(estado, data, data2, skip, take, c));
        }
        [HttpGet, Route("/listar/producao")]
        public async Task<IActionResult> ListarProducaoAsync(int skip = 0, int take = 60, CancellationToken c = default)
        {
            return Ok(await service.ListarProducao(skip, take, c));
        }

        [HttpGet, Route("/listar/producao/cliente/{id}")]
        public async Task<IActionResult> ListarProducaoAsyncCliente(int id, int skip = 0, int take = 60, CancellationToken c = default)
        {
            return Ok(await service.ListarProducaoCliente(id, skip, take, c));
        }

        [HttpGet, Route("/listar/producao/cliente/pagamento/{id}")]
        public async Task<IActionResult> ListarProducaoAsyncClientePagamento(int id, int skip = 0, int take = 60, CancellationToken c = default)
        {
            return Ok(await service.ListarProducaoClientePagamento(id, skip, take, c));
        }

        [HttpDelete, Route("/remover/producao/{id}")]
        public async Task<IActionResult> RemoverProducao(int id)
        {
            var response = await service.RemoverAsync(id);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response) return BadRequest("Erro ao remover a produção");
            return Ok("Produção removida com sucesso");
        }

        [HttpPost, Route("/adicionar/capacidade/producao")]
        public async Task<IActionResult> AdicionarCapacidadeProducao(Post_Capacidade_Producao capacidadeProducao)
        {
            var response = await service.AdicionarCapacidadeProducaoAsync(capacidadeProducao);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPut, Route("/editar/capacidade/producao")]
        public async Task<IActionResult> EditarCapacidadeProducao(Put_Capacidade_Producao capacidadeProducao)
        {
            var response = await service.AtualizarCapacidadeProducaoAsync(capacidadeProducao);
            await _hubContext.Clients.All.SendAsync("notificar");
            if (!response.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase)) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpGet, Route("/listar/capacidade/producao")]
        public async Task<IActionResult> ListarCapacidadeProducao(int skip = 0, int take = 30, CancellationToken c = default)
        {
            return Ok(await service.ListarCapacidadeProducao(skip, take, c));
        }

    }
}
