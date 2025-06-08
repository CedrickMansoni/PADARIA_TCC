using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.DNS_App;
using Padaria.Share.Producao.DTO;

namespace Padaria.Mobile.ViewModel.ClienteViewModels;

public class Pedidos_ClientesViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
    public Pedidos_ClientesViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
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
                        ListarPedidosCommand.Execute(null);
                        FiltrarPedidosCommand.Execute(null);
                    }
                });
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        });
    }

    private bool estadoPedido = false;

    private Timer _timer;
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

    private ObservableCollection<Get_Producao_DTO> _pedidos = [];
    public ObservableCollection<Get_Producao_DTO> Pedidos
    {
        get => _pedidos;
        set
        {
            _pedidos = value;
            OnPropertyChanged(nameof(Pedidos));
        }
    }

    public ICommand ListarPedidosCommand => new Command(async () =>
    {
        int id = Convert.ToInt32(await SecureStorage.Default.GetAsync("IdUsuario"));
        var response = await client.GetAsync($"listar/producao/cliente/{id}");
        if (response.IsSuccessStatusCode)
        {
            using var content = await response.Content.ReadAsStreamAsync();
            var l = JsonSerializer.Deserialize<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];
            Pedidos = new ObservableCollection<Get_Producao_DTO>(l.Where(x => x.Estado != "Pendente por falta de pagamento"));

            CalcularTotalPagarCommand.Execute(false);
        }
    });

    private decimal totalPagar;
    public decimal TotalPagar
    {
        get => totalPagar;
        set
        {
            totalPagar = value;
            OnPropertyChanged(nameof(TotalPagar));
        }
    }
    public ICommand CalcularTotalPagarCommand => new Command(() =>
    {
        if (estadoPago)
        {
            TotalPagar = 0;
            return;
        }
        decimal total = 0;
        foreach (var pedido in Pedidos)
        {
            total += pedido.PrecoTotal;
        }
        TotalPagar = total;
    });

    private bool estadoPago = false;
    public bool EstadoPago
    {
        get => estadoPago;
        set
        {
            estadoPago = value;
            OnPropertyChanged(nameof(EstadoPago));
        }
    }
    private bool estadoPendente = true;
    public bool EstadoPendente
    {
        get => estadoPendente;
        set
        {
            estadoPendente = value;
            OnPropertyChanged(nameof(EstadoPendente));
        }
    }

    public ICommand FiltrarPedidosCommand => new Command(async () =>
    {
        int id = Convert.ToInt32(await SecureStorage.Default.GetAsync("IdUsuario"));
        if (EstadoPendente)
        {
            var response = await client.GetAsync($"listar/producao/cliente/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var content = await response.Content.ReadAsStreamAsync();
                Pedidos = JsonSerializer.Deserialize<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];
                CalcularTotalPagarCommand.Execute(null);
            }
            return;
        }
        if (estadoPago)
        {
            var response = await client.GetAsync($"listar/producao/cliente/pagamento/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var content = await response.Content.ReadAsStreamAsync();
                Pedidos = JsonSerializer.Deserialize<ObservableCollection<Get_Producao_DTO>>(content, options) ?? [];
                CalcularTotalPagarCommand.Execute(null);
            }
        }
    });

}
