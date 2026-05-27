using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CustoServicoMap : EntityTypeConfiguration<CustoServico>
    {
        public CustoServicoMap()
        {
            ToTable("topsys.con_custo_serv");

            HasKey(t => new { t.UsinaCodigo, t.DataInicioVigencia });

            Property(t => t.UsinaCodigo)
                 .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.DataInicioVigencia)
            .HasColumnOrder(1)
            .HasColumnName("dt_inicio_valid");

            Property(t => t.UsinaTabelaPreco)
                .HasColumnName("usina_tab_preco");

            Property(t => t.CustoMedioServico)
                .HasColumnName("vlr_custo_serv");

            Property(t => t.CustoMedioCombustivel)
                .HasColumnName("vlr_custo_combu");

            Property(t => t.CustoMedioImpostos)
                .HasColumnName("vlr_custo_impos");

            Property(t => t.CustoMedioLubrificantes)
                .HasColumnName("vlr_custo_lubri");

            Property(t => t.CustoMedioManutencao)
                .HasColumnName("vlr_custo_manut");

            Property(t => t.CustoMedioMaoDeObra)
                .HasColumnName("vlr_custo_mo");

            Property(t => t.CustoMedioHoraTransporte)
                    .HasColumnName("vlr_custo_trans");

            Property(t => t.CustoMedioProducao)
                    .HasColumnName("vlr_custo_prod");

            Property(t => t.CustoMedioBombagem)
                    .HasColumnName("vlr_custo_bombagem");

            Property(t => t.CustoMedioLaboratorio)
                    .HasColumnName("vlr_custo_lab");

            Property(t => t.CustoMedioComercial)
                    .HasColumnName("vlr_custo_comer");

            Property(t => t.CustoMedioAdministrativo)
                    .HasColumnName("vlr_custo_adm");

            Property(t => t.Lucro)
                    .HasColumnName("vlr_lucro");

            Property(t => t.OutrosCustosMateriais)
                    .HasColumnName("vlr_custo_outro");

            Property(t => t.Markup)
                    .HasColumnName("markup");

            Property(t => t.FormaMedidaAditivo)
                    .HasColumnName("medida_aditivo");

            Property(t => t.ImpostoEstadual)
                    .HasColumnName("imposto_aplicado_estadual");

            Property(t => t.ImpostoFederal)
                    .HasColumnName("imposto_aplicado_federal");

            HasRequired(t => t.Usina)
               .WithMany()
               .HasForeignKey(t => t.UsinaCodigo);



        }
    }
}
