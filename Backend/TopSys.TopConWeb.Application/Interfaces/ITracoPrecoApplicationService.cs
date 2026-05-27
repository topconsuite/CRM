using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.TracoPreco.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracoStatus;
using TopSys.TopConWeb.Application.DTOS.Response.TracoParticuliaridades;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITracoPrecoApplicationService : IApplicationServiceBase<TracoPreco>
    {
        ObraTracosStatusResponse VerificarObraPossuiTracoStatusCustoVirtual(int idUsina, DateTime dataBase, Obra obra);

        int ObterStatusPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra);

        void Atualizar(string usuario, TracoPrecoAlteracaoRequest tracoPreco);

        int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina);

        IEnumerable<TracoPrecoResponse> ListarPorDataUsina(DateTime data, int idUsina);

        IEnumerable<TracoPrecoResponse> ListarPrecosAtuaisPorObra(int idUsina, int obraNumero, int contratoNumero, int contratoAno);

        PagedList<TracoPrecoResponse> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, Expression<Func<TracoPreco, bool>> filter);

        TracoPrecoResponse ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);

        TracoPrecoResponse ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, Obra obra);

        float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela);

        IEnumerable<TracoPrecoNumeracaoProdutoResponse> ListarNumeracoesProdutoPorNumeroTabelaUsina(int idUsina, int idSegmentacao);

        IEnumerable<TracoPrecoNumeracaoProdutoResponse> ListarNumeracoesProduto();

        TracoPrecoResponse ObterPorNumeracaoProduto(int idUsina, int numeracaoProduto , Obra obra);

        IEnumerable<UsoDTO> ListarUsosPorNumeroTabelaUsina(int idUsina, int idSegmentacao);

        IEnumerable<PedraDTO> ListarPedrasPorNumeroTabelaUsinaUso(int idUsina, int idUso);

        IEnumerable<SlumpDTO> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra);

        IEnumerable<SlumpDTO> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra);

        IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump
            (int idUsina, int idUso, int idPedra, int idSlump);

        IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);

        IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo);

        TracoParticuliaridadesResponse ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo , float mpa, int consumo);

        void AtualizarLote(IEnumerable<TracoPrecoAlteracaoRequest> tracosPrecos, string usuario, ETipoAlteracaoLoteTabelaVenda tipoAlteracao, float valorAlteracao);

        bool VerificaTracoPendenteAprovacaoTabelaDeVenda(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
        IEnumerable<UsoDTO> ListarUsosPorSegmentacao(int idSegmentacao);

        int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
    }
}
