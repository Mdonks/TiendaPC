using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPC.Models
{
    public class Computadora
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class ComputadorasLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class Response<T>
    {
        public List<T> Items { get; set; }
        public bool HasMore { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public List<Link> Links { get; set; }
    }
}

