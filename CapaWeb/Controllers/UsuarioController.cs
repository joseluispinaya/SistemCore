using CapaData.Interfaaces;
using CapaEntidades;
using CapaEntidades.DTO;
using CapaWeb.Utilidades.Helpers;
using CapaWeb.Utilidades.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CapaWeb.Models;

namespace CapaWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IRolUsuarioRepositorio _repositorioRol;
        private readonly IConverterHelper _converterHelper;
        //private readonly IConverterHelper converterHelper;

        public UsuarioController(IUsuarioRepositorio repositorio, IRolUsuarioRepositorio repositorioRol, IConverterHelper converterHelper)
        {
            _repositorio = repositorio;
            _repositorioRol = repositorioRol;
            _converterHelper = converterHelper;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<RolUsuario> lista = await _repositorioRol.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> ListaUsuarios()
        {
            List<UsuarioListDTO> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO objeto)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                List<UsuarioListDTO> listaUsuarios = await _repositorio.Lista();

                bool correoExiste = listaUsuarios.Any(u =>
                    u.Correo.Equals(objeto.Correo, StringComparison.OrdinalIgnoreCase));

                if (correoExiste)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "El correo ya está siendo usado por otro usuario.";
                    return StatusCode(StatusCodes.Status200OK, gResponse);
                    //return Ok(gResponse);
                }

                var model = _converterHelper.ToUsuario(objeto);

                bool respuesta = await _repositorio.Guardar(model);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "Se registró correctamente."
                    : "Error al registrar.";
            }
            catch (Exception)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO objeto)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                // Obtener la lista de usuarios existentes
                List<UsuarioListDTO> listaUsuarios = await _repositorio.Lista();

                // Validar correo duplicado (excluyendo al usuario actual)
                bool correoExiste = listaUsuarios.Any(u =>
                    u.Correo.Equals(objeto.Correo, StringComparison.OrdinalIgnoreCase)
                    && u.IdUsuario != objeto.IdUsuario
                );

                if (correoExiste)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "El correo ya está siendo usado por otro usuario.";
                    return StatusCode(StatusCodes.Status200OK, gResponse);
                }

                // Mapeo: DTO → Usuario (el repositorio espera Usuario)
                var model = _converterHelper.ToUsuario(objeto);

                // Llamar al repositorio
                bool respuesta = await _repositorio.Editar(model);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "El usuario fue actualizado correctamente."
                    : "No se pudo actualizar el usuario.";
            }
            catch (Exception)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = "Ocurrió un error inesperado.";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CambioEstado([FromBody] VMCambioEstado request)
        {
            GenericResponse<bool> gResponse = new();

            try
            {
                bool respuesta = await _repositorio.CambioEstado(request.Id, request.Activo);

                gResponse.Estado = respuesta;
                gResponse.Mensaje = respuesta
                    ? "Cambio realizado correctamente."
                    : "No se pudo actualizar el Estado del usuario.";
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
