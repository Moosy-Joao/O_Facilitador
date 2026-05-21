using facilitador_application.Application.Interfaces;
using facilitador_domain.Domain.DTOs;

namespace facilitador_application.Application.Services
{
    internal class PainelDeDadosService : IPainelDeDadosService
    {
        public Task<PainelDeDadosDTO> ObterDados()
        {
            throw new NotImplementedException();
        }

        public Task<List<PainelDeGraficosDTO>> ObterDadosGraficoVendas()
        {
            throw new NotImplementedException();
        }

        public Task<List<PainelDeTransacoesDTO>> ObterTransacoesRecentes()
        {
            throw new NotImplementedException();
        }
    }
}
