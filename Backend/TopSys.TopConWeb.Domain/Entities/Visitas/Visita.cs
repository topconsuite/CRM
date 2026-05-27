using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities.Visitas
{
    public class Visita : IHasEndereco
    {
        public Usina Usina { get; set; }
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }
        public VisitaTipo TipoVisita { get; set; }
        public int VisitaTipoCodigo { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraVisita { get; set; }
        public string Cliente { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public Vendedor Vendedor { get; set; }
        public int VendedorCodigo { get; set; }
        public string ObraNome { get; set; }
        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public Municipio EnderecoMunicipio { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; }
        public string ObservacaoInterna { get; set; }
        public VisitaContato ContatoPrincipal { get; set; }
        public VisitaContato ContatoSecundario { get; set; }
        public ICollection<VisitaLog> Logs { get; set; }

        public int LeadNumero { get; set; }
        public int LeadAno { get; set; }

    }
}
