using facilitador_domain.Domain.DTOs;

namespace facilitador_application.Application.Interfaces
{
    public interface IPainelDeDadosService
    {
        Task<PainelDeDadosDTO> ObterDados();
        Task<List<PainelDeTransacoesDTO>> ObterTransacoesRecentes();
        Task<List<PainelDeGraficosDTO>> ObterDadosGraficoVendas();
    }
}
