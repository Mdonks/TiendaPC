using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPC.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ComputadoraId { get; set; }
        public string Direccion { get; set; }

        public override string ToString()
        {
            return $"Pedido #{Id}";
        }
    }
}


