using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalMap : EntityTypeConfiguration<NotaFiscalDigital>
    {
        public NotaFiscalDigitalMap()
        {
            ToTable("topsys.fis_nf");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero, t.Sequencia });

            Ignore(t => t.Complemento);
            Ignore(t => t.DetalhesFiscais);
            Ignore(t => t.DetalhesDistribuicao);
            Ignore(t => t.FaturaVendaDeMateriais);
            Ignore(t => t.ChaveTituloContasAReceber);
            Ignore(t => t.ChaveNotaServicoPai);
            Ignore(t => t.ChaveNfDevolucao);
            Ignore(t => t.UsinaExternalId);
            Ignore(t => t.ClienteCfpCnpj);
            Ignore(t => t.ClienteInscEstadual);

            Property(t => t.Filial)
                .HasColumnOrder(0)
                .HasColumnName("filial");

            Property(t => t.Cliente)
                .HasColumnOrder(1)
                .HasColumnName("interv");

            Property(t => t.TipoDocumento)
                .HasColumnOrder(2)
                .HasColumnName("tp_doc");

            Property(t => t.Serie)
              .HasColumnOrder(3)
              .HasColumnName("serie");

            Property(t => t.Numero)
                .HasColumnOrder(4)
                .HasColumnName("num_nf");

            Property(t => t.Sequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq_nf");

            Property(t => t.DataNf)
                .HasColumnName("dt_nf");

            Property(t => t.DataOperacao)
                .HasColumnName("dt_oper");

            Property(t => t.DataEmissao)
                .HasColumnName("dt_emi_nf");

            Property(t => t.HoraEmissao)
                .HasColumnName("hr_emissao");

            Property(t => t.DataLancamentoCancelamento)
                .HasColumnName("dt_lcto_cancela");

            Property(t => t.Cancelada)
                .HasColumnName("cancelada");

            Property(t => t.IndicadorOperacao)
                .HasColumnName("ind_oper");

            Property(t => t.IndicadorEmitente)
                .HasColumnName("ind_emitente");

            Property(t => t.TransacaoDaOperacao)
                .HasColumnName("trans");

            Property(t => t.ModeloDocumentoFiscalSefaz)
                .HasColumnName("mod_doc_fiscal");

            Property(t => t.SituacaoFiscal)
                .HasColumnName("sit_fiscal");

            Property(t => t.Cfop)
                .HasColumnName("cfop");

            Property(t => t.SequenciaCfop)
                .HasColumnName("seq_cfop");

            Property(t => t.Vendedor)
                .HasColumnName("vend");

            Property(t => t.UfDestino)
                .HasColumnName("uf_orig_dest");

            Property(t => t.ValorMercadoria)
                .HasColumnName("vl_merc");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desc");

            Property(t => t.ValorFrete)
                .HasColumnName("vl_frete");

            Property(t => t.ValorSeguro)
                .HasColumnName("vl_seg");

            Property(t => t.ValorOutrasDespesas)
                .HasColumnName("vl_o_desp");

            Property(t => t.ValorContabil)
                .HasColumnName("vl_cont_nf");

            Property(t => t.BaseCalculoIcms)
                .HasColumnName("vl_base_icms");

            Property(t => t.AliquotaIcms)
                .HasColumnName("aliq_icms");

            Property(t => t.ValorIcms)
                .HasColumnName("vl_icms");

            Property(t => t.BaseCalculoIcmsSubstituicao)
                .HasColumnName("vl_base_icms_su");

            Property(t => t.ValorIcmsSubstituicao)
                .HasColumnName("vl_icms_sub");

            Property(t => t.ValorIpi)
                .HasColumnName("vl_ipi");

            Property(t => t.ValorFicalIcms1)
                .HasColumnName("vl_fisc_1_icms");

            Property(t => t.ValorFicalIcms2)
                .HasColumnName("vl_fisc_2_icms");

            Property(t => t.ValorFicalIcms3)
                .HasColumnName("vl_fisc_3_icms");

            Property(t => t.ValorFicalIpi1)
                .HasColumnName("vl_fisc_1_ipi");

            Property(t => t.ValorFicalIpi2)
                .HasColumnName("vl_fisc_2_ipi");

            Property(t => t.ValorFicalIpi3)
                .HasColumnName("vl_fisc_3_ipi");

            Property(t => t.Transportador)
                .HasColumnName("transp");

            Property(t => t.QuantidadeVolume)
                .HasColumnName("qt_vol");

            Property(t => t.EspecieVolume)
                .HasColumnName("esp_vol");

            Property(t => t.PesoBruto)
                .HasColumnName("peso_bruto");

            Property(t => t.PesoLiquido)
                .HasColumnName("peso_liq");

            Property(t => t.IndicadorFrete)
                .HasColumnName("ind_frete");

            Property(t => t.IdentificacaoVeiculoPlaca)
                .HasColumnName("ident_veic");

            Property(t => t.ObservacaoFiscal)
                .HasColumnName("obs_fisc_nf");

            Property(t => t.FilialDestino)
                .HasColumnName("filial_destino");

            Property(t => t.UfVeiculo)
                .HasColumnName("uf_veic");

            Property(t => t.ValorPis)
                .HasColumnName("vlr_pis");

            Property(t => t.ValorCofins)
                .HasColumnName("vlr_cofins");

            Property(t => t.MensagemFiscalNfe)
                .HasColumnName("mens_fiscal_nfe");

            Property(t => t.FornecedorIss)
                .HasColumnName("forn_iss");

            Property(t => t.TotalBasePis)
                .HasColumnName("total_base_pis");

            Property(t => t.TotalPis)
                .HasColumnName("total_pis");

            Property(t => t.TotalBaseCofins)
                .HasColumnName("total_base_cofins");

            Property(t => t.TotalCofins)
                .HasColumnName("total_cofins");

            Property(t => t.ChaveNfe)
                .HasColumnName("chave_nfe");

            Property(t => t.Operacao)
                .HasColumnName("oper");

            Property(t => t.ValorServico)
                .HasColumnName("vl_serv");

            Property(t => t.CentroCusto)
                .HasColumnName("cc");

            Property(t => t.Requisitante)
                .HasColumnName("requisitante");

            Property(t => t.ClassificacaoConsumoEnergia)
                .HasColumnName("class_cons_ee");

            Property(t => t.TipoLigacao)
                .HasColumnName("tipo_ligacao");

            Property(t => t.GrupoTensaoEnregia)
                .HasColumnName("grupo_tensao");

            Property(t => t.TipoAssinante)
                .HasColumnName("tipo_assinante");

            Property(t => t.TipoFrete)
                .HasColumnName("tipo_frete");

            Property(t => t.NaturezaFrete)
                .HasColumnName("natureza_frete");

            Property(t => t.FilialEstoque)
                .HasColumnName("filial_estoque");

            Property(t => t.NumeroRequisicao)
                .HasColumnName("num_req");

            Property(t => t.AnoRequisicao)
                .HasColumnName("ano_req");

            Property(t => t.EmpresaRequisicao)
                .HasColumnName("emp_req");

            Property(t => t.DataAtualizacao)
                .HasColumnName("atualizado_em")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            HasMany(t => t.Itens)
                .WithOptional()
                .HasForeignKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero, t.Sequencia });
        }
    }
}
