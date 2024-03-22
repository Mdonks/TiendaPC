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
    public partial class usuarios : ContentPage
    {
        UsuarioViewModel viewModel; // Declarar una instancia del ViewModel

        public usuarios()
        {
            InitializeComponent();

            viewModel = new UsuarioViewModel();
            BindingContext = viewModel;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.OnItemSelected(sender, e);
        }
    }
}