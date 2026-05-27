using Dapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class FaturaRepository : RepositoryBase<Fatura>, IFaturaRepository
    {
        private readonly IObraRepository _obraRepository;
        private readonly INotaFiscalFisicaRepository _notaFiscalFisicaRepository;
        private readonly IIntervenienteRepository _intervenienteRepository;

        public FaturaRepository(AppDataContext context, IObraRepository obraRepository, INotaFiscalFisicaRepository notaFiscalFisicaRepository, IIntervenienteRepository intervenienteRepository) : base(context)
        {
            _context = context;
            _obraRepository = obraRepository;
            _notaFiscalFisicaRepository = notaFiscalFisicaRepository;
            _intervenienteRepository = intervenienteRepository;
        }

        private Segmentacao ObterSegmentacaoContrato(Fatura fatura)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT seg.id {nameof(Segmentacao.Id)}");
            sqlComando.Append($", seg.nome {nameof(Segmentacao.Nome)}");
            sqlComando.Append($", seg.nome_abreviado {nameof(Segmentacao.NomeAbreviado)}");
            sqlComando.Append($" FROM con_contrato conc");
            sqlComando.Append($" INNER JOIN con_segmentacao seg");
            sqlComando.Append($" ON conc.segmentacao=seg.id");
            sqlComando.Append($" WHERE usina=@usinaContrato");
            sqlComando.Append($" AND num_contrato=@numeroContrato");
            sqlComando.Append($" AND ano_contrato=@anoContrato");

            return _context.Database.Connection.Query<Segmentacao>(sqlComando.ToString(), new
            {
                usinaContrato = fatura.ContratoUsina,
                numeroContrato = fatura.ContratoNumero,
                anoContrato = fatura.ContratoAno
            }).FirstOrDefault(); 
        }
        
        private ChaveNotaFiscalDigital ObterChaveNotaVendaMateriais(Fatura fatura)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT filial {nameof(ChaveNotaFiscalDigital.Filial)}");
            sqlComando.Append($", interv {nameof(ChaveNotaFiscalDigital.Cliente)}");
            sqlComando.Append($", tp_doc {nameof(ChaveNotaFiscalDigital.TipoDocumento)}");
            sqlComando.Append($", serie {nameof(ChaveNotaFiscalDigital.Serie)}");
            sqlComando.Append($", num_nf {nameof(ChaveNotaFiscalDigital.Numero)}");
            sqlComando.Append($", seq_nf {nameof(ChaveNotaFiscalDigital.Sequencia)}");
            sqlComando.Append($" FROM fis_faturamento");
            sqlComando.Append($" WHERE filial_faturamento=@Filial");
            sqlComando.Append($" AND num_faturamento=@numFaturamento");
            sqlComando.Append($" AND fatura=@numFatura");

            return _context.Database.Connection.Query<ChaveNotaFiscalDigital>(sqlComando.ToString(), new
            {
                filial = fatura.Filial,
                numFaturamento = fatura.NumeroFaturamento,
                numFatura = fatura.Numero,
            }).FirstOrDefault(); 
        }
        
        private ChaveTituloContasAReceber ObterChaveTituloJuncao(Fatura fatura)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT empj {nameof(ChaveTituloContasAReceber.EmpresaCodigo)}");
            sqlComando.Append($", tp_docj {nameof(ChaveTituloContasAReceber.DocumentoTipoCodigo)}");
            sqlComando.Append($", ser_docj {nameof(ChaveTituloContasAReceber.DocumentoSerie)}");
            sqlComando.Append($", num_docj {nameof(ChaveTituloContasAReceber.DocumentoNumero)}");
            sqlComando.Append($", seqj {nameof(ChaveTituloContasAReceber.DocumentoSequencia)}");
            sqlComando.Append($", cod_banco_bandj {nameof(ChaveTituloContasAReceber.BancoCodigoOficial)}");
            sqlComando.Append($", num_agenciaj {nameof(ChaveTituloContasAReceber.BancoNumeroAgencia)}");
            sqlComando.Append($", num_contaj {nameof(ChaveTituloContasAReceber.BancoNumeroConta)}");
            sqlComando.Append($", dv_contaj {nameof(ChaveTituloContasAReceber.BancoDvConta)}");
            sqlComando.Append($" FROM fin_car_juncao");
            sqlComando.Append($" WHERE emp=@empresa");
            sqlComando.Append($" AND tp_doc=@tpDoc");
            sqlComando.Append($" AND ser_doc=@serDoc");
            sqlComando.Append($" AND num_doc=@numDoc");
            sqlComando.Append($" AND seq=@seq");

            return _context.Database.Connection.Query<ChaveTituloContasAReceber>(sqlComando.ToString(), new
            {
                empresa = Math.Floor((double)(fatura.Filial/1000)),
                tpDoc = fatura.TipoDocumento,
                serDoc = fatura.Serie,
                numDoc = fatura.Numero,
                seq = 1
            }).FirstOrDefault(); 
        }
        
        private ChaveFatura ObterChaveNotaServicoPai(Fatura fatura)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nfs_pai.filial {nameof(ChaveFatura.Filial)}");
            sqlComando.Append($", nfs_pai.cli {nameof(ChaveFatura.Cliente)}");
            sqlComando.Append($", nfs_pai.tp_doc {nameof(ChaveFatura.TipoDocumento)}");
            sqlComando.Append($", nfs_pai.num_nf {nameof(ChaveFatura.Numero)}");
            sqlComando.Append($", nfs_pai.ser {nameof(ChaveFatura.Serie)}");
            sqlComando.Append($", nfs_pai.sub_ser {nameof(ChaveFatura.SubSerie)}");
            sqlComando.Append($", nfs_pai.versao_contrato versao");
            sqlComando.Append($" FROM con_fat_parcial a");
            sqlComando.Append($" LEFT JOIN con_fat_parcial b ON a.filial = b.filial");
            sqlComando.Append($" AND a.interv = b.interv");
            sqlComando.Append($" AND a.tp_doc = b.tp_doc");
            sqlComando.Append($" AND a.num_nf = b.num_nf");
            sqlComando.Append($" AND a.serie = b.serie");
            sqlComando.Append($" AND a.seq_nf = b.seq_nf");
            sqlComando.Append($" AND b.tp_fatura = 'TSV' AND a.tp_fatura <> 'TSV'");
            sqlComando.Append($" LEFT JOIN fis_nf_servico nfs_pai ON b.filial_faturamento = nfs_pai.filial");
            sqlComando.Append($" AND b.num_faturamento = nfs_pai.no_faturamento");
            sqlComando.Append($" AND b.fatura = nfs_pai.num_nf");
            sqlComando.Append($" AND b.interv = nfs_pai.cli");
            sqlComando.Append($" AND nfs_pai.tp_doc = 16");
            sqlComando.Append($" WHERE a.filial_faturamento = @Filial");
            sqlComando.Append($" AND a.fatura = @numFatura");
            sqlComando.Append($" AND a.num_faturamento=@numFaturamento");
            
            //Bombeamento não tem versionamento de contrato, logo não se pode filtrar pela versão.
            //A bomba será vinculada a nota de serviço com a maior versão de contrato
            if (fatura.SegmentacaoNome!="Bombeamento")
                sqlComando.Append($" AND nfs_pai.versao_contrato = @versaoContrato");
            
            sqlComando.Append($" AND NOT ISNULL(nfs_pai.num_nf)");
            sqlComando.Append($" UNION ");
            sqlComando.Append($"SELECT nfs_pai.filial {nameof(ChaveFatura.Filial)}");
            sqlComando.Append($", nfs_pai.cli {nameof(ChaveFatura.Cliente)}");
            sqlComando.Append($", nfs_pai.tp_doc {nameof(ChaveFatura.TipoDocumento)}");
            sqlComando.Append($", nfs_pai.num_nf {nameof(ChaveFatura.Numero)}");
            sqlComando.Append($", nfs_pai.ser {nameof(ChaveFatura.Serie)}");
            sqlComando.Append($", nfs_pai.sub_ser {nameof(ChaveFatura.SubSerie)}");
            sqlComando.Append($", nfs_pai.versao_contrato versao");
            sqlComando.Append($" FROM con_fat_parcial a");
            sqlComando.Append($" LEFT JOIN con_fat_parcial b ON a.interv = b.interv");
            sqlComando.Append($" AND a.filial_faturamento = b.filial_faturamento");
            sqlComando.Append($" AND a.num_faturamento = b.num_faturamento");
            sqlComando.Append($" AND b.tp_fatura = 'TSV'");
            sqlComando.Append($" AND a.tp_fatura <> 'TSV'");
            sqlComando.Append($" LEFT JOIN fis_nf_servico nfs_pai ON b.filial_faturamento = nfs_pai.filial");
            sqlComando.Append($" AND b.num_faturamento = nfs_pai.no_faturamento");
            sqlComando.Append($" AND b.fatura = nfs_pai.num_nf");
            sqlComando.Append($" AND b.interv = nfs_pai.cli");
            sqlComando.Append($" AND nfs_pai.tp_doc = 16");
            sqlComando.Append($" WHERE a.filial_faturamento = @Filial");
            sqlComando.Append($" AND a.fatura = @numFatura");
            sqlComando.Append($" AND a.num_faturamento=@numFaturamento");
            
            //Bombeamento não tem versionamento de contrato, logo não se pode filtrar pela versão.
            //A bomba será vinculada a nota de serviço com a maior versão de contrato
            if (fatura.SegmentacaoNome!="Bombeamento")
                sqlComando.Append($" AND nfs_pai.versao_contrato = @versaoContrato");
            
            sqlComando.Append($" AND nfs_pai.usina_contrato = @usinaContrato");
            sqlComando.Append($" AND nfs_pai.num_contrato = @numeroContrato");
            sqlComando.Append($" AND nfs_pai.ano_contrato = @anoContrato");
            
            sqlComando.Append($" AND NOT ISNULL(nfs_pai.num_nf)");
            sqlComando.Append($" ORDER BY versao DESC");

            return _context.Database.Connection.Query<ChaveFatura>(sqlComando.ToString(), new
            {
                filial = fatura.Filial,
                numFaturamento = fatura.NumeroFaturamento,
                numFatura = fatura.Numero,
                usinaContrato = fatura.ContratoUsina,
                numeroContrato = fatura.ContratoNumero,
                anoContrato = fatura.ContratoAno,
                versaoContrato = fatura.VersaoContrato
            }).FirstOrDefault(); 
        }

        private void CarregarDadosComplementares(List<Fatura> faturas)
        {
            CarregarDadosRemessa(faturas);
            CarregarDadosCliente(faturas);
        }

        private void CarregarDadosComplementares(Fatura fatura)
        {
            CarregarDadosRemessa(fatura);
            CarregarDadosCliente(fatura);
        }

        private void CarregarDadosRemessa(List<Fatura> faturas)
        {
            if (faturas == null || faturas.Count == 0)
                return;

            foreach (var fatura in faturas)
            {
                foreach (var item in fatura.Itens)
                {
                    var remessa = _notaFiscalFisicaRepository.ObterPorChave(
                       item.FilialNf,
                       item.Cliente,
                       item.TipoDocumentoNf,
                       item.SerieNf,
                       item.NumeroNf,
                       0
                   );

                    if (remessa != null)
                    {
                        item.UsinaFaturamentoNf = remessa.UsinaFaturamento;
                        item.UsinaPesagemNf = remessa.UsinaPesagem;
                    }
                }
            }
        }

        private void CarregarDadosRemessa(Fatura fatura)
        {
            if (fatura == null || fatura.Itens == null)
                return;

            foreach (var item in fatura.Itens)
            {
                var remessa = _notaFiscalFisicaRepository.ObterPorChave(
                    item.FilialNf,
                    item.Cliente,
                    item.TipoDocumentoNf,
                    item.SerieNf,
                    item.NumeroNf,
                    0
                );

                if (remessa != null)
                {
                    item.UsinaFaturamentoNf = remessa.UsinaFaturamento;
                    item.UsinaPesagemNf = remessa.UsinaPesagem;
                }
            }

        }

        private void CarregarDadosCliente(List<Fatura> faturas)
        {
            if (faturas?.Any() != true) return;

            foreach (var fatura in faturas)
                CarregarDadosCliente(fatura);
        }

        private void CarregarDadosCliente(Fatura fatura)
        {
            if (fatura?.Itens == null)
                return;

            var interveniente = _intervenienteRepository.ObterPorCodigo(fatura.Cliente);

            if (interveniente == null) return;

            fatura.ClienteCfpCnpj = interveniente.CpfCnpj;
            fatura.ClienteCodigoExterno = interveniente.IdExterno;
            fatura.ClienteInscEstadual = interveniente.InscricaoEstadual;
        }

        public Fatura ObterPorChaveFatura(Expression<Func<Fatura, bool>> filter, bool tracking = false)
        {
            var fatura = _context.Faturas
                .Include(f => f.Itens.Select(item => item.Mercadoria))
                .Where(filter)
                .Tracking(tracking)
                .ToList()
                .Select(faturaItem =>
                {
                    faturaItem.Itens = faturaItem.Itens
                        .OrderBy(item => item.SequenciaItem)
                        .ToList();

                    return faturaItem;
                })
                .FirstOrDefault();

            if (fatura != null)
            {
                fatura.SegmentacaoContrato = ObterSegmentacaoContrato(fatura);
                fatura.SetSegmentacaoContratoPorBombaTaxaExtra();
                fatura.Obra= _obraRepository.ObterObraPorContrato(fatura.ContratoUsina, fatura.ContratoAno, fatura.ContratoNumero);
                fatura.ChaveNotaVendaMateriais = ObterChaveNotaVendaMateriais(fatura);
                fatura.ChaveTituloJuncao = ObterChaveTituloJuncao(fatura);
                fatura.ChaveNotaServicoPai = ObterChaveNotaServicoPai(fatura);
            }

            CarregarDadosComplementares(fatura);

            return fatura;
        }



        public ICollection<Fatura> ListarComPaginacao(Expression<Func<Fatura, bool>> filter, int page, int limit)
        {
            var faturas = _context.Faturas
                .Include(f => f.Itens.Select(item => item.Mercadoria))
                .Where(filter)
                .OrderBy(t => t.Numero)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList()
                .Select(faturaItem =>
                {
                    faturaItem.Itens = faturaItem.Itens.OrderBy(item => item.SequenciaItem).ToList();
                    
                    return faturaItem;
                })
                .ToList();

            CarregarDadosComplementares(faturas);

            foreach (var fatura in faturas)
            {
                if (fatura != null)
                {
                    fatura.SegmentacaoContrato = ObterSegmentacaoContrato(fatura);
                    fatura.SetSegmentacaoContratoPorBombaTaxaExtra();
                    fatura.Obra= _obraRepository.ObterObraPorContrato(fatura.ContratoUsina, fatura.ContratoAno, fatura.ContratoNumero);
                    fatura.ChaveNotaVendaMateriais = ObterChaveNotaVendaMateriais(fatura);
                    fatura.ChaveTituloJuncao = ObterChaveTituloJuncao(fatura);
                    fatura.ChaveNotaServicoPai = ObterChaveNotaServicoPai(fatura);
                }
            }

            return faturas;
        }

        public PagedList<Fatura> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nfs.filial {nameof(Fatura.Filial)}");
            sqlComando.Append($", nfs.cli {nameof(Fatura.Cliente)}");
            sqlComando.Append($", nfs.tp_doc {nameof(Fatura.TipoDocumento)}");
            sqlComando.Append($", nfs.num_nf {nameof(Fatura.Numero)}");
            sqlComando.Append($", nfs.ser {nameof(Fatura.Serie)}");
            sqlComando.Append($", nfs.sub_ser {nameof(Fatura.SubSerie)}");
            sqlComando.Append($", nfs.num_rps {nameof(Fatura.NumeroRps)}");
            sqlComando.Append($", nfs.num_nf_serv_seq {nameof(Fatura.NumeroNfse)}");
            sqlComando.Append($", nfs.dt_nf {nameof(Fatura.DataNf)}");
            sqlComando.Append($", nfs.cod_fisc_pserv {nameof(Fatura.CodFiscalPrestadorServico)}");
            sqlComando.Append($", nfs.munic_pserv {nameof(Fatura.MunicipioPrestacaoServico)}");
            sqlComando.Append($", nfs.nat_prestacao {nameof(Fatura.NaturezaPrestacao)}");
            sqlComando.Append($", nfs.usina_contrato {nameof(Fatura.ContratoUsina)}");
            sqlComando.Append($", nfs.num_contrato {nameof(Fatura.ContratoNumero)}");
            sqlComando.Append($", nfs.ano_contrato {nameof(Fatura.ContratoAno)}");
            sqlComando.Append($", nfs.faturamento_ac {nameof(Fatura.FaturamentoAC)}");
            sqlComando.Append($", nfs.local_faturamen {nameof(Fatura.LocalFaturamento)}");
            sqlComando.Append($", nfs.local_cobranca {nameof(Fatura.LocalCobranca)}");
            sqlComando.Append($", nfs.cond_pgto {nameof(Fatura.CondicaoPagamento)}");
            sqlComando.Append($", nfs.ind_pgto {nameof(Fatura.IndicadorPagamento)}");
            sqlComando.Append($", nfs.cancelada {nameof(Fatura.Cancelada)}");
            sqlComando.Append($", nfs.motivo_cancelam {nameof(Fatura.MotivoCancelamento)}");
            sqlComando.Append($", nfs.vl_serv_bruto {nameof(Fatura.ValorServicoBruto)}");
            sqlComando.Append($", nfs.vl_desco {nameof(Fatura.ValorDesconto)}");
            sqlComando.Append($", nfs.vl_serv {nameof(Fatura.ValorServico)}");
            sqlComando.Append($", nfs.vlr_mat_prop_ut {nameof(Fatura.ValorMateriaisProprio)}");
            sqlComando.Append($", nfs.vlr_mat_3os_ut {nameof(Fatura.ValorMateriaisTerceiros)}");
            sqlComando.Append($", nfs.desp_acessorias {nameof(Fatura.ValorDespesasAcessorias)}");
            sqlComando.Append($", nfs.vlr_subcontrata {nameof(Fatura.ValorSubContratada)}");
            sqlComando.Append($", nfs.valor_total {nameof(Fatura.ValorTotal)}");
            sqlComando.Append($", nfs.base_calc_iss {nameof(Fatura.BaseCalculoIss)}");
            sqlComando.Append($", nfs.pct_iss {nameof(Fatura.PercentualIss)}");
            sqlComando.Append($", nfs.vl_iss {nameof(Fatura.ValorIss)}");
            sqlComando.Append($", nfs.obs_fisc_nf {nameof(Fatura.ObservacaoFiscalNf)}");
            sqlComando.Append($", nfs.base_calc_reten {nameof(Fatura.BaseCalculoRetencao)}");
            sqlComando.Append($", nfs.pct_iss_retido {nameof(Fatura.PercentualIssRetido)}");
            sqlComando.Append($", nfs.vlr_iss_retido {nameof(Fatura.ValorIssRetido)}");
            sqlComando.Append($", nfs.base_cal_irrf {nameof(Fatura.BaseCalculoIrrf)}");
            sqlComando.Append($", nfs.pct_irrf {nameof(Fatura.PercentualIrrf)}");
            sqlComando.Append($", nfs.vlr_irrf {nameof(Fatura.ValorIrrf)}");
            sqlComando.Append($", nfs.pct_pis {nameof(Fatura.PercentualPis)}");
            sqlComando.Append($", nfs.vlr_pis {nameof(Fatura.ValorPis)}");
            sqlComando.Append($", nfs.pct_cofins {nameof(Fatura.PercentualCofins)}");
            sqlComando.Append($", nfs.vlr_cofins {nameof(Fatura.ValorCofins)}");
            sqlComando.Append($", nfs.base_cal_inss_ret {nameof(Fatura.BaseCalculoInssRetido)}");
            sqlComando.Append($", nfs.vlr_inss_retido {nameof(Fatura.ValorInssRetido)}");
            sqlComando.Append($", nfs.representante {nameof(Fatura.Representante)}");
            sqlComando.Append($", nfs.vlr_comis_repre {nameof(Fatura.ValorComissaoRepresentante)}");
            sqlComando.Append($", nfs.vendedor {nameof(Fatura.Vendedor)}");
            sqlComando.Append($", nfs.vlr_comis_vende {nameof(Fatura.ValorComissaoVendedor)}");
            sqlComando.Append($", nfs.qtde_parcelas {nameof(Fatura.QuantidadeParcelas)}");
            sqlComando.Append($", nfs.no_faturamento {nameof(Fatura.NumeroFaturamento)}");
            sqlComando.Append($", nfs.valor_total_bomba {nameof(Fatura.ValorTotalBomba)}");
            sqlComando.Append($", nfs.base_calc_iss_bomba {nameof(Fatura.BaseCalculoIssBomba)}");
            sqlComando.Append($", nfs.vl_iss_bomba {nameof(Fatura.ValorIssBomba)}");
            sqlComando.Append($", nfs.vlr_iss_retido_bomba {nameof(Fatura.ValorIssRetidoBomba)}");
            sqlComando.Append($", nfs.pct_inss_retido {nameof(Fatura.PercentualInssRetido)}");
            sqlComando.Append($", nfs.base_calc_pis {nameof(Fatura.BaseCalculoPis)}");
            sqlComando.Append($", nfs.base_cal_cofins {nameof(Fatura.BaseCalculoCofins)}");
            sqlComando.Append($", nfs.base_calc_irpj {nameof(Fatura.BaseCalculoIrpj)}");
            sqlComando.Append($", nfs.pct_irpj {nameof(Fatura.PercentualIrpj)}");
            sqlComando.Append($", nfs.vlr_irpj {nameof(Fatura.ValorIrpj)}");
            sqlComando.Append($", nfs.base_calc_csll {nameof(Fatura.BaseCalculoCsll)}");
            sqlComando.Append($", nfs.pct_csll {nameof(Fatura.PercentualCsll)}");
            sqlComando.Append($", nfs.vlr_csll {nameof(Fatura.ValorCsll)}");
            sqlComando.Append($", nfs.cod_verfic_nfse {nameof(Fatura.CodigoVerificadorNfse)}");
            sqlComando.Append($", nfs.requis_interna {nameof(Fatura.RequisicaoInterna)}");
            sqlComando.Append($", nfs.requisitante {nameof(Fatura.Requisitante)}");
            sqlComando.Append($", nfs.forn_iss {nameof(Fatura.FornecedorIss)}");
            sqlComando.Append($", nfs.total_base_pis {nameof(Fatura.TotalBasePis)}");
            sqlComando.Append($", nfs.total_pis {nameof(Fatura.TotalPis)}");
            sqlComando.Append($", nfs.total_base_cofins {nameof(Fatura.TotalBaseCofins)}");
            sqlComando.Append($", nfs.total_cofins {nameof(Fatura.TotalCofins)}");
            sqlComando.Append($", nfs.dt_lancamento {nameof(Fatura.DataLancamento)}");
            sqlComando.Append($", nfs.pendente {nameof(Fatura.Pendente)}");
            sqlComando.Append($", nfs.bc_retencoes {nameof(Fatura.BcRetencoes)}");
            sqlComando.Append($", nfs.cc {nameof(Fatura.CentroCusto)}");
            sqlComando.Append($", nfs.num_recibo {nameof(Fatura.NumeroRecibo)}");
            sqlComando.Append($", nfs.encapsulamento {nameof(Fatura.Encapsulamento)}");
            sqlComando.Append($", nfs.num_protocolo {nameof(Fatura.NumeroProtocolo)}");
            sqlComando.Append($", nfs.pct_inss {nameof(Fatura.PercentualInss)}");
            sqlComando.Append($", nfs.vlr_inss {nameof(Fatura.ValorInss)}");
            sqlComando.Append($", nfs.base_cal_inss {nameof(Fatura.BaseCalculoInss)}");
            sqlComando.Append($", nfs.versao_contrato {nameof(Fatura.VersaoContrato)}");
            sqlComando.Append($", nfs.atualizado_em {nameof(Fatura.DataAtualizacao)}");
            sqlComando.Append($", nfs.base_ibscbs {nameof(Fatura.BaseCbsIbs)}");
            sqlComando.Append($", nfs.vl_cbs {nameof(Fatura.ValorCbs)}");
            sqlComando.Append($", nfs.vl_ibs {nameof(Fatura.ValorIbs)}");
            sqlComando.Append($", nfs.vl_ibs_mun {nameof(Fatura.ValorIbsMunicipal)}");
            sqlComando.Append($", nfs.vl_ibs_uf {nameof(Fatura.ValorIbsEstadual)}");
            
            sqlComando.Append($" FROM fis_nf_servico nfs");
            sqlComando.Append($" WHERE atualizado_em>='{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");

            if (dataFim != null)
                sqlComando.Append($" AND atualizado_em<='{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" ORDER BY atualizado_em");

            var faturas = _context.Connection.QueryPagedList<Fatura>(sqlComando.ToString(), page, limit);

            var faturasLista = new List<Fatura>();

            var faturasResultPagedList = new PagedList<Fatura>
            {
                CurrentPage = faturas.CurrentPage,
                PageCount = faturas.PageCount,
                PageSize = faturas.PageSize,
                RecordCount = faturas.RecordCount
            };

            foreach (var record in faturas.Records)
            {
                var fatura = (Fatura)record;

                sqlComando.Clear();
                sqlComando.Append($"SELECT item.filial {nameof(FaturaItem.Filial)}");
                sqlComando.Append($", item.cli {nameof(FaturaItem.Cliente)}");
                sqlComando.Append($", item.tp_docs {nameof(FaturaItem.TipoDocumento)}");
                sqlComando.Append($", item.num_nfs {nameof(FaturaItem.Numero)}");
                sqlComando.Append($", item.serie_nfs {nameof(FaturaItem.Serie)}");
                sqlComando.Append($", item.sub_series {nameof(FaturaItem.SubSerie)}");
                sqlComando.Append($", item.seq_item {nameof(FaturaItem.SequenciaItem)}");
                sqlComando.Append($", item.filial_nf {nameof(FaturaItem.FilialNf)}");
                sqlComando.Append($", item.tp_doc_nf {nameof(FaturaItem.TipoDocumentoNf)}");
                sqlComando.Append($", item.num_nf {nameof(FaturaItem.NumeroNf)}");
                sqlComando.Append($", item.serie_nf {nameof(FaturaItem.SerieNf)}");
                sqlComando.Append($", item.dt_nf {nameof(FaturaItem.DataNf)}");
                sqlComando.Append($", item.prod_serv {nameof(FaturaItem.CodigoTraco)}");
                sqlComando.Append($", item.unidade {nameof(FaturaItem.Unidade)}");
                sqlComando.Append($", item.qt {nameof(FaturaItem.Quantidade)}");
                sqlComando.Append($", item.preco_unit {nameof(FaturaItem.PrecoUnitario)}");
                sqlComando.Append($", item.preco_tot {nameof(FaturaItem.PrecoTotal)}");
                sqlComando.Append($", item.vlr_material {nameof(FaturaItem.ValorMaterial)}");
                sqlComando.Append($", item.vl_serv_bruto {nameof(FaturaItem.ValorServicoBruto)}");
                sqlComando.Append($", item.vlr_servico {nameof(FaturaItem.ValorServico)}");
                sqlComando.Append($", item.vl_desco {nameof(FaturaItem.ValorDesconto)}");
                sqlComando.Append($", item.vl_liq {nameof(FaturaItem.ValorLiquido)}");
                sqlComando.Append($", item.PisCst {nameof(FaturaItem.PisCodigoSituacaoTributaria)}");
                sqlComando.Append($", item.PisBC {nameof(FaturaItem.PisBaseCalculo)}");
                sqlComando.Append($", item.PisPct {nameof(FaturaItem.PisPercentual)}");
                sqlComando.Append($", item.PisValor {nameof(FaturaItem.PisValor)}");
                sqlComando.Append($", item.CofinsCst {nameof(FaturaItem.CofinsCodigoSituacaoTributaria)}");
                sqlComando.Append($", item.CofinsBC {nameof(FaturaItem.CofinsBaseCalculo)}");
                sqlComando.Append($", item.CofinsPct {nameof(FaturaItem.CofinsPercentual)}");
                sqlComando.Append($", item.CofinsValor {nameof(FaturaItem.CofinsValor)}");
                sqlComando.Append($", item.tribContrib {nameof(FaturaItem.TributacaoContribuicao)}");
                sqlComando.Append($", item.IniVigTribCont {nameof(FaturaItem.InicioVigenciaTribContribuicao)}");
                sqlComando.Append($", item.vlr_pis_ret {nameof(FaturaItem.ValorPisRetido)}");
                sqlComando.Append($", item.vlr_cofins_ret {nameof(FaturaItem.ValorCofinsRetido)}");
                sqlComando.Append($", item.vlr_irrf {nameof(FaturaItem.ValorIrrf)}");
                sqlComando.Append($", item.vlr_csll_ret {nameof(FaturaItem.ValorCsllRetido)}");
                sqlComando.Append($", item.iss_bc {nameof(FaturaItem.IssBaseCalculo)}");
                sqlComando.Append($", item.iss_pct {nameof(FaturaItem.IssPercentual)}");
                sqlComando.Append($", item.iss_vlr {nameof(FaturaItem.IssValor)}");
                sqlComando.Append($", item.iss_vlr_ret {nameof(FaturaItem.IssValorRetido)}");
                sqlComando.Append($", item.num_recibo {nameof(FaturaItem.NumeroRecibo)}");
                sqlComando.Append($", item.ccusto {nameof(FaturaItem.CentroCusto)}");
                sqlComando.Append($", item.id_imp_cbs {nameof(FaturaItem.IdImpostoCbs)}");
                sqlComando.Append($", item.id_imp_ibs {nameof(FaturaItem.IdImpostoIbs)}");
                sqlComando.Append($", item.cst_cbs_ibs {nameof(FaturaItem.CstCbsIbs)}");
                sqlComando.Append($", item.clas_trib_cbs_ibs {nameof(FaturaItem.ClassificacaoTributariaCbsIbs)}");
                sqlComando.Append($", item.base_ibscbs {nameof(FaturaItem.BaseCbsIbs)}");
                sqlComando.Append($", item.aliq_cbs {nameof(FaturaItem.AliquotaCbs)}");
                sqlComando.Append($", item.aliq_cbs_efet {nameof(FaturaItem.AliquotaCbsEfetiva)}");
                sqlComando.Append($", item.p_redcbs {nameof(FaturaItem.PercentualReducaoCbs)}");
                sqlComando.Append($", item.vl_cbs {nameof(FaturaItem.ValorCbs)}");
                sqlComando.Append($", item.aliq_ibs_mun_efet {nameof(FaturaItem.AliquotaIbsMunicipalEfetiva)}");
                sqlComando.Append($", item.aliq_ibs_mun {nameof(FaturaItem.AliquotaIbsMunicipal)}");
                sqlComando.Append($", item.p_redibs_mun {nameof(FaturaItem.PercentualReducaoIbsMunicipal)}");
                sqlComando.Append($", item.vl_ibs_mun {nameof(FaturaItem.ValorIbsMunicipal)}");
                sqlComando.Append($", item.aliq_ibs_uf_efet {nameof(FaturaItem.AliquotaIbsEstadualEfetiva)}");
                sqlComando.Append($", item.aliq_ibs_uf {nameof(FaturaItem.AliquotaIbsEstadual)}");
                sqlComando.Append($", item.p_redibs_uf {nameof(FaturaItem.PercentualReducaoIbsEstadual)}");
                sqlComando.Append($", item.vl_ibs_uf {nameof(FaturaItem.ValorIbsEstadual)}");
                sqlComando.Append($", item.vl_ibs {nameof(FaturaItem.ValorIbs)}");
                sqlComando.Append($" FROM fis_item_nf_serv item");
                sqlComando.Append($" WHERE item.filial=@filial");
                sqlComando.Append($" AND item.cli=@cliente");
                sqlComando.Append($" AND item.tp_docs=@tipoDocumento");
                sqlComando.Append($" AND item.num_nfs=@numero");
                sqlComando.Append($" AND item.serie_nfs=@serie");
                sqlComando.Append($" AND item.sub_series=@subSerie");

                fatura.Itens = _context.Database.Connection.Query<FaturaItem>(sqlComando.ToString(), new
                {
                    filial = fatura.Filial,
                    cliente = fatura.Cliente,
                    tipoDocumento = fatura.TipoDocumento,
                    numero = fatura.Numero,
                    serie = fatura.Serie,
                    subSerie = fatura.SubSerie
                }).ToList();
                
                foreach (var faturaItem in fatura.Itens)
                {
                    faturaItem.Mercadoria = _context.Mercadoria.FirstOrDefault(t => t.Codigo == faturaItem.CodigoTraco);
                }
                
                fatura.SegmentacaoContrato = ObterSegmentacaoContrato(fatura);
                fatura.SetSegmentacaoContratoPorBombaTaxaExtra();
                fatura.Obra= _obraRepository.ObterObraPorContrato(fatura.ContratoUsina, fatura.ContratoAno, fatura.ContratoNumero);
                fatura.ChaveNotaVendaMateriais = ObterChaveNotaVendaMateriais(fatura);
                fatura.ChaveTituloJuncao = ObterChaveTituloJuncao(fatura);
                fatura.ChaveNotaServicoPai = ObterChaveNotaServicoPai(fatura);

                faturasLista.Add(fatura);
            }

            CarregarDadosComplementares(faturasLista);

            faturasResultPagedList.Records = faturasLista;

            return faturasResultPagedList;
        }
    }
}
