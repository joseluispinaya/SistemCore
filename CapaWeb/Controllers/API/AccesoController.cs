using CapaData.Interfaaces;
using CapaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CapaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IConfiguration _configuration;

        public AccesoController(IUsuarioRepositorio repositorio, IConfiguration configuration)
        {
            _repositorio = repositorio;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CrearToken([FromBody] LoginDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest();

            var usuario_encontrado = await _repositorio.LoginNuevo(modelo.Correo, modelo.Clave);

            if (usuario_encontrado == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario_encontrado.IdUsuario.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, $"{usuario_encontrado.Nombre} {usuario_encontrado.Apellido}"),
                new(ClaimTypes.Role, usuario_encontrado.RolUsuario.Nombre!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

    }
}
