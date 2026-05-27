using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades
{
    public class OportunidadeMap : EntityTypeConfiguration<Oportunidade>
    {
        public OportunidadeMap()
        {
            ToTable("topsys.con_oportunidade");

            HasKey(t => new { t.UsinaCodigo, t.Ano, t.Numero });

            Property(t => t.UsinaCodigo)
                .HasColumnName("usina");

            Property(t => t.Ano)
                .HasColumnName("ano_oportunidade");

            Property(t => t.Numero)
                .HasColumnName("num_oportunidade");

            Property(t => t.Cliente)
                .HasColumnName("cliente");

            Property(t => t.ObraNome)
                .HasColumnName("obra_nome");

            Property(t => t.DddTelefone)
                .HasColumnName("ddd_telefone");

            Property(t => t.Telefone)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.VendedorCodigo)
                .HasColumnName("vendedor");

            Property(t => t.SegmentacaoCodigo)
                .HasColumnName("segmentacao");

            Property(t => t.OportunidadeTipoCodigo)
                .HasColumnName("tipo_oportunidade");

            Property(t => t.ViaCaptacaoCodigo)
                .HasColumnName("via_captacao");

            Property(t => t.FaseCodigo)
                .HasColumnName("fase");

            Property(t => t.Classificacao)
                .HasColumnName("classificacao");

            Property(t => t.ProximaEtapa)
                .HasColumnName("proxima_etapa");

            Property(t => t.PrevisaoFechamento)
                .HasColumnName("previsao_fechamento");

            Property(t => t.MotivoPerdaCodigo)
                .HasColumnName("motivo_perda");

            Property(t => t.ConcorrenteCodigo)
                .HasColumnName("concorrente");

            Property(t => t.PorteObraCodigo)
                .HasColumnName("obra_porte");

            Property(t => t.ObraFase)
                .HasColumnName("obra_fase");

            Property(t => t.VolumeEstimadoObra)
                .HasColumnName("obra_volume_estimado");

            Property(t => t.ValorEstimadoObra)
                .HasColumnName("obra_valor_estimado");

            Property(t => t.PrevisaoInicio)
                .HasColumnName("previsao_inicio");

            Property(t => t.PrevisaoTermino)
                .HasColumnName("previsao_termino");

            Property(t => t.ReferenciaAcesso)
                .HasColumnName("obra_refer_acesso");

            Property(t => t.AnoLead)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnName("num_lead");

            Property(t => t.AnoVisita)
                .HasColumnName("ano_visita");

            Property(t => t.NumeroVisita)
                .HasColumnName("num_visita");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("endereco");

            Property(t => t.EnderecoNumero)
                .HasColumnName("numero");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("complemento");

            Property(t => t.EnderecoBairro)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("municipio");

            Property(t => t.ObservacaoInterna)
                .HasColumnName("obs_interna");

            Property(t => t.UsinaEntregaCodigo)
                .HasColumnName("usina_entrega");

            Property(t => t.DistanciaUsina)
                .HasColumnName("distancia_usina");

            Property(t => t.IntervenienteCodigo)
                .HasColumnName("interveniente");

            Property(t => t.OportunidadeNome)
                .HasColumnName("oportunidade_nome");

            Property(t => t.Data)
                .HasColumnName("data");

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasOptional(t => t.UsinaEntrega)
                .WithMany()
                .HasForeignKey(t => t.UsinaEntregaCodigo);

            HasRequired(t => t.Vendedor)
                .WithMany()
                .HasForeignKey(t => t.VendedorCodigo);

            HasRequired(t => t.Segmentacao)
                .WithMany()
                .HasForeignKey(t => t.SegmentacaoCodigo);

            HasOptional(t => t.MotivoPerda)
                .WithMany()
                .HasForeignKey(t => t.MotivoPerdaCodigo);

            HasRequired(t => t.OportunidadeTipo)
                .WithMany()
                .HasForeignKey(t => t.OportunidadeTipoCodigo);

            HasRequired(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            HasRequired(t => t.ViaCaptacao)
                .WithMany()
                .HasForeignKey(t => t.ViaCaptacaoCodigo);

            HasRequired(t => t.Fase)
                .WithMany()
                .HasForeignKey(t => t.FaseCodigo);

            HasOptional(t => t.Concorrente)
                .WithMany()
                .HasForeignKey(t => t.ConcorrenteCodigo);

            HasOptional(t => t.PorteObra)
                .WithMany()
                .HasForeignKey(t => t.PorteObraCodigo);

            HasMany(t => t.Logs)
                  .WithRequired(t => t.Oportunidade)
                  .HasForeignKey(t => new { t.Usina, t.Ano, t.Numero });

            HasMany(t => t.Propostas)
                .WithOptional()
                .HasForeignKey(t => new { t.UsinaCodigo, t.AnoOportunidade, t.NumeroOportunidade });


            Ignore(x => x.ContatoPrincipal);
            Ignore(x => x.ContatoSecundario);

        }
    }

}
