using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TiendaPC.Models;

namespace TiendaPC.Models
{
    public class ConsumoServicios
    {
        public string url { get; set; }

        public ConsumoServicios(string newUrl)
        {
            url = newUrl;
        }

        public async Task<T> Get<T>(string specificUrl = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                string requestUrl = string.IsNullOrEmpty(specificUrl) ? url : specificUrl;
                var response = await client.GetAsync(requestUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.OK && response != null)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
                }
            }
            catch (Exception err)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Error de Comunicacion", "Ok");
            }

            return default(T);
        }

        // Método para agregar un pedido
        public async Task<bool> AgregarPedido(string addUrl, int usuarioId, int computadoraId, string direccion)
        {
            try
            {
                var url = $"{addUrl}?usuario={usuarioId}&computadora={computadoraId}&direccion={direccion}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, null);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al agregar pedido: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> AgregarUsuario(string addUrl, string nombre, string direccion, string correo)
        {
            try
            {
                var url = $"{addUrl}?nombre={nombre}&direccion={direccion}&correo={correo}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, null);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al agregar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ActualizarUsuario(string updateUrl, USUARIO usuario)
        {
            try
            {
                string url = $"{updateUrl}?id={usuario.id}";

                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(url, content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Eliminar(string deleteUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(deleteUrl);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al eliminar: " + ex.Message);
                return false;
            }
        }

        // Función para agregar una computadora
        public async Task<bool> AgregarComputadora(string addUrl, string nombre, string marca, decimal precio, int stock)
        {
            try
            {
                var url = $"{addUrl}?nombre={nombre}&marca={marca}&precio={precio}&stock={stock}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, null);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al agregar computadora: " + ex.Message);
                return false;
            }
        }

        // Método para actualizar un pedido
        public async Task<bool> ActualizarPedido(string updateUrl, Pedidos pedido)
        {
            try
            {
                string url = $"{updateUrl}?id={pedido.Id}";

                var json = JsonConvert.SerializeObject(pedido);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(url, content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar pedido: " + ex.Message);
                return false;
            }
        }


        // Función para actualizar una computadora
        public async Task<bool> ActualizarComputadora(string updateUrl, COMPUTADORA computadora)
        {
            try
            {
                string url = $"{updateUrl}?id={computadora.id}";

                var json = JsonConvert.SerializeObject(computadora);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(url, content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar computadora: " + ex.Message);
                return false;
            }
        }

    }
}


