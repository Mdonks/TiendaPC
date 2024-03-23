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
    public partial class computadoras : ContentPage
    {
        ComputadoraViewModel viewModel; // Declarar una instancia del ViewModel

        public computadoras()
        {
            InitializeComponent();

            viewModel = new ComputadoraViewModel();
            BindingContext = viewModel;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.OnItemSelected(sender, e);
        }
    }
}

