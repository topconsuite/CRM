using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class CompromissoScope
    {
        public static bool AdicionarCompromissoScopeIsValid(this Compromisso compromisso)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(compromisso, "compromisso", "Obrigatório informar o objeto compromisso!"),
                AssertionConcern.AssertTrue(compromisso.DiaInteiro || (compromisso.HoraInicio.HasValue && compromisso.HoraFim.HasValue), "Horario", "Horario é obrigatorio")
            );
        }

        public static bool AtualizarCompromissoScopeIsValid(this Compromisso compromisso)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(compromisso, "compromisso", "Obrigatório informar o objeto compromisso!"),
                AssertionConcern.AssertAreNotEquals(compromisso.Codigo, 0,"compromisso", "Obrigatório informar o código do compromisso!"),
                AssertionConcern.AssertTrue(compromisso.DiaInteiro || (compromisso.HoraInicio.HasValue && compromisso.HoraFim.HasValue), "Horario", "Horario é obrigatorio")
            );
        }

    }
}
