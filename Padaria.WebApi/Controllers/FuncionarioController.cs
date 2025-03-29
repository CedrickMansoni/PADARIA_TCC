using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padaria.Share.Funcionario.DTO;
using Padaria.WebApi.Service.Funcionario;

namespace Padaria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController(IFuncionarioService service) : ControllerBase
    {
        private readonly IFuncionarioService _service = service;

        [HttpPost, Route("/cadastrar/funcionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdicionarFuncionario([FromBody] Post_Func_DTO funcionario)
        {            
            try
            {
                var resultado = await _service.AdicionarFuncionarioAsync(funcionario);

                if (!resultado.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest(resultado);
                }

                return StatusCode(201, resultado);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }


        [HttpPut, Route("/actualizar/funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarFuncionario([FromBody] Post_Func_DTO funcionario)
        {
            try
            {
                var resultado = await _service.AtualizarFuncionarioAsync(funcionario);

                if (!resultado.Contains("sucesso", StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }


        [HttpPut("/actualizar/avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarAvatarFuncionario([FromForm] Update_Avatar_DTO avatar)
        {
            try
            {
                var resultado = await _service.AtualizarAvatarFuncionarioAsync(avatar);

                if (resultado != "Avatar atualizado com sucesso")
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }


        [HttpPut("/editar/estado/funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarEstadoFuncionario([FromBody] Put_UserState_DTO estado)
        {
            try
            {
                var resultado = await _service.AtualizarEstadoFuncionarioAsync(estado.UserId, estado.EstadoFuncionario);

                if (!resultado)
                {
                    return NotFound("Funcionário não encontrado");
                }

                return Ok("Estado atualizado com sucesso");
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }


        [HttpPut("/editar/senha/funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarSenhaFuncionario([FromBody] Put_Password_DTO password)
        {
            try
            {
                var resultado = await _service.AtualizarSenhaFuncionarioAsync(password.UserId, password.NewPassword);

                if (!resultado)
                {
                    return NotFound("Funcionário não encontrado");
                }

                return Ok("Senha actualizada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }



        [HttpPost("/autenticar/funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AutenticarFuncionario([FromBody] Login_Func_DTO login)
        {
            try
            {
                var funcionario = await _service.AutenticarFuncionarioAsync(login);

                if (funcionario == null)
                {
                    return Unauthorized("Credenciais inválidas");
                }

                if (!"Activo".Equals(funcionario.EstadoFuncionario))
                {
                    return Unauthorized("Funcionário sem permissão para entrar no sistema. Por favor contacte um dos gerentes.");
                }

                return Ok(funcionario);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }


        [HttpGet, Route("/listar/funcionarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodosFuncionarios()
        {
            try
            {
                var funcionarios = await _service.ObterTodosFuncionariosAsync();
                return Ok(funcionarios);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }



        [HttpGet("/listar/funcionarios/categoria/{idCategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterFuncionariosPorCategoria(int idCategoria)
        {
            try
            {
                var funcionarios = await _service.ObterFuncionariosPorCategoriaAsync(idCategoria);
                return Ok(funcionarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet, Route("/listar/categorias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterCategoriasAsync()
        {
            try
            {
                var categorias = await _service.ObterCategoriasAsync();
                return Ok(categorias);
            }
            catch
            {
                return StatusCode(500, $"Erro interno");
            }
        }
    }
}
