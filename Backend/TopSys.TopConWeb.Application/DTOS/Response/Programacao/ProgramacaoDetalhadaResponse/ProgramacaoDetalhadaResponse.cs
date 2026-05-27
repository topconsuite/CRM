using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoDetalhadaResponse
{
    public class ProgramacaoDetalhadaResponse : IHasEnderecoDTO
    {
        public virtual UsinaDTO Usina { get; set; }

        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }

        public int Sequencia { get; set; }

        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public int ObraNumero { get; set; }

        public string ObraNome { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public PedraDTO Pedra { get; set; }

        public SlumpDTO Slump { get; set; }

        public string SlumpNotaFiscal { get; set; }

        public UsoDTO Uso { get; set; }

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

        public string HorarioBomba { get; set; }

        public ResistenciaTipo TracoPesadoResistenciaTipo { get; set; }

        public float TracoPesadoMpa { get; set; }

        public int TracoPesadoConsumo { get; set; }

        public PedraDTO TracoPesadoPedra { get; set; }

        public SlumpDTO TracoPesadoSlump { get; set; }

        public UsoDTO TracoPesadoUso { get; set; }

        public int VibradorQuantidade { get; set; }

        public float VibradorValorUnitario { get; set; }

        public float VibradorValorTotal { get; set; }

        public VendedorDTO VibradorVendedor { get; set; }

        public EProgramacaoStatus Status { get; set; }

        public string Solicitante { get; set; }

        public string Observacao { get; set; }

        public bool TemNotaFicalEmitida { get; set; }

        public string ObservacaoInterna { get; set; }

        public string CorpoDeProvaTipo { get; set; }
        public int CorpoDeProvaQuantidade { get; set; }
        public int CorpoDeProvaIntervalo { get; set; }
        public int CorpoDeProvaMoldador { get; set; }
        public string CorpoDeProvaMoldagemRemota { get; set; }

        public int ObraFrenteSequencia { get; set; }

        public ICollection<ProgramacaoDemaisServicosDTO> DemaisServicos { get; set; }

        public int TempoBtNaObra { get; set; }

        public int TempoAteAObra { get; set; }

        public int TempoDescarga { get; set; }
    }
}
