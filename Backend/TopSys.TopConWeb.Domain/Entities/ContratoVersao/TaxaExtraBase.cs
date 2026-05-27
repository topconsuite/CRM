using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class TaxaExtraBase
    {
         //usina
        public int UsinaCodigo { get; set; }

        //seq
        public int Sequencia { get; set; }

        //obra
        public int ObraCodigo { get; set; }

        //dt_inicio_valid
        public DateTime DataInicioVigencia { get; set; }

        //data_inicio
        public DateTime PeriodoDe { get; set; }

        //data_fim
        public DateTime PeriodoAte { get; set; }

        //texto
        public string DescricaoFormula { get; set; }

        //texto mesclado
        public string Descricao { get; set; }

        //taxa_adicional
        public string Tipo { get; set; }

        //tipo_pessoa
        public string TipoPessoa { get; set; }

        //tipo_valor
        public string ValorTipo { get; set; }

        //valor
        public float Valor { get; set; }

        //valor_por
        public string ValorPor { get; set; }

        //cod_mun_obra
        public int? ObraMunicipioCodigo { get; set; } = 0;

        //quando_de
        public string QuandoDe { get; set; }

        //quando_oper
        public string QuandoOperacao { get; set; }

        //quando_ate
        public string QuandoAte { get; set; }

        //horario_antes
        public string HorarioAntesDas { get; set; }

        //horario
        public string HorarioAposAs { get; set; }

        //cobrar_volume
        public string CobrarVolume { get; set; }

        //volume
        public string Volume { get; set; }

        //da_pedra
        public string PedraDe { get; set; }

        //para_pedra
        public string PedraPara { get; set; }

        //da_resistenc
        public string ResistenciaDe { get; set; }

        //para_resistenc
        public string ResistenciaPara { get; set; }

        //do_slump
        public string SlumpDe { get; set; }

        //para_slump
        public string SlumpPara { get; set; }

        //acima_de
        public int AcimaDe { get; set; }

        //antecedencia
        public string Antecedencia { get; set; }

        //quantidade
        public int Quantidade { get; set; }

        //id_cadast
        public string IdCadastro { get; set; }

        //id_atual
        public string IdAtualizacao { get; set; }

        //external_id
        public string ExternalId { get; set; }

        public int IdSegmentacao { get; set; }

        //prazo_tolerancia_de
        public int PrazoToleranciaDe { get; set; }
    }
}
