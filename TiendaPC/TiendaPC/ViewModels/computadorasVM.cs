using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TuNombreDeEspacioDeTrabajo
{
    public class ComputadorasVM
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc";

        public async Task<List<Computadora>> GetComputadorasAsync()
        {
            var response = await _client.GetAsync($"{BaseUrl}/computadoras");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var computadoras = JsonConvert.DeserializeObject<List<Computadora>>(content);
                return computadoras;
            }
            else
            {
                return new List<Computadora>();
            }
        }

        public async Task CrearComputadoraAsync(Computadora computadora)
        {
            var json = JsonConvert.SerializeObject(computadora);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{BaseUrl}/computadoras", data);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Computadora creada exitosamente.");
            }
            else
            {
                Console.WriteLine("Error al crear la computadora:", response.StatusCode);
            }
        }

        public async Task ActualizarComputadoraAsync(int id, Computadora computadora)
        {
            var json = JsonConvert.SerializeObject(computadora);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{BaseUrl}/computadoras/{id}", data);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Computadora actualizada exitosamente.");
            }
            else
            {
                Console.WriteLine("Error al actualizar la computadora:", response.StatusCode);
            }
        }

        public async Task EliminarComputadoraAsync(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/computadoras/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Computadora eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine("Error al eliminar la computadora:", response.StatusCode);
            }
        }
    }

    public class Computadora
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}

