using System;
using Padaria.Share.DNS_App;
using Padaria.Share.Funcionario.DTO;
using Padaria.Share.Hash_Password;
using Padaria.WebApi.Models;
using Padaria.WebApi.Repository.Funcionario;
using Padaria.WebApi.SMS_Service.Model_SMS;
using Padaria.WebApi.SMS_Service.Service;

namespace Padaria.WebApi.Service.Funcionario;

public class FuncionarioService(IFuncionarioRepository repository, IHash_PWD pwd, ISMS_enviar enviarSMS) : IFuncionarioService
{
    private readonly IFuncionarioRepository repository = repository;
    private readonly IHash_PWD hash_PWD = pwd;
    private readonly ISMS_enviar enviarSMS = enviarSMS;

    public async Task<string> AdicionarFuncionarioAsync(Post_Func_DTO funcionario)
    {
        var m = new MensagemResponse();
        int total_func = await repository.ContarFuncionarios();
        if (total_func == 0 && funcionario.IdCategoria != 1) return "O primeiro funcionário da plataforma deve ser obrigatóriamente um adminstrador.\nSelecione a categoria de Administrador";
        if (total_func >= 1 && funcionario.IdCategoria == 0) return "Informe a categoria do funcionário";
        if (string.IsNullOrEmpty(funcionario.NomeFuncionario)) return "Informe o nome do funcionário";
        if (string.IsNullOrEmpty(funcionario.TelefoneFuncionario)) return "Informe o telefone do funcionário";
        if (string.IsNullOrEmpty(funcionario.AvatarFuncionario)) funcionario.AvatarFuncionario = "user";
        if (string.IsNullOrEmpty(funcionario.EstadoFuncionario)) funcionario.EstadoFuncionario = "Activo";
        if (string.IsNullOrEmpty(funcionario.SenhaFuncionario))
        {
            string senha = new Random().Next(100000, 999999).ToString();
            string senhaHash = hash_PWD.HashSenha(senha);
            funcionario.SenhaFuncionario = hash_PWD.HashSenha(senhaHash);
            var mensagem = new Mensagem
            {
                PhoneNumber = funcionario.TelefoneFuncionario,
                MessageBody = $"Olá, {funcionario.NomeFuncionario}! 🎉\n\n" +
                $"Você foi cadastrado como {await repository.ObterCategoriaPorIdAsync(funcionario.IdCategoria)} na plataforma da Padaria Manuel & Filhos.\n\n" +
                $"Acesse com as credenciais:\n" +
                $"👤 Usuário: {funcionario.TelefoneFuncionario}\n" +
                $"🔑 Senha: {senha}\n\n" +
                $"Altere sua senha no primeiro acesso."
            };

            var sms = new EnviarMensagem
            {
                Mensagem = mensagem
            };

            await enviarSMS.EnviarSMS(sms);
    
        }
        return await repository.AdicionarFuncionarioAsync(funcionario);
    }

    public async Task<string> AtualizarAvatarFuncionarioAsync(Update_Avatar_DTO avatar)
    {
        if (avatar.Ficheiro == null || avatar.Ficheiro.Length == 0) return "Arquivo inválido.";

        var funcionario = await repository.ObterFuncionarioPorIdAsync(avatar.Id);
        if (funcionario is null) return "Não existe nenhum funcionário com o id fornecido";
        if (!"user".Equals(funcionario.AvatarFuncionario))
        {
            string filePath = Path.Combine(My_DNS.App_DNS, "images", "Avatar", $"{funcionario.AvatarFuncionario}");

            if (!System.IO.File.Exists(filePath)) return "Arquivo não encontrado";

            System.IO.File.Delete(filePath);

            string newfilePath = Path.Combine(My_DNS.App_DNS, "images", "Avatar", $"{avatar.Ficheiro.FileName}");

            using var stream = new FileStream(filePath, FileMode.Create);
            await avatar.Ficheiro.CopyToAsync(stream);
        }

        return await repository.AtualizarAvatarFuncionarioAsync(avatar.Id, avatar.Ficheiro.FileName);

    }

    public async Task<bool> AtualizarEstadoFuncionarioAsync(int id, string novoEstado)
    {
        return await repository.AtualizarEstadoFuncionarioAsync(id, novoEstado);
    }

    public async Task<string> AtualizarFuncionarioAsync(Post_Func_DTO funcionario)
    {
        if (funcionario.IdCategoria < 1) return "Informe a categoria do funcionário";
        if (string.IsNullOrEmpty(funcionario.NomeFuncionario)) return "Informe o nome do funcionário";
        if (string.IsNullOrEmpty(funcionario.TelefoneFuncionario)) return "Informe o telefone do funcionário";

        return await repository.AtualizarFuncionarioAsync(funcionario);
    }

    public async Task<bool> AtualizarSenhaFuncionarioAsync(int id, string novaSenha = "")
    {
        string senha = string.Empty;
        string senhaHash = string.Empty;

        if(string.IsNullOrEmpty(novaSenha))
        {
            senha = new Random().Next(100000, 999999).ToString();
            senhaHash = hash_PWD.HashSenha(senha);
        } else
        {
            senhaHash = novaSenha;
        }

        senhaHash = hash_PWD.HashSenha(senhaHash);
        var response = await repository.AtualizarSenhaFuncionarioAsync(id, senhaHash);
        var funcionario = await repository.ObterFuncionarioPorIdAsync(id);
        var mensagem = new Mensagem
        {
            PhoneNumber = funcionario!.TelefoneFuncionario,
            MessageBody = $"Olá, {funcionario.NomeFuncionario}! 🎉\n\n" +
            $"A sua senha foi redefinida na plataforma da Padaria Manuel & Filhos.\n\n" +
            $"Acesse com as credenciais:\n" +
            $"👤 Usuário: {funcionario.TelefoneFuncionario}\n" +
            $"🔑 Senha: {senha}\n\n" +
            $"Altere sua senha apos acesso."
        };

        var sms = new EnviarMensagem
        {
            Mensagem = mensagem
        };

        await enviarSMS.EnviarSMS(sms);
        return response;
    }

    public async Task<Get_Func_DTO?> AutenticarFuncionarioAsync(Login_Func_DTO login)
    {
        login.SenhaFuncionario = hash_PWD.HashSenha(login.SenhaFuncionario);
        return await repository.AutenticarFuncionarioAsync(login);
    }

    public async Task<IEnumerable<Get_Categ_Func_DTO>> ObterCategoriasAsync()
    {
        var categ = await repository.ObterCategoriasAsync();
        return categ.Select(x => new Get_Categ_Func_DTO
        {
            Id = x.Id,
            Categoria = x.Descricao
        });
    }

    public async Task<IEnumerable<Get_Func_DTO>> ObterFuncionariosPorCategoriaAsync(int idCategoria)
    {
        return await repository.ObterFuncionariosPorCategoriaAsync(idCategoria);
    }

    public async Task<IEnumerable<Get_Func_DTO>> ObterTodosFuncionariosAsync()
    {
        return await repository.ObterTodosFuncionariosAsync();
    }
}
