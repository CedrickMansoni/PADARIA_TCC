using Padaria.Mobile.ViewModel.ProdutoViewModels;

namespace Padaria.Mobile.View.ProdutoView;

public partial class Produto_CategoriaPage : ContentPage
{
	public Produto_CategoriaPage(Produto_PostViewModel t)
	{
		InitializeComponent();
		BindingContext = new Produto_CategoriaViewModel(t);
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		var vm = (Produto_CategoriaViewModel)BindingContext;
		vm.EnableButtonCommand.Execute(null);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = (Produto_CategoriaViewModel)BindingContext;
		vm.ListarCategoriasCommand.Execute(null);
    }
}