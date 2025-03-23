using Padaria.Mobile.View.FuncionarioViews.Views;

namespace Padaria.Mobile.Core;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterTabletRoutes();
	}

	private void RegisterTabletRoutes()
	{
		/* -------------------------------------------------------------------- */
		Routing.RegisterRoute("Categoria_Func", typeof(Categoria_Func_Page));
		Routing.RegisterRoute("Criar_Conta", typeof(Criar_Conta_Page));
		Routing.RegisterRoute("Login_Telefone", typeof(Login_Numero_Telefone_Page));
		Routing.RegisterRoute("Login_Pin", typeof(Login_Pin_Page));
		/* -------------------------------------------------------------------- */

	}

}