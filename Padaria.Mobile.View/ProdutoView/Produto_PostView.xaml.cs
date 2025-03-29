using Padaria.Mobile.ViewModel.INavigationServices;

namespace Padaria.Mobile.View.ProdutoView;

public partial class Produto_PostView : ContentPage
{
	public Produto_PostView(INavigationService service)
	{
		InitializeComponent();
		BindingContext = new ViewModel.ProdutoViewModels.Produto_PostViewModel(service);
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
    }
}