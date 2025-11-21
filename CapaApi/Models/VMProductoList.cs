namespace CapaApi.Models
{
    public class VMProductoList
    {
        public int IdProducto { get; set; }
        public string? ImagenPro { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public bool? Activo { get; set; }

        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string ImageFull =>
            !string.IsNullOrEmpty(ImagenPro)
            ? ImagenPro
            : "https://joseluis1989-007-site1.ltempurl.com/Imagenes/sinimagen.png";
    }
}
