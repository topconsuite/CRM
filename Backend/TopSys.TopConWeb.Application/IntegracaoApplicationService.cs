using System.Collections.Generic;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Integracao;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Application
{
    public class IntegracaoApplicationService : IIntegracaoApplicationService
    {

        private readonly IIntegracaoService _integracaoService;

        public IntegracaoApplicationService(IIntegracaoService integracaoService)
        {
            _integracaoService = integracaoService;
        }

        public void SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno)
        {
            _integracaoService.SalvarRetornoHorariosBetoneiraLab360(retorno);
        }

        public IEnumerable<TotaisContrato> ObterTotaisRemessaContratosPorCliente(int codigoCliente)
        {
            return _integracaoService.ObterTotaisRemessaContratosPorCliente(codigoCliente);
        }

        public TotaisContasRecebereRemessasPorCliente ObterTotaisContasReceberPorCliente(string cpfCnpj, string inscricaoEstadual)
        {
            return _integracaoService.ObterTotaisContasReceberPorCliente(cpfCnpj, inscricaoEstadual);
        }

    }
}
