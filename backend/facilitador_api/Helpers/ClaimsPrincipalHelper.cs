using System.Security.Claims;

namespace facilitador_api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid ObterUsuarioId(this ClaimsPrincipal user)
        {
            var usuarioId = user.FindFirst("usuarioId")?.Value;

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            return Guid.Parse(usuarioId);
        }

        public static Guid ObterEmpresaId(this ClaimsPrincipal user)
        {
            var empresaId = user.FindFirst("empresaId")?.Value;

            if (string.IsNullOrWhiteSpace(empresaId))
            {
                throw new UnauthorizedAccessException("Empresa não identificada no token.");
            }

            return Guid.Parse(empresaId);
        }

        public static string ObterCargo(this ClaimsPrincipal user)
        {
            var cargo = user.FindFirst("cargo")?.Value;

            if (string.IsNullOrWhiteSpace(cargo))
            {
                throw new UnauthorizedAccessException("Cargo não identificado no token.");
            }

            return cargo;
        }
    }
}