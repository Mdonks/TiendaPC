﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPC.Models
{
    public class Computadora
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
    }

    public class ComputadoraLink
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class ComputadoraResponse
    {
        public List<Computadora> items { get; set; }
        public bool hasMore { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<ComputadoraLink> links { get; set; }
    }
}


