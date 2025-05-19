using Padaria.Mobile.ViewModel.FuncionarioViewModels;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Cliente_ListarView : ContentPage
{
	public Cliente_ListarView()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var vm = (Cliente_ListarViewModel)BindingContext;
		if (vm != null)
		{
			vm.ListarClientesCommand.Execute(null);
		}
	}
}