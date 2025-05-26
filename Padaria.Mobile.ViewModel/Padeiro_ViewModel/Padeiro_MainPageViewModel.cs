using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Funcionario.DTO;
using Padaria.Share.Producao.DTO;
using Padaria.Share.Produto.DTO;

namespace Padaria.Mobile.ViewModel.Padeiro_ViewModel;
// Adicionamos a propriedade para receber o parâmetro via Shell
[QueryProperty(nameof(Funcionario_), "funcLogado")]
public class Padeiro_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Padeiro_MainPageViewModel()
    {
        client = new HttpClient()
        {
            BaseAddress = new Uri($"{Share.DNS_App.My_DNS.App_DNS}")
        };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private string _funcionario = string.Empty;
    public string Funcionario_
    {
        get => _funcionario;
        set
        {
            _funcionario = value;
            OnPropertyChanged(nameof(Funcionario_));
            if (!string.IsNullOrEmpty(_funcionario))
            {
                // Desserializa o JSON de volta para um objeto
                Funcionario = JsonSerializer.Deserialize<Get_Func_DTO>(_funcionario) ?? new Get_Func_DTO();
            }
        }
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

    public ICommand LogountCommand => new Command(async () =>
    {
        bool sair = await Shell.Current.DisplayAlert("...", "Deseja realmente terminar sessão?", "Sim", "Não");

        if (!sair) return;

        SecureStorage.Default.RemoveAll();

        await Shell.Current.GoToAsync("//Login_Telefone");

    });


    private ObservableCollection<Get_Capacidade_Producao> _capacidadeProducao = [];
    public ObservableCollection<Get_Capacidade_Producao> CapacidadeProducao
    {
        get => _capacidadeProducao;
        set
        {
            _capacidadeProducao = value;
            OnPropertyChanged(nameof(CapacidadeProducao));
        }
    }

    public ICommand ListarCapacidadeProducaoCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/capacidade/producao");
        if (response.IsSuccessStatusCode)
        {
            using var json = await response.Content.ReadAsStreamAsync();
            CapacidadeProducao = JsonSerializer.Deserialize<ObservableCollection<Get_Capacidade_Producao>>(json, options) ?? [];
        }
    });
    public ICommand AddCapacidadeProducaoCommand => new Command<Get_Produto_DTO>(async produto_ =>
    {
        string qtd = await Shell.Current.DisplayPromptAsync("Adicionar Capacidade de Produção", "Digite a quantidade máxima:", initialValue: "0");
        try
        {
            if (string.IsNullOrEmpty(qtd) || Convert.ToInt32(qtd) == 0) return;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
            return;
        }

        var newCapacidade = new Post_Capacidade_Producao
        {
            Produto = produto_.Id,
            QuantidadeMaxima = Convert.ToInt32(qtd),
            DataProducao = DateTime.Now
        };

        var json = JsonSerializer.Serialize(newCapacidade);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("adicionar/capacidade/producao", content);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            await Shell.Current.DisplayAlert("Sucesso", "Capacidade de produção adicionada com sucesso!", "OK");
            // Atualiza a lista de capacidade de produção
            ListarCapacidadeProducaoCommand.Execute(null);
        }
    });


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

    public ICommand ListarProdutosCommand => new Command(async () =>
   {
       var response = await client.GetAsync("listar/produtos");
       if (response.IsSuccessStatusCode)
       {
           var content = await response.Content.ReadAsStringAsync();
           Produtos = JsonSerializer.Deserialize<ObservableCollection<Get_Produto_DTO>>(content, options);
       }
   });

    private Get_Producao_DTO? _producao = new();
    public Get_Producao_DTO? Producao
    {
        get => _producao;
        set
        {
            _producao = value;
            OnPropertyChanged(nameof(Producao));
        }
    }

    private ObservableCollection<Get_Producao_DTO> producaoPedidos = [];
    public ObservableCollection<Get_Producao_DTO> ProducaoPedidos
    {
        get => producaoPedidos;
        set
        {
            producaoPedidos = value;
            OnPropertyChanged(nameof(ProducaoPedidos));
        }
    }

    public ICommand ListarProducaoCommand => new Command(async () =>
    {
        var response = await client.GetAsync($"listar/producao/data/{DateTime.Now.ToString("yyyy-MM-dd")}");
        if (response.IsSuccessStatusCode)
        {
            using var content = await response.Content.ReadAsStreamAsync();
            ProducaoPedidos = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];
        }
    });

    public ICommand ListarPedidosCommand => new Command(async () =>
{
    var response = await client.GetAsync($"listar/producao");
    if (response.IsSuccessStatusCode)
    {
        using var content = await response.Content.ReadAsStreamAsync();
        ProducaoPedidos = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];
    }
});


}
