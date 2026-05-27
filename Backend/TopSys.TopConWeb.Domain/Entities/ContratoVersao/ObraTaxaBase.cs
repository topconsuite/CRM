using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraTaxaBase<TTaxaExtra>

        where TTaxaExtra : TaxaExtraBase
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

        public string Antecedencia { get; set; }

        public int Quantidade { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public string ExternalId { get; set; }

        public int PrazoToleranciaDe { get; set; }

        public EStatusAprovacao StatusAprovacao { get; private set; }

        public string LogObservacao { get; set; }

        public bool Nova { get; set; }

        protected ObraTaxaBase() { }

        public ObraTaxaBase(int usinaCodigo, int obraCodigo, int sequencia, string selecionada, string aprovacaoSolicitante, string aprovada, string aprovacaoUsuario, string aprovacaoCiente)
        {
            this.UsinaCodigo = usinaCodigo;
            this.ObraCodigo = obraCodigo;
            this.Sequencia = sequencia;
            this.Selecionada = selecionada;
            this.AprovacaoSolicitante = aprovacaoSolicitante;
            this.Aprovada = aprovada;
            this.AprovacaoUsuario = aprovacaoUsuario;
            this.AprovacaoCiente = aprovacaoCiente;
        }

        public ObraTaxaBase(TTaxaExtra taxa, int obraCodigo, string aprovacaoSolicitante, string aprovada, string selecionada, bool isPersonalizada)
        {
            this.UsinaCodigo = taxa.UsinaCodigo;
            this.ObraCodigo = obraCodigo;
            this.Sequencia = taxa.Sequencia;
            this.DataInicioVigencia = taxa.DataInicioVigencia;
            this.PeriodoDe = taxa.PeriodoDe;
            this.PeriodoAte = taxa.PeriodoAte;
            this.DescricaoFormula = taxa.DescricaoFormula;
            this.Descricao = taxa.Descricao;
            this.AprovacaoSolicitante = aprovacaoSolicitante;
            this.Aprovada = aprovada;
            this.Selecionada = selecionada;
            this.IsPersonalizada = isPersonalizada;
            this.TipoPessoa = taxa.TipoPessoa;
            this.Tipo = taxa.Tipo;
            this.CobrarVolume = taxa.CobrarVolume;
            this.Volume = taxa.Volume;
            this.ValorTipo = taxa.ValorTipo;
            this.Valor = taxa.Valor;
            this.ValorPor = taxa.ValorPor;
            this.ObraMunicipioCodigo = taxa.ObraMunicipioCodigo;
            this.QuandoDe = taxa.QuandoDe;
            this.QuandoOperacao = taxa.QuandoOperacao;
            this.QuandoAte = taxa.QuandoAte;
            this.HorarioAntesDas = taxa.HorarioAntesDas;
            this.HorarioAposAs = taxa.HorarioAposAs;
            this.PedraDe = taxa.PedraDe;
            this.PedraPara = taxa.PedraPara;
            this.SlumpDe = taxa.SlumpDe;
            this.SlumpPara = taxa.SlumpPara;
            this.ResistenciaDe = taxa.ResistenciaDe;
            this.ResistenciaPara = taxa.ResistenciaPara;
            this.AcimaDe = taxa.AcimaDe;
            this.IdCadastro = taxa.IdCadastro;
            this.IdAtualizacao = taxa.IdAtualizacao;
            this.Antecedencia = taxa.Antecedencia;
            this.Quantidade = taxa.Quantidade;
            this.ExternalId = taxa.ExternalId;
            this.PrazoToleranciaDe = taxa.PrazoToleranciaDe;
        }

        public void AtualizaStatusAprovacao()
        {
            if (this.Aprovada == "N")
            {
                this.StatusAprovacao = EStatusAprovacao.Pendente;
            }
            else
            {
                this.StatusAprovacao = EStatusAprovacao.NaoNecessita;
            }
        }

        //TODO: Adicionar ValidationScopes
        public void Aprovar(string usuario)
        {
            //aprov_desc
            this.Aprovada = "S";

            //id_aprov
            this.AprovacaoUsuario = usuario;

            //ciente
            this.AprovacaoCiente = "N";

        }

        public void Reprovar(string usuario)
        {
            //aprov_desc
            this.Aprovada = "X";

            //id_aprov
            this.AprovacaoUsuario = usuario;

            //ciente
            this.AprovacaoCiente = "N";
        }

    }
}
