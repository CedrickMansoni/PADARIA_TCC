using System;
using Padaria.Mobile.ViewModel.FuncionarioViewModels;

namespace Padaria.Mobile.ViewModel.INavigationServices;

public interface INavigationService
{
    Task GotoCategoriaPage(Criar_Conta_ViewModel T);
}
