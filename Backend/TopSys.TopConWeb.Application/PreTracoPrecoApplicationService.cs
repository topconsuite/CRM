using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.PreTracoPreco;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class PreTracoPrecoApplicationService : ApplicationServiceBase<PreTracoPreco>, IPreTracoPrecoApplicationService
    {

        private readonly IPreTracoPrecoService _preTracoPrecoService;
        private readonly ITracoPrecoService _tracoPrecoService;
        private readonly ITracoPrecoApplicationService _tracoPrecoApplicationService;

        public PreTracoPrecoApplicationService(IPreTracoPrecoService preTracoPrecoService, ITracoPrecoService tracoPrecoService, ITracoPrecoApplicationService tracoPrecoApplicationService, IUnitOfWork unityOfWork)
            : base(preTracoPrecoService, unityOfWork)
        {
            _preTracoPrecoService = preTracoPrecoService;
            _tracoPrecoService = tracoPrecoService;
            _tracoPrecoApplicationService = tracoPrecoApplicationService;
        }

        public PagedList<PreTracoPrecoResponse> ListarAguardandoCienciaPorPagina(int pagina, int porPagina, int segmentacao, Expression<Func<PreTracoPreco, bool>> filter)
        {
            var result = AutoMapper.Mapper.Map<PagedList<PreTracoPreco>, PagedList<PreTracoPrecoResponse>>(
                    _preTracoPrecoService.ListarAguardandoCienciaPorPagina(pagina, porPagina, segmentacao, filter),
                    new PagedList<PreTracoPrecoResponse>()
                );

            foreach (var preTracoPreco in result.Records)
            {

                preTracoPreco.TracoPrecoVigente = _tracoPrecoApplicationService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    preTracoPreco.UsinaCodigo,
                    preTracoPreco.UsoCodigo,
                    preTracoPreco.PedraCodigo,
                    preTracoPreco.SlumpCodigo,
                    preTracoPreco.ResistenciaTipoCodigo,
                    preTracoPreco.Mpa,
                    preTracoPreco.Consumo);

            }

            return result;
        }

        public PreTracoPrecoResponse ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistencia, float mpa, int consumo)
        {
            return AutoMapper.Mapper.Map<PreTracoPreco, PreTracoPrecoResponse>(
                    _preTracoPrecoService.ObterUltimoAguardandoCienciaPorTraco(usina, uso, pedra, slump, resistencia, mpa, consumo, false),
                    new PreTracoPrecoResponse()
                );
        }

        public PreTracoPrecoResponse ObterPorId(string id)
        {
            return AutoMapper.Mapper.Map<PreTracoPreco, PreTracoPrecoResponse>(_preTracoPrecoService.ObterPorId(new Guid(id)), new PreTracoPrecoResponse());
        }

        public void Atualizar(PreTracoPrecoAlteracaoRequest preTracoPreco, string usuario) {

            var preTracoPrecoOld = _preTracoPrecoService.ObterUltimoAguardandoCienciaPorTraco(
                    preTracoPreco.UsinaCodigo, 
                    preTracoPreco.UsoCodigo, 
                    preTracoPreco.PedraCodigo, 
                    preTracoPreco.SlumpCodigo, 
                    preTracoPreco.ResistenciaTipoCodigo, 
                    preTracoPreco.Mpa, 
                    preTracoPreco.Consumo, 
                    true
                );

            if (preTracoPrecoOld == null)
            {
                AssertionConcern.Notify("AlteracaoTracoPreco", "Alteração de preço do traço não encontrado.");
                return;
            }
                

            if (preTracoPrecoOld.Id != new Guid(preTracoPreco.Id))
            {
                AssertionConcern.Notify("Alteracao", "Não é possível alterar o valor do traço, pois existe uma alteração mais recente para este traço.");
                return;
            }

            var tabelaVigenteTracoPreco = _tracoPrecoService.ObterNumeroTabelaVigentePorUsina(preTracoPrecoOld.UsinaCodigo);
            var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    tabelaVigenteTracoPreco,
                    preTracoPrecoOld.UsinaCodigo,
                    preTracoPrecoOld.UsoCodigo,
                    preTracoPrecoOld.PedraCodigo,
                    preTracoPrecoOld.SlumpCodigo,
                    preTracoPrecoOld.ResistenciaTipoCodigo,
                    preTracoPrecoOld.Mpa,
                    preTracoPrecoOld.Consumo
                );

            var novoTraco = (tracoPreco == null);

            if (novoTraco)
            {
                tracoPreco = new TracoPreco()
                {
                    NumeroTabela = tabelaVigenteTracoPreco,
                    UsinaBaseCodigo = preTracoPrecoOld.UsinaCodigo,
                    VendedorRepresentanteCodigo = 999,
                    UsoCodigo = preTracoPrecoOld.UsoCodigo,
                    ResistenciaTipoCodigo = preTracoPrecoOld.ResistenciaTipoCodigo,
                    Mpa = preTracoPrecoOld.Mpa,
                    Consumo = preTracoPrecoOld.Consumo,
                    PedraCodigo = preTracoPrecoOld.PedraCodigo,
                    SlumpCodigo = preTracoPrecoOld.SlumpCodigo,
                    TracoEspecificacao = preTracoPrecoOld.TracoEspecificacao,
                    UsinaReferenciaCodigo = preTracoPrecoOld.UsinaCodigo,
                    NumeracaoProduto = preTracoPrecoOld.NumeracaoProduto,
                    DataInicioVigencia = _tracoPrecoService.ObterDataVigenciaPorDataBaseUsina(DateTime.Now, preTracoPrecoOld.UsinaCodigo),
                    ComissaoServicoSobrePreco = "",
                    ComissaoSobreMaior = "",
                    IdCadastro = StringHelper.GetIDD(usuario),
                    IdAtualizacao = ""
            };
            }

            preTracoPrecoOld.ValorServico = preTracoPreco.ValorServico;
            preTracoPrecoOld.Markup = preTracoPreco.Markup;
            preTracoPrecoOld.M3Preco = preTracoPreco.M3Preco;

            var tracoPrecosNaoCientesAnterioresAoAtual = _preTracoPrecoService.ListarAguardandoCienciaPorTracoUsina(
                    preTracoPrecoOld.UsinaCodigo,
                    preTracoPrecoOld.UsoCodigo,
                    preTracoPrecoOld.PedraCodigo,
                    preTracoPrecoOld.SlumpCodigo,
                    preTracoPrecoOld.ResistenciaTipoCodigo,
                    preTracoPrecoOld.Mpa,
                    preTracoPrecoOld.Consumo
                );

            foreach(var tracoPrecoNaoCiente in tracoPrecosNaoCientesAnterioresAoAtual)
            {
                tracoPrecoNaoCiente.IDCiencia = StringHelper.GetIDD(usuario);
                tracoPrecoNaoCiente.DataCiencia = DateTime.Now;
                tracoPrecoNaoCiente.UpdatedAt = DateTime.Now;
            }
            


            tracoPreco.CustoMaterial = preTracoPreco.CustoMaterial;
            tracoPreco.Markup = preTracoPreco.Markup;
            tracoPreco.M3Preco = preTracoPreco.M3Preco;
            tracoPreco.TracoEspecificacao = preTracoPrecoOld.TracoEspecificacao;
            tracoPreco.NumeracaoProduto = preTracoPrecoOld.NumeracaoProduto;

            if(!novoTraco)
                tracoPreco.IdAtualizacao = StringHelper.GetIDD(usuario);

            if(novoTraco)
                _tracoPrecoService.Adicionar(tracoPreco);

            Commit();

        }

        public void Reprovar(PreTracoPrecoAlteracaoRequest preTracoPreco, string usuario)
        {
            var preTracoPrecoOld = _preTracoPrecoService.ObterUltimoAguardandoCienciaPorTraco(
                preTracoPreco.UsinaCodigo,
                preTracoPreco.UsoCodigo,
                preTracoPreco.PedraCodigo,
                preTracoPreco.SlumpCodigo,
                preTracoPreco.ResistenciaTipoCodigo,
                preTracoPreco.Mpa,
                preTracoPreco.Consumo,
                true
            );

            if (preTracoPrecoOld == null)
            {
                AssertionConcern.Notify("AlteracaoTracoPreco", "Alteração de preço do traço não encontrado.");
                return;
            }


            if (preTracoPrecoOld.Id != new Guid(preTracoPreco.Id))
            {
                AssertionConcern.Notify("Alteracao", "Não é possível alterar o valor do traço, pois existe uma alteração mais recente para este traço.");
                return;
            }

            var tabelaVigenteTracoPreco = _tracoPrecoService.ObterNumeroTabelaVigentePorUsina(preTracoPrecoOld.UsinaCodigo);
            var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    tabelaVigenteTracoPreco,
                    preTracoPrecoOld.UsinaCodigo,
                    preTracoPrecoOld.UsoCodigo,
                    preTracoPrecoOld.PedraCodigo,
                    preTracoPrecoOld.SlumpCodigo,
                    preTracoPrecoOld.ResistenciaTipoCodigo,
                    preTracoPrecoOld.Mpa,
                    preTracoPrecoOld.Consumo
                );

            var novoTraco = (tracoPreco == null);

            if (novoTraco)
            {
                tracoPreco = new TracoPreco()
                {
                    NumeroTabela = tabelaVigenteTracoPreco,
                    UsinaBaseCodigo = preTracoPrecoOld.UsinaCodigo,
                    VendedorRepresentanteCodigo = 999,
                    UsoCodigo = preTracoPrecoOld.UsoCodigo,
                    ResistenciaTipoCodigo = preTracoPrecoOld.ResistenciaTipoCodigo,
                    Mpa = preTracoPrecoOld.Mpa,
                    Consumo = preTracoPrecoOld.Consumo,
                    PedraCodigo = preTracoPrecoOld.PedraCodigo,
                    SlumpCodigo = preTracoPrecoOld.SlumpCodigo,
                    TracoEspecificacao = preTracoPrecoOld.TracoEspecificacao,
                    UsinaReferenciaCodigo = preTracoPrecoOld.UsinaCodigo,
                    NumeracaoProduto = preTracoPrecoOld.NumeracaoProduto,
                    DataInicioVigencia = _tracoPrecoService.ObterDataVigenciaPorDataBaseUsina(DateTime.Now, preTracoPrecoOld.UsinaCodigo),
                    ComissaoServicoSobrePreco = "",
                    ComissaoSobreMaior = "",
                    IdCadastro = StringHelper.GetIDD(usuario),
                    IdAtualizacao = ""
                };
            }

            var tracoPrecosNaoCientesAnterioresAoAtual = _preTracoPrecoService.ListarAguardandoCienciaPorTracoUsina(
                preTracoPrecoOld.UsinaCodigo,
                preTracoPrecoOld.UsoCodigo,
                preTracoPrecoOld.PedraCodigo,
                preTracoPrecoOld.SlumpCodigo,
                preTracoPrecoOld.ResistenciaTipoCodigo,
                preTracoPrecoOld.Mpa,
                preTracoPrecoOld.Consumo
            );

            foreach (var tracoPrecoNaoCiente in tracoPrecosNaoCientesAnterioresAoAtual)
            {
                tracoPrecoNaoCiente.IDCiencia = StringHelper.GetIDD(usuario);
                tracoPrecoNaoCiente.DataCiencia = DateTime.Now;
                tracoPrecoNaoCiente.UpdatedAt = DateTime.Now;
            }

            //if (!tracoPreco.TracoEspecificacao.Equals(""))
                tracoPreco.TracoEspecificacao = preTracoPrecoOld.TracoEspecificacao;

            if (!novoTraco)
                tracoPreco.IdAtualizacao = StringHelper.GetIDD(usuario);

            if (novoTraco)
                _tracoPrecoService.Adicionar(tracoPreco);

            Commit();

        }

        public void ReprovarTodos(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario)
        {
            foreach (var preTracoPreco in preTracosPrecos)
            {
                Reprovar(preTracoPreco, usuario);
            }
        }

        public void AprovarTodos(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario)
        {
            foreach (var preTracoPreco in preTracosPrecos)
            {
                Atualizar(preTracoPreco, usuario);
            }
        }

        public void AtualizarLote(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario, ETipoAlteracaoLoteTabelaVenda tipoAlteracao, float valorAlteracao)
        {
            foreach (var preTracoPreco in preTracosPrecos)
            {
                var tracoPrecoAlterar = _preTracoPrecoService.ObterUltimoAguardandoCienciaPorTraco(
                   preTracoPreco.UsinaCodigo,
                   preTracoPreco.UsoCodigo,
                   preTracoPreco.PedraCodigo,
                   preTracoPreco.SlumpCodigo,
                   preTracoPreco.ResistenciaTipoCodigo,
                   preTracoPreco.Mpa,
                   preTracoPreco.Consumo,
                   true
               );

                var precoM3SemMarkup = ((tracoPrecoAlterar.M3Preco / 100) * (100 - tracoPrecoAlterar.Markup));
                var valorServico = precoM3SemMarkup - tracoPrecoAlterar.CustoMaterial;

                switch (tipoAlteracao)
                {
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoPorcentagem:
                        tracoPrecoAlterar.AjustarValorServicoPorPorcentagem(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoReais:
                        tracoPrecoAlterar.AjustarValorServicoPorReais(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.ValorServicoFixo:
                        tracoPrecoAlterar.AjustarValorServicoPorValorFixo(valorAlteracao);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.ManterPrecoVenda:
                        tracoPrecoAlterar.AjustarMantendoPrecoVenda( preTracoPreco.TracoPrecoVigente is null ? preTracoPreco.M3Preco : preTracoPreco.TracoPrecoVigente.M3Preco);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.MarkupPorcentagem:
                        tracoPrecoAlterar.AjustarValorMarkupPorPorcentagem(valorAlteracao, valorServico);
                        break;
                    case ETipoAlteracaoLoteTabelaVenda.MarkupFixo:
                        tracoPrecoAlterar.AjustarValorMarkupPorPorcentagemFixa(valorAlteracao, valorServico);
                        break;
                }

                Commit();
            }

        }

    }
}
