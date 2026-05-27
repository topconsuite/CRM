using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao
{
    public class ObraTaxaDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public string Descricao { get; set; }

        public bool IsPersonalizada { get; set; }

        public string Selecionada { get; set; }

        public string AprovacaoSolicitante { get; set; }

        public string AprovacaoUsuario { get; set; }

        public string AprovacaoCiente { get; set; }

        public string Aprovada { get; set; }

        public string Tipo { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public DateTime PeriodoDe { get; set; }

        public DateTime PeriodoAte { get; set; }

        public string DescricaoFormula { get; set; }

        public string TipoPessoa { get; set; }

        public string ValorTipo { get; set; }

        public float Valor { get; set; }

        public string ValorPor { get; set; }

        public int? ObraMunicipioCodigo { get; set; } = 0;

        public string QuandoDe { get; set; }

        public string QuandoOperacao { get; set; }

        public string QuandoAte { get; set; }

        public string HorarioAntesDas { get; set; }

        public string HorarioAposAs { get; set; }

        public string CobrarVolume { get; set; }

        public string Volume { get; set; }

        public string PedraDe { get; set; }

        public string PedraPara { get; set; }

        public string ResistenciaDe { get; set; }

        public string ResistenciaPara { get; set; }

        public string SlumpDe { get; set; }

        public string SlumpPara { get; set; }

        public int AcimaDe { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public bool Nova { get; set; }

        public string Antecedencia { get; set; }

        public int Quantidade { get; set; }

        public string ExternalId { get; set; }

        public int PrazoToleranciaDe { get; set; }
    }
}
