using System;
using Microsoft.EntityFrameworkCore;
using Padaria.Share.Cliente.DTO;
using Padaria.WebApi.Data;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Cliente;

public class ClienteRepository(AppDataContext context) : IClienteRepository
{
    private readonly AppDataContext context = context;

    public async Task<string> ActualizarCliente(Get_Cliente_DTO cliente)
    {
        var clienteExistente = await context.TabelaClienteModel.FindAsync(cliente.Id);
        if (clienteExistente == null) return "Cliente não encontrado";

        var telefoneExistente = await context.TabelaTelefoneModel.FirstOrDefaultAsync(t => t.IdCliente == cliente.Id);
        if (telefoneExistente != null)
        {
            telefoneExistente.NumeroTelefone = cliente.Telefone;
            context.TabelaTelefoneModel.Update(telefoneExistente);
        }

        clienteExistente.Nome = cliente.Nome;
        //clienteExistente.Senha = cliente.Senha;
        clienteExistente.Nif = cliente.Nif;

        await context.SaveChangesAsync();
        return "Cliente actualizado com sucesso";
    }

    public async Task<string> AdicionarCliente(Post_Cliente_DTO cliente)
    {

        var clienteExistente = await context.TabelaClienteModel.FirstOrDefaultAsync(c => c.Nif == cliente.Nif);
        if (clienteExistente != null) return "Cliente já existe";

        var telefoneExistente = await context.TabelaTelefoneModel.FirstOrDefaultAsync(t => t.NumeroTelefone == cliente.Telefone);
        if (telefoneExistente != null) return "Telefone já existe";



        var transacao = await context.Database.BeginTransactionAsync();
        try
        {
            var clienteModel = new ClienteModel
            {
                IdCategoria = 6,
                Nome = cliente.Nome,
                Senha = cliente.Telefone + cliente.Nif,
                Nif = cliente.Nif,
                Estado = "Activo",
            };
            await context.TabelaClienteModel.AddAsync(clienteModel);
            await context.SaveChangesAsync();
            clienteModel = await context.TabelaClienteModel.OrderByDescending(i  => i.Id).FirstOrDefaultAsync(c => c.Nif == cliente.Nif);
            if (clienteModel == null) throw new Exception("Erro ao adicionar cliente");
            var clienteTelefone = new TelefoneModel
            {
                IdCliente = clienteModel.Id,
                NumeroTelefone = cliente.Telefone
            };
            await context.TabelaTelefoneModel.AddAsync(clienteTelefone);
            await context.SaveChangesAsync();
            await transacao.CommitAsync();
            return "Cliente adicionado com sucesso";
        }
        catch (Exception ex)
        {
            await transacao.RollbackAsync();
            return $"Erro ao adicionar cliente: {ex.Message}";
        }

    }

    public async Task<string> DeletarCliente(int id)
    {
        var cliente = await context.TabelaClienteModel.FindAsync(id);
        if (cliente == null) return "Cliente não encontrado";

        var telefone = await context.TabelaTelefoneModel.FirstOrDefaultAsync(t => t.IdCliente == id);
        if (telefone != null)
        {
            context.TabelaTelefoneModel.Remove(telefone);
        }

        context.TabelaClienteModel.Remove(cliente);
        await context.SaveChangesAsync();
        return "Cliente deletado com sucesso";
    }

    public async Task<Get_Cliente_DTO?> ListarCliente(string telefone)
    {
        var cliente = from c in context.TabelaClienteModel
                      join t in context.TabelaTelefoneModel on c.Id equals t.IdCliente
                      where t.NumeroTelefone == telefone
                      select new Get_Cliente_DTO
                      {
                          Id = c.Id,
                          Nome = c.Nome,
                          Nif = c.Nif,
                          Telefone = t.NumeroTelefone,
                          Estado = c.Estado
                      };
        return await cliente.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Get_Cliente_DTO>> ListarClientes()
    {
        var cliente = from c in context.TabelaClienteModel
                      join t in context.TabelaTelefoneModel on c.Id equals t.IdCliente
                      where c.Estado == "Activo"
                      select new Get_Cliente_DTO
                      {
                          Id = c.Id,
                          Nome = c.Nome,
                          Nif = c.Nif,
                          Telefone = t.NumeroTelefone
                      };
        return await cliente.ToListAsync();
    }
}
