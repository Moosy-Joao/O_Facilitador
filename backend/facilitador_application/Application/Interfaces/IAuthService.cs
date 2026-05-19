using facilitador_domain.Domain.DTOs;

namespace facilitador_api.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> Login(LoginDTO dto);
    }
}