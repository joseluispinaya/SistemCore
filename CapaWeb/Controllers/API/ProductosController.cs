using CapaData.Interfaaces;
using CapaEntidades.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CapaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepositorio _repositorio;

        public ProductosController(IProductoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<ProductoListDTO> lista = await _repositorio.ListaProducDto();
            return Ok(lista);
        }

    }
}
