using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;

namespace TopSys.TopConWeb.Application.DTOS.Response.Visita
{
    public class VisitaResponse : IHasEnderecoDTO
    {

        public UsinaDTO Usina { get; set; }
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }
        public VisitaTipoDTO TipoVisita { get; set; }
        public int VisitaTipoCodigo { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraVisita { get; set; }
        public string Cliente { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public VendedorDTO Vendedor { get; set; }
        public int VendedorCodigo { get; set; }
        public string ObraNome { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public string ObservacaoInterna { get; set; }
        public VisitaContatoDTO ContatoPrincipal { get; set; }
        public VisitaContatoDTO ContatoSecundario { get; set; }
        public ICollection<VisitaLogDTO> Logs { get; set; }
        public int LeadNumero { get; set; }
        public int LeadAno { get; set; }

    }
}
