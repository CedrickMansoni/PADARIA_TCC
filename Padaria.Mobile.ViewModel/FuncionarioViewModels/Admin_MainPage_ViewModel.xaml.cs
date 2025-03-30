using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Funcionario.DTO;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

// Adicionamos a propriedade para receber o parâmetro via Shell
[QueryProperty(nameof(Funcionario_), "funcLogado")]
public class Admin_MainPage_ViewModel : BindableObject
{

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

	public ICommand GotoFuncPageCommand => new Command(async ()=> 
	{
		await Shell.Current.GoToAsync("Listar_Func");
	});

	//
	public ICommand GotoProdutoPageCommand => new Command(async ()=> 
	{
		await Shell.Current.GoToAsync("Produto_MainPage");
	});

}