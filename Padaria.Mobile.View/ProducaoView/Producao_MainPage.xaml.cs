using Padaria.Mobile.ViewModel.Producao_ViewModel;

namespace Padaria.Mobile.View.ProducaoView;

public partial class Producao_MainPage : ContentPage
{
	public Producao_MainPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = (Producao_MainPageViewModel)BindingContext;
		vm.GetProdutosCommand.Execute(null);
    }
}