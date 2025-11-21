using CapaApi.Models;
using CapaData.Interfaaces;
using CapaEntidades;
using CapaEntidades.DTO;
using Mapster;
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
            return Ok(lista);
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            var productos = await _repositorio.Lista();
            var result = productos.Adapt<List<VMProductoList>>();
            return Ok(result);
        }

    }
}
