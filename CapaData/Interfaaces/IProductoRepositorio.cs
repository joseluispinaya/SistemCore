using CapaEntidades;
using CapaEntidades.DTO;

namespace CapaData.Interfaaces
{
    public interface IProductoRepositorio
    {
        Task<List<Producto>> Lista();
        Task<bool> Guardar(Producto objeto);
        Task<bool> Editar(Producto objeto);
        Task<Producto> Obtener(int IdProducto);
        Task<List<ProductoListDTO>> ListaProducDto();

        //Task<List<ProductoListDTO>> ListaProducDto();
    }
}
