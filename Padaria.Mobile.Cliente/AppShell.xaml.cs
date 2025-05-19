using Padaria.Mobile.View.ClienteView;

namespace Padaria.Mobile.Cliente;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegistarRoutas();
	}

	void RegistarRoutas()
	{
		Routing.RegisterRoute(nameof(Conta_ClientesView), typeof(Conta_ClientesView));
		Routing.RegisterRoute(nameof(Perfil_ClientesView), typeof(Perfil_ClientesView));
		Routing.RegisterRoute(nameof(RepoSenha_ClientesView), typeof(RepoSenha_ClientesView));
	}
}
