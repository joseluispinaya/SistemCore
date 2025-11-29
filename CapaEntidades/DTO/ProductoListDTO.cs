using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades.DTO
{
    public class ProductoListDTO
    {
        public int IdProducto { get; set; }
        public string? ImagenPro { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public bool? Activo { get; set; }

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public string ImageFull =>
            !string.IsNullOrEmpty(ImagenPro)
            ? ImagenPro
            : "/images/sinimagen.png";

        //"https://localhost:7111/images/sinimagen.png";
        //public string ImageFull => string.IsNullOrEmpty(ImagenPro) ? "/images/sinimagen.png" : ImagenPro;
    }
}
