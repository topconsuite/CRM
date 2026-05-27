using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraBombaBase<TObra>
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public int? BombaTipoCodigo { get; set; } = 0;

        public virtual CadastroGeral BombaTipo { get; set; }

        public string BombaPropriaSimNao { get; set; }

        public int? TerceiroCodigo { get; set; } = 0;

        public virtual Interveniente Terceiro { get; set; }

        public string FaturamentoDiretoSimNao { get; set; }

        public string AlugadaPeloClienteSimNao { get; set; }

        public int M3TabelaAte { get; set; }

        public float TaxaMinimaPrecoTabela { get; set; }

        public float M3PrecoTabela { get; set; }

        public int M3PropostoAte { get; set; }

        public float TaxaMinimaPrecoProposto { get; set; }

        public float M3PrecoProposto { get; set; }

        public float TaxaMinimaReajustadaAnterior { get; set; }

        public int M3ReajustadoAteAnterior { get; set; }

        public float M3PrecoReajustadoAnterior { get; set; }

        public float TaxaMinimaReajustadaAtual { get; set; }

        public int M3ReajustadoAteAtual { get; set; }

        public float M3PrecoReajustadoAtual { get; set; }

        public DateTime? DataUltimoReajuste { get; set; }

        public float DescontoPercentual { get; set; }

        public string DescontoSolicitante { get; set; }

        public string AprovacaoVerbal { get; set; }

        public string AprovacaoObservacao { get; set; }

        public string AprovacaoOperacao { get; set; }

        public string AprovacaoCiente { get; set; }

        public EStatusAprovacao StatusAprovacao { get; private set; }

        public string Justificativa { get; set; }

        public string LogObservacao { get; set; }

        public int DistanciaTubulacao { get; set; }

        public float ValorAdicionalTubulacao { get; set; }

        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public EBombaM3CalculoTipo TipoCalculo { get; set; }

        public float HoraTabelaAte { get; set; }

        public float HoraTaxaMinimaPrecoTabela { get; set; }

        public float HoraPrecoTabela { get; set; }

        public float HoraPropostoAte { get; set; }

        public float HoraTaxaMinimaPrecoProposto { get; set; }

        public float HoraPrecoProposto { get; set; }

        public float HoraDescontoPercentual { get; set; }

        public EBombaHoraCalculoTipo HoraTipoCalculo { get; set; }

        public virtual TObra Obra { get; set; }

        public float IssDedutivel { get; set; }
        public float ImpostoAplicadoEstadual { get; set; }
        public float ImpostoAplicadoFederal { get; set; }
        public float Ebitda { get; set; }
        public float TotalImpostos
        {
            get
            {
                return IssDedutivel + ImpostoAplicadoEstadual + ImpostoAplicadoFederal;
            }
        }

        public virtual bool BombaPropria
        {
            get { return (BombaPropriaSimNao ?? "").Equals("S"); }
            set { BombaPropriaSimNao = value ? "S" : "N"; }
        }

        public virtual bool FaturamentoDireto
        {
            get { return (FaturamentoDiretoSimNao ?? "").Equals("S"); }
            set { FaturamentoDiretoSimNao = value ? "S" : "N"; }
        }

        public virtual bool AlugadaPeloCliente
        {
            get { return (AlugadaPeloClienteSimNao ?? "").Equals("S"); }
            set { AlugadaPeloClienteSimNao = value ? "S" : "N"; }
        }

        public string Ativo { get; set; }

        public Boolean Inativo
        {
            get
            {
                return Ativo == "N";
            }
            set
            {
                if (value)
                    Ativo = "N";
                else
                    Ativo = "S";
            }
        }

        public void AtualizaStatusAprovacao(string usuario)
        {
            if (this.AprovacaoVerbal == "S" && (this.AprovacaoObservacao == "" || this.AprovacaoObservacao == usuario))
            {
                this.StatusAprovacao = EStatusAprovacao.Pendente;
            }
            else if (this.AprovacaoVerbal == "N" && this.AprovacaoOperacao == "S" && this.AprovacaoCiente == "N" && !string.IsNullOrEmpty(this.AprovacaoObservacao))
            {
                this.StatusAprovacao = EStatusAprovacao.Aprovado;
            }
            else
            {
                this.StatusAprovacao = EStatusAprovacao.NaoNecessita;
            }
        }

        //TODO: Adicionar ValidationsScopes
        public void Aprovar(string usuario)
        {
            //aprov_verbal
            this.AprovacaoVerbal = "N";

            //status_aprov
            this.AprovacaoOperacao = "S";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;
        }

        //TODO: Adicionar ValidationsScopes
        public void Alterar(string usuario, int m3PropostoAte, float taxaMinimaPrecoProposto, float m3PrecoProposto)
        {

            //aprov_verbal
            this.AprovacaoVerbal = "N";

            //status_aprov
            this.AprovacaoOperacao = "V";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            //m3_pr_prop
            this.M3PropostoAte = m3PropostoAte;

            //txa_min_pr_prop              
            this.TaxaMinimaPrecoProposto = taxaMinimaPrecoProposto;

            //pr_m3_bomb_pr_p      
            this.M3PrecoProposto = m3PrecoProposto;

            //pct_descto
            this.DescontoPercentual = (this.M3PrecoProposto - this.M3PrecoTabela) / this.M3PrecoTabela * 100;
        }

        //TODO: Adicionar ValidationsScopes
        public void Reprovar(string usuario)
        {

            //aprov_verbal
            this.AprovacaoVerbal = "N";

            //status_aprov
            this.AprovacaoOperacao = "X";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            //m3_pr_prop         //m3_pr_tab
            this.M3PropostoAte = this.M3TabelaAte;

            //txa_min_pr_prop              //taxa_minima_tab
            this.TaxaMinimaPrecoProposto = this.TaxaMinimaPrecoTabela;

            //pr_m3_bomb_pr_p      //pr_m3_tab
            this.M3PrecoProposto = this.M3PrecoTabela;

            //pct_descto
            this.DescontoPercentual = 0;
        }

        public void ReprovarPorAlcada(string usuario)
        {

            //aprov_verbal
            this.AprovacaoVerbal = "N";

            //status_aprov
            this.AprovacaoOperacao = "X";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            //m3_pr_prop         //m3_pr_tab
            this.M3PropostoAte = this.M3TabelaAte;

            //txa_min_pr_prop              //taxa_minima_tab
            this.TaxaMinimaPrecoProposto = this.TaxaMinimaPrecoTabela;

            //pr_m3_bomb_pr_p      //pr_m3_tab
            this.M3PrecoProposto = this.M3PrecoTabela;

            //pct_descto
            this.DescontoPercentual = 0;

            this.StatusAprovacao = EStatusAprovacao.Reprovado;

            this.LogObservacao = "";

        }


        public float CalculaValorBomba(float VolumeBombeavel)
        {
            var result = ValorAdicionalTubulacao;

            switch (TipoCalculo)
            {
                case EBombaM3CalculoTipo.TaxaMinimaOuExcedente:
                    result += (VolumeBombeavel * M3PrecoProposto);
                    break;
                case EBombaM3CalculoTipo.TaxaMinimaMaisExcedente:
                    result += TaxaMinimaPrecoProposto
                        + (M3PropostoAte > 0 && VolumeBombeavel > M3PropostoAte ?
                            (VolumeBombeavel - M3PropostoAte) * M3PrecoProposto : 0f);
                    break;
            }

            if (BombaPropria && HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca)
            {
                result += HoraTaxaMinimaPrecoProposto;
            }

            return result;
        }

        public int RetornaVolumeM3()
        {
            if (DataUltimoReajuste == null)
                return M3PropostoAte;
            else if (DataUltimoReajuste > DateTime.Today)
                return M3ReajustadoAteAnterior;
            else
                return M3ReajustadoAteAtual;
        }

        public float RetornaTaxaMinima()
        {
            if (DataUltimoReajuste == null)
                return TaxaMinimaPrecoProposto;
            else if (DataUltimoReajuste > DateTime.Today)
                return TaxaMinimaReajustadaAnterior;
            else
                return TaxaMinimaReajustadaAtual;
        }

        public float RetornaPrecoM3()
        {
            if (DataUltimoReajuste == null)
                return M3PrecoProposto;
            else if (DataUltimoReajuste > DateTime.Today)
                return M3PrecoReajustadoAnterior;
            else
                return M3PrecoReajustadoAtual;
        }
    }
}