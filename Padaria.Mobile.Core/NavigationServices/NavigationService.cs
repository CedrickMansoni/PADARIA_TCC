using System;
using Android.Content.Res;
using Padaria.Mobile.View.FuncionarioViews.Views;
using Padaria.Mobile.ViewModel.FuncionarioViewModels;
using Padaria.Mobile.ViewModel.INavigationServices;

namespace Padaria.Mobile.Core.NavigationServices;

public class NavigationService : INavigationService
{
    public async Task GotoCategoriaPage(Criar_Conta_ViewModel T)
    {
        await Shell.Current.Navigation.PushAsync(new Categoria_Func_Page(T));
    }
}
