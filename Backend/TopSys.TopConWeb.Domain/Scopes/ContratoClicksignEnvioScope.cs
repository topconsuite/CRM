using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ContratoClicksignEnvioScope
    {
        public static bool ContratoClicksignEnvioEmProcessamentoIsValid(this IEnumerable<ContratoClicksignEnvio> contratoClicksignEnvios )
        {

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(contratoClicksignEnvios.Where(t => t.StatusClicksignDocumento == Enums.EStatusClicksignDocumento.Processando).Count(), 0, "clicksign", "Já existe uma solicitação de assinatura em processamento para esse contrato!")
            );

        }
    }
}
