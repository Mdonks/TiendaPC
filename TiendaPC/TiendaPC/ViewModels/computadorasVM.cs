using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using TiendaPC.Models;
using Xamarin.Forms;

namespace TiendaPC.ViewModels
{
    public class ComputadoraViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Computadora> Computadoras { get; set; } = new ObservableCollection<Computadora>();
        public ICommand AgregarComputadoraCommand { get; }
        public ICommand EliminarComputadoraCommand { get; }
        public ICommand ActualizarComputadoraCommand { get; }

        private Computadora _computadoraSeleccionada;
        public Computadora ComputadoraSeleccionada
        {
            get => _computadoraSeleccionada;
            set
            {
                _computadoraSeleccionada = value;
                OnPropertyChanged();
            }
        }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        private string _marca;
        public string Marca
        {
            get => _marca;
            set
            {
                _marca = value;
                OnPropertyChanged();
            }
        }

        private decimal _precio;
        public decimal Precio
        {
            get => _precio;
            set
            {
                _precio = value;
                OnPropertyChanged();
            }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged();
            }
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            ComputadoraSeleccionada = (Computadora)e.SelectedItem;

            Nombre = ComputadoraSeleccionada.Nombre;
            Marca = ComputadoraSeleccionada.Marca;
            Precio = ComputadoraSeleccionada.Precio;
            Stock = ComputadoraSeleccionada.Stock;

            ((ListView)sender).SelectedItem = null;
        }

        public ComputadoraViewModel()
        {
            Computadoras = new ObservableCollection<Computadora>();
            AgregarComputadoraCommand = new Command(async () => await AgregarComputadora());
            EliminarComputadoraCommand = new Command(async () => await EliminarComputadora());
            ActualizarComputadoraCommand = new Command(async () => await ActualizarComputadora());
            CargarComputadoras();
        }

        private async Task AgregarComputadora()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                string addUrl = $"{Url}/computadoras";

                ConsumoServicios servicios = new ConsumoServicios(Url);
                var computadora = new Computadora
                {
                    Nombre = Nombre,
                    Marca = Marca,
                    Precio = Precio,
                    Stock = Stock
                };

                var exito = await servicios.AgregarComputadora(addUrl, computadora);

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
                    string deleteUrl = $"{Url}/{ComputadoraSeleccionada.Id}";

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
                    ComputadoraSeleccionada.Nombre = Nombre;
                    ComputadoraSeleccionada.Marca = Marca;
                    ComputadoraSeleccionada.Precio = Precio;
                    ComputadoraSeleccionada.Stock = Stock;

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var updateUrl = $"{Url}/computadoras/{ComputadoraSeleccionada.Id}";
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

        private async Task CargarComputadoras()
        {
            string url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/computadoras";

            try
            {
                ConsumoServicios servicios = new ConsumoServicios(url);
                var response = await servicios.Get<Response<Computadora>>();

                Computadoras.Clear();

                foreach (Computadora x in response.Items)
                {
                    Computadoras.Add(x);
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

