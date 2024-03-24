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

        private async void EliminarPedido_Clicked(object sender, EventArgs e)
        {
            // Llamar al método para eliminar el pedido seleccionado
            await viewModel.EliminarPedido();
        }

        private async void ActualizarPedido_Clicked(object sender, EventArgs e)
        {
            // Llamar al método para actualizar el pedido seleccionado
            await viewModel.ActualizarPedido();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Obtener el pedido seleccionado
            var selectedPedido = e.Item as Pedidos;

            // Asignar el pedido seleccionado al ViewModel
            viewModel.SelectedPedido = selectedPedido;

            // Aquí puedes mostrar un mensaje de confirmación o realizar otras acciones
        }
    }
}






