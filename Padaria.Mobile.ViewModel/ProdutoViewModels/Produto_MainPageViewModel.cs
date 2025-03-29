using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Mobile.ViewModel.INavigationServices;
using Padaria.Share.DNS_App;
using Padaria.Share.Produto.DTO;

namespace Padaria.Mobile.ViewModel.ProdutoViewModels;

public class Produto_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Produto_MainPageViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private Get_Produto_DTO produto = new();
    public Get_Produto_DTO Produto
    {
        get => produto;
        set
        {
            produto = value;
            OnPropertyChanged(nameof(Produto));
        }
    }

    private ObservableCollection<Get_Produto_DTO>? produtos = [];
    public ObservableCollection<Get_Produto_DTO>? Produtos
    {
        get => produtos;
        set
        {
            produtos = value;
            OnPropertyChanged(nameof(Produtos));
        }
    }

    public ICommand GotoCadastrarProdutoCommand => new Command(async () =>
    {   
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Produto_PostPage");
        ActivityCommand.Execute(null);
    });

    public ICommand ListarProdutosCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/produtos");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Produtos = JsonSerializer.Deserialize<ObservableCollection<Get_Produto_DTO>>(content, options);
        }
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

    public ICommand ActivityCommand => new Command(() =>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });

}
