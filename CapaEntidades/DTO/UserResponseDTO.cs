using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades.DTO
{
    public class UserResponseDTO
    {
        public int IdUsuario { get; set; }
        public string NroCi { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public bool Activo { get; set; }
        public string NombreRol { get; set; } = null!;
    }
}
