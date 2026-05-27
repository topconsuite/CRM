using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class IntervenienteScopes
    {
        public static bool IntervenienteHistoricoScopeIsValid(this IntervenienteHistorico intervenienteHistorico)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotEmpty(intervenienteHistorico.Descricao, "intervenienteHistorico-Descricao", "Observação não informada!"),
                AssertionConcern.AssertTrue(intervenienteHistorico.DataPrevistaDeRetorno.HasValue ? intervenienteHistorico.DataPrevistaDeRetorno.Value.Date >= intervenienteHistorico.Data.Value.Date : true, "intervenienteHistorico-Data", "Data de previsão do retorno não pode ser menor que a data atual!")
            );
        }
    }
}
