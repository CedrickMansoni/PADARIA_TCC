using Padaria.Mobile.ViewModel.Padeiro_ViewModel;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Padeiro_MainPage : ContentPage
{
	public Padeiro_MainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Padeiro_MainPageViewModel)BindingContext;
		vm.ListarCapacidadeProducaoCommand.Execute(null);
		vm.ListarProdutosCommand.Execute(null);
		vm.ListarProducaoCommand.Execute(null);
	}
}