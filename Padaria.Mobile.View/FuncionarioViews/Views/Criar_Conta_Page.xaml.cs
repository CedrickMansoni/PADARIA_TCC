using Padaria.Mobile.ViewModel.FuncionarioViewModels;
using Padaria.Mobile.ViewModel.INavigationServices;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Criar_Conta_Page : ContentPage
{
	public Criar_Conta_Page(INavigationService navigation)
	{
		InitializeComponent();
		BindingContext = new Criar_Conta_ViewModel(navigation);
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		var vm = (Criar_Conta_ViewModel)BindingContext;
		vm.EstadoBotaoCommand.Execute(null);
    }
}