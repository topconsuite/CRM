using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.SolicitacaoPagamento
{
    public class SolicitacaoPagamentoRequest
    {
        public CartaoBandeiraDTO CartaoBandeira { get; set; }
        public float ValorTotal { get; set; }
        public string CpfCnpj { get; set; }
        public string IntervenienteRazao { get; set; }
        public string TipoCobranca { get; set; }
        public int QuantidadeParcelas { get; set; }
        public ObraDTO Obra { get; set; }
        public string Observacao { get; set; }
        public string Solicitante { get; set; }
    }

    public class CartaoBandeiraDTO
    {
        public int Codigo { get; set; }
        public string TipoIntegracao { get; set; }
    }

    public class ObraDTO
    {
        public int UsinaNumero { get; set; }
        public int Numero { get; set; }
        public int AnoContrato { get; set; }
        public int NumContrato { get; set; }
        public EnderecoDTO Endereco { get; set; }
    }
}
