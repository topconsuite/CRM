using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraBase<TObra, TContrato, TProposta, TObraFrente, TObraTraco, TObraBomba, TObraTaxa, TObraLog, TObraDemaisServicos, TPropostaPagamento, TContratoPagamento, TContratoPagamentoDetalhe, TObraTributacaoMunicipal, TObraMensagemPadrao, TObraReajuste, TObraIndicador> : IHasEndereco, IObra<TObra, TContrato, TObraTraco, TObraBomba>
        where TContrato : IHasInterveniente, IHasVendedor, IContrato
        where TProposta : IHasInterveniente, IHasIntervenienteRazao, IHasVendedor
        where TObraFrente : ObraFrenteBase
        where TObraTraco : ObraTracoBase<TObra>
        where TObraBomba : ObraBombaBase<TObra>
        where TContratoPagamento : ContratoPagamentoBase<TContratoPagamentoDetalhe>
        where TContratoPagamentoDetalhe : ContratoPagamentoDetalheBase
        where TObraDemaisServicos : IObraDemaisServicos<TObra, TContrato, TObraTraco, TObraBomba>
        where TObra : IObra<TObra, TContrato, TObraTraco, TObraBomba>
        where TObraReajuste : ObraReajusteBase<TObra>
        where TObraIndicador : ObraIndicadorBase
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public string Nome { get; set; }

        public int? AnoContrato { get; set; } = 0;
        public int? NumContrato { get; set; } = 0;

        public int UsinaEntregaCodigo { get; set; }

        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        public int? CondicaoPagamentoCodigo { get; set; } = 0;

        public int? TipoCobrancaCodigo { get; set; } = 0;

        public int DistanciaUsina { get; set; }

        public int DistanciaUsinaGoogleApi { get; set; } = 0;

        public DateTime? PrevisaoInicio { get; set; }

        public DateTime? PrevisaoTermino { get; set; }

        public string RadioNextel { get; set; }

        public string ContatoPrincipalNome { get; set; }
        public int? ContatoPrincipalFuncaoCodigo { get; set; } = 0;
        public virtual CadastroGeral ContatoPrincipalFuncao { get; set; }
        public int ContatoPrincipalTelefoneDdd { get; set; }
        public int ContatoPrincipalTelefoneNumero { get; set; }
        public int ContatoPrincipalCelularDdd { get; set; }
        public int ContatoPrincipalCelularNumero { get; set; }

        public string ContatoSecundarioNome { get; set; }
        public int? ContatoSecundarioFuncaoCodigo { get; set; } = 0;
        public virtual CadastroGeral ContatoSecundarioFuncao { get; set; }
        public int ContatoSecundarioTelefoneDdd { get; set; }
        public int ContatoSecundarioTelefoneNumero { get; set; }
        public int ContatoSecundarioCelularDdd { get; set; }
        public int ContatoSecundarioCelularNumero { get; set; }

        public int? ViaCaptacaoCodigo { get; set; } = 0;
        public virtual CadastroGeral ViaCaptacao { get; set; }

        public int? TipoObraCodigo { get; set; } = 0;
        public virtual CadastroGeral TipoObra { get; set; }

        public int? PorteObraCodigo { get; set; } = 0;
        public virtual CadastroGeral PorteObra { get; set; }

        public string IniciadaPorConcorrenteSimNao { get; set; }
        public int? ConcorrenteCodigo { get; set; } = 0;
        public virtual CadastroGeral Concorrente { get; set; }

        public string Email { get; set; }

        public float VolumeEstimado { get; set; }

        public float VolumePorCarga { get; set; }

        public string ObraNova { get; set; }

        public string Itinerante { get; set; }

        public string Cei { get; set; }

        public string CodigoCib { get; set; }

        public EConstrucaoCivilTipoAlvara ConstrucaoCivilTipoAlvara { get; set; }

        public string TributacaoPisCofinsCodigo { get; set; }
        
        public virtual TributacaoPisCofins TributacaoPisCofins { get; set; }

        public int TributacaoISCodigo { get; set; }

        public virtual TributacaoReforma TributacaoIS { get; set; }

        public int TributacaoIBSCodigo { get; set; }

        public virtual TributacaoReforma TributacaoIBS { get; set; }

        public int TributacaoCBSCodigo { get; set; }

        public virtual TributacaoReforma TributacaoCBS { get; set; }

        public string ObservacaoNf { get; set; }

        public string ReferenciaAcesso { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }

        public EObraStatusComercial StatusComercial { get; set; }
        public EObraDemaisStatusComercial VolumeStatusComercial { get; set; }
        public EObraDemaisStatusComercial CondicaoPagamentoStatusComercial { get; set; }
        public EObraStatusCadastro StatusCadastro { get; set; }
        public EObraStatusEngenharia StatusEngenharia { get; set; }
        public EObraStatusFinanceiro StatusFinanceiro { get; set; }

        public int VibradorQuantidade { get; set; }

        public float VibradorValorUnitario { get; set; }

        public float ValorDemaisServicos { get; set; }

        public float PercentualRepasseReajusteCimento { get; set; }
        public float PercentualRepasseReajustePedra { get; set; }
        public float PercentualRepasseReajusteAreia { get; set; }
        public float PercentualRepasseReajusteMaoDeObra { get; set; }
        public float PercentualRepasseReajusteDiesel { get; set; }

        //TODO: ANALISAR O MAPEAMENTO PARCIAL DO ENDEREÇO

        public string EnderecoCep { get; set; }

        public string EnderecoLogradouro { get; set; }

        public int EnderecoNumero { get; set; }

        public string EnderecoComplemento { get; set; }

        public string EnderecoBairro { get; set; }

        public bool PendenteAprovacaoDistanciaUsinaCEP { get; set; } = false;

        public virtual Municipio EnderecoMunicipio { get; set; }

        public virtual CondicaoPagamento CondicaoPagamento { get; set; }

        public virtual TipoCobranca TipoCobranca { get; set; }

        public virtual Usina UsinaEntrega { get; set; }

        public virtual TContrato Contrato { get; set; }

        public virtual TProposta Proposta { get; set; }

        public virtual TObraIndicador Indicador { get; set; }

        public virtual ICollection<TObraFrente> ObraFrentes { get; set; }

        public virtual ICollection<TObraTraco> ObraTracos { get; set; }

        public virtual ICollection<TObraBomba> ObraBombas { get; set; }

        public virtual ICollection<TObraTaxa> ObraTaxas { get; set; }

        public virtual ICollection<TObraTributacaoMunicipal> ObraTributacoesMunicipais { get; set; }

        public virtual ICollection<TObraMensagemPadrao> ObraMensagensPadrao { get; set; }

        public virtual ICollection<DemaisAprovacao> DemaisAprovacoes { get; set; }

        public virtual ICollection<ObraProjecao> ObraProjecao { get; set;  }

        public virtual ICollection<TObraLog> ObraLogs { get; set; }

        public virtual ICollection<TObraDemaisServicos> ObraDemaisServicos { get; set; }

        public virtual ICollection<TContratoPagamento> ContratoPagamentos { get; set; }
        public virtual ICollection<TPropostaPagamento> PropostaPagamentos { get; set; }

        public string ObservacaoInterna { get; set; }

        public int TempoBtNaObra { get; set; }

        public int TempoAteAObra { get; set; }

        public int TempoDescarga { get; set; }

        public float TempoCicloPrevisto { get; set; }

        public float CustoProjetadoTransporte { get; set; }

        public string CodigoBeneficioFiscal { get; set; }

        public virtual TObraReajuste ObraReajuste { get; set; }

        public string EmailResponsavelTecnico { get; set; }

        public string UsaAdicionalZMRC{ get; set; }

        public string NecessitaAprovacaoZMRC { get; set; }

        public virtual EStatusProjecao StatusProjecao { get; set; }

        public virtual IEnumerable<ObraPagamento> ObraPagamentos
        {
            get
            {
                if (NumContrato != null && NumContrato > 0)
                    return ContratoPagamentos ?? (IEnumerable<ObraPagamento>)PropostaPagamentos;
                else
                    return (IEnumerable<ObraPagamento>)PropostaPagamentos;
            }
        }

        public string getChaveToString()
        {
            return UsinaCodigo + "-" + Numero;
        }

        public string getChaveProposaToString()
        {
            return $"{UsinaCodigo}/{NumChamada}-{AnoChamada}";
        }

        //TODO: Analisar se este é o melhor approach
        public string Origem
        {
            get
            {

                string origem = "";

                if (Contrato != null)
                    origem = "CTR:" + NumContrato + "/" + AnoContrato;
                else if (Proposta != null)
                    origem = "TEL:" + NumChamada + "/" + AnoChamada;

                return origem;
            }

        }
        //TODO: Analisar se este é o melhor approach
        public string IntervenienteNome
        {
            get
            {
                string nome = "";

                if (Contrato != null && Contrato.Interveniente != null)
                    nome = Contrato.Interveniente.Nome;
                else if (Proposta != null && Proposta.Interveniente != null)
                    nome = Proposta.Interveniente.Nome;
                else
                    nome = Proposta?.IntervenienteRazao ?? "";
                return nome;
            }
        }

        public string VendedorNome
        {
            get
            {
                string nome = "";

                if (Contrato != null && Contrato.Vendedor != null)
                    nome = Contrato.Vendedor.Nome;
                else if (Proposta != null && Proposta.Vendedor != null)
                    nome = Proposta.Vendedor.Nome;

                return nome;
            }
        }

        public decimal CalcularValorConcreto()
        {
            return (decimal)Math.Round(ObraTracos?.Sum(t => (decimal)t.M3PrecoProposto * (decimal)t.M3Quantidade) ?? 0m, 2);
        }

        public decimal CalcularValorBomba(bool NaoConsideraTodasBombas = false)
        {
            var volumeBombeavel = (float)Math.Round(ObraTracos?.Where(t => t.SlumpCodigo >= 9)?.Sum(t => t.M3QuantidadeBombeada) ?? 0f, 1);

            Func<TObraBomba, float> calculoValorBomba =
                t => {
                    var result = t.ValorAdicionalTubulacao;

                    switch (t.TipoCalculo)
                    {
                        case EBombaM3CalculoTipo.TaxaMinimaOuExcedente:
                            result += (volumeBombeavel * t.M3PrecoProposto);
                            break;
                        case EBombaM3CalculoTipo.TaxaMinimaMaisExcedente:
                            result += t.TaxaMinimaPrecoProposto
                                + (t.M3PropostoAte > 0 && volumeBombeavel > t.M3PropostoAte ?
                                    (volumeBombeavel - t.M3PropostoAte) * t.M3PrecoProposto : 0f);
                            break;
                    }

                    if (t.BombaPropria && t.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca)
                    {
                        result += t.HoraTaxaMinimaPrecoProposto;
                    }

                    return result;
                };

            if (NaoConsideraTodasBombas)
            {
                List<TObraBomba> bombas = new List<TObraBomba>();
                var primeiraBomba = ObraBombas?.FirstOrDefault();
                if (primeiraBomba != null)
                    bombas.Add(ObraBombas?.FirstOrDefault());

                if (bombas.Count > 0)
                    return (decimal)Math.Round(bombas?.Max(calculoValorBomba) ?? 0f, 2);

                return (decimal)Math.Round(0f, 2);
            }
            else
            {
                return (decimal)Math.Round(ObraBombas?.Sum(calculoValorBomba) ?? 0f, 2);
            }

        }

        public float CalcularValorDemaisServicos()
        {
            var valorVibrador = (VibradorValorUnitario * VibradorQuantidade);
            return (float)Math.Round((ObraDemaisServicos?.Sum(t => t.CalcularValorTotal(this)) ?? 0f) + valorVibrador, 2);
        }

        public bool PossuiBomba()
        {
            return (ObraBombas?.Count() ?? 0) > 0;
        }

        public void AprovarEngenharia()
        {
            this.StatusEngenharia = EObraStatusEngenharia.Aprovado;
        }

        public void AlteraStatusCadastro(EObraStatusCadastro novoStatus)
        {
            this.StatusCadastro = novoStatus;
        }

        public string GetEnderecoString()
        {
            return $"{EnderecoCep} {EnderecoLogradouro} {EnderecoNumero} {EnderecoComplemento}";
        }

        public bool AguardandoConfirmacaoContratoPagamento()
        {
            return (this.ContratoPagamentos?.Count ?? 0) > 0
                 && this.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao == "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
        }

        public bool AguardandoDadosPagamento()
        {
            return (this.ContratoPagamentos?.Count ?? 0) == 0
               // ou existe pelo menos uma condição antecipada que não é 'BL'(boleto) nem 'CT'(carteira) que não foi informado detalhamento
               || this.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && (t.Detalhes?.Count ?? 0) == 0 && t.Forma != "CT" && t.Forma != "BL").Count() > 0
               // ou existe pelo menos uma condição que foi reprovada
               || this.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "" && t.IdAprovacao == "").Count() > 0
               // ou o valor total dos pagamentos é menor que o valor total do contrato
               || Math.Round((float)this.Contrato.ValorTotalContrato - 10f, 2) > Math.Round(this.ContratoPagamentos.Sum(t => t.Valor), 2);
        }

        public void CalcularTempoDeCiclo(Usina usina)
        {
            var tempoAteObra = TempoAteAObra == 0 ? usina.TempoBtAteAObra : TempoAteAObra;
            var tempoBtNaObra = TempoBtNaObra == 0 ? usina.TempoBtNaObra : TempoBtNaObra;

            TempoCicloPrevisto = (int)(usina.PrazoPesagem + tempoBtNaObra + tempoAteObra + (tempoAteObra * (double)(usina.PorcentagemRetornoObra / 100)));
        }

        public void CalcularCustoProjetadoTransporte(Usina usina, CustoServico custoServico)
        {
            if (custoServico == null || usina == null) return;

            this.CalcularTempoDeCiclo(usina);
            var volumeTracos = this.ObraTracos.Sum(t => t.M3Quantidade);
            var volumeMedio = VolumePorCarga == 0 ? 8 : VolumePorCarga;
            var volumeTotal = (float)Math.Ceiling(volumeTracos / volumeMedio);

            CustoProjetadoTransporte = (custoServico.CustoMedioHoraTransporte * ((float)TempoCicloPrevisto / 60f) * volumeTotal) / volumeTracos;
        }


        public void AtualizarStatusAprovacao(string usuario)
        {
            foreach (var traco in ObraTracos ?? new List<TObraTraco>())
            {
                traco.AtualizaStatusAprovacao(usuario);
            }

            foreach (var bomba in ObraBombas ?? new List<TObraBomba>())
            {
                bomba.AtualizaStatusAprovacao(usuario);
            }
        }
    }
}
