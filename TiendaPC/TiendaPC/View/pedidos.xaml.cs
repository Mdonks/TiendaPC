using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TiendaPC.ViewModels;

namespace TiendaPC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pedidos : ContentPage
    {
        PedidosVM viewModel;

        public pedidos()
        {
            InitializeComponent();
            viewModel = new PedidosVM();
            BindingContext = viewModel;
        }

        private async void AgregarPedido_Clicked(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un usuario y una computadora
            if (viewModel.SelectedUser == null || viewModel.SelectedComputer == null)
            {
                await DisplayAlert("Alerta", "Por favor, seleccione un usuario y una computadora", "Aceptar");
                return;
            }

            // Llamar al método para ingresar el pedido
            await viewModel.IngresarPedido();
        }
    }
}



