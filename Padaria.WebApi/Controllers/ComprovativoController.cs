using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Comprovativos.DTO;
using Padaria.Share.DNS_App;
using Padaria.WebApi.Service.Cliente;
using Padaria.WebApi.Service.Comprovativo;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprovativoController(IComprovativoService service, IClienteService _service) : ControllerBase
    {
        private readonly IComprovativoService service = service;
        private readonly IClienteService _service = _service;

        [HttpPost, Route("/salvar/comprovativo")]
        public async Task<IActionResult> CadastrarProduto()
        {
            // Verificar se a requisição possui conteúdo multimídia
            if (!Request.HasFormContentType)
            {
                return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
            }

            var form = await Request.ReadFormAsync();
            var comprovativo = new ComprovativoDTO
            {
                IdCliente = int.TryParse(form["idCliente"], out int idCliente) ? idCliente : 0,
                Arquivo = form.Files.GetFile("comprovativo")!,
                Telefone = form["telefone"]!
            };
            var comprovativoCadastrado = await service.SalvarComprovativo(comprovativo);

            return comprovativoCadastrado.Contains("sucesso") ? Ok(comprovativoCadastrado) : BadRequest(comprovativoCadastrado);
        }

        [HttpGet("/download/{id}")]
        public async Task<IActionResult> DownloadComprovativo(int id)
        {
            // Obter o caminho físico do comprovativo
            var Arquivo = await service.DownloadComprativo(id);
            var caminho = $"/Users/cedrickmansoni/Storage/Padaria/Comprovativos/{Arquivo!.Telefone}/{Arquivo.Comprovativo}";

            if (string.IsNullOrEmpty(caminho) || !System.IO.File.Exists(caminho))
            {
                return NotFound($"Comprovativo não encontrado.\n{Arquivo!.Telefone}");
            }

            var tipoConteudo = "application/octet-stream"; // ou use ContentTypeProvider para descobrir

            var bytes = await System.IO.File.ReadAllBytesAsync(caminho);
            return File(bytes, tipoConteudo, Arquivo.Comprovativo);
        }

    }
}
