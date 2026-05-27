using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ProgramacaoScopes
    {
        public static bool TemNotaEmitidaScopeIsValid(this Programacao programacao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(!programacao.TemNotaFicalEmitida, "programacao", "Programação já possui nota fiscal emitida!")
            );
        }

        public static bool CancelamentoScopeIsValid(this Programacao programacao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(programacao.Status != EProgramacaoStatus.Cancelada, "programacao", "Programação já cancelada!"),
                AssertionConcern.AssertTrue(programacao.Status != EProgramacaoStatus.Revalidacao, "programacao", "Programação em revalidação!"),
                AssertionConcern.AssertTrue(programacao.DataConcretagem >= DateTime.Today, "programacao", "Data de concretagem inferior à data atual!")
            );
        }

        public static bool EnderecoProgramacaoScopeIsValid(this Programacao programacao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(!programacao.PossuiEndereco(), "enderecoProgramacao", "Ocorreu um erro ao confirmar programação, favor entrar em contato com a Telluria")
            );
        }

        public static bool TracoProgramacaoScopeIsValid(this Programacao programacao , ObraTraco obraTraco )
        {
            if (programacao.ObraTracoSequencia >= 0) return true;

            var alertaAlteracaoTraco = "Progamação não concluida pois houve alteração na proposta " +
                "(informações do traço selecionado) enquanto a programação estava sendo editada." +
                " Recarregue a página e tente novamente!";

            var alertaExclusaoTraco = "Progamação não concluida pois o traço selecionado foi excluido na proposta!" +
                " Recarregue a página e tente novamente!";

            var houveAlteracao = false;

            if (obraTraco != null)
            {
                houveAlteracao |= (obraTraco.ResistenciaTipoCodigo != (programacao.ResistenciaTipoCodigo ?? 0));
                houveAlteracao |= (obraTraco.Fck != programacao.Mpa);
                houveAlteracao |= (obraTraco.Consumo != programacao.Consumo);
                houveAlteracao |= (obraTraco.PedraCodigo != (programacao.PedraCodigo ?? 0));
                houveAlteracao |= (obraTraco.SlumpCodigo != (programacao.SlumpCodigo ?? 0));
                houveAlteracao |= (obraTraco.UsoCodigo != (programacao.UsoCodigo ?? 0));
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(obraTraco, "obraTraco", alertaExclusaoTraco),
                AssertionConcern.AssertTrue(!houveAlteracao, "obraTraco", alertaAlteracaoTraco)
            );
        }

        public static bool VolumeScopeIsValid(this Programacao programacao, float volumeOutrasProgramacoes, float volumeProposto, bool isPagamentoAntecipado)
        {
            if (!isPagamentoAntecipado)
                return true;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(volumeProposto, programacao.VolumeTotal + volumeOutrasProgramacoes, "volumeTotal", $"Volume programado maior que o proposto! Não é possível programar volume superior ao proposto quando o pagamento é antecipado! (Proposto: {volumeProposto}m3 | Programado: {programacao.VolumeTotal}m3 desta programação + {volumeOutrasProgramacoes}m3 de outras programações)")
            );
        }

        public static bool HorarioBombaScopeIsValid(this Programacao programacao)
        {

            if (programacao.HorarioBomba.Equals("")) 
                return true;

            var Horario = DateTime.Parse($"{programacao.Horario.Substring(0, 2)}:{programacao.Horario.Substring(2, 2)}");
            var HorarioBomba = DateTime.Parse($"{programacao.HorarioBomba.Substring(0, 2)}:{programacao.HorarioBomba.Substring(2, 2)}");
            var Mensagem = $"Horário de bomba fora do intervalo permitido, o horário minimo é {Horario.AddHours(-2).ToString("HH:mm")} e o máximo {Horario.ToString("HH:mm")}";

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue((HorarioBomba > Horario) || (HorarioBomba < Horario.AddHours(-2)), "HorarioBomba", Mensagem)
            );

        }

        public static bool ObraProgramacaoIsValid(this Programacao programacao, Obra obra)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(obra == null, "obra", "Obra Não Localizada!")
            ) ;
        }

    }
}
