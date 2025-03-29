using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.DNS_App;
using Padaria.Share.Funcionario.DTO;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Listar_Func_ViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
    public Listar_Func_ViewModel()
    {
        client = new HttpClient { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private Get_Func_DTO funcionario = new();
    public Get_Func_DTO Funcionario
    {
        get => funcionario;
        set
        {
            funcionario = value;
            OnPropertyChanged(nameof(Funcionario));
        }
    }

    private ObservableCollection<Get_Func_DTO>? funcionarios = [];
    public ObservableCollection<Get_Func_DTO>? Funcionarios
    {
        get => funcionarios;
        set
        {
            funcionarios = value;
            OnPropertyChanged(nameof(Funcionarios));
        }
    }

    public ICommand ListarFuncionariosCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/funcionarios");
        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();

            Funcionarios = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Func_DTO>>(stream, options);

        }
    });

    public ICommand RedefinirSenha => new Command<Get_Func_DTO>(async user =>
    {
        var password = new Put_Password_DTO { UserId = user.Id };
        string jsonPassword = JsonSerializer.Serialize<Put_Password_DTO>(password, options);
        StringContent content = new(jsonPassword, Encoding.UTF8, "application/json");

        var response = await client.PutAsync("editar/senha/funcionario", content);

        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Sucesso", $"{successMessage}", "Ok");
        }
        else
        {
            var ErrorMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", $"{ErrorMessage}", "Ok");
        }
    });


    public ICommand RedefinirEstado => new Command<Put_UserState_DTO>(async user =>
    {
        // Inclua o estado atualizado no objeto
        var estado = new Put_UserState_DTO
        {
            UserId = user.UserId,
            EstadoFuncionario = user.EstadoFuncionario // Certifique-se de que este valor está correto
        };

        // Serializa o objeto com o estado atualizado
        string jsonEstado = JsonSerializer.Serialize<Put_UserState_DTO>(estado, options);
        StringContent content = new(jsonEstado, Encoding.UTF8, "application/json");

        // Envia a requisição PUT para o backend
        var response = await client.PutAsync("editar/estado/funcionario", content);

        // Opcional: Verifique a resposta do backend
        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
        }
    });

    public ICommand GotoContaPageCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Criar_Conta");
        ActivityCommand.Execute(null);
    });

    private bool activity = false;
    public bool Activity
    {
        get => activity;
        set
        {
            activity = value;
            OnPropertyChanged(nameof(Activity));
        }
    }

    private bool enablePage = true;
    public bool EnablePage
    {
        get => enablePage;
        set 
        {
            enablePage = value;
            OnPropertyChanged(nameof(EnablePage));
        }
    }

    public ICommand ActivityCommand => new Command(()=>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });
}
