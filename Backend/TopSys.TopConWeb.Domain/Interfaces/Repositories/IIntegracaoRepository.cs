using System.Collections;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.Integracao;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IIntegracaoRepository
    {

        void SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno);

        IEnumerable<TotaisContrato> ObterTotaisRemessaContratosPorCliente(int codigoCliente);


        ResumoCredito obterInformacoesCreditoPorCliente(int codCliente);


    }
}
