using Padaria.Mobile.ViewModel.FuncionarioViewModels;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Categoria_Func_Page : ContentPage
{
	public Categoria_Func_Page(Criar_Conta_ViewModel t)
	{
		InitializeComponent();
		BindingContext = new Categ_Func_ViewModel(t);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = (Categ_Func_ViewModel)BindingContext;
		vm.ListarCategorias.Execute(null);
    }
}