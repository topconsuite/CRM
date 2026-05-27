using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraReajusteVersaoMap : EntityTypeConfiguration<ObraReajusteVersao>
    {
        public ObraReajusteVersaoMap()
        {
            ToTable("topsys.con_obras_reajuste_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo });

            HasRequired(t => t.Obra)
                .WithOptional(t => t.ObraReajuste);

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ObraCodigo)
                .HasColumnOrder(2)
                .HasColumnName("obra");

            Property(t => t.MensagemReajuste)
                .HasColumnName("men_reajuste");
        }
    }
}