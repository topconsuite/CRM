using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.MotivoPerda;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Lead
{
    public class LeadResponse : IHasEnderecoDTO
    {
        public UsinaDTO Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public int VisitaNumero { get; set; }
        public int VisitaAno { get; set; }
        public int OportunidadeNumero { get; set; }
        public int OportunidadeAno { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public VendedorDTO Vendedor { get; set; }
        public CadastroGeralDTO ViaCaptacao { get; set; }
        public LeadFaseResponse Fase { get; set; }
        public EClassificacaoTemperatura Classificacao { get; set; }
        public string ProximaEtapa { get; set; }
        public string ObraNome { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public MotivoPerdaResponse MotivoPerda { get; set; }
        public string ObservacaoInterna { get; set; }
        public LeadContatoDTO ContatoPrincipal { get; set; }
        public LeadContatoDTO ContatoSecundario { get; set; }
    }
}
