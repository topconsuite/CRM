using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class UsinaMap : EntityTypeConfiguration<Usina>
    {
        public UsinaMap()
        {
            ToTable("topsys.con_usina");

            HasKey(t => t.Codigo);

            Ignore(t => t.GeraCreditoClientePagamentoAntecipado);

            Property(t => t.Codigo)
                .HasColumnName("cod");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.Sigla)
                .HasColumnName("sigla");

            Property(t => t.FilialCodigo)
                .HasColumnName("emp_filial");

            Property(t => t.IntervaloEmMinutosEntreCargas)
                .HasColumnName("intervalo_carga");

            Property(t => t.GeraCreditoClientePagamentoAntecipadoSimNao)
                .HasColumnName("gera_cred_cli");

            Property(t => t.PrazoPesagem)
                .HasColumnName("min_pesagem");

            Property(t => t.PorcentagemRetornoObra)
                .HasColumnName("pct_retorn_obra");

            Property(t => t.TempoBtNaObra)
                .HasColumnName("temp_bt_na_obra");

            Property(t => t.TempoBtAteAObra)
                .HasColumnName("temp_ate_a_obra");


            Property(t => t.MoldagemRemota)
                .HasColumnName("moldagem_remota");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

            Property(t => t.ClicksignConfigId)
                .HasColumnName("clicksign_config_id");

            HasOptional(t => t.ClicksignConfiguracao)
                .WithMany()
                .HasForeignKey(t => t.ClicksignConfigId);

        }
    }
}
