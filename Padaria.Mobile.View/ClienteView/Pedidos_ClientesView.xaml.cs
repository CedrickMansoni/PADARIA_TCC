using Padaria.Mobile.ViewModel.ClienteViewModels;

namespace Padaria.Mobile.View.ClienteView;

public partial class Pedidos_ClientesView : ContentPage
{
	public Pedidos_ClientesView()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Pedidos_ClientesViewModel)BindingContext;
		//vm.ListarPedidosCommand.Execute(null);
		vm.FiltrarPedidosCommand.Execute(false);
	}

	private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		var vm = (Pedidos_ClientesViewModel)BindingContext;
		vm.FiltrarPedidosCommand.Execute(false);
    }

	private void RadioButton_CheckedChanged1(object sender, CheckedChangedEventArgs e)
	{
		var vm = (Pedidos_ClientesViewModel)BindingContext;
		vm.FiltrarPedidosCommand.Execute(true);
    }
}