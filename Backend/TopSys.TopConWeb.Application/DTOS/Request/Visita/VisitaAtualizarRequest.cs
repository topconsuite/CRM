using System;
using TopSys.TopConWeb.Application.DTOS.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.Visita
{
    public class VisitaAtualizarRequest
    {

        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }

        public int VisitaTipoCodigo { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraVisita { get; set; }
        public string Cliente { get; set; }
        public string ObraNome { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public int VendedorCodigo { get; set; }

        public EnderecoDTO Endereco { get; set; }
        public string ObservacaoInterna { get; set; }
        public VisitaContatoAtualizarRequest ContatoPrincipal { get; set; }
        public VisitaContatoAtualizarRequest ContatoSecundario { get; set; }

    }
}
