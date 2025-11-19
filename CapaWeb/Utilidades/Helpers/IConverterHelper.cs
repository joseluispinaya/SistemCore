using CapaEntidades.DTO;
using CapaEntidades;

namespace CapaWeb.Utilidades.Helpers
{
    public interface IConverterHelper
    {
        Usuario ToUsuario(UsuarioDTO usuarioEntity);
        Usuario ToUsuarioNew(UsuarioDTO model, bool isNew);
    }
}
