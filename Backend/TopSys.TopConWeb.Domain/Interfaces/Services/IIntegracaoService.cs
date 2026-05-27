using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.Integracao;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IIntegracaoService
    {

        void SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno);

        IEnumerable<TotaisContrato> ObterTotaisRemessaContratosPorCliente(int codigoCliente);

        TotaisContasRecebereRemessasPorCliente ObterTotaisContasReceberPorCliente(string cpfCnpj, string inscricaoEstadual);

    }
}
