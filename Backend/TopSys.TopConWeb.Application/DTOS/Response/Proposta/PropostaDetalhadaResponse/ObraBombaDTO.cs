using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class ObraBombaDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public int Sequencia { get; set; }

        public CadastroGeralDTO BombaTipo { get; set; }

        public bool BombaPropria { get; set; }

        public IntervenienteDTO Terceiro { get; set; }

        public bool FaturamentoDireto { get; set; }

        public bool AlugadaPeloCliente { get; set; }

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

        public int StatusAprovacao { get; set; }

        public string Justificativa { get; set; }

        public int DistanciaTubulacao { get; set; }

        public float ValorAdicionalTubulacao { get; set; }

        public EBombaM3CalculoTipo TipoCalculo { get; set; }

        public float HoraTabelaAte { get; set; }

        public float HoraTaxaMinimaPrecoTabela { get; set; }

        public float HoraPrecoTabela { get; set; }

        public float HoraPropostoAte { get; set; }

        public float HoraTaxaMinimaPrecoProposto { get; set; }

        public float HoraPrecoProposto { get; set; }

        public float HoraDescontoPercentual { get; set; }

        public EBombaHoraCalculoTipo HoraTipoCalculo { get; set; }

        public string Ativo { get; set; }

        public Boolean Inativo { get; set; }
    }
}
