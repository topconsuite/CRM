namespace TopSys.TopConWeb.Domain.Entities
{
    public class ChaveTituloContasAReceber
    {
        public int EmpresaCodigo { get; set; }
        public int DocumentoTipoCodigo { get; set; }
        public string DocumentoSerie { get; set; }
        public long DocumentoNumero { get; set; }
        public string DocumentoSequencia { get; set; }
        public int BancoCodigoOficial { get; set; }
        public int BancoNumeroAgencia { get; set; }
        public long BancoNumeroConta { get; set; }
        public byte BancoDvConta { get; set; }
        public int Desdobramento { get; set; }
    }
}