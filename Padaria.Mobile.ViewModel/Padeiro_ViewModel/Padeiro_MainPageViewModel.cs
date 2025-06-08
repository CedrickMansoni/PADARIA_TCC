using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Funcionario.DTO;
using Padaria.Share.Pedido.DTO;
using Padaria.Share.Producao.DTO;
using Padaria.Share.Produto.DTO;
using Microsoft.Maui.Storage;

namespace Padaria.Mobile.ViewModel.Padeiro_ViewModel;
// Adicionamos a propriedade para receber o parâmetro via Shell
[QueryProperty(nameof(Funcionario_), "funcLogado")]
public class Padeiro_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    private Timer _timer;
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

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _timer = new Timer(_ =>
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Hora = DateTime.Now.ToString("T");
                    if (Convert.ToDateTime(hora).Second == 10 ||
                        Convert.ToDateTime(hora).Second == 20 ||
                        Convert.ToDateTime(hora).Second == 30 ||
                        Convert.ToDateTime(hora).Second == 40 ||
                        Convert.ToDateTime(hora).Second == 50)
                    {
                        ListarCapacidadeProducaoCommand.Execute(null);
                        ListarPedidosCommand.Execute(null);
                        ListarProducaoCommand.Execute(null);
                    }
                });
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        });
    }

    private string hora = string.Empty;
    public string Hora
    {
        get => hora;
        set
        {
            hora = value;
            OnPropertyChanged(nameof(Hora));
        }
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
            var l = JsonSerializer.Deserialize<ObservableCollection<Get_Capacidade_Producao>>(json, options) ?? [];
            if (CapacidadeProducao.Count == 0)
            {
                CapacidadeProducao = l;
                return;
            }
            foreach (var item in CapacidadeProducao)
            {
                var i = l.FirstOrDefault(x => x.Id == item.Id);
                if (i is not null) l.Remove(i);
            }
            if (l.Count == 0) return;
            foreach (var item in l)
            {
                CapacidadeProducao.Add(item);
            }
        }
    });
    public ICommand AddCapacidadeProducaoCommand => new Command<Get_Produto_DTO>(async produto_ =>
    {
        int quantidade = 0;
        string qtd = await Shell.Current.DisplayPromptAsync("Adicionar Capacidade de Produção", "Digite a quantidade máxima:", initialValue: "0", keyboard: Keyboard.Numeric);
        try
        {
            if (int.TryParse(qtd, out int quantidadeMaxima))
            {
                quantidade = quantidadeMaxima;
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Digite apenas números inteiros maior que zero.", "OK");
            }

        }
        catch
        {
            await Shell.Current.DisplayAlert("Erro", "Digite apenas números inteiros maior que zero.", "OK");
            return;
        }
        if (quantidade <= 0) { await Shell.Current.DisplayAlert("Erro", "Digite apenas números inteiros maior que zero.", "OK"); return; }
        if (CapacidadeProducao.Any(p => p.IdProduto == produto_.Id))
        {
            await Shell.Current.DisplayAlert("Erro", "Este produto já faz parte da produção de hoje.", "OK");
            return;
        }
        var newCapacidade = new Post_Capacidade_Producao
        {
            Produto = produto_.Id,
            QuantidadeMaxima = quantidade,
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

    public ICommand ListarProdutosCommand => new Command(async () =>
   {
       var response = await client.GetAsync("listar/produtos");
       if (response.IsSuccessStatusCode)
       {
           var content = await response.Content.ReadAsStringAsync();
           var l = JsonSerializer.Deserialize<ObservableCollection<Get_Produto_DTO>>(content, options) ?? [];
           if (Produtos.Count == 0)
           {
               Produtos = l;
               return;
           }
           foreach (var item in Produtos)
           {
               var i = l.FirstOrDefault(x => x.Id == item.Id);
               if (i is not null) l.Remove(i);
           }
           if (l.Count == 0) return;
           foreach (var item in l)
           {
               Produtos.Add(item);
           }
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

    private ObservableCollection<Get_Producao_DTO> producaoPadaria = [];
    public ObservableCollection<Get_Producao_DTO> ProducaoPadaria
    {
        get => producaoPadaria;
        set
        {
            producaoPadaria = value;
            OnPropertyChanged(nameof(ProducaoPadaria));
        }
    }

    private ObservableCollection<Get_Producao_DTO> producaoPedidosConfirmados = [];
    public ObservableCollection<Get_Producao_DTO> ProducaoPedidosConfirmados
    {
        get => producaoPedidosConfirmados;
        set
        {
            producaoPedidosConfirmados = value;
            OnPropertyChanged(nameof(ProducaoPedidosConfirmados));
        }
    }

    public ICommand ListarProducaoCommand => new Command(async () =>
    {
        var response = await client.GetAsync($"listar/producao/data/{DateTime.Now.ToString("yyyy-MM-dd")}");
        if (response.IsSuccessStatusCode)
        {
            using var content = await response.Content.ReadAsStreamAsync();
            var l = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];

            if (ProducaoPadaria.Count == 0)
            {
                ProducaoPadaria = l;
                return;
            }
            foreach (var item in ProducaoPadaria)
            {
                var i = l.FirstOrDefault(x => x.Id == item.Id);

                if (i is not null)
                {
                    item.Estado = i.Estado;
                    l.Remove(i);
                }
            }
            if (l.Count == 0) return;
            foreach (var item in l)
            {
                ProducaoPadaria.Add(item);
            }
        }
    });

    public ICommand ListarPedidosCommand => new Command(async () =>
    {
        var response = await client.GetAsync($"listar/producao");
        if (response.IsSuccessStatusCode)
        {
            using var content = await response.Content.ReadAsStreamAsync();
            var lista = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];

            var l = new ObservableCollection<Get_Producao_DTO>(lista.Where(x => x.Estado == "Pendente por falta de pagamento"));
            if (ProducaoPedidos.Count == 0)
            {
                ProducaoPedidos = l;
                return;
            }
            foreach (var item in ProducaoPedidos)
            {
                var i = l.FirstOrDefault(x => x.Id == item.Id);
                if (i is not null)
                {
                    item.Estado = i.Estado;
                    l.Remove(i);
                }
            }
            if (l.Count == 0) return;
            foreach (var item in l)
            {
                ProducaoPedidos.Add(item);
            }

            l = new ObservableCollection<Get_Producao_DTO>(lista.Where(x => x.Estado != "Pendente por falta de pagamento"));
            if (ProducaoPedidosConfirmados.Count == 0)
            {
                ProducaoPedidosConfirmados = l;
                return;
            }
            foreach (var item in ProducaoPedidosConfirmados)
            {
                var i = l.FirstOrDefault(x => x.Id == item.Id);
                if (i is not null)
                {
                    item.Estado = i.Estado;
                    l.Remove(i);
                }
            }
            if (l.Count == 0) return;
            foreach (var item in l)
            {
                ProducaoPedidosConfirmados.Add(item);
            }
        }
    });

    public ICommand RedefinirEstado => new Command<Get_Producao_DTO>(async p =>
    {
        // Inclua o estado atualizado no objeto
        var estado = new Put_PedidoState_DTO
        {
            IdPedido = p.Id,
            Estado = p.Estado.Contains("pagamento") ? "Pendente" : "Pendente por falta de pagamento" // Certifique-se de que este valor está correto
        };

        // Serializa o objeto com o estado atualizado
        string jsonEstado = JsonSerializer.Serialize<Put_PedidoState_DTO>(estado, options);
        StringContent content = new(jsonEstado, Encoding.UTF8, "application/json");

        // Envia a requisição PUT para o backend
        var response = await client.PutAsync("editar/estado/pedido", content);

        // Opcional: Verifique a resposta do backend
        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            var item = ProducaoPedidos.FirstOrDefault(x => x.Id == p.Id);
            if (item is null) return;
            int indice = ProducaoPedidos.IndexOf(item);
            producaoPedidos.RemoveAt(indice);
            item.Estado = "Pendente";
            producaoPedidosConfirmados.Add(item);
        }
        else
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", $"{successMessage}", "Ok");
        }
    });


    public ICommand ProducaoEstado => new Command<Get_Producao_DTO>(async p =>
    {
        string[] opcoes = { "Em andamento", "Concluído", "Cancelado" };

        string opcaoEscolhida = await Shell.Current.DisplayActionSheet(
            "Escolha um estado",  // Título
            "Cancelar",           // Botão de cancelar
            null,                 // Botão de destruição (pode ser null)
            opcoes);              // Opções

        if (string.IsNullOrEmpty(opcaoEscolhida)) return;
        // Inclua o estado atualizado no objeto
        var estado = new Put_PedidoState_DTO
        {
            IdPedido = p.Id,
            Estado = opcaoEscolhida // Certifique-se de que este valor está correto
        };

        // Serializa o objeto com o estado atualizado
        string jsonEstado = JsonSerializer.Serialize<Put_PedidoState_DTO>(estado, options);
        StringContent content = new(jsonEstado, Encoding.UTF8, "application/json");

        // Envia a requisição PUT para o backend
        var response = await client.PutAsync("editar/estado/pedido", content);

        // Opcional: Verifique a resposta do backend
        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            var item = ProducaoPedidos.FirstOrDefault(x => x.Id == p.Id);
            if (item is null) return;
            int indice = ProducaoPedidos.IndexOf(item);
            producaoPedidos.RemoveAt(indice);
            item.Estado = opcaoEscolhida;
            ProducaoPedidos.Insert(indice, item);
        }
        else
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", $"{successMessage}", "Ok");
        }
    });

    public ICommand DownloandComprovativoCommand => new Command<Get_Producao_DTO>(async p =>
    {
        try
        {
            var response = await client.GetAsync($"download/{p.Id}");

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var nomeArquivo = GetFileNameFromContentDisposition(response.Content.Headers);
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var caminho = Path.Combine(FileSystem.AppDataDirectory, nomeArquivo);

            File.WriteAllBytes(caminho, bytes);

            var StatusMensagem = $"Comprovativo salvo em: {caminho}";
            await Shell.Current.DisplayAlert("...", $"{StatusMensagem}", "Ok");


        }
        catch (Exception ex)
        {
            var StatusMensagem = $"Erro: {ex.Message}";
            await Shell.Current.DisplayAlert("Erro", $"Erro: {ex.Message}", "Ok");
            await Shell.Current.DisplayAlert("...", $"{StatusMensagem}", "Ok");
        }
    });

    private string GetFileNameFromContentDisposition(HttpContentHeaders headers)
    {
        if (headers.ContentDisposition != null && !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))
            return headers.ContentDisposition.FileName.Trim('"');

        return $"comprovativo_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
    }





}
