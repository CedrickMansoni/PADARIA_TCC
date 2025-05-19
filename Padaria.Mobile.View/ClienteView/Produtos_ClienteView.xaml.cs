using Padaria.Mobile.ViewModel.Producao_ViewModel;

namespace Padaria.Mobile.View.ClienteView;

public partial class Produtos_ClienteView : ContentPage
{
	public Produtos_ClienteView()
	{
		InitializeComponent();
	}

	 protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = (Producao_MainPageViewModel)BindingContext;
		vm.GetProdutosCommand.Execute(null);
		vm.ListarCapacidadeProducaoCommand.Execute(null);
		
    }
}