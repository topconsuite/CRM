using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Segmentacao;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse
{
    public class PropostaSimplesResponse 
    {
        public UsinaDTO Usina;
        public int Ano;
        public int Numero;
        public DateTime Data;
        public VendedorDTO Vendedor;
        public IntervenienteDTO Interveniente;
        public ObraDTO Obra;

        public int IntervenienteCodigo { get; set; }

        public string IntervenienteRazao { get; set; }

        public int TelefoneComercialDdd { get; set; }
        public int TelefoneComercialNumero { get; set; }

        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }

        public int CelularDdd { get; set; }
        public int CelularNumero { get; set; }

        public string Contato { get; set; }

        public DateTime? DataEncerramentoContrato { get; set; }

        public bool IsContratoEncerrado { get; set; }

        public bool IsContratoFechado { get; set; }

        public EPropostaStatusCliente StatusProposta { get; set; }
        public EObraStatusComercial StatusComercial { get; set; }
        public EContratoStatus StatusContrato { get; set; }
        public PropostaResponsavelSolidario ResponsavelSolidario { get; set; }
        public SegmentacaoResponse Segmento { get; set; }

        public int AnoVisita { get; set; }

        public int NumeroVisita { get; set; }

        public int AnoLead { get; set; }

        public int NumeroLead { get; set; }

        public int AnoOportunidade { get; set; }

        public int NumeroOportunidade { get; set; }

        public EContratoFinalidade ContratoFinalidade { get; set; }
    }
}
