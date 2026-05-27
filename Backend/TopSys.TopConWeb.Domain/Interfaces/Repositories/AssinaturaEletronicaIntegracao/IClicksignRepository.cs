using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao
{
    public interface IClicksignRepository
    {
        ClicksignParametros ObterParametros();
        void AtualizarParametros(ClicksignParametros clicksignParametro);
        void SalvarIdClicksignDocument(Guid documentClicksignId, SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinaturaEletronicaClicksign, int qtdEnvioAssinaturaWhatsApp);
        IEnumerable<ContratoClicksignEnvio> ListarContratoClicksignEnvios(int usinaContrato, int anoContrato, int numContrato);
        ContratoClicksignEnvio ObterUltimoContratoAssinadoClicksignEnvio(int usinaContrato, int anoContrato, int numContrato);
        ContratoClicksignEnvio ObterUltimoContratoClicksignEnvio(int usinaContrato, int anoContrato, int numContrato);
        ContratoClicksignEnvio ObterContratoClicksignEnvio(Guid idDocumento);
        void AtualizarDataCancelamento(Guid id, DateTime dataCancelamento);
        void AtualizarDataAssinatura(Guid id, DateTime dataAssinatura);
    }
}
