using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Pedido.DTO;
using Padaria.WebApi.Service.Pedido;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController(IPedidoService service) : ControllerBase
    {
        private readonly IPedidoService _service = service;

        [HttpGet, Route("/listar/pedidos")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpPost, Route("/criar/pedido")]
        public async Task<IActionResult> CreateAsync([FromBody] Post_Pedido_DTO pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Pedido não pode ser nulo");
            }
            var result = await _service.CreateAsync(pedido);
            return Ok(result);
        }

        [HttpPut, Route("/actualizar/pedido")]
        public async Task<IActionResult> UpdateAsync([FromBody] Put_Pedido_DTO pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Pedido não pode ser nulo");
            }
            var result = await _service.UpdateAsync(pedido);
            return Ok(result);
        }

        [HttpDelete("/apagar/pedido/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id não pode ser menor que 1");
            }
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        

        

    }
}
