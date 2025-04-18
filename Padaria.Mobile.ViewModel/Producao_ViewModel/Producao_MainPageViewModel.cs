using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.DNS_App;
using Padaria.Share.Producao.DTO;
using Padaria.Share.Produto.DTO;

namespace Padaria.Mobile.ViewModel.Producao_ViewModel;

public class Producao_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Producao_MainPageViewModel()
    {
        client = new HttpClient(){BaseAddress = new Uri($"{My_DNS.App_DNS}")};
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }

    private Post_Producao_DTO producao = new();
    public Post_Producao_DTO Producao
    {
        get => producao;
        set
        {
            producao = value;
            OnPropertyChanged(nameof(Producao));
        }
    }

    private ObservableCollection<Post_Producao_DTO> producoes = [];
    public ObservableCollection<Post_Producao_DTO> Producoes
    {
        get => producoes;
        set
        {
            producoes = value;
            OnPropertyChanged(nameof(Producoes));
        }
    }

    public ICommand AddProdutoCommand => new Command<Get_Produto_DTO>(async p =>
    {
        if (p == null) return;
        int funcionarioId = Convert.ToInt32(await SecureStorage.Default.GetAsync("IdUsuario"));
        var newProducao = new Post_Producao_DTO
        {
            Produto = p.Id,
            Nome = p.Nome ?? string.Empty, 
            Imagem = p.Imagem ?? string.Empty, 
            Quantidade = 1,
            Funcionario = funcionarioId
        };

        Producoes.Add(newProducao);

    });

    public ICommand RemoveProdutoCommand => new Command<Post_Producao_DTO>(producao =>
    {
        if (producao != null)
        {
            Producoes.Remove(producao);
            OnPropertyChanged(nameof(Producoes));
        }
    });

    public ICommand SalvarProducoesCommand => new Command(async () =>
    {
        try
        {
            ActivityCommand.Execute(null);
            var json = JsonSerializer.Serialize(Producoes, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("adicionar/producao", content);
            if (response.IsSuccessStatusCode)
            {
                ActivityCommand.Execute(null);
                // Handle success response
                await Shell.Current.DisplayAlert("Mensagem","Produçao solicitada com sucesso!","Ok");
                Producoes.Clear();
            }
            else
            {
                ActivityCommand.Execute(null);
                // Handle error response
               await Shell.Current.DisplayAlert("Erro",$"Error: {response.StatusCode}","Ok");
            }
        }
        catch (Exception ex)
        {
            ActivityCommand.Execute(null);
            // Handle exceptions
            Console.WriteLine($"Exception: {ex.Message}");
        }
    });
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
    private ObservableCollection<Get_Produto_DTO> produtos = [];
    public ObservableCollection<Get_Produto_DTO> Produtos
    {
        get => produtos;
        set
        {
            produtos = value;
            OnPropertyChanged(nameof(Produtos));
        }
    }

    public ICommand GetProdutosCommand => new Command(async () =>
    {
        try
        {
            var response = await client.GetAsync("listar/produtos");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Produtos = JsonSerializer.Deserialize<ObservableCollection<Get_Produto_DTO>>(json, options)?? [];
            }
            else
            {
                // Handle error response
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Exception: {ex.Message}");
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
