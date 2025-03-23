using Padaria.Mobile.ViewModel.FuncionarioViewModels;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Login_Numero_Telefone_Page : ContentPage
{
	public Login_Numero_Telefone_Page()
	{
		InitializeComponent();
	}

	private void Entry_TextChanged(object sender, TextChangedEventArgs e)
	{
		var vm = (Login_Telefone_ViewModel)BindingContext;
		vm.EstadoBotaoCommand.Execute(null);
	}
}