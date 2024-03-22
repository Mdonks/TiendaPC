using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPC.Models
{
    public class USUARIO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class USUARIORESPONSE
    {
        public List<USUARIO> items { get; set; }
        public bool hasMore { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<Link> links { get; set; }
    }
}

