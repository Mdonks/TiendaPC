using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using TiendaPC.Models;
using Xamarin.Forms;

namespace TiendaPC.ViewModels
{
    public class ComputadoraViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<COMPUTADORA> Computadoras { get; set; } = new ObservableCollection<COMPUTADORA>();
        public ICommand AgregarComputadoraCommand { get; }
        public ICommand EliminarComputadoraCommand { get; }
        public ICommand ActualizarComputadoraCommand { get; }

        private COMPUTADORA _computadoraSeleccionada;
        public COMPUTADORA ComputadoraSeleccionada
        {
            get => _computadoraSeleccionada;
            set
            {
                _computadoraSeleccionada = value;
                var args = new PropertyChangedEventArgs(nameof(ComputadoraSeleccionada));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                var args = new PropertyChangedEventArgs(nameof(Nombre));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private string _marca;
        public string Marca
        {
            get => _marca;
            set
            {
                _marca = value;
                var args = new PropertyChangedEventArgs(nameof(Marca));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private decimal _precio;
        public decimal Precio
        {
            get => _precio;
            set
            {
                _precio = value;
                var args = new PropertyChangedEventArgs(nameof(Precio));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                var args = new PropertyChangedEventArgs(nameof(Stock));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            ComputadoraSeleccionada = (COMPUTADORA)e.SelectedItem;

            Nombre = ComputadoraSeleccionada.nombre;
            Marca = ComputadoraSeleccionada.marca;
            Precio = ComputadoraSeleccionada.precio;
            Stock = ComputadoraSeleccionada.stock;

            ((ListView)sender).SelectedItem = null;
        }

        public ComputadoraViewModel()
        {
            Computadoras = new ObservableCollection<COMPUTADORA>();
            AgregarComputadoraCommand = new Command(async () => await AgregarComputadora());
            EliminarComputadoraCommand = new Command(async () => await EliminarComputadora());
            ActualizarComputadoraCommand = new Command(async () => await ActualizarComputadora());
        }

        private async Task AgregarComputadora()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                string addUrl = $"{Url}/computadoras";

                ConsumoServicios servicios = new ConsumoServicios(Url);
                var computadora = new COMPUTADORA
                {
                    nombre = Nombre,
                    marca = Marca,
                    precio = Precio,
                    stock = Stock
                };

                var exito = await servicios.AgregarComputadora(addUrl, computadora.nombre, computadora.marca, computadora.precio, computadora.stock);

                if (exito)
                {
                    await App.Current.MainPage.DisplayAlert("Éxito", "Computadora ingresada correctamente", "Aceptar");

                    Nombre = string.Empty;
                    Marca = string.Empty;
                    Precio = 0;
                    Stock = 0;

                    await CargarComputadoras();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo ingresar la computadora", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al agregar la computadora", "Aceptar");
            }
        }


        private async Task EliminarComputadora()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/computadoras";

            try
            {
                if (ComputadoraSeleccionada != null)
                {
                    string deleteUrl = $"{Url}?id={ComputadoraSeleccionada.id}";

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var exito = await servicios.Eliminar(deleteUrl);

                    if (exito)
                    {
                        Nombre = string.Empty;
                        Marca = string.Empty;
                        Precio = 0;
                        Stock = 0;
                        Computadoras.Remove(ComputadoraSeleccionada);
                        await App.Current.MainPage.DisplayAlert("Éxito", "Computadora eliminada correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la computadora", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione una computadora", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al eliminar la computadora", "Aceptar");
            }
        }

        private async Task ActualizarComputadora()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                if (ComputadoraSeleccionada != null)
                {
                    ComputadoraSeleccionada.nombre = Nombre;
                    ComputadoraSeleccionada.marca = Marca;
                    ComputadoraSeleccionada.precio = Precio;
                    ComputadoraSeleccionada.stock = Stock;

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var updateUrl = $"{Url}/computadoras";
                    var exito = await servicios.ActualizarComputadora(updateUrl, ComputadoraSeleccionada);

                    if (exito)
                    {
                        Nombre = string.Empty;
                        Marca = string.Empty;
                        Precio = 0;
                        Stock = 0;
                        await CargarComputadoras();
                        await App.Current.MainPage.DisplayAlert("Éxito", "Computadora actualizada correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la computadora", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione una computadora", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al actualizar la computadora", "Aceptar");
            }
        }

        public async Task CargarComputadoras()
        {
            string url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/computadoras";

            await Task.Delay(100);
            try
            {
                ConsumoServicios servicios = new ConsumoServicios(url);
                var response = await servicios.Get<COMPUTADORARESPONSE>();

                Computadoras.Clear();

                foreach (COMPUTADORA x in response.items)
                {
                    COMPUTADORA temp = new COMPUTADORA()
                    { 
                        id = x.id,
                        nombre = x.nombre,
                        marca = x.marca,
                        precio = x.precio,
                        stock = x.stock,
                    };

                    Computadoras.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener computadoras: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




