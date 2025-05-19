using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Cliente.DTO;
using Padaria.WebApi.Service.Cliente;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("/adicionar/cliente")]
        public async Task<IActionResult> AdicionarCliente([FromBody] Post_Cliente_DTO cliente)
        {
            var result = await _clienteService.AdicionarCliente(cliente);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpPut("/actualizar/cliente")]
        public async Task<IActionResult> ActualizarCliente([FromBody] Get_Cliente_DTO cliente)
        {
            var result = await _clienteService.ActualizarCliente(cliente);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("/deletar/cliente/{id}")]
        public async Task<IActionResult> DeletarCliente(int id)
        {
            var result = await _clienteService.DeletarCliente(id);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpGet("/listar/clientes")]
        public async Task<IActionResult> ListarClientes()
        {
            var result = await _clienteService.ListarClientes();
            return Ok(result);
        }

        [HttpGet("/listar/cliente/{telefone}")]
        public async Task<IActionResult> ListarCliente(string telefone)
        {
            var result = await _clienteService.ListarCliente(telefone);
            return result is not null ? Ok(result) : BadRequest();
        }
    }
}
