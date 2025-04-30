using System.ComponentModel.Design.Serialization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Mobile.ViewModel.INavigationServices;
using Padaria.Share.DNS_App;
using Padaria.Share.Produto.DTO;

namespace Padaria.Mobile.ViewModel.ProdutoViewModels;

public class Produto_PostViewModel : BindableObject
{
	HttpClient client;
	JsonSerializerOptions options;
	private readonly INavigationService _service;
	public Produto_PostViewModel(INavigationService service)
	{
		client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
		options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};
		_service = service;
	}

	private Post_Produto_DTO produto = new();
	public Post_Produto_DTO Produto
	{
		get => produto;
		set
		{
			produto = value;
			OnPropertyChanged(nameof(Produto));
		}
	}

	private int idCategoria;
	public int IdCategoria
	{
		get => idCategoria;
		set
		{
			idCategoria = value;
			OnPropertyChanged(nameof(IdCategoria));
			Produto.IdCategoria = value;
		}
	}

	private string categoria = string.Empty;
	public string Categoria
	{
		get => categoria;
		set
		{
			categoria = value;
			OnPropertyChanged(nameof(Categoria));
		}
	}

	public ICommand SelecionarCategoriaCommand => new Command(async () =>
	{

		await _service.GotoCategoriaProdutoPage(this);
	});

	private string caminhoImagem = string.Empty;
	public string CaminhoImagem
	{
		get => caminhoImagem;
		set
		{
			caminhoImagem = value;
			OnPropertyChanged(nameof(CaminhoImagem));
		}
	}

	public ICommand AbrirCameraCommand => new Command(async () =>
   {
	   bool response = await Shell.Current.DisplayAlert("...", "Como pretende obter a imagem do produto?", "Camera", "Galeria");
	   var doc = response ?
		   await MediaPicker.CapturePhotoAsync() :
		   await MediaPicker.PickPhotoAsync();

	   if (doc is null) return;

	   CaminhoImagem = doc.FullPath;

   });


	public ICommand CadastrarProdutoCommand => new Command(async () =>
	{
		ActivityCommand.Execute(null);
		Produto.IdFuncionario = Convert.ToInt32(await SecureStorage.Default.GetAsync("IdUsuario"));
		if (string.IsNullOrEmpty(Produto.Nome) || Produto.IdCategoria == 0 || Produto.IdFuncionario == 0 || Produto.Preco == 0 || Produto.Unidade == 0)
		{
			ActivityCommand.Execute(null);
			await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos!", "OK");
			return;
		}
		if (string.IsNullOrEmpty(CaminhoImagem))
		{
			ActivityCommand.Execute(null);
			await Shell.Current.DisplayAlert("Erro", "Selecione uma imagem!", "OK");
			return;
		}

		var formData = new MultipartFormDataContent
		{
			{ new StringContent(Produto.IdCategoria.ToString()), "idCategoria" },
			{ new StringContent(Produto.IdFuncionario.ToString()), "idFuncionario" },
			{ new StringContent(Produto.Nome), "nome" },
			{ new StringContent(Produto.Unidade.ToString()), "unidade" },
			{ new StringContent(Produto.Preco.ToString()), "preco" },
			{ new StringContent(Produto.Descricao), "descricao" },
		};

		AdicionarArquivoAoFormData(formData, CaminhoImagem, "imagem");

		var response = await client.PostAsync("cadastrar/produto", formData);
		if (response.IsSuccessStatusCode)
		{
			ActivityCommand.Execute(null);
			await Shell.Current.DisplayAlert("Sucesso", "Produto cadastrado com sucesso!", "OK");

			await Shell.Current.GoToAsync("..");
		}
		else
		{
			ActivityCommand.Execute(null);
			await Shell.Current.DisplayAlert("Erro", "Erro ao cadastrar produto!", "OK");
		}
	});

	// Método para adicionar arquivos ao formData
	private void AdicionarArquivoAoFormData(MultipartFormDataContent formData, string caminho, string nomeCampo)
	{
		if (!string.IsNullOrEmpty(caminho) && File.Exists(caminho))
		{
			var mimeType = ObterTipoMime(caminho);
			var imagemStream = new StreamContent(File.OpenRead(caminho));
			imagemStream.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
			formData.Add(imagemStream, nomeCampo, Path.GetFileName(caminho));
		}
	}

	// Método auxiliar para determinar o tipo MIME com base na extensão do arquivo
	private string ObterTipoMime(string fileName)
	{
		var extension = Path.GetExtension(fileName).ToLowerInvariant();

		return extension switch
		{
			".jpg" or ".jpeg" => "image/jpeg",
			".png" => "image/png",
			".gif" => "image/gif",
			".bmp" => "image/bmp",
			".tiff" => "image/tiff",
			".pdf" => "application/pdf",
			_ => "application/octet-stream", // Padrão para tipos desconhecidos
		};
	}

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