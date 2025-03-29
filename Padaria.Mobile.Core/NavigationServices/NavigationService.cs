using Padaria.Mobile.View.FuncionarioViews.Views;
using Padaria.Mobile.View.ProdutoView;
using Padaria.Mobile.ViewModel.FuncionarioViewModels;
using Padaria.Mobile.ViewModel.INavigationServices;
using Padaria.Mobile.ViewModel.ProdutoViewModels;

namespace Padaria.Mobile.Core.NavigationServices;

public class NavigationService : INavigationService
{
    public async Task GotoCategoriaPage(Criar_Conta_ViewModel T)
    {
        await Shell.Current.Navigation.PushAsync(new Categoria_Func_Page(T));
    }

    public async Task GotoCategoriaProdutoPage(Produto_PostViewModel T)
    {
        await Shell.Current.Navigation.PushAsync(new Produto_CategoriaPage(T));
    }
}
