using CapaData.Interfaaces;
using CapaEntidades;
using CapaEntidades.DTO;
using CapaWeb.Utilidades.Helpers;
using CapaWeb.Utilidades.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CapaWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ICategoriaRepositorio _repositCate;
        private readonly IProductoRepositorio _repositorio;
        private readonly IImageHelper _imageHelper;

        public ProductoController(ICategoriaRepositorio repositCate, IProductoRepositorio repositorio, IImageHelper imageHelper)
        {
            _repositCate = repositCate;
            _repositorio = repositorio;
            _imageHelper = imageHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<Categoria> lista = await _repositCate.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Producto> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> ListaProducDto()
        {
            List<ProductoListDTO> lista = await _repositorio.ListaProducDto();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarOriginal([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            bool rpta;

            try
            {
                // Deserializar el JSON recibido desde el FormData
                Producto producto = JsonConvert.DeserializeObject<Producto>(modelo)!;

                // Si hay foto, subirla
                if (foto != null)
                {
                    producto.ImagenPro = await _imageHelper.UploadImageAsync(foto, "images");
                }

                // Guardar en la BD
                rpta = await _repositorio.Guardar(producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Guardar Producto: " + ex.Message);
                rpta = false;
            }

            return StatusCode(StatusCodes.Status200OK, new { data = rpta });
        }

        [HttpPut]
        public async Task<IActionResult> EditarOriginal([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                Producto producto = JsonConvert.DeserializeObject<Producto>(modelo)!;
                List<Producto> lista = await _repositorio.Lista();
                var item = lista.FirstOrDefault(x => x.IdProducto == producto.IdProducto);

                if (item == null)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "Producto no encontrado.";
                    return StatusCode(StatusCodes.Status200OK, gResponse);
                }

                string imageUrl = item.ImagenPro!;

                // Si viene una nueva imagen
                if (foto != null)
                {
                    string newImageUrl = await _imageHelper.UploadImageAsync(foto, "images");

                    // Eliminar imagen anterior si existía
                    if (!string.IsNullOrEmpty(item.ImagenPro))
                    {
                        await _imageHelper.DeleteImage(item.ImagenPro);
                    }

                    imageUrl = newImageUrl;
                }

                producto.ImagenPro = imageUrl;

                bool respuesta = await _repositorio.Editar(producto);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "El producto fue actualizado correctamente."
                    : "No se pudo actualizar el producto, intente nuevamente.";
            }
            catch (Exception)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                // Deserializar DTO
                ProductoDTO dto = JsonConvert.DeserializeObject<ProductoDTO>(modelo)!;

                // Mapear a entidad Producto
                Producto producto = new()
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Cantidad = dto.Cantidad,
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVenta = dto.PrecioVenta,
                    Categoria = new Categoria { IdCategoria = dto.IdCategoria }
                };

                // Si viene imagen → subirla
                if (foto != null)
                {
                    producto.ImagenPro = await _imageHelper.UploadImageAsync(foto, "images");
                }
                else
                {
                    producto.ImagenPro = ""; // o null si lo prefieres
                }

                bool respuesta = await _repositorio.Guardar(producto);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "Se registró correctamente."
                    : "Error al registrar, intente más tarde.";
            }
            catch (Exception)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile? foto, [FromForm] string modelo)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                ProductoDTO dto = JsonConvert.DeserializeObject<ProductoDTO>(modelo)!;

                // Obtener el producto actual desde BD
                //Producto? item = await _repositorio.Obtener(dto.IdProducto);
                Producto item = await _repositorio.Obtener(dto.IdProducto);

                if (item == null)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "Producto no encontrado.";
                    return StatusCode(StatusCodes.Status200OK, gResponse);
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
            catch (Exception)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}
