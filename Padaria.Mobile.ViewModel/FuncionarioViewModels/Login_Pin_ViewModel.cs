using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Mobile.ViewModel.INavigationServices;
using Padaria.Share.DNS_App;
using Padaria.Share.Funcionario.DTO;
using Padaria.Share.Hash_Password;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

[QueryProperty(nameof(Telefone), "telef")]
public class Login_Pin_ViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Login_Pin_ViewModel()
    {
        client = new HttpClient { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    private string telefone = string.Empty;
    public string Telefone
    {
        get => telefone;
        set { telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    private string pin = string.Empty;
    public string Pin
    {
        get => pin;
        set { pin = value; OnPropertyChanged(nameof(Pin)); }
    }


    private string nBorder0 = "#d7d7d7";
    public string NBorder0 { get => nBorder0; set { nBorder0 = value; OnPropertyChanged(nameof(NBorder0)); } }
    private string nBorder1 = "#d7d7d7";
    public string NBorder1 { get => nBorder1; set { nBorder1 = value; OnPropertyChanged(nameof(NBorder1)); } }
    private string nBorder2 = "#d7d7d7";
    public string NBorder2 { get => nBorder2; set { nBorder2 = value; OnPropertyChanged(nameof(NBorder2)); } }
    private string nBorder3 = "#d7d7d7";
    public string NBorder3 { get => nBorder3; set { nBorder3 = value; OnPropertyChanged(nameof(NBorder3)); } }
    private string nBorder4 = "#d7d7d7";
    public string NBorder4 { get => nBorder4; set { nBorder4 = value; OnPropertyChanged(nameof(NBorder4)); } }
    private string nBorder5 = "#d7d7d7";
    public string NBorder5 { get => nBorder5; set { nBorder5 = value; OnPropertyChanged(nameof(NBorder5)); } }

    public ICommand AddPinCommand => new Command<string>(async n =>
    {
        Pin += n;

        if ("#d7d7d7".Equals(NBorder0))
        {
            NBorder0 = "#5e5e5e";
            return;
        }
        if ("#d7d7d7".Equals(NBorder1))
        {
            NBorder1 = "#5e5e5e";
            return;
        }
        if ("#d7d7d7".Equals(NBorder2))
        {
            NBorder2 = "#5e5e5e";
            return;
        }
        if ("#d7d7d7".Equals(NBorder3))
        {
            NBorder3 = "#5e5e5e";
            return;
        }
        if ("#d7d7d7".Equals(NBorder4))
        {
            NBorder4 = "#5e5e5e";
            return;
        }
        if ("#d7d7d7".Equals(NBorder5))
        {
            NBorder5 = "#5e5e5e";

            ActivityCommand.Execute(null);

            Pin = new Hash_PWD().HashSenha(Pin);

            var credenciais = new Login_Func_DTO
            {
                TelefoneFuncionario = Telefone,
                SenhaFuncionario = Pin
            };

            string jsonCredenciais = JsonSerializer.Serialize<Login_Func_DTO>(credenciais, options);

            StringContent content = new(jsonCredenciais, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("autenticar/funcionario", content);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var funcionario = await JsonSerializer.DeserializeAsync<Get_Func_DTO>(stream, options);
                if (funcionario is null) return;
                string jsonFuncionario = JsonSerializer.Serialize(funcionario);

                switch (funcionario.Categoria)
                {
                    case "Administrador":
                        await Shell.Current.GoToAsync($"//Admin_MainPage?funcLogado={Uri.EscapeDataString(jsonFuncionario)}");
                        break;
                    case "Gerente":
                        await Shell.Current.GoToAsync($"//Gerente_MainPage?funcLogado={Uri.EscapeDataString(jsonFuncionario)}");
                        break;
                    case "Caixa":
                        await Shell.Current.GoToAsync($"//Caixa_MainPage?funcLogado={Uri.EscapeDataString(jsonFuncionario)}");
                        break;
                     case "Padeiro":
                        await Shell.Current.GoToAsync($"//Padeiro_MainPage?funcLogado={Uri.EscapeDataString(jsonFuncionario)}");
                        break;
                    default:
                        break;
                }
                ActivityCommand.Execute(null);
                await SecureStorage.Default.SetAsync("IdUsuario", funcionario.Id.ToString());
            }
            else
            {
                // Captura a mensagem de erro do servidor
                var errorMessage = await response.Content.ReadAsStringAsync();

                // Exibe a mensagem para o usuário
                foreach (var p in Pin)
                {
                    DeletePinCommand.Execute(null);
                }
                ActivityCommand.Execute(null);
                await Shell.Current.DisplayAlert("Erro de Autenticação", errorMessage, "OK");
            }
            return;
        }
    });

    public ICommand DeletePinCommand => new Command(() =>
    {
        if (Pin.Length == 0) return;
        string texto = Pin;
        Pin = texto[..^1];
        if (!"#d7d7d7".Equals(NBorder5))
        {
            NBorder5 = "#d7d7d7";
            return;
        }
        if (!"#d7d7d7".Equals(NBorder4))
        {
            NBorder4 = "#d7d7d7";
            return;
        }
        if (!"#d7d7d7".Equals(NBorder3))
        {
            NBorder3 = "#d7d7d7";
            return;
        }
        if (!"#d7d7d7".Equals(NBorder2))
        {
            NBorder2 = "#d7d7d7";
            return;
        }
        if (!"#d7d7d7".Equals(NBorder1))
        {
            NBorder1 = "#d7d7d7";
            return;
        }
        if (!"#d7d7d7".Equals(NBorder0))
        {
            NBorder0 = "#d7d7d7";
            return;
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
