namespace TopSys.TopConWeb.Domain.Entities
{
    public class SolicitacaoPagamento
    {
        public long Id { get; set; }
        public int CartaoBandeiraCodigo { get; set; }
        public string IntegracaoId { get; set; }
        public float ValorTotal { get; set; }
        public string CpfCnpj { get; set; }
        public string IntervenienteRazao { get; set; }
        public string TipoCobranca { get; set; }
        public int QuantidadeParcelas { get; set; }
        public int ObraUsinaCodigo { get; set; }
        public int ObraNumero { get; set; }
        public int ObraAnoContrato { get; set; }
        public int ObraNumContrato { get; set; }
        public string ObraEnderecoCep { get; set; }
        public string ObraEnderecoLogradouro { get; set; }
        public int ObraEnderecoNumero { get; set; }
        public string ObraEnderecoComplemento { get; set; }
        public string ObraEnderecoBairro { get; set; }
        public virtual Municipio ObraEnderecoMunicipio { get; set; }
        public string Observacao { get; set; }
        public string Solicitante { get; set; }
    }
}
