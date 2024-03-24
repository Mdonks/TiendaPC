using System.Collections.Generic;

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

public class PedidosResponse
{
    public List<Pedidos> Items { get; set; }
}




