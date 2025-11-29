using CapaData.Interfaaces;
using CapaEntidades;
using CapaEntidades.DTO;
using CapaWeb.Utilidades.Helpers;
using CapaWeb.Utilidades.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CapaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "JwtScheme")]
    public class ProductosController : ControllerBase
    {
        private readonly ICategoriaRepositorio _repositCate;
        private readonly IProductoRepositorio _repositorio;
        private readonly IImageHelper _imageHelper;

        public ProductosController(ICategoriaRepositorio repositCate, IProductoRepositorio repositorio, IImageHelper imageHelper)
        {
            _repositCate = repositCate;
            _repositorio = repositorio;
            _imageHelper = imageHelper;
        }

        [HttpGet]
        public async Task<IActionResult> ListaProducApi()
        {
            List<ProductoListDTO> lista = await _repositorio.ListaProducDto();
            return Ok(lista);
        }

        [HttpGet("ListaCategoApi")]
        public async Task<IActionResult> ListaCategoApi()
        {
            List<Categoria> lista = await _repositCate.Lista();
            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarApi([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                ProductoDTO dto = JsonConvert.DeserializeObject<ProductoDTO>(modelo)!;

                Producto producto = new()
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Cantidad = dto.Cantidad,
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVenta = dto.PrecioVenta,
                    Categoria = new Categoria { IdCategoria = dto.IdCategoria }
                };

                if (foto != null)
                {
                    producto.ImagenPro = await _imageHelper.UploadImageAsync(foto, "images");
                }
                else
                {
                    producto.ImagenPro = "";
                }

                bool respuesta = await _repositorio.Guardar(producto);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "Se registró correctamente."
                    : "Error al registrar, intente más tarde.";
            }
            catch
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error.";
            }

            return Ok(gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditarApi([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                ProductoDTO dto = JsonConvert.DeserializeObject<ProductoDTO>(modelo)!;

                // Obtener el producto actual desde BD
                Producto item = await _repositorio.Obtener(dto.IdProducto);

                if (item == null)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "Producto no encontrado.";
                    return Ok(gResponse);
                }

                // Guardar URL actual de la imagen
                string imageUrl = item.ImagenPro ?? "";

                // Si viene una nueva imagen → subirla
                if (foto != null)
                {
                    string newImageUrl = await _imageHelper.UploadImageAsync(foto, "images");

                    // Solo si se guardó correctamente la nueva imagen
                    if (!string.IsNullOrEmpty(newImageUrl))
                    {
                        // Eliminar imagen anterior si existía
                        if (!string.IsNullOrEmpty(item.ImagenPro))
                            await _imageHelper.DeleteImage(item.ImagenPro);

                        // Reemplazar con la nueva imagen
                        imageUrl = newImageUrl;
                    }
                }

                // Mapear DTO a entidad existente
                item.Nombre = dto.Nombre;
                item.Descripcion = dto.Descripcion;
                item.PrecioCompra = dto.PrecioCompra;
                item.PrecioVenta = dto.PrecioVenta;
                item.Cantidad = dto.Cantidad;
                item.Categoria = new Categoria { IdCategoria = dto.IdCategoria };
                item.ImagenPro = imageUrl;

                bool respuesta = await _repositorio.Editar(item);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "El producto fue actualizado correctamente."
                    : "No se pudo actualizar el producto, intente nuevamente.";
            }
            catch
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return Ok(gResponse);
        }

    }
}
