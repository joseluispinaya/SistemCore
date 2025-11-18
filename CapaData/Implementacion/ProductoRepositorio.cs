using CapaData.Configuracion;
using CapaData.Interfaaces;
using CapaEntidades;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using CapaEntidades.DTO;

namespace CapaData.Implementacion
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly ConnectionStrings con;

        public ProductoRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<bool> Editar(Producto objeto)
        {
            bool rpta = false;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("usp_ModificarProducto", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IdProducto", objeto.IdProducto);
                cmd.Parameters.AddWithValue("@IdCategoria", objeto.Categoria!.IdCategoria);
                cmd.Parameters.AddWithValue("@ImagenPro", objeto.ImagenPro);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioCompra", objeto.PrecioCompra);
                cmd.Parameters.AddWithValue("@PrecioVenta", objeto.PrecioVenta);
                cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);

                cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    rpta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch
                {
                    rpta = false;
                }

            }
            return rpta;
        }

        public async Task<bool> Guardar(Producto objeto)
        {
            bool rpta = false;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("usp_RegistrarProducto", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IdCategoria", objeto.Categoria!.IdCategoria);
                cmd.Parameters.AddWithValue("@ImagenPro", objeto.ImagenPro);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioCompra", objeto.PrecioCompra);
                cmd.Parameters.AddWithValue("@PrecioVenta", objeto.PrecioVenta);
                cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);
                cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;


                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    rpta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch
                {
                    rpta = false;
                }

            }
            return rpta;
        }

        public async Task<List<Producto>> Lista()
        {
            List<Producto> lista = [];

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("usp_listaProductos", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new Producto()
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        ImagenPro = dr["ImagenPro"].ToString(),
                        Nombre = dr["Nombre"].ToString()!,
                        Descripcion = dr["Descripcion"].ToString(),
                        PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                        PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        Categoria = new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Nombre = dr["NombreCategoria"].ToString()!
                        }
                    });
                }
            }
            return lista;
        }

        public async Task<List<ProductoListDTO>> ListaProducDto()
        {
            List<ProductoListDTO> lista = [];

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("usp_listaProductos", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new ProductoListDTO
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        ImagenPro = dr["ImagenPro"].ToString(),
                        Nombre = dr["Nombre"].ToString()!,
                        Descripcion = dr["Descripcion"].ToString(),
                        PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                        PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                        NombreCategoria = dr["NombreCategoria"].ToString()!
                    });
                }
            }

            return lista;
        }

        public async Task<Producto> Obtener(int IdProducto)
        {
            Producto objeto = null!;
            //Usuario objeto = new();
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new("usp_obtenerProductoId", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IdProducto", IdProducto);

                using var dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    objeto = new Producto()
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        ImagenPro = dr["ImagenPro"].ToString(),
                        Nombre = dr["Nombre"].ToString()!,
                        Descripcion = dr["Descripcion"].ToString(),
                        PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                        PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        Categoria = new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Nombre = dr["NombreCategoria"].ToString()!
                        }
                    };
                }

            }
            return objeto;
        }

    }
}
