using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest
{
    public class MovimentoBancoDTO
    {
        public long Id { get; set; }
        public int EmpresaCodigo { get; set; }
        public int ContaCodigo { get; set; }
        public DateTime DataOperacao { get; set; }
        public int DocumentoTipo { get; set; }
        public string DocumentoNumero { get; set; }
        public string EntradaSaida { get; set; }
        public int OperacaoCodigo { get; set; }
        public string OperacaoDescricao { get; set; }
        public float Valor { get; set; }
        public float Saldo { get; set; }
        public string Origem { get; set; }
        public int CentroCustoCodigo { get; set; }
        public string IdCadastro { get; set; }
        public string Observacao { get; set; }
    }
}
