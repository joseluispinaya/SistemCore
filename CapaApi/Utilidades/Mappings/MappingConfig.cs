using CapaApi.Models;
using CapaEntidades;
using Mapster;

namespace CapaApi.Utilidades.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            // Producto → VMProductoList (para listar)
            TypeAdapterConfig<Producto, VMProductoList>.NewConfig()
                .Map(dest => dest.IdCategoria, src => src.Categoria!.IdCategoria)
                .Map(dest => dest.NombreCategoria, src => src.Categoria!.Nombre);

            // VMProducto → Producto (para guardar o editar)
            TypeAdapterConfig<VMProducto, Producto>.NewConfig()
                .Map(dest => dest.Categoria, src => new Categoria { IdCategoria = src.IdCategoria });
        }
    }
}
