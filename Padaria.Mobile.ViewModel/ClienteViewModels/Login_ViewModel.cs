using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Cliente.DTO;
using Padaria.Share.DNS_App;

namespace Padaria.Mobile.ViewModel.ClienteViewModels;

public class Login_ViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Login_ViewModel()
    {
        client = new HttpClient { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private string telefone;
    public string Telefone
    {
        get => telefone;
        set { telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    private Get_Cliente_DTO cliente;
    public Get_Cliente_DTO Cliente
    {
        get => cliente;
        set { cliente = value; OnPropertyChanged(nameof(Cliente)); }
    }

    public ICommand LoginCommand => new Command(async () =>
    {
        if (string.IsNullOrEmpty(Telefone))
        {
            await Shell.Current.DisplayAlert("Erro", "Informe o numero de telefone da sua padaria", "Ok");
            return;
        }

        try
        {
            var response = await client.GetAsync($"listar/cliente/{Telefone}");

            if (response.IsSuccessStatusCode)
            {
                // Handle successful login
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var cliente_ = await JsonSerializer.DeserializeAsync<Get_Cliente_DTO>(responseStream, options);
                if (cliente_ == null)
                {
                    await Shell.Current.DisplayAlert("Erro", "Cliente não encontrado", "Ok");
                    return;
                }
                if (cliente_.Estado != "Activo")
                {
                    await Shell.Current.DisplayAlert("Erro", "Cliente não está activo", "Ok");
                    return;
                }
                await SecureStorage.Default.SetAsync("telefoneUsuario", Telefone);
                await SecureStorage.Default.SetAsync("IdUsuario", cliente_.Id.ToString());
                await SecureStorage.Default.SetAsync("CategoriaUsuario", "Pessoa Juridica");
                await Shell.Current.GoToAsync("//Produtos_ClienteView");

                return;
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Login nao realizado com sucesso", "Ok");
                return;
            }
        }
        catch (System.Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"{ex}", "Ok");
        }
    });
}
