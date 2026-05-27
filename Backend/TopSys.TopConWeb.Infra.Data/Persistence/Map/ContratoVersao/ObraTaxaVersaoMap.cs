using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraTaxaVersaoMap : EntityTypeConfiguration<ObraTaxaVersao>
    {
        public ObraTaxaVersaoMap()
        {
            ToTable("topsys.con_obras_tx_versao");

            Ignore(t => t.DataInicioVigencia);
            Ignore(t => t.PeriodoDe);
            Ignore(t => t.PeriodoAte);
            Ignore(t => t.DescricaoFormula);
            Ignore(t => t.Descricao);
            Ignore(t => t.IsPersonalizada);
            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);
            Ignore(t => t.Tipo);
            Ignore(t => t.ObraMunicipioCodigo);
            Ignore(t => t.QuandoDe);
            Ignore(t => t.QuandoOperacao);
            Ignore(t => t.QuandoAte);
            Ignore(t => t.HorarioAntesDas);
            Ignore(t => t.HorarioAposAs);
            Ignore(t => t.CobrarVolume);
            Ignore(t => t.Volume);
            Ignore(t => t.TipoPessoa);
            Ignore(t => t.ValorTipo);
            Ignore(t => t.Valor);
            Ignore(t => t.ValorPor);
            Ignore(t => t.PedraDe);
            Ignore(t => t.PedraPara);
            Ignore(t => t.ResistenciaDe);
            Ignore(t => t.ResistenciaPara);
            Ignore(t => t.SlumpDe);
            Ignore(t => t.SlumpPara);
            Ignore(t => t.AcimaDe);
            Ignore(t => t.IdCadastro);
            Ignore(t => t.IdAtualizacao);
            Ignore(t => t.Nova);
            Ignore(t => t.Antecedencia);
            Ignore(t => t.Quantidade);
            Ignore(t => t.ExternalId);
            Ignore(t => t.PrazoToleranciaDe);

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo, t.Sequencia });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(2)
                .HasColumnName("obra");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq");

            Property(t => t.Selecionada)
                .HasColumnName("selecionado");

            Property(t => t.Aprovada)
                .HasColumnName("aprov_desc");

            Property(t => t.AprovacaoSolicitante)
               .HasColumnName("id_solic");

            Property(t => t.AprovacaoUsuario)
                 .HasColumnName("id_aprov");

            Property(t => t.AprovacaoCiente)
                .HasColumnName("ciente");

        }
    }
}