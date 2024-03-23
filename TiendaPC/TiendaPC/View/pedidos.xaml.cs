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
            BindingContext = viewModel = new PedidosVM();
        }

        private void AgregarPedido_Clicked(object sender, EventArgs e)
        {
            var selectedUser = viewModel.SelectedUser;
            var selectedComputer = viewModel.SelectedComputer;
            var address = viewModel.Address;
        }
    }
}

