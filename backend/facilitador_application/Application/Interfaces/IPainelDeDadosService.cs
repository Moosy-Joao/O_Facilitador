using facilitador_domain.Domain.DTOs;

namespace facilitador_application.Application.Interfaces
{
    public interface IPainelDeDadosService
    {
        Task<PainelDeDadosDTO> ObterDados(Guid empresaId);
        Task<List<PainelDeTransacoesDTO>> ObterTransacoesRecentes(Guid empresaId);
        Task<List<PainelDeGraficosDTO>> ObterDadosGraficoVendas(Guid empresaId);
    }
}
