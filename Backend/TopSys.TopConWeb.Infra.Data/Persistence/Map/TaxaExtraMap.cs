using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TaxaExtraMap:EntityTypeConfiguration<TaxaExtra>
    {
        public TaxaExtraMap()
        {
            ToTable("topsys.con_taxa_extra");

            HasKey(t => new { t.UsinaCodigo, t.Sequencia, t.ObraCodigo });

            Property(t => t.UsinaCodigo)
              .HasColumnOrder(0)
              .HasColumnName("usina");

            Property(t => t.Sequencia)
                .HasColumnOrder(1)
                .HasColumnName("seq");

            Property(t => t.ObraCodigo)
               .HasColumnOrder(2)
               .HasColumnName("obra");

            Property(t => t.DataInicioVigencia)
              .HasColumnName("dt_inicio_valid");

            Property(t => t.PeriodoDe)
              .HasColumnName("data_inicio");

            Property(t => t.PeriodoAte)
              .HasColumnName("data_fim");

            Property(t => t.DescricaoFormula)
                .HasColumnName("texto");

            Property(t => t.Descricao)
                .HasColumnName("texto_mesclado");

            Property(t => t.Tipo)
                .HasColumnName("taxa_adicional");

            Property(t => t.TipoPessoa)
                .HasColumnName("tipo_pessoa");

            Property(t => t.ValorTipo)
                .HasColumnName("tipo_valor");

            Property(t => t.Valor)
                .HasColumnName("valor");

            Property(t => t.ValorPor)
                .HasColumnName("valor_por");

            Property(t => t.ObraMunicipioCodigo)
                .HasColumnName("cod_mun_obra");

            Property(t => t.QuandoDe)
                .HasColumnName("quando_de");

            Property(t => t.QuandoOperacao)
                .HasColumnName("quando_oper");

            Property(t => t.QuandoAte)
                .HasColumnName("quando_ate");

            Property(t => t.HorarioAntesDas)
                .HasColumnName("horario_antes");

            Property(t => t.HorarioAposAs)
                .HasColumnName("horario");

            Property(t => t.CobrarVolume)
                .HasColumnName("cobrar_volume");

            Property(t => t.Volume)
                .HasColumnName("volume");

            Property(t => t.PedraDe)
                .HasColumnName("da_pedra");

            Property(t => t.PedraPara)
                .HasColumnName("para_pedra");

            Property(t => t.ResistenciaDe)
                .HasColumnName("da_resistenc");

            Property(t => t.ResistenciaPara)
                .HasColumnName("para_resistenc");

            Property(t => t.SlumpDe)
                .HasColumnName("do_slump");

            Property(t => t.SlumpPara)
                .HasColumnName("para_slump");

            Property(t => t.AcimaDe)
                .HasColumnName("acima_de");

            Property(t => t.Antecedencia)
                .HasColumnName("antecedencia");

            Property(t => t.Quantidade)
                .HasColumnName("quantidade");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

            Property(t => t.IdSegmentacao)
                .HasColumnName("id_segmentacao");

            Property(t => t.PrazoToleranciaDe)
                .HasColumnName("prazo_tolerancia_de");
        }
    }
}
