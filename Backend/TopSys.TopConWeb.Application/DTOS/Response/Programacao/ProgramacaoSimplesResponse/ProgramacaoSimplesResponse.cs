using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse
{
    public class ProgramacaoSimplesResponse : IHasEnderecoDTO
    {
        public UsinaDTO Usina { get; set; }

        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public int Sequencia { get; set; }

        public int PropostaAno { get; set; }

        public int PropostaNumero { get; set; }

        public int ObraNumero { get; set; }

        public string ObraNome { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public DateTime DataConcretagem { get; set; }

        public EProgramacaoStatus Status { get; set; }
        
        public EContratoStatus ContratoStatus { get; set; }

        public string Solicitante { get; set; }

        public string Horario { get; set; }

        public string NecessitaConfirmacao { get; set; }

        public float VolumeTotal { get; set; }

        public float VolumeEntregue { get; set; }

        public float VolumeLiberado { get; set; }

        public float VolumePorCarga { get; set; }

        public string Observacao { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public PropostaDTO Proposta { get; set; }

        public int ObraTracoSequencia { get; set; }
        public int ObraBombaSequencia { get; set; }

        public ResistenciaTipo ResistenciaTipo { get; set; }
        public UsoDTO Uso { get; set; }
        public PedraDTO Pedra { get; set; }
        public SlumpDTO Slump { get; set; }
        public float Mpa { get; set; }
        public int Consumo { get; set; }
        public string PecaConcretar { get; set; }

        public string CorpoDeProvaTipo { get; set; }
        public int CorpoDeProvaQuantidade { get; set; }
        public int CorpoDeProvaIntervalo { get; set; }
        public int CorpoDeProvaMoldador { get; set; }
        public string CorpoDeProvaMoldagemRemota { get; set; }

        public decimal ValorTotal { get; set; }
        public decimal ValorConcreto { get; set; }
        public decimal ValorExtras { get; set; }
        public decimal ValorBomba { get; set; }
        public decimal ValorDemaisServicos { get; set; }

        public decimal ValorTotalRemessasEmitidas { get; set; }
    }
}
