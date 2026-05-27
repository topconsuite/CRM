using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class TarefaScope
    {
        public static bool AdicionarTarefaScopeIsValid(this Tarefa tarefa)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(tarefa, "tarefa", "Obrigatório informar o objeto tarefa!"),
                AssertionConcern.AssertTrue(tarefa.DiaInteiro || tarefa.Horario.HasValue, "Horario", "Horario é obrigatorio")
            );
        }

        public static bool AtualizarTarefaScopeIsValid(this Tarefa tarefa)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(tarefa, "tarefa", "Obrigatório informar o objeto tarefa!"),
                AssertionConcern.AssertTrue(tarefa.DiaInteiro || tarefa.Horario.HasValue, "Horario", "Horario é obrigatorio")
            );
        }
    }
}
