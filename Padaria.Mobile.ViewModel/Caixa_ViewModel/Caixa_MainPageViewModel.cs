using System;
using System.Windows.Input;

namespace Padaria.Mobile.ViewModel.Caixa_ViewModel;

public class Caixa_MainPageViewModel
{
    public ICommand GotoProducaoPageCommand => new Command(async()=> 
    {
        await Shell.Current.GoToAsync("Producao_MainPage");
    }); 

    public ICommand LogountCommand => new Command(async () =>
	{
		bool sair = await Shell.Current.DisplayAlert("...", "Deseja realmente terminar sessão?", "Sim", "Não");

		if (!sair) return;

		SecureStorage.Default.RemoveAll();

		await Shell.Current.GoToAsync("//Login_Telefone");

	});

}
