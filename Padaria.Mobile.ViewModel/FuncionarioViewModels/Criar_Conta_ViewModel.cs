using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Mobile.ViewModel.INavigationServices;
using Padaria.Share.DNS_App;
using Padaria.Share.Funcionario.DTO;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Criar_Conta_ViewModel : BindableObject
{
    private readonly INavigationService navigation;
    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;
    public Criar_Conta_ViewModel(INavigationService navigation)
    {
        this.navigation = navigation;
        client = new HttpClient { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private int idCategoria;
    public int IdCategoria
    {
        get => idCategoria;
        set { idCategoria = value; OnPropertyChanged(nameof(IdCategoria)); }
    }
    private string categoria = string.Empty;
    public string Categoria
    {
        get => categoria;
        set { categoria = value; OnPropertyChanged(nameof(Categoria)); }
    }
    private string nome = string.Empty;
    public string Nome
    {
        get => nome;
        set { nome = value; OnPropertyChanged(nameof(Nome)); }
    }
    private string telefone = string.Empty;
    public string Telefone
    {
        get => telefone;
        set { telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    public ICommand GotoCategoriaPageCommand => new Command(async () =>
    {
        await navigation.GotoCategoriaPage(this);
    });

    private bool estadoBotao = false;
    public bool EstadoBotao
    {
        get => estadoBotao;
        set { estadoBotao = value; OnPropertyChanged(nameof(EstadoBotao)); }
    }

    public ICommand EstadoBotaoCommand => new Command(() =>
    {
        EstadoBotao = !string.IsNullOrEmpty(Categoria) &&
                      !string.IsNullOrEmpty(Nome) &&
                      !string.IsNullOrEmpty(Telefone);
    });

    public ICommand CadastrarCommand => new Command(async () =>
    {
        if (!EstadoBotao) return;

        var funcionario = new Post_Func_DTO
        {
            IdCategoria = IdCategoria,
            NomeFuncionario = Nome,
            TelefoneFuncionario = Telefone
        };

        string json = JsonSerializer.Serialize<Post_Func_DTO>(funcionario, options);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("cadastrar/funcionario", content);

        if (response.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("...", $"Funcionário(a) {Nome}, foi cadastrado(a) com sucesso", "Ok");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro: {response.StatusCode}, Conteúdo: {errorContent}");
            await Shell.Current.DisplayAlert("Erro", $"{errorContent}", "Ok");
        }
    });
}
