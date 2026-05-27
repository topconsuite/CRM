using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TarefaMap : EntityTypeConfiguration<Tarefa>
    {
        public TarefaMap()
        {
            ToTable("topsys.con_tarefa");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnName("codigo");

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.DiaInteiro)
                .HasColumnName("dia_inteiro");

            Property(t => t.Data)
                .HasColumnName("data");

            Property(t => t.Horario)
                .HasColumnName("horario");

            Property(t => t.Observacao)
                .HasColumnName("observacao");

            Property(t => t.Contato)
                .HasColumnName("contato");

            Property(t => t.DddTelefone)
                .HasColumnName("ddd");

            Property(t => t.Telefone)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.Finalizado)
                .HasColumnName("finalizado");

            Property(t => t.Providencia)
                .HasColumnName("providencia");

            Property(t => t.Conclusao)
                .HasColumnName("conclusao");

            Property(t => t.UsinaCodigo)
                .HasColumnName("usina");

            Property(t => t.AnoVisita)
                .HasColumnName("ano_visita");

            Property(t => t.NumeroVisita)
                .HasColumnName("numero_visita");

            Property(t => t.AnoLead)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnName("numero_lead");

            Property(t => t.AnoOportunidade)
                .HasColumnName("ano_oportunidade");

            Property(t => t.NumeroOportunidade)
                .HasColumnName("numero_oportunidade");

            Property(t => t.DataCriacao)
                .HasColumnName("data_criacao");

            Property(t => t.IdAgrupamento)
                .HasColumnName("id_agrupamento");

        }
    }

}
