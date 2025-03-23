using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Start_Page_ViewModel : BindableObject
{
    public ICommand GotoLoginPageCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Login_Telefone");
        ActivityCommand.Execute(null);
    });

    public ICommand GotoContaPageCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Criar_Conta");
        ActivityCommand.Execute(null);
    });

    private bool activity = false;
    public bool Activity
    {
        get => activity;
        set
        {
            activity = value;
            OnPropertyChanged(nameof(Activity));
        }
    }

    private bool enablePage = true;
    public bool EnablePage
    {
        get => enablePage;
        set 
        {
            enablePage = value;
            OnPropertyChanged(nameof(EnablePage));
        }
    }

    public ICommand ActivityCommand => new Command(()=>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });
}
