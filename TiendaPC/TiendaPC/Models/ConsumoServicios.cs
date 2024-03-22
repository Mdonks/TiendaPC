using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TiendaPC.Models
{
    public class ConsumoServicios
    {
        public string url { get; set; }

        public ConsumoServicios(string newUrl)
        {

            url = newUrl;
        }

        public async Task<T> Get<T>()
        {

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);

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


    }
}
