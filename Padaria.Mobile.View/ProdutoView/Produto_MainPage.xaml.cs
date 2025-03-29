using Padaria.Mobile.ViewModel.ProdutoViewModels;

namespace Padaria.Mobile.View.ProdutoView;

public partial class Produto_MainPage : ContentPage
{
	public Produto_MainPage()
	{
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Produto_MainPageViewModel)BindingContext;
		vm.ListarProdutosCommand.Execute(null);
	}
}