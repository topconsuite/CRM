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
    public interface ITracoPrecoRepository : IRepositoryBase<TracoPreco>
    {
        DateTime ObterDataVigenciaPorDataBaseUsina(DateTime dataBase, int idUsina);
        DateTime ObterDataVigenciaPorDataBase(DateTime dataBase);

        int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina);
        int ObterStatusPorTracoPreco(TracoPreco tracoPreco);

        IEnumerable<TracoPreco> ListarPorDataUsina(DateTime data, int idUsina);

        PagedList<TracoPreco> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, IEnumerable<string> tracosAtivos, Expression<Func<TracoPreco, bool>> filter);

        PagedList<TracoPreco> ListarPorDataPagina(DateTime data, int pagina, int porPagina, Expression<Func<TracoPreco, bool>> filter);

        PagedList<TracoPreco> ListarTodosPorPagina(int pagina, int porPagina, int? segmentacao, IEnumerable<string> tracosAtivos, Expression<Func<TracoPreco, bool>> filter);

        TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, bool tracking = false);

        float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela);

        IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProdutoPorNumeroTabelaUsina(int numeroTabela, int idUsina, int idSegmentacao);

        IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProduto();

        TracoPreco ObterPorNumeracaoProduto(int numeroTabela, int idUsina, int numeracaoProduto);

        IEnumerable<Uso> ListarUsosPorNumeroTabelaUsina(int numeroTabela, int idUsina, int idSegmentacao);

        IEnumerable<Pedra> ListarPedrasPorNumeroTabelaUsinaUso(int numeroTabela, int idUsina, int idUso);

        IEnumerable<SlumpReal> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int numeroTabela, int idUsina, int idUso, int idPedra);

        IEnumerable<Slump> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int numeroTabela, int idUsina, int idUso, int idPedra);

        IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump);

        IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);

        IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);

        TracoParticularidades ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
        int ObterNumeroTabelaVigentePorUsina(int idUsina);
        int ObterNumeroTabelaVigentePorDataBase(DateTime dataBase);

        void SalvarLogUpdate(TracoPreco tracoPreco, string usuario);
        IEnumerable<string> ListarTracosAtivos(int idUsina = 0);

        IEnumerable<Uso> ListarUsosPorSegmentacao(int idSegmentacao);

        int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
    }
}
