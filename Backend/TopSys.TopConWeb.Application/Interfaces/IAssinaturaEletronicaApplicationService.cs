using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Application.DTOS.Response.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IAssinaturaEletronicaApplicationService
    {
        ClicksignParametrosResponse ObterParametrosClicksign();
        void AtualizarParametrosClicksign(ClicksignParametrosRequest clicksignParametro);
        void SolicitarAssinaturaClicksign(SolicitacaoAssinaturaClicksignRequest solicitacaoAssinaturaClicksign);
        void ProcessarEventoClicksign(ClicksignEvento clicksignEvento);
        IEnumerable<ContratoClicksignEnvioDTO> ListarContratosClicksignEnvios(int usina, int anoContrato, int numeroContrato);
        ContratoClicksignEnvioDTO ObterUltimoContratoClicksignEnvio(int usina, int anoContrato, int numeroContrato);
        void CancelarDocumentoClicksign(Guid idDocumento);
    }
}
