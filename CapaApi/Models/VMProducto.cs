namespace CapaApi.Models
{
    public class VMProducto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
    }
}
