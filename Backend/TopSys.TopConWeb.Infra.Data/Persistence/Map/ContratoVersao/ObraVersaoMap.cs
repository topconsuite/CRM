using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraVersaoMap : EntityTypeConfiguration<ObraVersao>
    {
        public ObraVersaoMap()
        {
            ToTable("topsys.con_obras_versao");

            Ignore(t => t.Origem);
            Ignore(t => t.ObraTaxas);
            Ignore(t => t.ObraFrentes);
            Ignore(t => t.DemaisAprovacoes);
            Ignore(t => t.PendenteAprovacaoDistanciaUsinaCEP);
            Ignore(t => t.StatusProjecao);
            Ignore(t => t.TributacaoCBS);
            Ignore(t => t.TributacaoIBS);
            Ignore(t => t.TributacaoIS);


            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.Numero });

            HasOptional(t => t.Contrato)
                .WithMany()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.AnoContrato, t.NumContrato })
                .WillCascadeOnDelete(false);

            HasOptional(t => t.Proposta)
                .WithMany()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.AnoChamada, t.NumChamada })
                .WillCascadeOnDelete(false);

            HasRequired(t => t.UsinaEntrega)
                .WithMany()
                .HasForeignKey(t => t.UsinaEntregaCodigo)
                .WillCascadeOnDelete(false);

            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo)
                .WillCascadeOnDelete(false);

            HasOptional(t => t.CondicaoPagamento)
              .WithMany()
              .HasForeignKey(t => t.CondicaoPagamentoCodigo)
              .WillCascadeOnDelete(false);

            HasOptional(t => t.TipoCobranca)
             .WithMany()
             .HasForeignKey(t => t.TipoCobrancaCodigo)
             .WillCascadeOnDelete(false);

            HasOptional(t => t.ViaCaptacao)
             .WithMany()
             .HasForeignKey(t => t.ViaCaptacaoCodigo)
             .WillCascadeOnDelete(false);

            HasOptional(t => t.TipoObra)
             .WithMany()
             .HasForeignKey(t => t.TipoObraCodigo)
             .WillCascadeOnDelete(false);

            HasOptional(t => t.PorteObra)
             .WithMany()
             .HasForeignKey(t => t.PorteObraCodigo)
             .WillCascadeOnDelete(false);

            Ignore(x => x.ObraProjecao);

            HasMany(t => t.ObraLogs)
              .WithRequired(t => t.Obra)
              .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasMany(t => t.ObraTracos)
                .WithRequired(t => t.Obra)
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasMany(t => t.ObraBombas)
                .WithRequired(t => t.Obra)
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasMany(t => t.ContratoPagamentos)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasMany(t => t.PropostaPagamentos)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasMany(t => t.ObraMensagensPadrao)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraNumero });

            HasMany(t => t.ObraTributacoesMunicipais)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.ObraUsinaCodigo, t.ObraNumero });

            HasMany(t => t.ObraDemaisServicos)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraNumero });

            HasOptional(t => t.Indicador)
                .WithRequired()
                .WillCascadeOnDelete(false);

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnName("numero")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.AnoChamada)
                .HasColumnOrder(3)
                .HasColumnName("ano_chamada");

            Property(t => t.NumChamada)
               .HasColumnOrder(4)
               .HasColumnName("no_chamada")
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.UsinaEntregaCodigo)
                .HasColumnName("obra_usina");

            Property(t => t.EnderecoMunicipioCodigo)
               .HasColumnName("obra_mun");

            Property(t => t.CondicaoPagamentoCodigo)
                .HasColumnName("cond_pgto");

            Property(t => t.TipoCobrancaCodigo)
                 .HasColumnName("tipo_cobranca");

            Property(t => t.AnoContrato)
                .HasColumnName("ano_contrato");

            Property(t => t.NumContrato)
               .HasColumnName("no_contrato")
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.EnderecoCep)
                .HasColumnName("obra_cep");

            Property(t => t.Nome)
                .HasColumnName("obra_nome")
                .HasMaxLength(40);

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("obra_end");

            Property(t => t.EnderecoNumero)
                .HasColumnName("obra_num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("obra_compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("obra_bairro");

            Property(t => t.DistanciaUsina)
                .HasColumnName("obra_km_usina");

            Property(t => t.DistanciaUsinaGoogleApi)
                .HasColumnName("obra_km_usina_via_google");

            Property(t => t.RadioNextel)
                .HasColumnName("obra_radio");

            Property(t => t.ContatoPrincipalNome)
                .HasColumnName("obra_contato");

            Property(t => t.ContatoPrincipalFuncaoCodigo)
                .HasColumnName("obra_cont_func");
            HasOptional(t => t.ContatoPrincipalFuncao)
                .WithMany()
                .HasForeignKey(t => t.ContatoPrincipalFuncaoCodigo)
                .WillCascadeOnDelete(false);
            
            HasOptional(t => t.TributacaoPisCofins)
                .WithMany()
                .HasForeignKey(t => t.TributacaoPisCofinsCodigo);

            Property(t => t.ContatoPrincipalTelefoneDdd)
                .HasColumnName("obra_cont_ddd");

            Property(t => t.ContatoPrincipalTelefoneNumero)
                .HasColumnName("obra_cont_fone");

            Property(t => t.ContatoPrincipalCelularDdd)
                .HasColumnName("obra_cel_ddd");

            Property(t => t.ContatoPrincipalCelularNumero)
                .HasColumnName("obra_cont_cel");

            Property(t => t.ContatoSecundarioNome)
                .HasColumnName("obra_contato1");

            Property(t => t.ContatoSecundarioFuncaoCodigo)
                .HasColumnName("obra_cont1_func");
            HasOptional(t => t.ContatoSecundarioFuncao)
                .WithMany()
                .HasForeignKey(t => t.ContatoSecundarioFuncaoCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.ContatoSecundarioTelefoneDdd)
                .HasColumnName("obra_cont_ddd1");

            Property(t => t.ContatoSecundarioTelefoneNumero)
                .HasColumnName("obra_cont1_fone");

            Property(t => t.ContatoSecundarioCelularDdd)
                .HasColumnName("obra_cel_ddd1");

            Property(t => t.ContatoSecundarioCelularNumero)
                .HasColumnName("obra_cont1_cel");

            Property(t => t.ViaCaptacaoCodigo)
                .HasColumnName("obra_captado_v");

            Property(t => t.TipoObraCodigo)
                .HasColumnName("obra_tipo");

            Property(t => t.PorteObraCodigo)
                .HasColumnName("obra_porte");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.PrevisaoInicio)
                .HasColumnName("obra_prev_inic");

            Property(t => t.PrevisaoTermino)
                .HasColumnName("obra_prev_term");

            Property(t => t.VolumeEstimado)
                .HasColumnName("volume_concreto");

            Property(t => t.VolumePorCarga)
                .HasColumnName("volume_pcarga");

            Property(t => t.ObraNova)
                .HasColumnName("obra_nova");

            Property(t => t.Itinerante)
                .HasColumnName("intinerante");

            Property(t => t.ObservacaoNf)
                .HasColumnName("obs");

            Property(t => t.ReferenciaAcesso)
                .HasColumnName("obra_refer_aces");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.StatusComercial)
                .HasColumnName("status_comercial");

            Property(t => t.StatusCadastro)
                .HasColumnName("status_cadastro");

            Property(t => t.StatusEngenharia)
                .HasColumnName("status_engenharia");

            Property(t => t.StatusFinanceiro)
                .HasColumnName("status_financeiro");

            Property(t => t.VibradorQuantidade)
                .HasColumnName("vibr_qtde");

            Property(t => t.VibradorValorUnitario)
                .HasColumnName("vibr_vlr_unit");

            Property(t => t.ValorDemaisServicos)
                .HasColumnName("vlr_demais_servicos");

            Property(t => t.IniciadaPorConcorrenteSimNao)
                .HasColumnName("obra_inic_conc");

            Property(t => t.ConcorrenteCodigo)
                .HasColumnName("concorrente");
            HasOptional(t => t.Concorrente)
                .WithMany()
                .HasForeignKey(t => t.ConcorrenteCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.PercentualRepasseReajusteAreia)
                .HasColumnName("obra_pct_rp_are");

            Property(t => t.PercentualRepasseReajusteCimento)
                .HasColumnName("obra_pct_rp_cim");

            Property(t => t.PercentualRepasseReajusteDiesel)
                .HasColumnName("obra_pct_rp_die");

            Property(t => t.PercentualRepasseReajusteMaoDeObra)
                .HasColumnName("obra_pct_rp_obr");

            Property(t => t.PercentualRepasseReajustePedra)
                .HasColumnName("obra_pct_rp_ped");

            Property(t => t.Cei)
                .HasColumnName("obra_cei");

            Property(t => t.ObservacaoInterna)
                .HasColumnName("obra_obs");

            Property(t => t.TempoBtNaObra)
                .HasColumnName("temp_bt_na_obra");

            Property(t => t.TempoAteAObra)
                .HasColumnName("temp_ate_a_obra");

            Property(t => t.TempoDescarga)
                .HasColumnName("tempo_descarga");

            Property(t => t.TempoCicloPrevisto)
                .HasColumnName("tempo_ciclo_prev");

            Property(t => t.CustoProjetadoTransporte)
                .HasColumnName("custo_proje_trans");

            Property(t => t.CodigoBeneficioFiscal)
                .HasColumnName("obra_benef_fiscal");

            Property(t => t.EmailResponsavelTecnico)
                .HasColumnName("email_resp_tecnico");

            Property(t => t.UsaAdicionalZMRC)
                .HasColumnName("usa_adicional_zmrc");

            Property(t => t.NecessitaAprovacaoZMRC)
                .HasColumnName("necessita_aprov_zmrc");

            Property(t => t.VolumeStatusComercial)
                .HasColumnName("volume_status_comercial");
            
            Property(t => t.CondicaoPagamentoStatusComercial)
                .HasColumnName("condicao_pagamento_status_comercial");

            Property(t => t.TributacaoPisCofinsCodigo)
                .HasColumnName("tribcontrib_nfs");

            Property(t => t.TributacaoISCodigo)
                .HasColumnName("id_imp_is");

            Property(t => t.TributacaoIBSCodigo)
                .HasColumnName("id_imp_ibs");

            Property(t => t.TributacaoCBSCodigo)
                .HasColumnName("id_imp_cbs");

            Property(t => t.CodigoCib)
                .HasColumnName("codigo_cib");

            Property(t => t.ConstrucaoCivilTipoAlvara)
                .HasColumnName("construcao_civil_tipo_alvara");
        }
    }
}