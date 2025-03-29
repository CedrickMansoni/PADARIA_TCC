using System;
using Padaria.Mobile.ViewModel.FuncionarioViewModels;
using Padaria.Mobile.ViewModel.ProdutoViewModels;

namespace Padaria.Mobile.ViewModel.INavigationServices;

public interface INavigationService
{
    Task GotoCategoriaPage(Criar_Conta_ViewModel T);
    Task GotoCategoriaProdutoPage(Produto_PostViewModel T);
}
