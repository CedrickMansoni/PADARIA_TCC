using Padaria.Mobile.View.ClienteView;
using Padaria.Mobile.View.FuncionarioViews.Views;
using Padaria.Mobile.View.ProducaoView;
using Padaria.Mobile.View.ProdutoView;

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
		//Routing.RegisterRoute("Login_Telefone", typeof(Login_Numero_Telefone_Page));
		Routing.RegisterRoute("Login_Pin", typeof(Login_Pin_Page));
		/* -------------------------------------------------------------------- */
		Routing.RegisterRoute("Listar_Func", typeof(Listar_Func_Page));
		Routing.RegisterRoute("Produto_Categoria", typeof(Produto_CategoriaPage));
		Routing.RegisterRoute("Produto_MainPage", typeof(Produto_MainPage));
		Routing.RegisterRoute("Produto_PostPage", typeof(Produto_PostView)); 
		/* -------------------------------------------------------------------- */
		Routing.RegisterRoute("Producao_MainPage", typeof(Producao_MainPage));
		Routing.RegisterRoute("Get_Clientes_View", typeof(Get_Clientes_View));

	}

}