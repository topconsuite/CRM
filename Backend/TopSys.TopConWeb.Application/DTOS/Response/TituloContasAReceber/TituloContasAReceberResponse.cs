namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class TituloContasAReceberResponse
    {
        public int EmpresaCodigo { get; set; }
        public int DocumentoTipoCodigo { get; set; }
        public string DocumentoSerie { get; set; }
        public ulong DocumentoNumero { get; set; }
        public string DocumentoSequencia { get; set; }
        public int BancoCodigoOficial { get; set; }
        public int BancoNumeroAgencia { get; set; }
        public long BancoNumeroConta { get; set; }
        public int BancoDvConta { get; set; }
        public int Desdobramento { get; set; }
        public int IntervenienteCodigo { get; set; }
        public float Valor { get; set; }
        public int ContratoUsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public string CartaoNumero { get; set; }
        public string CartaoAutorizacao { get; set; }
    }
}
