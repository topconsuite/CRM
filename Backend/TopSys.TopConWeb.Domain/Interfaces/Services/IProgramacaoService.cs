using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IProgramacaoService : IServiceBase<Programacao>
    {
        IEnumerable<Programacao> ListarComPropostaContrato();

        PagedList<Programacao> ListarComPropostaContratoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        PagedList<Programacao> ListarComPropostaContratoEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        Programacao ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia, bool tracking = false);

        bool TemNotaFiscalEmitida(int idUsina, int obraNumero, int sequencia);

        IEnumerable<ProgramacaoHora> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia);

        IEnumerable<ProgramacaoLog> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia);

        float ObterVolumeTotalProgramado(int idUsina, int obraNumero);

        bool Validar(Programacao programacao);

        bool GeraProgramacao(int idUsina, int obraNumero, int sequencia,  bool atualizaComplexidadeBombeado,  bool gravaContinuidadeProgramacao, string usuario);

        bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, string observacao, string usuario);

        void GeraValorAvulsoCancelamento(Programacao programacao, string idUsuario);

        void AprovaFinanceiro(int idUsina, int obraNumero, int sequencia, string usuario);

        bool ValidaQuantidadeProgramada();
    }
}
