using CapaData.Interfaaces;
using CapaEntidades.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapaApi.Controllers
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
            return StatusCode(StatusCodes.Status200OK, lista);
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            List<ProductoListDTO> lista = await _repositorio.ListaProducDto();
            return Ok(lista);
        }

    }
}
