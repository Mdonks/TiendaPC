using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace TiendaPC.ViewModels
{
    public class UsuarioVM : INotifyPropertyChanged
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/";

        public UsuarioVM()
        {
            CrearUsuario = new Command(async () =>
            {
                string url = $"{BaseUrl}/usuarios";
                var nuevoUsuario = new Usuario
                {
                    Nombre = Nombre,
                    Direccion = Direccion,
                    Correo = Correo
                };

                var json = JsonConvert.SerializeObject(nuevoUsuario);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    Result = "Usuario creado exitosamente.";
                    // Actualizar lista de usuarios después de crear uno nuevo
                    ObtenerUsuarios();
                }
                else
                {
                    Result = "Error al crear el usuario.";
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
                    Usuarios = JsonConvert.DeserializeObject<List<Usuario>>(content);
                }
                else
                {
                    // Manejar error al obtener usuarios
                }
            }
            catch (Exception ex)
            {
                // Manejar excepción
            }
        }

        private List<Usuario> _usuarios;
        public List<Usuario> Usuarios
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

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
    }
}


