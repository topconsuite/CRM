using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TaxaExtra : TaxaExtraBase
    {
        public TaxaExtra() { }

        public TaxaExtra(ObraTaxa obraTaxa)
        {
            UsinaCodigo = obraTaxa.UsinaCodigo;
            ObraCodigo = obraTaxa.ObraCodigo;
            Sequencia = obraTaxa.Sequencia;
            DataInicioVigencia = obraTaxa.DataInicioVigencia;
            PeriodoDe = obraTaxa.PeriodoDe;
            PeriodoAte = obraTaxa.PeriodoAte;
            DescricaoFormula = obraTaxa.DescricaoFormula;
            Descricao = obraTaxa.Descricao;
            TipoPessoa = obraTaxa.TipoPessoa;
            Tipo = obraTaxa.Tipo;
            CobrarVolume = obraTaxa.CobrarVolume;
            Volume = obraTaxa.Volume;
            ValorTipo = obraTaxa.ValorTipo;
            Valor = obraTaxa.Valor;
            ValorPor = obraTaxa.ValorPor;
            ObraMunicipioCodigo = obraTaxa.ObraMunicipioCodigo;
            QuandoDe = obraTaxa.QuandoDe;
            QuandoOperacao = obraTaxa.QuandoOperacao;
            QuandoAte = obraTaxa.QuandoAte;
            HorarioAntesDas = obraTaxa.HorarioAntesDas;
            HorarioAposAs = obraTaxa.HorarioAposAs;
            PedraDe = obraTaxa.PedraDe;
            PedraPara = obraTaxa.PedraPara;
            SlumpDe = obraTaxa.SlumpDe;
            SlumpPara = obraTaxa.SlumpPara;
            ResistenciaDe = obraTaxa.ResistenciaDe;
            ResistenciaPara = obraTaxa.ResistenciaPara;
            AcimaDe = obraTaxa.AcimaDe;
            IdCadastro = obraTaxa.IdCadastro;
            IdAtualizacao = obraTaxa.IdAtualizacao;
            Antecedencia = obraTaxa.Antecedencia;
            Quantidade = obraTaxa.Quantidade;
            ExternalId = obraTaxa.ExternalId;
            PrazoToleranciaDe = obraTaxa.PrazoToleranciaDe;
        }

        public static explicit operator TaxaExtraVersao(TaxaExtra taxaExtra)
        {
            var taxaExtraVersao = new TaxaExtraVersao();

            var properties = typeof(TaxaExtraBase).GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                var value = property.GetValue(taxaExtra);
                property.SetValue(taxaExtraVersao, value);
            }

            return taxaExtraVersao;
        }

       
    }
}
