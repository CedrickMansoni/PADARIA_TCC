using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Cliente.DTO;
using Padaria.Share.DNS_App;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Cliente_AddViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Cliente_AddViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private Post_Cliente_DTO cliente = new();
    public Post_Cliente_DTO Cliente
    {
        get => cliente;
        set
        {
            cliente = value;
            OnPropertyChanged(nameof(Cliente));
        }
    }

    public ICommand AddClienteCommand => new Command(async () =>
    {
        if (string.IsNullOrWhiteSpace(Cliente.Nome) || string.IsNullOrWhiteSpace(Cliente.Nif) || string.IsNullOrWhiteSpace(Cliente.Telefone))
        {
            await Shell.Current.DisplayAlert("Error", "Preencha todos os campos", "OK");
            return;
        }
        try
        {
            var json = JsonSerializer.Serialize(Cliente, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("adicionar/cliente", content);

            if (response.IsSuccessStatusCode)
            {
                // Handle success
                await Shell.Current.DisplayAlert("Success", "Cliente cadastrado com sucesso", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                // Handle error
                await Shell.Current.DisplayAlert("Error", "Falha no cadastro do cliente", "OK");
            }
        }
        catch 
        {
            // Handle exception
            await Shell.Current.DisplayAlert("Error", "Ocorreu um erro ao cadastrar o cliente", "OK");
        }
    });
}
