using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Cliente.DTO;
using Padaria.Share.DNS_App;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Cliente_ListarViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Cliente_ListarViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private Get_Cliente_DTO cliente = new();
    public Get_Cliente_DTO Cliente
    {
        get => cliente;
        set
        {
            cliente = value;
            OnPropertyChanged(nameof(Cliente));
        }
    }

    private ObservableCollection<Get_Cliente_DTO> clientes = new();
    public ObservableCollection<Get_Cliente_DTO> Clientes
    {
        get => clientes;
        set
        {
            clientes = value;
            OnPropertyChanged(nameof(Clientes));
        }
    }

    public ICommand ListarClientesCommand => new Command(async () =>
    {
        try
        {
            var response = await client.GetAsync("listar/clientes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Clientes = JsonSerializer.Deserialize<ObservableCollection<Get_Cliente_DTO>>(json, options) ?? [];
                // Process the list of clientes as needed
            }
        }
        catch
        {
            // Handle exceptions
        }
    });
    
    public ICommand AddClienteCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Cliente_AddView");
    });
}
