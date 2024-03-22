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
    public class UsuarioViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<USUARIO> Usuarios { get; set; } = new ObservableCollection<USUARIO>();
        public ICommand AgregarUsuarioCommand { get; }
        public ICommand EliminarUsuarioCommand { get; }
        public ICommand ActualizarUsuarioCommand { get; }

        private USUARIO _usuarioSeleccionado;
        public USUARIO UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                var args = new PropertyChangedEventArgs(nameof(UsuarioSeleccionado));
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

        private string _direccion;
        public string Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                var args = new PropertyChangedEventArgs(nameof(Direccion));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private string _correo;
        public string Correo
        {
            get => _correo;
            set
            {
                _correo = value;
                var args = new PropertyChangedEventArgs(nameof(Correo));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            UsuarioSeleccionado = (USUARIO)e.SelectedItem;

            Nombre = UsuarioSeleccionado.nombre;
            Direccion = UsuarioSeleccionado.direccion;
            Correo = UsuarioSeleccionado.correo;

            ((ListView)sender).SelectedItem = null;
        }

        public UsuarioViewModel()
        {
            Usuarios = new ObservableCollection<USUARIO>();
            AgregarUsuarioCommand = new Command(async () => await AgregarUsuario());
            EliminarUsuarioCommand = new Command(async () => await EliminarUsuario());
            ActualizarUsuarioCommand = new Command(async () => await ActualizarUsuario());
            CargarUsuarios();
        }

        private async Task AgregarUsuario()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                string addUrl = $"{Url}/usuarios";

                ConsumoServicios servicios = new ConsumoServicios(Url);
                var exito = await servicios.AgregarUsuario(addUrl, Nombre, Direccion, Correo);

                if (exito)
                {
                    await App.Current.MainPage.DisplayAlert("Éxito", "Usuario ingresado correctamente", "Aceptar");

                    Nombre = string.Empty;
                    Direccion = string.Empty;
                    Correo = string.Empty;

                    await CargarUsuarios();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo ingresar el usuario", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al agregar el usuario", "Aceptar");
            }
        }


        private async Task EliminarUsuario()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/usuarios";

            try
            {
                if (UsuarioSeleccionado != null)
                {
                    string deleteUrl = $"{Url}?id={UsuarioSeleccionado.id}";

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var exito = await servicios.Eliminar(deleteUrl);

                    if (exito)
                    {
                        Nombre = string.Empty;
                        Direccion = string.Empty;
                        Correo = string.Empty;
                        Usuarios.Remove(UsuarioSeleccionado);
                        await App.Current.MainPage.DisplayAlert("Éxito", "Usuario eliminado correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el usuario", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione un usuario", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al eliminar el usuario", "Aceptar");
            }
        }


        private async Task ActualizarUsuario()
        {
            string Url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

            try
            {
                if (UsuarioSeleccionado != null)
                {
                    UsuarioSeleccionado.nombre = Nombre;
                    UsuarioSeleccionado.direccion = Direccion;
                    UsuarioSeleccionado.correo = Correo;

                    ConsumoServicios servicios = new ConsumoServicios(Url);
                    var updateUrl = $"{Url}/usuarios";
                    var exito = await servicios.ActualizarUsuario(updateUrl, UsuarioSeleccionado);

                    if (exito)
                    {
                        Nombre = string.Empty;
                        Direccion = string.Empty;
                        Correo = string.Empty;
                        await CargarUsuarios();
                        await App.Current.MainPage.DisplayAlert("Éxito", "Usuario actualizado correctamente", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el usuario", "Aceptar");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", "Por favor, seleccione un usuario", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al actualizar el usuario", "Aceptar");
            }
        }

        private async Task CargarUsuarios()
        {
            string url = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/usuarios";

            try
            {
                ConsumoServicios servicios = new ConsumoServicios(url);
                var response = await servicios.Get<USUARIORESPONSE>();

                Usuarios.Clear();

                foreach (USUARIO x in response.items)
                {
                    USUARIO temp = new USUARIO()
                    {
                        id = x.id,
                        nombre = x.nombre,
                        direccion = x.direccion,
                        correo = x.correo,
                    };

                    Usuarios.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener usuarios: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}





