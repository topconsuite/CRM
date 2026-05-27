using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IProgramacaoRepository : IRepositoryBase<Programacao>
    {
        PagedList<Programacao> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        PagedList<Programacao> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        IEnumerable<Programacao> ListarComPropostaContrato();

        Programacao ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia, bool tracking = false);

        IEnumerable<ProgramacaoHora> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia);

        IEnumerable<ProgramacaoLog> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia);

        float ObterVolumeTotalProgramado(int idUsina, int obraNumero);

        int ObterQuantidadeDeProgramacoesHora(int idUsina, int contratoAno, int contratoNumero, int sequencia);

        void AdicionarValorAvulsoTaxaCancelamentoProgramacao(Programacao programacao, Mercadoria mercadoriaTaxa, int seqVlrAvulso, decimal valorTaxa, string idUsuario);

        void AlterarStatusLiberacaoProgramacao(Programacao programacao, string idUsuario);
    }
}
