using System;
using System.Windows.Input;

namespace Padaria.Mobile.ViewModel.Caixa_ViewModel;

public class Caixa_MainPageViewModel
{
    public ICommand GotoProducaoPageCommand => new Command(async()=> 
    {
        await Shell.Current.GoToAsync("Producao_MainPage");
    }); 
}
