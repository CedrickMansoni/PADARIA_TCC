using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.DNS_App;
using Padaria.Share.Produto.DTO;

namespace Padaria.Mobile.ViewModel.ProdutoViewModels;

public class Produto_CategoriaViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    private readonly Produto_PostViewModel t;
    public Produto_CategoriaViewModel(Produto_PostViewModel t)
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        this.t = t;
    }

    private Post_Categ_Produto_DTO categoria = new();
    public Post_Categ_Produto_DTO Categoria
    {
        get => categoria;
        set
        {
            categoria = value;
            OnPropertyChanged(nameof(Categoria));
        }
    }

    private string _categoria = string.Empty;
    public string _Categoria
    {
        get => _categoria;
        set
        {
            _categoria = value;            
            OnPropertyChanged(nameof(_Categoria));
            Categoria.Categoria = _Categoria;
        }
    }


    private ObservableCollection<Get_Categ_Produto_DTO>? categorias = new();
    public ObservableCollection<Get_Categ_Produto_DTO>? Categorias
    {
        get => categorias;
        set
        {
            categorias = value;
            OnPropertyChanged(nameof(Categorias));
        }
    }

    public ICommand CadastrarCategoriaCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        var json = JsonSerializer.Serialize(Categoria);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("cadastrar/categoria", content);
        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Sucesso", "Categoria cadastrada com sucesso", "Ok");
            _Categoria = string.Empty;
            Categoria = new Post_Categ_Produto_DTO();
            ListarCategoriasCommand.Execute(null);
            EnableButtonCommand.Execute(null);
        }
        else
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Erro", "Erro ao cadastrar categoria", "Ok");
        }
    });

    public ICommand ListarCategoriasCommand => new Command(async () =>
    {
        var response = await client.GetAsync("pesquisar/categorias");
        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            Categorias = await JsonSerializer.DeserializeAsync<ObservableCollection<Get_Categ_Produto_DTO>>(stream, options);
        }
        else
        {
            await Shell.Current.DisplayAlert("Erro", "Erro ao listar categorias", "Ok");
        }
    });

    public ICommand SelecionarCategoriaCommand => new Command<Post_Categ_Produto_DTO>(async categ =>
    {
        t.Categoria = categ.Categoria;
        t.IdCategoria = categ.Id;
        await Shell.Current.Navigation.PopAsync();
    });

    private bool buttonState = false;
    public bool ButtonState
    {
        get => buttonState;
        set
        {
            buttonState = value;
            OnPropertyChanged(nameof(ButtonState));
        }
    }

    public ICommand EnableButtonCommand => new Command(() => 
    ButtonState = !string.IsNullOrEmpty(Categoria.Categoria));


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
