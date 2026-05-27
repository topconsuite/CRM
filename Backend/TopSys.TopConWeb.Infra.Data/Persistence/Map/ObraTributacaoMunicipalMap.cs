using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraTributacaoMunicipalMap : EntityTypeConfiguration<ObraTributacaoMunicipal>
    {
        public ObraTributacaoMunicipalMap()
        {
            ToTable("topsys.con_obras_trib_mun");

            HasKey(t => new { t.ObraUsinaCodigo, t.ObraNumero, t.UsinaEntregaCodigo });
            
            Property(t => t.ObraUsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina_contrato");

            Property(t => t.ObraNumero)
                .HasColumnOrder(1)
                .HasColumnName("no_obra");

            Property(t => t.UsinaEntregaCodigo)
                .HasColumnOrder(2)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnName("num_contrato");

            Property(t => t.CodigoObraPrefeitura)
                .HasColumnName("no_obra_pref");

            Property(t => t.ObraCCM)
                .HasColumnName("ccm_obra");

            Property(t => t.TributacaoISS)
                .HasColumnName("trib_iss");

            Property(t => t.RetencaoISS)
                .HasColumnName("ret_iss");
        }
    }
}
