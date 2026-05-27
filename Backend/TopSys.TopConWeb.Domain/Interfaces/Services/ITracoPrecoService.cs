using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITracoPrecoService : IServiceBase<TracoPreco>
    {
        int ObterStatusPorTracoPreco(TracoPreco tracoPreco);

        int ObterStatusTracoPorObraVersao(ObraTracoVersao traco, ObraVersao obra);

        int ObterStatusTracoPorObra(ObraTraco traco, Obra obra);

        int ObterStatusPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra);

        int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina);

        DateTime ObterDataVigenciaPorDataBaseUsina(DateTime dataBase, int idUsina);

        IEnumerable<TracoPreco> ListarPorDataUsina(DateTime data, int idUsina);

        PagedList<TracoPreco> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, Expression<Func<TracoPreco, bool>> filter);

        TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, bool tracking = false );

        TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, Obra obra);

        TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, ObraVersao obra);

        TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);

        float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela);

        IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProdutoPorNumeroTabelaUsina(int idUsina, int idSegmentacao);

        IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProduto();

        TracoPreco ObterPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra);

        IEnumerable<Uso> ListarUsosPorNumeroTabelaUsina(int idUsina, int idSegmentacao);

        IEnumerable<Pedra> ListarPedrasPorNumeroTabelaUsinaUso(int idUsina, int idUso);

        IEnumerable<SlumpReal> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra);

        IEnumerable<Slump> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra);

        IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(int idUsina, int idUso, int idPedra, int idSlump);

        IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);

        IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);
        TracoParticularidades ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
        int ObterNumeroTabelaVigentePorUsina(int idUsina);
        IEnumerable<TracoPreco> ListarPrecosAtuaisPorObra(int idUsina, int obraNumero);
        IEnumerable<TracoPreco> ListarPrecosAtuaisPorObra(int numVersao, int idUsina, int obraNumero);
        void SalvarLogUpdate(TracoPreco tracoPreco, string usuario);

        IEnumerable<Uso> ListarUsosPorSegmentacao(int idSegmentacao);

        int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
    }
}
