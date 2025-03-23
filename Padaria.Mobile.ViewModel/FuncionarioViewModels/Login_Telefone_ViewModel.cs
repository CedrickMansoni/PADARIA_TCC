using System;
using System.Windows.Input;

namespace Padaria.Mobile.ViewModel.FuncionarioViewModels;

public class Login_Telefone_ViewModel : BindableObject
{
    private string telefone = string.Empty;
    public string Telefone
    {
        get => telefone;
        set { telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    private bool estadoBotao = false;
    public bool EstadoBotao
    {
        get => estadoBotao;
        set { estadoBotao = value; OnPropertyChanged(nameof(EstadoBotao)); }
    }

    public ICommand EstadoBotaoCommand => new Command(() =>
    {
        EstadoBotao = !string.IsNullOrEmpty(Telefone) && Telefone.Length == 9;
    });

    public ICommand GotoLoginPinPageCommand => new Command(async () =>
    {
        if (!EstadoBotao) return;
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync($"Login_Pin?telef={Telefone}");
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

    public ICommand ActivityCommand => new Command(() =>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });
}
