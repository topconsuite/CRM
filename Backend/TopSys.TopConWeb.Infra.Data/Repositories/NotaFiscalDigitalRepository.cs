using Dapper;
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
    public class NotaFiscalDigitalRepository : RepositoryBase<NotaFiscalDigital>, INotaFiscalDigitalRepository
    {
        private readonly IIntervenienteRepository _intervenienteRepository;
        private readonly IFaturaRepository _faturaRepository;
        public NotaFiscalDigitalRepository(AppDataContext context, IIntervenienteRepository intervenienteRepository, IFaturaRepository faturaRepository) : base(context)
        {
            _context = context;
            _intervenienteRepository = intervenienteRepository;
            _faturaRepository = faturaRepository;
        }

        private int ObterIndicadorDestIe(int codInterveniente)
        {
            var interv = _intervenienteRepository.ObterPorId(codInterveniente);

            if (interv == null)
                return (int)EIndicadorDestInscEstadual.NaoDefinido;

            if (interv.ContribuiIcms != 0)
                return interv.ContribuiIcms;
            
            if (interv.InscricaoEstadual == "ISENTO")
                return (int)EIndicadorDestInscEstadual.ContribIsento;

            return interv.CpfCnpj.Length == 14 ? (int)EIndicadorDestInscEstadual.ContribIcms : (int)EIndicadorDestInscEstadual.NaoContribuinte;
        }

        private void CarregarDadosCliente(NotaFiscalDigital notaFiscalDigital)
        {
            if (notaFiscalDigital?.Itens == null)
                return;

            var interveniente = _intervenienteRepository.ObterPorCodigo(notaFiscalDigital.Cliente);

            if (interveniente == null) return;

            notaFiscalDigital.ClienteCfpCnpj = interveniente.CpfCnpj;
            notaFiscalDigital.ClienteInscEstadual = interveniente.InscricaoEstadual;
        }
        
        private Fatura ObterFaturaMateriais(NotaFiscalDigital notaFiscal)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nfs.filial {nameof(ChaveFatura.Filial)}");
            sqlComando.Append($", nfs.cli {nameof(ChaveFatura.Cliente)}");
            sqlComando.Append($", nfs.tp_doc {nameof(ChaveFatura.TipoDocumento)}");
            sqlComando.Append($", nfs.num_nf {nameof(ChaveFatura.Numero)}");
            sqlComando.Append($", nfs.ser {nameof(ChaveFatura.Serie)}");
            sqlComando.Append($", nfs.sub_ser {nameof(ChaveFatura.SubSerie)}");
            sqlComando.Append($" FROM fis_faturamento fis_fat");
            sqlComando.Append($" LEFT JOIN fis_nf_servico nfs ON fis_fat.filial_faturamento = nfs.filial");
            sqlComando.Append($" AND fis_fat.num_faturamento = nfs.no_faturamento");
            sqlComando.Append($" AND fis_fat.fatura = nfs.num_nf");
            sqlComando.Append($" AND fis_fat.interv = nfs.cli");
            sqlComando.Append($" AND nfs.tp_doc = 16");
            sqlComando.Append($" WHERE fis_fat.filial=@Filial");
            sqlComando.Append($" AND fis_fat.interv=@interv");
            sqlComando.Append($" AND fis_fat.tp_doc=@tpDoc");
            sqlComando.Append($" AND fis_fat.serie=@serie");
            sqlComando.Append($" AND fis_fat.num_nf=@numNf");
            sqlComando.Append($" AND fis_fat.seq_nf=@seqNf");

            var chave = _context.Database.Connection.Query<ChaveFatura>(sqlComando.ToString(), new
            {
                filial = notaFiscal.Filial,
                interv = notaFiscal.Cliente,
                tpDoc = notaFiscal.TipoDocumento,
                serie = notaFiscal.Serie,
                numNf = notaFiscal.Numero,
                seqNf = notaFiscal.Sequencia,
            }).FirstOrDefault();

            if (chave == null) return null;
            
            return  _faturaRepository.ObterPorChaveFatura(x => x.Filial == chave.Filial && x.Cliente == chave.Cliente
                && x.TipoDocumento == chave.TipoDocumento && x.Numero == chave.Numero && x.Serie == chave.Serie);
        }

        private string ObterChaveNfDevolucao(NotaFiscalDigital notaFiscal)
        {
            StringBuilder sqlComando = new StringBuilder();
            
            
            sqlComando.Append($"SELECT chave_nfe_ref FROM fis_vinc_nfe_ref");
            sqlComando.Append($" WHERE filial=@Filial");
            sqlComando.Append($" AND interv=@interv");
            sqlComando.Append($" AND tp_doc=@tpDoc");
            sqlComando.Append($" AND serie=@serie");
            sqlComando.Append($" AND num_nf=@numNf");
            sqlComando.Append($" AND seq_nf=@seqNf");
            
            
            return  _context.Database.Connection.Query<string>(sqlComando.ToString(), new
            {
                filial = notaFiscal.Filial,
                interv = notaFiscal.Cliente,
                tpDoc = notaFiscal.TipoDocumento,
                serie = notaFiscal.Serie,
                numNf = notaFiscal.Numero,
                seqNf = notaFiscal.Sequencia,
            }).FirstOrDefault();
        }

        private NotaFiscalDigitalComplemento ObterNFComplemento(int filial, int cliente, int tipoDocumento, string serie, long numero, int sequencia)
        {
            return _context.NotasFiscaisDigitalComplemento
                .Where(t => t.Filial == filial && 
                    t.Cliente == cliente && 
                    t.TipoDocumento == tipoDocumento &&
                    t.Serie == serie &&
                    t.Numero == numero &&
                    t.Sequencia == sequencia) 
                .FirstOrDefault();
        }

        private NotaFiscalDigitalItemComplemento ObterItemNFComplemento(int filial, int cliente, int tipoDocumento, string serie, long numero, int sequencia, int sequenciaItem)
        {
            return _context.NotaFiscalDigitalItemComplemento
                .Where(t => t.Filial == filial &&
                    t.Cliente == cliente &&
                    t.TipoDocumento == tipoDocumento &&
                    t.Serie == serie &&
                    t.Numero == numero &&
                    t.Sequencia == sequencia && 
                    t.SequenciaItem == sequenciaItem)
                .FirstOrDefault();
        }


        public NotaFiscalDigitalDetalhesFiscais ObterNFDetalhesFiscais(NotaFiscalDigital notaFiscalDigital)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nfe.ger_emp_Cod {nameof(NotaFiscalDigitalDetalhesFiscais.Filial)}");
            sqlComando.Append($", nfe.ger_Interv_Cod {nameof(NotaFiscalDigitalDetalhesFiscais.Cliente)}");
            sqlComando.Append($", nfe.nfe_Mod {nameof(NotaFiscalDigitalDetalhesFiscais.TipoDocumento)}");
            sqlComando.Append($", nfe.nfe_Ser {nameof(NotaFiscalDigitalDetalhesFiscais.Serie)}");
            sqlComando.Append($", nfe.nfe_nNF {nameof(NotaFiscalDigitalDetalhesFiscais.Numero)}");
            sqlComando.Append($", nfe.nfe_cSit {nameof(NotaFiscalDigitalDetalhesFiscais.SituacaoSefaz)}");
            sqlComando.Append($", nfe.nfe_nRec {nameof(NotaFiscalDigitalDetalhesFiscais.ReciboSefaz)}");
            sqlComando.Append($", nfe.nfe_nProt {nameof(NotaFiscalDigitalDetalhesFiscais.ProtocoloSefaz)}");
            sqlComando.Append($", nfe.nfe_cStat {nameof(NotaFiscalDigitalDetalhesFiscais.StatusAutorizacao)}");
            sqlComando.Append($", nfe.nfe_Motivo {nameof(NotaFiscalDigitalDetalhesFiscais.MotivoDescricaoStatus)}");
            sqlComando.Append($", nfe.nfe_dhProc {nameof(NotaFiscalDigitalDetalhesFiscais.DataHoraProtocolo)}");
            sqlComando.Append($", nfe.nfe_XML {nameof(NotaFiscalDigitalDetalhesFiscais.Xml)}");
            sqlComando.Append($", nfe.nfe_XMLAutor {nameof(NotaFiscalDigitalDetalhesFiscais.XmlAutor)}");
            sqlComando.Append($", nfe.nfe_Uf {nameof(NotaFiscalDigitalDetalhesFiscais.NfeUf)}");
            sqlComando.Append($", IFNULL(interv.ger_Interv_indIEDest, 0) {nameof(NotaFiscalDigitalDetalhesFiscais.IndicadorDestinatarioIe)}");
            
            sqlComando.Append($" FROM topnfe.nfe nfe");
            
            sqlComando.Append($" LEFT JOIN Topnfe.ger_interv interv");
            sqlComando.Append($" ON interv.ger_Interv_Cod=nfe.ger_Interv_Cod");
            
            sqlComando.Append($" WHERE (nfe.ger_emp_Cod=@filial");
            sqlComando.Append($" AND nfe.ger_Interv_Cod=@interveniente");
            sqlComando.Append($" AND nfe.nfe_Mod=@tipoDocumento");
            sqlComando.Append($" AND nfe.nfe_Ser=@serie");
            sqlComando.Append($" AND nfe.nfe_nNF=@numeroNf)");
            sqlComando.Append($" OR (nfe.nfe_chNFe=@chaveNfe AND nfe.nfe_chNFe<>'')");
            
            var detalhesFiscaisNota =  _context.Database.Connection.Query<NotaFiscalDigitalDetalhesFiscais>(sqlComando.ToString(), new
            {
                filial = notaFiscalDigital.Filial,
                interveniente = notaFiscalDigital.Cliente,
                tipoDocumento = notaFiscalDigital.TipoDocumento,
                serie = notaFiscalDigital.Serie,
                numeroNf = notaFiscalDigital.Numero,
                chaveNfe = notaFiscalDigital.ChaveNfe
            }).FirstOrDefault();

            if (detalhesFiscaisNota?.IndicadorDestinatarioIe==0)
                detalhesFiscaisNota.IndicadorDestinatarioIe = ObterIndicadorDestIe(detalhesFiscaisNota.Cliente);

            return detalhesFiscaisNota;
        }

        public NotaFiscalDigitalDetalhesDistribuicao ObterNFDetalhesDistribuicao(NotaFiscalDigital notaFiscalDigital)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT dist.nfe_distribuicao_NSU {nameof(NotaFiscalDigitalDetalhesDistribuicao.Nsu)}");
            sqlComando.Append($", dist.nfe_distribuicao_chNFe {nameof(NotaFiscalDigitalDetalhesDistribuicao.ChaveNfe)}");
            sqlComando.Append($", dist.nfe_distribuicao_schema {nameof(NotaFiscalDigitalDetalhesDistribuicao.SchemaDistribuicao)}");
            sqlComando.Append($", dist.nfe_distr_forn_CNPJ_CPF {nameof(NotaFiscalDigitalDetalhesDistribuicao.CnpjCpfFornecedor)}");
            sqlComando.Append($", dist.nfe_distribuicao_dhEmi {nameof(NotaFiscalDigitalDetalhesDistribuicao.DataHoraEmissao)}");
            sqlComando.Append($", dist.nfe_distribuicao_vNF {nameof(NotaFiscalDigitalDetalhesDistribuicao.ValorNotaFiscalSefaz)}");
            sqlComando.Append($", dist.nfe_distribuicao_dhRecbto {nameof(NotaFiscalDigitalDetalhesDistribuicao.DataHoraRecebimento)}");
            sqlComando.Append($", dist.nfe_distribuicao_nProt {nameof(NotaFiscalDigitalDetalhesDistribuicao.NumeroProtocolo)}");
            sqlComando.Append($", dist.nfe_distribuicao_tpEvento {nameof(NotaFiscalDigitalDetalhesDistribuicao.CodigoTipoEvento)}");
            sqlComando.Append($", dist.nfe_distribuicao_XML {nameof(NotaFiscalDigitalDetalhesDistribuicao.XmlEntrada)}");
            sqlComando.Append($", dist.nfe_distribuicao_dhEvento {nameof(NotaFiscalDigitalDetalhesDistribuicao.DataHoraEvento)}");
            sqlComando.Append($" FROM topnfe.nfe_distribuicao dist");
            sqlComando.Append($" WHERE dist.nfe_distribuicao_chNFe=@chaveNfe");
            sqlComando.Append($" AND dist.nfe_distribuicao_schema='procNFe'");

            var detalhesDist =  _context.Database.Connection.Query<NotaFiscalDigitalDetalhesDistribuicao>(sqlComando.ToString(), new
            {
                chaveNfe = notaFiscalDigital.ChaveNfe
            }).FirstOrDefault();
            
            if (detalhesDist != null)
                detalhesDist.IndicadorDestinatarioIe = ObterIndicadorDestIe(notaFiscalDigital.Cliente);


            return detalhesDist;
        }

        public NotaFiscalDigital ObterPorChaveNotaFiscalDigital(Expression<Func<NotaFiscalDigital, bool>> filter, bool tracking = false)
        {
            var notasFiscaisDigital = _context.NotasFiscaisDigital
                .Include(f => f.Itens.Select(item => item.Mercadoria))
                .Where(filter)
                .Tracking(tracking)
                .ToList()
                .Select(notaFiscalDigital =>
                {
                    notaFiscalDigital.Itens = notaFiscalDigital.Itens.OrderBy(item => item.SequenciaItem).ToList();
                    notaFiscalDigital.Complemento = ObterNFComplemento(notaFiscalDigital.Filial, notaFiscalDigital.Cliente, notaFiscalDigital.TipoDocumento, notaFiscalDigital.Serie, notaFiscalDigital.Numero, notaFiscalDigital.Sequencia);
                    foreach (var item in notaFiscalDigital.Itens)
                    {
                        item.Complemento = ObterItemNFComplemento(notaFiscalDigital.Filial, notaFiscalDigital.Cliente, notaFiscalDigital.TipoDocumento, notaFiscalDigital.Serie, notaFiscalDigital.Numero, notaFiscalDigital.Sequencia, item.SequenciaItem);
                    }
                    return notaFiscalDigital;
                })
                .FirstOrDefault();

            if (notasFiscaisDigital == null) return null;

            var volume = _context.NotasFiscaisFisicas
                            .Where(t => t.FilialCodigo == notasFiscaisDigital.Filial
                                     //&& t.IntervenienteCodigo == notasFiscaisDigital.Cliente
                                     && t.TipoDocumentoCodigo == notasFiscaisDigital.TipoDocumento
                                     && t.Numero == notasFiscaisDigital.Numero
                                     && t.Serie == notasFiscaisDigital.Serie
                                     && t.Sequencia == notasFiscaisDigital.Sequencia)
                            .FirstOrDefault().Volume;

            if (volume != 0)
                notasFiscaisDigital.QuantidadeVolume = volume;

            if (notasFiscaisDigital.IndicadorOperacao == EIndicadorOperacao.Saida)
                notasFiscaisDigital.DetalhesFiscais = ObterNFDetalhesFiscais(notasFiscaisDigital);

            if (notasFiscaisDigital.IndicadorOperacao == EIndicadorOperacao.Entrada && (notasFiscaisDigital.IndicadorEmitente == EIndicadorEmitente.Terceiros || notasFiscaisDigital.IndicadorEmitente == EIndicadorEmitente.Entradas))
                notasFiscaisDigital.DetalhesDistribuicao = ObterNFDetalhesDistribuicao(notasFiscaisDigital);

            notasFiscaisDigital.FaturaVendaDeMateriais = ObterFaturaMateriais(notasFiscaisDigital);
            notasFiscaisDigital.ChaveNfDevolucao = ObterChaveNfDevolucao(notasFiscaisDigital);

            notasFiscaisDigital.UsinaExternalId = _context.Usinas.FirstOrDefault(t => t.FilialCodigo == notasFiscaisDigital.Filial)?.ExternalId;

            CarregarDadosCliente(notasFiscaisDigital);
            
            return notasFiscaisDigital;
        }

        public ICollection<NotaFiscalDigital> ListarComPaginacao(Expression<Func<NotaFiscalDigital, bool>> filter, int page, int limit)
        {
            var notasFiscaisDigital = _context.NotasFiscaisDigital
                .Include(f => f.Itens.Select(item => item.Mercadoria))
                .Where(filter)
                .OrderBy(t => t.Numero)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList()
                .Select(notaFiscalDigital =>
                {
                    notaFiscalDigital.Itens = notaFiscalDigital.Itens.OrderBy(item => item.SequenciaItem).ToList();
                    notaFiscalDigital.Complemento = ObterNFComplemento(notaFiscalDigital.Filial, notaFiscalDigital.Cliente, notaFiscalDigital.TipoDocumento, notaFiscalDigital.Serie, notaFiscalDigital.Numero, notaFiscalDigital.Sequencia);
                    foreach (var item in notaFiscalDigital.Itens)
                    {
                        item.Complemento = ObterItemNFComplemento(notaFiscalDigital.Filial, notaFiscalDigital.Cliente, notaFiscalDigital.TipoDocumento, notaFiscalDigital.Serie, notaFiscalDigital.Numero, notaFiscalDigital.Sequencia, item.SequenciaItem);
                    }
                    return notaFiscalDigital;
                })
                .ToList();

            foreach (var nota in notasFiscaisDigital)
            {
                var volume = _context.NotasFiscaisFisicas
                            .Where(t => t.FilialCodigo == nota.Filial
                                     //&& t.IntervenienteCodigo == nota.Cliente
                                     && t.TipoDocumentoCodigo == nota.TipoDocumento
                                     && t.Numero == nota.Numero
                                     && t.Serie == nota.Serie
                                     && t.Sequencia == nota.Sequencia)
                            .FirstOrDefault()?.Volume ?? 0;

                if (volume != 0)
                    nota.QuantidadeVolume = volume;

                nota.Complemento = ObterNFComplemento(
                    nota.Filial,
                    nota.Cliente,
                    nota.TipoDocumento,
                    nota.Serie,
                    nota.Numero,
                    nota.Sequencia
                );

                if (nota.IndicadorOperacao == EIndicadorOperacao.Saida)
                    nota.DetalhesFiscais = ObterNFDetalhesFiscais(nota);

                if (nota.IndicadorOperacao == EIndicadorOperacao.Entrada && (nota.IndicadorEmitente == EIndicadorEmitente.Terceiros || nota.IndicadorEmitente == EIndicadorEmitente.Entradas))
                    nota.DetalhesDistribuicao = ObterNFDetalhesDistribuicao(nota);
                
                nota.FaturaVendaDeMateriais = ObterFaturaMateriais(nota);
                nota.ChaveNfDevolucao = ObterChaveNfDevolucao(nota);
                
                nota.UsinaExternalId = _context.Usinas.FirstOrDefault(t => t.FilialCodigo == nota.Filial)?.ExternalId;
                
                CarregarDadosCliente(nota);
            }

            return notasFiscaisDigital;
        }

        public PagedList<NotaFiscalDigital> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nf.filial {nameof(NotaFiscalDigital.Filial)}");
            sqlComando.Append($", nf.interv {nameof(NotaFiscalDigital.Cliente)}");
            sqlComando.Append($", nf.tp_doc {nameof(NotaFiscalDigital.TipoDocumento)}");
            sqlComando.Append($", nf.serie {nameof(NotaFiscalDigital.Serie)}");
            sqlComando.Append($", nf.num_nf {nameof(NotaFiscalDigital.Numero)}");
            sqlComando.Append($", nf.seq_nf {nameof(NotaFiscalDigital.Sequencia)}");
            sqlComando.Append($", nf.dt_nf {nameof(NotaFiscalDigital.DataNf)}");
            sqlComando.Append($", nf.dt_oper {nameof(NotaFiscalDigital.DataOperacao)}");
            sqlComando.Append($", nf.dt_emi_nf {nameof(NotaFiscalDigital.DataEmissao)}");
            sqlComando.Append($", nf.hr_emissao {nameof(NotaFiscalDigital.HoraEmissao)}");
            sqlComando.Append($", nf.dt_lcto_cancela {nameof(NotaFiscalDigital.DataLancamentoCancelamento)}");
            sqlComando.Append($", nf.cancelada {nameof(NotaFiscalDigital.Cancelada)}");
            sqlComando.Append($", nf.ind_oper {nameof(NotaFiscalDigital.IndicadorOperacao)}");
            sqlComando.Append($", nf.ind_emitente {nameof(NotaFiscalDigital.IndicadorEmitente)}");
            sqlComando.Append($", nf.trans {nameof(NotaFiscalDigital.TransacaoDaOperacao)}");
            sqlComando.Append($", nf.mod_doc_fiscal {nameof(NotaFiscalDigital.ModeloDocumentoFiscalSefaz)}");
            sqlComando.Append($", nf.sit_fiscal {nameof(NotaFiscalDigital.SituacaoFiscal)}");
            sqlComando.Append($", nf.cfop {nameof(NotaFiscalDigital.Cfop)}");
            sqlComando.Append($", nf.seq_cfop {nameof(NotaFiscalDigital.SequenciaCfop)}");
            sqlComando.Append($", nf.vend {nameof(NotaFiscalDigital.Vendedor)}");
            sqlComando.Append($", nf.uf_orig_dest {nameof(NotaFiscalDigital.UfDestino)}");
            sqlComando.Append($", nf.vl_merc {nameof(NotaFiscalDigital.ValorMercadoria)}");
            sqlComando.Append($", nf.vl_desc {nameof(NotaFiscalDigital.ValorDesconto)}");
            sqlComando.Append($", nf.vl_frete {nameof(NotaFiscalDigital.ValorFrete)}");
            sqlComando.Append($", nf.vl_seg {nameof(NotaFiscalDigital.ValorSeguro)}");
            sqlComando.Append($", nf.vl_o_desp {nameof(NotaFiscalDigital.ValorOutrasDespesas)}");
            sqlComando.Append($", nf.vl_cont_nf {nameof(NotaFiscalDigital.ValorContabil)}");
            sqlComando.Append($", nf.vl_base_icms {nameof(NotaFiscalDigital.BaseCalculoIcms)}");
            sqlComando.Append($", nf.aliq_icms {nameof(NotaFiscalDigital.AliquotaIcms)}");
            sqlComando.Append($", nf.vl_icms {nameof(NotaFiscalDigital.ValorIcms)}");
            sqlComando.Append($", nf.vl_base_icms_su {nameof(NotaFiscalDigital.BaseCalculoIcmsSubstituicao)}");
            sqlComando.Append($", nf.vl_icms_sub {nameof(NotaFiscalDigital.ValorIcmsSubstituicao)}");
            sqlComando.Append($", nf.vl_ipi {nameof(NotaFiscalDigital.ValorIpi)}");
            sqlComando.Append($", nf.vl_fisc_1_icms {nameof(NotaFiscalDigital.ValorFicalIcms1)}");
            sqlComando.Append($", nf.vl_fisc_2_icms {nameof(NotaFiscalDigital.ValorFicalIcms2)}");
            sqlComando.Append($", nf.vl_fisc_3_icms {nameof(NotaFiscalDigital.ValorFicalIcms3)}");
            sqlComando.Append($", nf.vl_fisc_1_ipi {nameof(NotaFiscalDigital.ValorFicalIpi1)}");
            sqlComando.Append($", nf.vl_fisc_2_ipi {nameof(NotaFiscalDigital.ValorFicalIpi2)}");
            sqlComando.Append($", nf.vl_fisc_3_ipi {nameof(NotaFiscalDigital.ValorFicalIpi3)}");
            sqlComando.Append($", nf.transp {nameof(NotaFiscalDigital.Transportador)}");
            sqlComando.Append($", IF(cnf.qtde_m3_bt = 0, nf.qt_vol, cnf.qtde_m3_bt) {nameof(NotaFiscalDigital.QuantidadeVolume)}");
            sqlComando.Append($", nf.esp_vol {nameof(NotaFiscalDigital.EspecieVolume)}");
            sqlComando.Append($", nf.peso_bruto {nameof(NotaFiscalDigital.PesoBruto)}");
            sqlComando.Append($", nf.peso_liq {nameof(NotaFiscalDigital.PesoLiquido)}");
            sqlComando.Append($", nf.ind_frete {nameof(NotaFiscalDigital.IndicadorFrete)}");
            sqlComando.Append($", nf.ident_veic {nameof(NotaFiscalDigital.IdentificacaoVeiculoPlaca)}");
            sqlComando.Append($", nf.obs_fisc_nf {nameof(NotaFiscalDigital.ObservacaoFiscal)}");
            sqlComando.Append($", nf.filial_destino {nameof(NotaFiscalDigital.FilialDestino)}");
            sqlComando.Append($", nf.uf_veic {nameof(NotaFiscalDigital.UfVeiculo)}");
            sqlComando.Append($", nf.vlr_pis {nameof(NotaFiscalDigital.ValorPis)}");
            sqlComando.Append($", nf.vlr_cofins {nameof(NotaFiscalDigital.ValorCofins)}");
            sqlComando.Append($", nf.mens_fiscal_nfe {nameof(NotaFiscalDigital.MensagemFiscalNfe)}");
            sqlComando.Append($", nf.forn_iss {nameof(NotaFiscalDigital.FornecedorIss)}");
            sqlComando.Append($", nf.total_base_pis {nameof(NotaFiscalDigital.TotalBasePis)}");
            sqlComando.Append($", nf.total_pis {nameof(NotaFiscalDigital.TotalPis)}");
            sqlComando.Append($", nf.total_base_cofins {nameof(NotaFiscalDigital.TotalBaseCofins)}");
            sqlComando.Append($", nf.total_cofins {nameof(NotaFiscalDigital.TotalCofins)}");
            sqlComando.Append($", nf.chave_nfe {nameof(NotaFiscalDigital.ChaveNfe)}");
            sqlComando.Append($", nf.oper {nameof(NotaFiscalDigital.Operacao)}");
            sqlComando.Append($", nf.vl_serv {nameof(NotaFiscalDigital.ValorServico)}");
            sqlComando.Append($", nf.cc {nameof(NotaFiscalDigital.CentroCusto)}");
            sqlComando.Append($", nf.requisitante {nameof(NotaFiscalDigital.Requisitante)}");
            sqlComando.Append($", nf.class_cons_ee {nameof(NotaFiscalDigital.ClassificacaoConsumoEnergia)}");
            sqlComando.Append($", nf.tipo_ligacao {nameof(NotaFiscalDigital.TipoLigacao)}");
            sqlComando.Append($", nf.grupo_tensao {nameof(NotaFiscalDigital.GrupoTensaoEnregia)}");
            sqlComando.Append($", nf.tipo_assinante {nameof(NotaFiscalDigital.TipoAssinante)}");
            sqlComando.Append($", nf.tipo_frete {nameof(NotaFiscalDigital.TipoFrete)}");
            sqlComando.Append($", nf.natureza_frete {nameof(NotaFiscalDigital.NaturezaFrete)}");
            sqlComando.Append($", nf.filial_estoque {nameof(NotaFiscalDigital.FilialEstoque)}");
            sqlComando.Append($", nf.num_req {nameof(NotaFiscalDigital.NumeroRequisicao)}");
            sqlComando.Append($", nf.ano_req {nameof(NotaFiscalDigital.AnoRequisicao)}");
            sqlComando.Append($", nf.emp_req {nameof(NotaFiscalDigital.EmpresaRequisicao)}");
            sqlComando.Append($", nf.atualizado_em {nameof(NotaFiscalDigital.DataAtualizacao)}");
            sqlComando.Append($" FROM fis_nf nf");

            sqlComando.Append($" INNER JOIN con_nf cnf");
            sqlComando.Append($" ON cnf.filial=nf.filial");
            //sqlComando.Append($" AND cnf.interv=nf.interv");
            sqlComando.Append($" AND cnf.tp_doc=nf.tp_doc");
            sqlComando.Append($" AND cnf.num_nf=nf.num_nf");
            sqlComando.Append($" AND cnf.serie=nf.serie");
            sqlComando.Append($" AND cnf.seq_nf=nf.seq_nf");

            sqlComando.Append($" WHERE nf.atualizado_em>='{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");

            if (dataFim != null)
                sqlComando.Append($" AND nf.atualizado_em<='{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" ORDER BY nf.atualizado_em");

            var notasFiscaisDigital = _context.Connection.QueryPagedList<NotaFiscalDigital>(sqlComando.ToString(), page, limit);

            var notasFiscaisDigitalLista = new List<NotaFiscalDigital>();

            var notasFiscaisDigitalResultPagedList = new PagedList<NotaFiscalDigital>
            {
                CurrentPage = notasFiscaisDigital.CurrentPage,
                PageCount = notasFiscaisDigital.PageCount,
                PageSize = notasFiscaisDigital.PageSize,
                RecordCount = notasFiscaisDigital.RecordCount
            };

            foreach (var record in notasFiscaisDigital.Records)
            {
                var nota = (NotaFiscalDigital)record;

                sqlComando.Clear();
                sqlComando.Append($"SELECT item.filial {nameof(NotaFiscalDigitalItem.Filial)}");
                sqlComando.Append($", item.interv {nameof(NotaFiscalDigitalItem.Cliente)}");
                sqlComando.Append($", item.tp_doc {nameof(NotaFiscalDigitalItem.TipoDocumento)}");
                sqlComando.Append($", item.ser {nameof(NotaFiscalDigitalItem.Serie)}");
                sqlComando.Append($", item.num_nf {nameof(NotaFiscalDigitalItem.Numero)}");
                sqlComando.Append($", item.seq_nf {nameof(NotaFiscalDigitalItem.Sequencia)}");
                sqlComando.Append($", item.num_seq_item_nf {nameof(NotaFiscalDigitalItem.SequenciaItem)}");
                sqlComando.Append($", item.dt_op {nameof(NotaFiscalDigitalItem.DataOperacao)}");
                sqlComando.Append($", item.trans {nameof(NotaFiscalDigitalItem.TransacaoDaOperacao)}");
                sqlComando.Append($", item.cfop {nameof(NotaFiscalDigitalItem.Cfop)}");
                sqlComando.Append($", item.seq_cfop {nameof(NotaFiscalDigitalItem.SequenciaCfop)}");
                sqlComando.Append($", item.tp_estq {nameof(NotaFiscalDigitalItem.TipoEstoque)}");
                sqlComando.Append($", item.merc {nameof(NotaFiscalDigitalItem.CódigoMercadoria)}");
                sqlComando.Append($", item.qt {nameof(NotaFiscalDigitalItem.Quantidade)}");
                sqlComando.Append($", item.preco_un {nameof(NotaFiscalDigitalItem.PrecoUnitario)}");
                sqlComando.Append($", item.vl_tot {nameof(NotaFiscalDigitalItem.ValorTotal)}");
                sqlComando.Append($", item.vl_desc {nameof(NotaFiscalDigitalItem.ValorDesconto)}");
                sqlComando.Append($", item.vl_frete {nameof(NotaFiscalDigitalItem.ValorFrete)}");
                sqlComando.Append($", item.vl_seg {nameof(NotaFiscalDigitalItem.ValorSeguro)}");
                sqlComando.Append($", item.vl_o_desp {nameof(NotaFiscalDigitalItem.ValorOutrasDespesas)}");
                sqlComando.Append($", item.sit_trib {nameof(NotaFiscalDigitalItem.SituacaoTributaria)}");
                sqlComando.Append($", item.base_calc_icms {nameof(NotaFiscalDigitalItem.BaseCalculoIcms)}");
                sqlComando.Append($", item.aliq_icms {nameof(NotaFiscalDigitalItem.AliquotaIcms)}");
                sqlComando.Append($", item.vl_icms {nameof(NotaFiscalDigitalItem.ValorIcms)}");
                sqlComando.Append($", item.base_icms_sub_t {nameof(NotaFiscalDigitalItem.BaseCalculoIcmsSubstituicao)}");
                sqlComando.Append($", item.aliq_icms_sub {nameof(NotaFiscalDigitalItem.AliquotaIcmsSubstituicao)}");
                sqlComando.Append($", item.vl_icms_sub_tri {nameof(NotaFiscalDigitalItem.ValorIcmsSubstituicao)}");
                sqlComando.Append($", item.base_calc_ipi {nameof(NotaFiscalDigitalItem.BaseCalculoIpi)}");
                sqlComando.Append($", item.aliq_ipi {nameof(NotaFiscalDigitalItem.AliquotaIpi)}");
                sqlComando.Append($", item.vl_ipi {nameof(NotaFiscalDigitalItem.ValorIpi)}");
                sqlComando.Append($", item.vl_pis_n_cum {nameof(NotaFiscalDigitalItem.ValorPisNaoCumulativo)}");
                sqlComando.Append($", item.vl_cofins_n_cum {nameof(NotaFiscalDigitalItem.ValorCofinsNaoCumulativo)}");
                sqlComando.Append($", item.custo_tot_item {nameof(NotaFiscalDigitalItem.CustoTotalItem)}");
                sqlComando.Append($", item.peso {nameof(NotaFiscalDigitalItem.Peso)}");
                sqlComando.Append($", item.traco_concreto {nameof(NotaFiscalDigitalItem.TracoConcreto)}");
                sqlComando.Append($", item.volume {nameof(NotaFiscalDigitalItem.Volume)}");
                sqlComando.Append($", item.qtde_estoque {nameof(NotaFiscalDigitalItem.QuantidadeEstoque)}");
                sqlComando.Append($", item.cst_pis {nameof(NotaFiscalDigitalItem.CodigoSituacaoTributariaPis)}");
                sqlComando.Append($", item.cst_cofins {nameof(NotaFiscalDigitalItem.CodigoSituacaoTributariaCofins)}");
                sqlComando.Append($", item.vl_fisc_1_icms {nameof(NotaFiscalDigitalItem.ValorFicalIcms1)}");
                sqlComando.Append($", item.vl_fisc_2_icms {nameof(NotaFiscalDigitalItem.ValorFicalIcms2)}");
                sqlComando.Append($", item.vl_fisc_3_icms {nameof(NotaFiscalDigitalItem.ValorFicalIcms3)}");
                sqlComando.Append($", item.vl_fisc_1_ipi {nameof(NotaFiscalDigitalItem.ValorFicalIpi1)}");
                sqlComando.Append($", item.vl_fisc_2_ipi {nameof(NotaFiscalDigitalItem.ValorFicalIpi2)}");
                sqlComando.Append($", item.vl_fisc_3_ipi {nameof(NotaFiscalDigitalItem.ValorFicalIpi3)}");
                sqlComando.Append($", item.pct_pis_n_cum {nameof(NotaFiscalDigitalItem.PercentualPisNaoCumulativo)}");
                sqlComando.Append($", item.pct_cofins_ncum {nameof(NotaFiscalDigitalItem.PercentualCofinsNaoCumulativo)}");
                sqlComando.Append($", item.ref_forn {nameof(NotaFiscalDigitalItem.NotaReferenciaFornecedor)}");
                sqlComando.Append($", item.ref_tp_doc {nameof(NotaFiscalDigitalItem.NotaReferenciaTipoDocumento)}");
                sqlComando.Append($", item.ref_serie {nameof(NotaFiscalDigitalItem.NotaReferenciaSerie)}");
                sqlComando.Append($", item.ref_num_nf {nameof(NotaFiscalDigitalItem.NotaReferenciaNumero)}");
                sqlComando.Append($", item.ref_item {nameof(NotaFiscalDigitalItem.NotaReferenciaItem)}");
                sqlComando.Append($", item.vlr_bc_pis {nameof(NotaFiscalDigitalItem.BaseCalculoPis)}");
                sqlComando.Append($", item.pct_pis {nameof(NotaFiscalDigitalItem.PercentualPis)}");
                sqlComando.Append($", item.vlr_pis {nameof(NotaFiscalDigitalItem.ValorPis)}");
                sqlComando.Append($", item.vlr_bc_cofins {nameof(NotaFiscalDigitalItem.BaseCalculoCofins)}");
                sqlComando.Append($", item.pct_cofins {nameof(NotaFiscalDigitalItem.PercentualCofins)}");
                sqlComando.Append($", item.vlr_cofins {nameof(NotaFiscalDigitalItem.ValorCofins)}");
                sqlComando.Append($", item.interv_estq {nameof(NotaFiscalDigitalItem.IntervenienteEstoque)}");
                sqlComando.Append($", item.PisCst {nameof(NotaFiscalDigitalItem.ItemPisCodigoSituacaoTributaria)}");
                sqlComando.Append($", item.PisBC {nameof(NotaFiscalDigitalItem.ItemPisBaseCalculo)}");
                sqlComando.Append($", item.PisPct {nameof(NotaFiscalDigitalItem.ItemPisPercentual)}");
                sqlComando.Append($", item.PisValor {nameof(NotaFiscalDigitalItem.ItemPisValor)}");
                sqlComando.Append($", item.CofinsCst {nameof(NotaFiscalDigitalItem.ItemCofinsCodigoSituacaoTributaria)}");
                sqlComando.Append($", item.CofinsBC {nameof(NotaFiscalDigitalItem.ItemCofinsBaseCalculo)}");
                sqlComando.Append($", item.CofinsPct {nameof(NotaFiscalDigitalItem.ItemCofinsPercentual)}");
                sqlComando.Append($", item.CofinsValor {nameof(NotaFiscalDigitalItem.ItemCofinsValor)}");
                sqlComando.Append($", item.tribContrib {nameof(NotaFiscalDigitalItem.TributacaoContribuicaoPisCofins)}");
                sqlComando.Append($", item.IniVigTribCont {nameof(NotaFiscalDigitalItem.InicioVigenciaTribContribuicao)}");
                sqlComando.Append($", item.oper {nameof(NotaFiscalDigitalItem.OperacaoFinanceira)}");
                sqlComando.Append($", item.ipi_cst {nameof(NotaFiscalDigitalItem.IpiCodigoSituacaoTributaria)}");
                sqlComando.Append($", item.unidade {nameof(NotaFiscalDigitalItem.Unidade)}");
                sqlComando.Append($", item.vCredICMSSN {nameof(NotaFiscalDigitalItem.ValorCreditoIcmsSimplesNacional)}");
                sqlComando.Append($", item.pCredSN {nameof(NotaFiscalDigitalItem.PercentualCreditoIcmsSimplesNacional)}");
                sqlComando.Append($", item.CSOSN {nameof(NotaFiscalDigitalItem.CsoSimplesNacional)}");
                sqlComando.Append($", item.base_calc_icmssn {nameof(NotaFiscalDigitalItem.BaseCalculoIcmsSimplesNacional)}");
                sqlComando.Append($" FROM fis_item_nf item");
                sqlComando.Append($" WHERE item.filial=@filial");
                sqlComando.Append($" AND item.interv=@interveniente");
                sqlComando.Append($" AND item.tp_doc=@tipoDocumento");
                sqlComando.Append($" AND item.ser=@serie");
                sqlComando.Append($" AND item.num_nf=@numeroNf");
                sqlComando.Append($" AND item.seq_nf=@sequenciaNf");

                nota.Itens = _context.Database.Connection.Query<NotaFiscalDigitalItem>(sqlComando.ToString(), new
                {
                    filial = nota.Filial,
                    interveniente = nota.Cliente,
                    tipoDocumento = nota.TipoDocumento,
                    serie = nota.Serie,
                    numeroNf = nota.Numero,
                    sequenciaNf = nota.Sequencia
                }).ToList();

                foreach (var notaItem in nota.Itens)
                {
                    notaItem.Mercadoria = _context.Mercadoria.FirstOrDefault(t => t.Codigo == notaItem.CódigoMercadoria);
                    notaItem.Complemento = ObterItemNFComplemento(
                        nota.Filial,
                        nota.Cliente,
                        nota.TipoDocumento,
                        nota.Serie,
                        nota.Numero,
                        nota.Sequencia,
                        notaItem.SequenciaItem
                    );
                }

                nota.Complemento = ObterNFComplemento(
                    nota.Filial,
                    nota.Cliente,
                    nota.TipoDocumento,
                    nota.Serie,
                    nota.Numero,
                    nota.Sequencia
                );

                if (nota.IndicadorOperacao == EIndicadorOperacao.Saida)
                    nota.DetalhesFiscais = ObterNFDetalhesFiscais(nota);

                if (nota.IndicadorOperacao == EIndicadorOperacao.Entrada && (nota.IndicadorEmitente == EIndicadorEmitente.Terceiros || nota.IndicadorEmitente == EIndicadorEmitente.Entradas))
                    nota.DetalhesDistribuicao = ObterNFDetalhesDistribuicao(nota);
                
                nota.FaturaVendaDeMateriais = ObterFaturaMateriais(nota);
                nota.ChaveNfDevolucao = ObterChaveNfDevolucao(nota);
                
                nota.UsinaExternalId = _context.Usinas.FirstOrDefault(t => t.FilialCodigo == nota.Filial)?.ExternalId;

                CarregarDadosCliente(nota);

                notasFiscaisDigitalLista.Add(nota);
            }

            notasFiscaisDigitalResultPagedList.Records = notasFiscaisDigitalLista;

            return notasFiscaisDigitalResultPagedList;
        }
        
    }
}
