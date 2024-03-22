using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TiendaPC.Models;
using Xamarin.Forms;

namespace TiendaPC.ViewModels
{
    public class UsuarioVM : INotifyPropertyChanged
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

        public UsuarioVM()
        {
            CrearUsuario = new Command(async () =>
            {
                string url = $"{BaseUrl}/usuarios";
                var nuevoUsuario = new USUARIO
                {
                    nombre = Nombre,
                    direccion = Direccion,
                    correo = Correo
                };

                var json = JsonConvert.SerializeObject(nuevoUsuario);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    Result = "Usuario creado exitosamente.";
                    // Actualizar lista de usuarios después de crear uno nuevo
                    ObtenerUsuarios();
                    Nombre = string.Empty;
                    Direccion = string.Empty;
                    Correo = string.Empty;
                }
                else
                {
                    Result = "Error al crear el usuario.";
                }
            });

            ActualizarUsuario = new Command(async () =>
            {
                string url = $"{BaseUrl}/usuarios/{UsuarioSeleccionado.id}";
                var json = JsonConvert.SerializeObject(UsuarioSeleccionado);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    Result = "Usuario actualizado exitosamente.";
                    // Actualizar lista de usuarios después de actualizar uno
                    ObtenerUsuarios();
                }
                else
                {
                    Result = "Error al actualizar el usuario.";
                }
            });

            EliminarUsuario = new Command(async () =>
            {
                string url = $"{BaseUrl}/usuarios/{UsuarioSeleccionado.id}";

                HttpResponseMessage response = await _client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Result = "Usuario eliminado exitosamente.";
                    // Actualizar lista de usuarios después de eliminar uno
                    ObtenerUsuarios();
                }
                else
                {
                    Result = "Error al eliminar el usuario.";
                }
            });

            // Obtener lista de usuarios al inicio
            ObtenerUsuarios();
        }

        private async void ObtenerUsuarios()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{BaseUrl}/usuarios");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Usuarios = JsonConvert.DeserializeObject<List<USUARIO>>(content);
                    foreach (var usuario in Usuarios)
                    {
                        Debug.WriteLine($"ID: {usuario.id}, Nombre: {usuario.nombre}, Dirección: {usuario.direccion}, Correo: {usuario.correo}");
                    }
                }
                else
                {
                    Result = "Error al traer los usuarios";
                }
            }
            catch (Exception ex)
            {
                // Manejar excepción
            }
        }

        private List<USUARIO> _usuarios;
        public List<USUARIO> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Usuarios)));
            }
        }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nombre)));
            }
        }

        private string _direccion;
        public string Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Direccion)));
            }
        }

        private string _correo;
        public string Correo
        {
            get => _correo;
            set
            {
                _correo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Correo)));
            }
        }

        private USUARIO _usuarioSeleccionado;
        public USUARIO UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsuarioSeleccionado)));
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        public Command CrearUsuario { get; }
        public Command ActualizarUsuario { get; }
        public Command EliminarUsuario { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}





