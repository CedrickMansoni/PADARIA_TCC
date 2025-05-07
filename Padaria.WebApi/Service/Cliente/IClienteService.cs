using System;
using Padaria.Share.Cliente.DTO;
using Padaria.WebApi.Repository.Cliente;

namespace Padaria.WebApi.Service.Cliente;

public interface IClienteService
{
    Task<string> AdicionarCliente(Post_Cliente_DTO cliente);
    Task<string> ActualizarCliente(Get_Cliente_DTO cliente);
    Task<string> DeletarCliente(int id);
    Task<Get_Cliente_DTO?> ListarCliente(int id);
    Task<IEnumerable<Get_Cliente_DTO>> ListarClientes();
}
