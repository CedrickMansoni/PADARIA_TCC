using System;
using Padaria.Share.Cliente.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Cliente;

public interface IClienteRepository
{
    Task<string> AdicionarCliente(Post_Cliente_DTO cliente);
    Task<string> ActualizarCliente(Get_Cliente_DTO cliente);
    Task<string> DeletarCliente(int id);
    Task<Get_Cliente_DTO?> ListarCliente(int id);
    Task<IEnumerable<Get_Cliente_DTO>> ListarClientes();

}
