using System.Threading.Tasks;
using Padaria.Mobile.ViewModel.FuncionarioViewModels;
using Padaria.Share.Funcionario.DTO;

namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Listar_Func_Page : ContentPage
{
	public Listar_Func_Page()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Listar_Func_ViewModel)BindingContext;
		vm.ListarFuncionariosCommand.Execute(null);
	}

	private void Switch_Toggled(object sender, ToggledEventArgs e)
	{
		if (sender is Switch switchControl)
		{
			// Pegando o item vinculado ao Switch
			if (switchControl.BindingContext is Get_Func_DTO usuario)
			{
				var state = new Put_UserState_DTO { UserId = usuario.Id, EstadoFuncionario = e.Value ? "Activo" : "Inactivo" };
				
				var vm = (Listar_Func_ViewModel)BindingContext; 
				vm.RedefinirEstado.Execute(state);
			}
		}
	}
}