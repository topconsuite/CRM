using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraTracoBase<TObra>
    {
        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public float Fck { get; set; }

        public int Consumo { get; set; }

        //Objetos

        //tp_resist
        public int ResistenciaTipoCodigo { get; set; }

        public virtual ResistenciaTipo ResistenciaTipo { get; set; }

        //uso
        public int UsoCodigo { get; set; }

        public virtual Uso Uso { get; set; }

        //pedra
        public int PedraCodigo { get; set; }

        public virtual Pedra Pedra { get; set; }

        //slump
        public int SlumpCodigo { get; set; }

        public virtual SlumpReal Slump { get; set; }

        //slump nominal
        public int SlumpNominalCodigo { get; set; }
        public virtual Slump SlumpNominal { get; set; }

        //Fim objetos

        public float M3Quantidade { get; set; }

        public float M3PrecoTabela { get; set; }

        public float M3PrecoProposto { get; set; }

        public float M3PrecoAjustado { get; set; }

        public float DescontoPercentual { get; set; }

        public string PecaConcretar { get; set; }

        public float PrecoConcorrencia { get; set; }

        public float ValorServico { get; set; }

        public float ValorRessarcido { get; set; }

        public string DescontoSolicitante { get; set; }

        public string AprovacaoTipo { get; set; }

        public string AprovacaoVerbal { get; set; }

        public string AprovacaoObservacao { get; set; }

        public string AprovacaoOperacao { get; set; }

        public string AprovacaoCiente { get; set; }

        public EStatusAprovacao StatusAprovacao { get; private set; }

        public string Justificativa { get; set; }

        public string LogObservacao { get; set; }

        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public virtual TObra Obra { get; set; }

        public float PrecoReajustadoAnterior { get; set; }

        public DateTime? DataUltimoReajuste { get; set; }

        public float PrecoReajustadoAtual { get; set; }

        public string IdAlteracaoTracoPesado { get; set; } = "";

        public string Observacao { get; set; }

        public string DescricaoPersonalizada { get; set; } = "";

        public float M3QuantidadeBombeada { get; set; }

        public float MargemPosTransporte { get; set; }
        public float Ebitda { get; set; }

        public float IssDedutivel { get; set; }

        public float ImpostoAplicadoEstadual { get; set; }

        public float ImpostoAplicadoFederal { get; set; }
        public float CustoBombagem { get; set; }

        public float CustoServicoReajustado { get; set; }

        public float CustoProjetadoTransporte { get; set; }

        public float CustoServicoAnterior { get; set; }

        public string Ativo { get; set; }

        public int? NumeracaoProduto { get; set; }

        public int NumeracaoFamilia { get; set; }

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

        public float TotalImpostos
        {
            get
            {
                return IssDedutivel + ImpostoAplicadoEstadual + ImpostoAplicadoFederal;
            }
        }

        //TODO: Analisar se este é o melhor approach
        public string Resistencia
        {
            get
            {
                return ResistenciaTipo.FormatarResistencia(Fck, Consumo);
            }
        }

        public void AtualizaStatusAprovacao(string usuario)
        {
            if (this.AprovacaoVerbal == "N" && (this.AprovacaoObservacao == "" || this.AprovacaoObservacao == usuario))
            {
                this.StatusAprovacao = EStatusAprovacao.Pendente;
            }
            else if(this.AprovacaoVerbal == "S" && this.AprovacaoOperacao == "S" && this.AprovacaoCiente == "N" && !string.IsNullOrEmpty(this.AprovacaoObservacao))
            {
                this.StatusAprovacao = EStatusAprovacao.Aprovado;
            }
            else
            {
                this.StatusAprovacao = EStatusAprovacao.NaoNecessita;
            }
        }

        public bool TracoAprovado()
        {
            return this.StatusAprovacao == EStatusAprovacao.Aprovado;
        }

        //TODO: Adicionar ValidationsScopes
        public void Aprovar(string usuario)
        {
            //aprov_verbal
            this.AprovacaoVerbal = "S";

            //status_aprov
            this.AprovacaoOperacao = "S";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;


        }

        //TODO: Adicionar ValidationsScopes
        public void Alterar(string usuario, float m3PrecoProposto)
        {

            //aprov_verbal
            this.AprovacaoVerbal = "S";

            //status_aprov
            this.AprovacaoOperacao = "V";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            //preco_unit_prop
            this.M3PrecoProposto = m3PrecoProposto;

            //pct_descto
            this.DescontoPercentual = (this.M3PrecoProposto - this.M3PrecoAjustado) / this.M3PrecoAjustado * 100;

        }

        //TODO: Adicionar ValidationsScopes
        public void Reprovar(string usuario, TracoPreco tracoPreco, float valorAdicional)
        {
            //aprov_verbal
            this.AprovacaoVerbal = "S";

            //status_aprov
            this.AprovacaoOperacao = "X";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            //preco_unit_tab
            this.M3PrecoTabela = tracoPreco.M3Preco;

            //preco_unit_prop      //preco_unit_tab
            this.M3PrecoProposto = tracoPreco.M3Preco + valorAdicional;

            //pct_descto
            this.DescontoPercentual = 0;
        }

        public void ReprovarPorAlcada(string usuario)
        {
            //aprov_verbal
            this.AprovacaoVerbal = "S";

            //status_aprov
            this.AprovacaoOperacao = "X";

            //ciente_aprov
            this.AprovacaoCiente = "N";

            //obs_aprov
            this.AprovacaoObservacao = usuario;

            this.StatusAprovacao = EStatusAprovacao.Reprovado;

            this.LogObservacao = "";
        }

        public bool Bombeavel()
        {
            return SlumpCodigo >= 9;
        }

        public string Descricao
        {
            get
            {
                if (ResistenciaTipo == null || Pedra == null || SlumpNominal == null || Uso == null) return "";
                if (DescricaoPersonalizada != "") return DescricaoPersonalizada;

                var vinculo = ResistenciaTipo.Vinculo;
                var mpaConsumo = (vinculo == EResistenciaVinculoTipo.Mpa ? Fck : (vinculo == EResistenciaVinculoTipo.Consumo ? Consumo : 0));

                return $"{ResistenciaTipo.Abreviatura} {mpaConsumo}/{Pedra.Descricao}/{SlumpNominal.Descricao}/{Uso.Descricao}";
            }
        }

        public float ValorM3
        {
            get
            {
                return PrecoReajustadoAtual > 0 ? PrecoReajustadoAtual : M3PrecoProposto;
            }
        }

        public float ValorMaterial(TracoCusto tracoCusto)
        {
            return CustoServicoReajustado > 0 ? (PrecoReajustadoAtual - CustoServicoReajustado) : tracoCusto?.CustoAjustado ?? 0;
        }

    }
}
