using System;
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
        private ObservableCollection<Pedidos> _listaPedidos;
        private Pedidos _selectedPedido;

        public ObservableCollection<USUARIO> Usuarios { get; set; }
        public ObservableCollection<COMPUTADORA> Computadoras { get; set; }

        public ICommand IngresarPedidoCommand { get; }
        public ICommand ActualizarPedidoCommand { get; }
        public ICommand EliminarPedidoCommand { get; }

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

        public ObservableCollection<Pedidos> ListaPedidos
        {
            get { return _listaPedidos; }
            set
            {
                _listaPedidos = value;
                OnPropertyChanged();
            }
        }

        public Pedidos SelectedPedido
        {
            get => _selectedPedido;
            set
            {
                _selectedPedido = value;
                OnPropertyChanged();
            }
        }

        public PedidosVM()
        {
            Usuarios = new ObservableCollection<USUARIO>();
            Computadoras = new ObservableCollection<COMPUTADORA>();
            ListaPedidos = new ObservableCollection<Pedidos>();

            IngresarPedidoCommand = new Command(async () => await IngresarPedido());
            ActualizarPedidoCommand = new Command(async () => await ActualizarPedido());
            EliminarPedidoCommand = new Command(async () => await EliminarPedido());

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

                // Cargar Pedidos
                var pedidosResponse = await consumoServicios.Get<PedidosResponse>("https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/pedidos");
                if (pedidosResponse != null && pedidosResponse.Items != null)
                {
                    ListaPedidos.Clear();
                    foreach (var pedido in pedidosResponse.Items)
                    {
                        ListaPedidos.Add(pedido);
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

                    // Refrescar la lista de pedidos
                    await LoadDataAsync();
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

        public async Task ActualizarPedido()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                if (SelectedPedido != null)
                {
                    SelectedPedido.UsuarioId = SelectedUser.id;
                    SelectedPedido.ComputadoraId = SelectedComputer.id;
                    SelectedPedido.Direccion = Address;

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var updateUrl = $"{Url}/pedidos";
                    var exito = await servicios.ActualizarPedido(updateUrl, SelectedPedido);

                    if (exito)
                    {
                        Address = string.Empty;
                        SelectedUser = null;
                        SelectedComputer = null;
                        SelectedPedido = null;
                        await LoadDataAsync(); // Recargar la lista de pedidos
                        await App.Current.MainPage.DisplayAlert("Éxito", "Pedido actualizado correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el pedido", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione un pedido", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Ha ocurrido un error al actualizar el pedido: {ex.Message}", "Aceptar");
            }
        }



        public async Task EliminarPedido()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/pedidos";

            try
            {
                if (SelectedPedido != null)
                {
                    string deleteUrl = $"{Url}?id={SelectedPedido.Id}";

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var exito = await servicios.Eliminar(deleteUrl);

                    if (exito)
                    {
                        ListaPedidos.Remove(SelectedPedido);
                        await App.Current.MainPage.DisplayAlert("Éxito", "Pedido eliminado correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el pedido", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione un pedido", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al eliminar el pedido", "Aceptar");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}













