using CapaEntidades;
using CapaEntidades.DTO;

namespace CapaWeb.Utilidades.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Usuario ToUsuario(UsuarioDTO usuarioEntity)
        {
            return new Usuario
            {
                IdUsuario = usuarioEntity.IdUsuario,
                NroCi = usuarioEntity.NroCi,
                Nombre = usuarioEntity.Nombre,
                Apellido = usuarioEntity.Apellido,
                Correo = usuarioEntity.Correo,
                Clave = usuarioEntity.Clave,
                RolUsuario = new RolUsuario { IdRolUsuario = usuarioEntity.IdRolUsuario }
            };
        }

        public Usuario ToUsuarioNew(UsuarioDTO model, bool isNew)
        {
            return new Usuario
            {
                IdUsuario = isNew ? 0 : model.IdUsuario,
                NroCi = model.NroCi,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Correo = model.Correo,
                Clave = model.Clave,
                RolUsuario = new RolUsuario { IdRolUsuario = model.IdRolUsuario }
            };
        }

    }
}
