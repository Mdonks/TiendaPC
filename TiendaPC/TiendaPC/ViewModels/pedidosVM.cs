using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TiendaPC.Models;
using Xamarin.Forms;

namespace TiendaPC.ViewModels
{
    public class PedidosVM : INotifyPropertyChanged
    {
        private USUARIO _selectedUser;
        private COMPUTADORA _selectedComputer;
        private string _address;

        public ObservableCollection<USUARIO> Usuarios { get; set; }
        public ObservableCollection<COMPUTADORA> Computadoras { get; set; }
        public ICommand IngresarPedidoCommand { get; }

        public USUARIO SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public COMPUTADORA SelectedComputer
        {
            get => _selectedComputer;
            set
            {
                _selectedComputer = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public PedidosVM()
        {
            Usuarios = new ObservableCollection<USUARIO>();
            Computadoras = new ObservableCollection<COMPUTADORA>();
            IngresarPedidoCommand = new Command(async () => await IngresarPedido());
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var consumoServicios = new ConsumoServicios("");

                // Cargar Usuarios
                var usuariosResponse = await consumoServicios.Get<USUARIORESPONSE>("https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/usuarios");
                if (usuariosResponse != null && usuariosResponse.items != null)
                {
                    Usuarios.Clear();
                    foreach (var usuario in usuariosResponse.items)
                    {
                        Usuarios.Add(usuario);
                    }
                }

                // Cargar Computadoras
                var computadorasResponse = await consumoServicios.Get<COMPUTADORARESPONSE>("https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/computadoras");
                if (computadorasResponse != null && computadorasResponse.items != null)
                {
                    Computadoras.Clear();
                    foreach (var computadora in computadorasResponse.items)
                    {
                        Computadoras.Add(computadora);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error según sea necesario
                Console.WriteLine("Error al cargar datos: " + ex.Message);
            }
        }

        public async Task IngresarPedido()
        {
            string url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/pedidos";

            try
            {
                if (SelectedUser == null || SelectedComputer == null)
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione un usuario y una computadora", "Aceptar");
                    return;
                }

                var pedido = new Pedidos
                {
                    UsuarioId = SelectedUser.id,
                    ComputadoraId = SelectedComputer.id,
                    Direccion = Address
                };

                ConsumoServicios servicios = new ConsumoServicios(url);
                var exito = await servicios.AgregarPedido(url, pedido.UsuarioId, pedido.ComputadoraId, pedido.Direccion);

                if (exito)
                {
                    await App.Current.MainPage.DisplayAlert("Éxito", "Pedido ingresado correctamente", "Aceptar");

                    Address = string.Empty;
                    SelectedUser = null;
                    SelectedComputer = null;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo ingresar el pedido", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al ingresar el pedido", "Aceptar");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}












