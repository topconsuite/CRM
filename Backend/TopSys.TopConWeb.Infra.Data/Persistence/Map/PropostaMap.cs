using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaMap:EntityTypeConfiguration<Proposta>
    {
        public PropostaMap()
        {
            ToTable("topsys.con_chtel");

            Ignore(t => t.StatusProposta);

            HasKey(t => new { t.UsinaCodigo, t.Ano, t.Numero });

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasOptional(t => t.Interveniente)
                .WithMany()
                .HasForeignKey(t => t.IntervenienteCodigo);

            HasRequired(t => t.Obra)
                .WithMany()
                .HasForeignKey(t => new { t.UsinaCodigo, t.ObraCodigo });

            HasOptional(t => t.Vendedor)
                .WithMany()
                .HasForeignKey(t => t.VendedorCodigo);

            HasOptional(t => t.VendedorPadrinho)
                .WithMany()
                .HasForeignKey(t => t.VendedorPadrinhoCodigo);

            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            HasOptional(t => t.Segmento)
                .WithMany()
                .HasForeignKey(t => t.Segmentacao);

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Ano)
                .HasColumnOrder(1)
                .HasColumnName("ano_chamada");

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnType("umediumint")
                .HasColumnName("num_chamada")
                .HasDatabaseGeneratedOption((DatabaseGeneratedOption.None));

            Property(t => t.IntervenienteCodigo)
                .HasColumnType("umediumint")
                .HasColumnName("cod_cliente");

            Property(t => t.VendedorCodigo)
                .HasColumnName("vendedor");

            Property(t => t.CpfCnpj)
                .HasColumnName("cnpj_cpf");

            Property(t => t.IntervenienteRazao)
                .HasColumnName("cliente");

            Property(t => t.IntervenienteNome)
                .HasColumnName("nome_cliente");

            Property(t => t.ObraCodigo)
                .HasColumnName("no_obra");

            Property(t => t.Data)
                .HasColumnName("dt");

            Property(t => t.Hora)
                .HasColumnName("hora");

            Property(t => t.RepresentanteCodigo)
                .HasColumnName("represent");

            Property(t => t.VendedorPadrinhoCodigo)
                .HasColumnName("vend_padrinho");

            Property(t => t.TracoPrecoNumeroTabela)
                .HasColumnName("num_tab_preco");

            Property(t => t.Contato)
                .HasColumnName("contato");

            Property(t => t.TelefoneDdd)
                .HasColumnName("ddd");

            Property(t => t.TelefoneNumero)
                .HasColumnName("tel");

            Property(t => t.Ramal)
                .HasColumnName("ramal");

            Property(t => t.CelularDdd)
                .HasColumnName("ddd_celular");

            Property(t => t.CelularNumero)
                .HasColumnName("celular");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("end");

            Property(t => t.EnderecoNumero)
                .HasColumnName("num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("mun");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.IntervenienteTipo)
                .HasColumnName("fis_jur");

            Property(t => t.ValorConcreto)
                .HasColumnName("vlr_concreto");

            Property(t => t.ValorBomba)
                .HasColumnName("vlr_bomba");

            Property(t => t.ValorExtras)
                .HasColumnName("vlr_extras");

            Property(t => t.ValorTotalContrato)
                .HasColumnName("vlr_total_ctr");

            Property(t => t.VolumeTotal)
                .HasColumnName("total_m3");

            Property(t => t.IdEmissao)
                .HasColumnName("id_emissao");

            Property(t => t.NomeMae)
                .HasColumnName("nome_mae");

            Property(t => t.NomeConjuge)
                .HasColumnName("conjuge");

            Property(t => t.Status)
                .HasColumnName("status");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("ie");

            Property(t => t.InscricaoMunicipal)
                .HasColumnName("ccm");

            Property(t => t.Rg)
                .HasColumnName("rg");

            Property(t => t.OrgaoExpedidor)
                .HasColumnName("org_uf_emi");

            Property(t => t.Profissao)
                .HasColumnName("profissao");

            Property(t => t.EmpresaTrabalho)
                .HasColumnName("emp_trabalho");

            Property(t => t.TelefoneComercialDdd)
                .HasColumnName("ddd_com");

            Property(t => t.TelefoneComercialNumero)
                .HasColumnName("tel_com");

            Property(t => t.FaturamentoAosCuidados)
                .HasColumnName("faturamento_ac");

            Property(t => t.EmailCobranca)
                .HasColumnName("fat_email");

            Property(t => t.StatusAnterior)
                .HasColumnName("status_anterior");

            Property(t => t.OrigemUsinaCodigo)
                .HasColumnName("origem_usina");

            Property(t => t.OrigemObraCodigo)
                .HasColumnName("origem_obra");

            Property(t => t.ProdutoTipoCodigo)
                .HasColumnName("produto");

            Property(t => t.TemObras)
                .HasColumnName("tem_obras");

            Property(t => t.CodigoObraPrefeitura)
                .HasColumnName("no_obra_pref");

            Property(t => t.EmissaoPropostaAprovada)
                .HasColumnName("Prop_Emi_Aprov");

            Property(t => t.ModeloDocumentoRemessaConcreto)
                .HasColumnName("modelo_doc_remessa_concreto");

            Property(t => t.ModeloDocumentoRemessaBomba)
                .HasColumnName("modelo_doc_remessa_bomba");

            Property(t => t.ModeloItensDanfeERomaneio)
                .HasColumnName("modelo_danfe");

            Property(t => t.CondicoesGerais)
                .HasColumnName("condicoes_gerais");

            Property(t => t.ValidadeProposta)
                .HasColumnName("validade_proposta");

            Property(t => t.Segmentacao)
                .HasColumnName("segmentacao");

            Property(t => t.DataUltimaVersaoGerada)
                .HasColumnName("dt_ult_versao_gerada");

            Property(t => t.AnoVisita)
                .HasColumnName("ano_visita_cliente");

            Property(t => t.NumeroVisita)
                .HasColumnName("num_visita_cliente");

            Property(t => t.AnoLead)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnName("num_lead");

            Property(t => t.AnoOportunidade)
                .HasColumnName("ano_oportunidade");

            Property(t => t.NumeroOportunidade)
                .HasColumnName("num_oportunidade");

            Property(t => t.ContratoFinalidade)
                .HasColumnName("finalidade_ctr");

            Property(t => t.AprovacaoMedicao)
                .HasColumnName("aprov_medicao");

            Property(t => t.TempoAprovacaoMedicao)
                .HasColumnName("tempo_aprov_medicao");
        }
    }
}
