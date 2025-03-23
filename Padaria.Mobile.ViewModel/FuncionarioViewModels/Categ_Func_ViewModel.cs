using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.DNS_App;
using Padaria.Share.Funcionario.DTO;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Categ_Func_ViewModel : BindableObject
{
    private readonly Criar_Conta_ViewModel contaPage;
    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;

    public Categ_Func_ViewModel(Criar_Conta_ViewModel T)
    {
        contaPage = T;
        client = new HttpClient { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //ListarCategorias.Execute(null);
    }

    private Get_Categ_Func_DTO categoria = new();
    public Get_Categ_Func_DTO Categoria
    {
        get => categoria;
        set { categoria = value; OnPropertyChanged(nameof(Categoria)); }
    }

    private ObservableCollection<Get_Categ_Func_DTO>? categorias = [];
    public ObservableCollection<Get_Categ_Func_DTO>? Categorias
    {
        get => categorias;
        set { categorias = value; OnPropertyChanged(nameof(Categorias)); }
    }

    public ICommand ListarCategorias => new Command(async () =>
    {
        var response = await client.GetAsync("listar/categorias");
        if (response.IsSuccessStatusCode)
        {
            using var streamCategorias = await response.Content.ReadAsStreamAsync();
            Categorias = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Categ_Func_DTO>>(streamCategorias, options);
        }
        else
        {
            await Shell.Current.DisplayAlert("Erro", "Não conseguimos carregar nenhuma categoria da base de dados", "Ok");
        }
    });

    public ICommand SelecionarCategoria => new Command<Get_Categ_Func_DTO>(async cat =>
    {
        contaPage.IdCategoria = cat.Id;
        contaPage.Categoria = cat.Categoria;
        await Shell.Current.Navigation.PopAsync();
    });
}
