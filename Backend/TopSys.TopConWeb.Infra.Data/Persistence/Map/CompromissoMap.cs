using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;


namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CompromissoMap : EntityTypeConfiguration<Compromisso>
    {
        public CompromissoMap()
        {
            ToTable("topsys.con_compromisso");

            HasKey(c => c.Codigo);

            Property(c => c.Codigo)
                .HasColumnName("codigo");

            Property(c => c.Usuario)
                .HasColumnName("usuario");

            Property(c => c.Descricao)
                .HasColumnName("descricao");

            Property(c => c.DiaInteiro)
                .HasColumnName("dia_inteiro");

            Property(c => c.DataInicio)
                .HasColumnName("data_inicio");

            Property(c => c.HoraInicio)
                .HasColumnName("hora_inicio");

            Property(c => c.DataFim)
                .HasColumnName("data_fim");

            Property(c => c.HoraFim)
                .HasColumnName("hora_fim");

            Property(c => c.Local)
                .HasColumnName("local");

            Property(c => c.Contato)
                .HasColumnName("contato");

            Property(t => t.DddTelefone)
                .HasColumnName("ddd");

            Property(t => t.Telefone)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(c => c.Email)
                .HasColumnName("email");

            Property(c => c.Observacao)
                .HasColumnName("observacao");

            Property(c => c.Providencia)
                .HasColumnName("providencia");

            Property(c => c.Conclusao)
                .HasColumnName("conclusao");

            Property(c => c.UsinaCodigo)
                .HasColumnName("usina");

            Property(c => c.AnoVisita)
                .HasColumnName("ano_visita");

            Property(c => c.NumeroVisita)
                .HasColumnName("numero_visita");

            Property(c => c.AnoLead)
                .HasColumnName("ano_lead");

            Property(c => c.NumeroLead)
                .HasColumnName("numero_lead");

            Property(c => c.AnoOportunidade)
                .HasColumnName("ano_oportunidade");

            Property(c => c.NumeroOportunidade)
                .HasColumnName("numero_oportunidade");

            Property(c => c.DataCriacao)
                .HasColumnName("data_criacao");

            Property(t => t.IdAgrupamento)
                .HasColumnName("id_agrupamento");
        }
    }
}
