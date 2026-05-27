using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Programacao : IHasEndereco
    {
        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }
        
        public int? ContratoAno { get; set; } = 0;
        public int? ContratoNumero { get; set; } = 0;
        public virtual Contrato Contrato { get; set; }

        public int Sequencia { get; set; }
        
        public int? PropostaAno { get; set; } = 0;
        public int? PropostaNumero { get; set; } = 0;
        public virtual Proposta Proposta { get; set; }

        public int ObraNumero { get; set; }
        
        public string ObraNome { get; set; }
        
        public int UsinaEntregaCodigo { get; set; }
        public virtual Usina UsinaEntrega { get; set; }
        
        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public virtual Municipio EnderecoMunicipio { get; set; }
        
        public int? ResistenciaTipoCodigo { get; set; } = 0;
        public virtual ResistenciaTipo ResistenciaTipo { get; set; }
        
        public float Mpa { get; set; }
        
        public int Consumo { get; set; }
        
        public int? PedraCodigo { get; set; } = 0;
        public virtual Pedra Pedra { get; set; }
        
        public int? SlumpCodigo { get; set; } = 0;
        public virtual Slump Slump { get; set; }

        public string SlumpNotaFiscal { get; set; }

        public int? UsoCodigo { get; set; } = 0;
        public virtual Uso Uso { get; set; }

        public string PecaConcretar { get; set; }

        public string Andar { get; set; }

        public int ObraTracoSequencia { get; set; }
        
        public float VolumeTotal { get; set; }
        
        public float VolumeEntregue { get; set; }
        
        public float VolumePorCarga { get; set; }
        
        public DateTime DataConcretagem { get; set; }
        
        public string Horario { get; set; }

        public string NecessitaConfirmacao { get; set; }

        public float VolumeLiberado { get; set; }

        public int IntervaloEmMinutosEntreCargas { get; set; }
        
        public int ObraBombaSequencia { get; set; }

        public int DistanciaTubulacao { get; set; }
        public float ValorAdicionalTubulacao { get; set; }

        public string HorarioBomba { get; set; }

        public string EquipamentoBombaCodigo { get; set; } = "";

        public int? TracoPesadoResistenciaTipoCodigo { get; set; } = 0;
        public virtual ResistenciaTipo TracoPesadoResistenciaTipo { get; set; }

        public float TracoPesadoMpa { get; set; }

        public int TracoPesadoConsumo { get; set; }

        public int? TracoPesadoPedraCodigo { get; set; } = 0;
        public virtual Pedra TracoPesadoPedra { get; set; }

        public int? TracoPesadoSlumpCodigo { get; set; } = 0;
        public virtual Slump TracoPesadoSlump { get; set; }

        public int? TracoPesadoUsoCodigo { get; set; } = 0;
        public virtual Uso TracoPesadoUso { get; set; }

        public int VibradorQuantidade { get; set; }

        public float VibradorValorUnitario { get; set; }

        public float VibradorValorTotal { get; set; }

        public int? VibradorVendedorCodigo { get; set; } = 0;
        public virtual Vendedor VibradorVendedor { get; set; }

        public EProgramacaoStatus Status { get; set; }

        public string Solicitante { get; set; }

        public string Observacao { get; set; }

        public bool TemNotaFicalEmitida { get; set; }

        public string FaxSimNao { get; set; }

        public virtual ICollection<ProgramacaoDemaisServicos> DemaisServicos { get; set; }

        public int ContinuidadeDaSequencia { get; set; }

        public string ObservacaoInterna { get; set; }

        public string DataHoraSolicitacao { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtual { get; set; }

        public string CorpoDeProvaTipo { get; set; }
        public int CorpoDeProvaQuantidade { get; set; }
        public int CorpoDeProvaIntervalo { get; set; }
        public int CorpoDeProvaMoldador { get; set; }
        public string CorpoDeProvaMoldagemRemota { get; set; }

        public int ObraFrenteSequencia { get; set; }
        
        public decimal ValorTotal { get; set; }
        public decimal ValorConcreto { get; set; }
        public decimal ValorExtras { get; set; }
        public decimal ValorBomba { get; set; }
        public decimal ValorDemaisServicos { get; set; }
        public string ExternalId { get; set; }

        public int TempoBtNaObra { get; set; }

        public int TempoAteAObra { get; set; }

        public int TempoDescarga { get; set; }

        public decimal ValorTotalRemessasEmitidas { get; set; }
        public ECanceladoPor CanceladoPor { get; set; }

        public bool PossuiEndereco()
        {
           return EnderecoLogradouro == "" || (EnderecoNumero == 0 && EnderecoComplemento == "") || EnderecoCep == "";
        }

        public void setValorTotalRemessasEmitidas(decimal valorTotalRemessas)
        {
            ValorTotalRemessasEmitidas = valorTotalRemessas;
        }
    }
}
