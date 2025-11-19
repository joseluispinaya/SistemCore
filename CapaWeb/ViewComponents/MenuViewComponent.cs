using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapaWeb.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var nomUsuario = string.Empty;
            var rolUsuario = string.Empty;

            ClaimsPrincipal claimsPrincipal = HttpContext.User;

            if (claimsPrincipal?.Identity?.IsAuthenticated == true)
            {
                nomUsuario = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

                // Obtener el rol del usuario
                rolUsuario = claimsPrincipal.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            }

            ViewData["nomUsuario"] = nomUsuario;
            ViewData["rolUsuario"] = rolUsuario;

            return View();
        }
    }
}
