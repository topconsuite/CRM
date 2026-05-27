using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.TracoPreco.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracoStatus;
using TopSys.TopConWeb.Application.DTOS.Response.TracoParticuliaridades;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class TracoPrecoApplicationService : ApplicationServiceBase<TracoPreco>, ITracoPrecoApplicationService
    {
        private readonly ITracoPrecoService _tracoPrecoService;
        private readonly IPreTracoPrecoService _preTracoPrecoService;
        private readonly IContratoService _contratoService;

        public TracoPrecoApplicationService(
            ITracoPrecoService tracoPrecoService, 
            IContratoService contratoService, 
            IPreTracoPrecoService preTracoPrecoService,
            IUnitOfWork unityOfWork) : base(tracoPrecoService, unityOfWork)
        {
            _tracoPrecoService = tracoPrecoService;
            _contratoService = contratoService;
            _preTracoPrecoService = preTracoPrecoService;
        }

        public void Atualizar(string usuario, TracoPrecoAlteracaoRequest tracoPreco)
        {

            var tracoPrecoOld = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                tracoPreco.NumeroTabela,
                tracoPreco.UsinaBase.Codigo,
                tracoPreco.Uso.Codigo,
                tracoPreco.Pedra.Codigo,
                tracoPreco.Slump.Codigo,
                tracoPreco.ResistenciaTipo.Codigo,
                tracoPreco.Mpa,
                tracoPreco.Consumo);

            if(tracoPrecoOld == null)
                AssertionConcern.Notify("tracoPreco", "Tabela de preço informado não encontrada !");

            if(tracoPreco.CustoMaterial <= 0)
                AssertionConcern.Notify("custoServiço", "Custo de serviço não pode ser igual ou inferior a R$ 0.");

            if (tracoPreco.M3Preco <= 0)
                AssertionConcern.Notify("M3Preco", "Preço do M3 não pode ser igual ou inferior a R$ 0.");

            if (tracoPreco.Markup < 0)
                AssertionConcern.Notify("Markup", "Markup não pode ser inferior a R$ 0.");

            tracoPrecoOld.IdAtualizacao = StringHelper.GetIDD(usuario);
            tracoPrecoOld.CustoMaterial = tracoPreco.CustoMaterial;
            tracoPrecoOld.Markup = tracoPreco.Markup;
            tracoPrecoOld.M3Preco = tracoPreco.M3Preco;
            

            Commit();

        }

        public int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            return _tracoPrecoService.ObterNumeroTabelaVigentePorDataBaseUsina(dataBase, idUsina);
        }

        public IEnumerable<TracoPrecoResponse> ListarPorDataUsina(DateTime data, int idUsina)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarPorDataUsina(data, idUsina), new List<TracoPrecoResponse>());
        }

        public PagedList<TracoPrecoResponse> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, Expression<Func<TracoPreco, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarPorDataUsinaPagina(data, idUsina, pagina, porPagina, segmentacao, filter), new PagedList<TracoPrecoResponse>());
        }

        public TracoPrecoResponse ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo), new TracoPrecoResponse());
        }

        public TracoPrecoResponse ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, Obra obra)
        {
            var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, obra);
            var result = AutoMapper.Mapper.Map(tracoPreco, new TracoPrecoResponse());

            if (result != null)
                result.StatusTraco = _tracoPrecoService.ObterStatusPorTracoPreco(tracoPreco);

            return result;
        }

        public IEnumerable<TracoPrecoNumeracaoProdutoResponse> ListarNumeracoesProdutoPorNumeroTabelaUsina(int idUsina, int idSegmentacao)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarNumeracoesProdutoPorNumeroTabelaUsina(idUsina, idSegmentacao), new List<TracoPrecoNumeracaoProdutoResponse>());
        }

        public IEnumerable<TracoPrecoNumeracaoProdutoResponse> ListarNumeracoesProduto()
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarNumeracoesProduto(), new List<TracoPrecoNumeracaoProdutoResponse>());
        }

        public ObraTracosStatusResponse VerificarObraPossuiTracoStatusCustoVirtual(int idUsina, DateTime dataBase, Obra obra)
        {

            var result = new ObraTracosStatusResponse();
            var STATUS_CUSTO_VIRTUAL = 7105;

            foreach (var traco in obra.ObraTracos)
            {

                var resultTraco = new ObraTracoStatusResponse()
                {
                    Sequencia = traco.Sequencia
                };

                if ((traco.NumeracaoProduto ?? 0) == 0)
                {
                    var numeracaoFamilia = ObterNumeroTabelaVigentePorDataBaseUsina(dataBase, idUsina);
                    var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                        idUsina,
                        traco.UsoCodigo,
                        traco.PedraCodigo,
                        traco.SlumpCodigo,
                        traco.ResistenciaTipoCodigo,
                        traco.Fck,
                        traco.Consumo, obra);

                    if (tracoPreco is null)
                        continue;

                    resultTraco.Status = _tracoPrecoService.ObterStatusPorTracoPreco(tracoPreco);

                } 
                else
                {
                    resultTraco.Status = _tracoPrecoService.ObterStatusPorNumeracaoProduto(idUsina, (traco.NumeracaoProduto ?? 0), obra);
                }

                result.Tracos.Add(resultTraco);

            }

            result.PossuiCustoVirtual = result.Tracos.Where(x => x.Status == STATUS_CUSTO_VIRTUAL).Count() > 0;

            return result;

        }

        public int ObterStatusPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra)
        {
            return _tracoPrecoService.ObterStatusPorNumeracaoProduto(idUsina, numeracaoProduto, obra);
        }

        public TracoPrecoResponse ObterPorNumeracaoProduto(int idUsina, int numeracaoProduto , Obra obra )
        {
            var result = AutoMapper.Mapper.Map(_tracoPrecoService.ObterPorNumeracaoProduto(idUsina, numeracaoProduto, obra), new TracoPrecoResponse());

            if (result != null)
                result.StatusTraco = _tracoPrecoService.ObterStatusPorNumeracaoProduto(idUsina, numeracaoProduto, obra);

            return result;
        }

        public IEnumerable<UsoDTO> ListarUsosPorNumeroTabelaUsina(int idUsina, int idSegmentacao)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarUsosPorNumeroTabelaUsina(idUsina, idSegmentacao), new List<UsoDTO>());
        }

        public IEnumerable<PedraDTO> ListarPedrasPorNumeroTabelaUsinaUso(int idUsina, int idUso)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarPedrasPorNumeroTabelaUsinaUso(idUsina, idUso), new List<PedraDTO>());
        }

        public IEnumerable<SlumpDTO> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarSlumpsPorNumeroTabelaUsinaUsoPedra(idUsina, idUso, idPedra), new List<SlumpDTO>());
        }

        public IEnumerable<SlumpDTO> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(idUsina, idUso, idPedra), new List<SlumpDTO>());
        }

        public IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(int idUsina, int idUso, int idPedra, int idSlump)
        {
            return _tracoPrecoService.ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(idUsina, idUso, idPedra, idSlump);
        }

        public IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            return _tracoPrecoService.ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo);
        }

        public IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            return _tracoPrecoService.ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo);
        }

        public float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela)
        {
            return _tracoPrecoService.ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(idUsina, volume, precoUnitarioTabela);
        }

        public TracoParticuliaridadesResponse ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo), new TracoParticuliaridadesResponse());
        }

        public IEnumerable<TracoPrecoResponse> ListarPrecosAtuaisPorObra(int idUsina, int obraNumero, int contratoNumero, int contratoAno)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(idUsina, contratoAno, contratoNumero);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_tracoPrecoService.ListarPrecosAtuaisPorObra(idUsina, obraNumero), new List<TracoPrecoResponse>());
            else return AutoMapper.Mapper.Map(_tracoPrecoService.ListarPrecosAtuaisPorObra(versaoAtual, idUsina, obraNumero), new List<TracoPrecoResponse>());
        }

        public void AtualizarLote(IEnumerable<TracoPrecoAlteracaoRequest> tracosPrecos, string usuario, ETipoAlteracaoLoteTabelaVenda tipoAlteracao, float valorAlteracao)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var tracoPreco in tracosPrecos)
            {
                var tracoPrecoAlterar = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                   tracoPreco.UsinaBase.Codigo,
                   tracoPreco.Uso.Codigo,
                   tracoPreco.Pedra.Codigo,
                   tracoPreco.Slump.Codigo,
                   tracoPreco.ResistenciaTipo.Codigo,
                   tracoPreco.Mpa,
                   tracoPreco.Consumo,
                   true
               );

                var precoM3SemMarkup = ((tracoPrecoAlterar.M3Preco / 100) *  (100 - tracoPrecoAlterar.Markup));
                var valorServico = precoM3SemMarkup - tracoPrecoAlterar.CustoMaterial;

                switch (tipoAlteracao)
                {
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoPorcentagem :
                        tracoPrecoAlterar.AjustarValorServicoPorPorcentagem(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoReais:
                        tracoPrecoAlterar.AjustarValorServicoPorReais(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoFixo:
                        tracoPrecoAlterar.AjustarValorServicoPorValorFixo(valorAlteracao);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.MarkupPorcentagem:
                        tracoPrecoAlterar.AjustarValorMarkupPorPorcentagem(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.MarkupFixo:
                        tracoPrecoAlterar.AjustarValorMarkupPorPorcentagemFixa(valorAlteracao, valorServico);
                        break;
                }

                        _tracoPrecoService.SalvarLogUpdate(tracoPrecoAlterar, usuario);

                        Commit();
                    }

                    scope.Complete();
                }
                catch(Exception e)
                {
                    scope.Dispose();
                    throw e;
                }
            }
        }

        public bool VerificaTracoPendenteAprovacaoTabelaDeVenda(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            var result = _preTracoPrecoService.ObterUltimoAguardandoCienciaPorTraco(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);

            return (result != null);
        }

        public IEnumerable<UsoDTO> ListarUsosPorSegmentacao(int idSegmentacao)
        {
            return AutoMapper.Mapper.Map(_tracoPrecoService.ListarUsosPorSegmentacao(idSegmentacao), new List<UsoDTO>());
        }

        public int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            return _tracoPrecoService.ObterNumeracaoFamilia(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);
        }
    }
}
