using Padaria.Mobile.ViewModel.Padeiro_ViewModel;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Estado_PedidosViewModel : ContentPage
{
	public Estado_PedidosViewModel()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Padeiro_MainPageViewModel)BindingContext;
		vm.ListarPedidosCommand.Execute(null);
		vm.ListarCapacidadeProducaoCommand.Execute(null);
    }
}