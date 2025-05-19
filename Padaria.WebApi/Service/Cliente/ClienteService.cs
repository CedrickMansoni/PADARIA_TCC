using System;
using Padaria.Share.Cliente.DTO;
using Padaria.WebApi.Repository.Cliente;

namespace Padaria.WebApi.Service.Cliente;

public class ClienteService(IClienteRepository repository) : IClienteService
{
    private readonly IClienteRepository _repository = repository;

    public async Task<string> ActualizarCliente(Get_Cliente_DTO cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        return await _repository.ActualizarCliente(cliente);
    }

    public async Task<string> AdicionarCliente(Post_Cliente_DTO cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        return await _repository.AdicionarCliente(cliente);
    }

    public async Task<string> DeletarCliente(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID deve ser maior que zero.");

        return await _repository.DeletarCliente(id);
    }

    public async Task<Get_Cliente_DTO?> ListarCliente(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
            throw new ArgumentOutOfRangeException(nameof(telefone), "ID deve ser maior que zero.");

        return await _repository.ListarCliente(telefone);
    }

    public async Task<IEnumerable<Get_Cliente_DTO>> ListarClientes()
    {
       return await _repository.ListarClientes();
    }
}
