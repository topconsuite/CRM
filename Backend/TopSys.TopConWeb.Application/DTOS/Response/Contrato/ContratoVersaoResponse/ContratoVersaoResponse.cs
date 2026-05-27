using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato
{
    public class ContratoVersaoResponse : IHasEnderecoDTO
    {
        public int NumeroVersao { get; set; }
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public EContratoStatus Status { get; set; }
        public ObraTracoDTO ObraTraco { get; set; }
        public ObraBombaDTO ObraBomba { get; set; }
        public ObraTaxaDTO ObraTaxa { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public ObraDemaisServicosDTO DemaisServicos { get; set; }
        public Obra.ObraPendenteAprovacaoResponse.ContratoTracoReajusteDTO ContratoTracoReajustes { get; set; }
        public DateTime? DataVersaoCriada { get; set; }
    }
}
