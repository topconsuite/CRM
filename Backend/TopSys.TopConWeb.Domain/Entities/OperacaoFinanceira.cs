namespace TopSys.TopConWeb.Domain.Entities
{
    public class OperacaoFinanceira
    {
        public int Codigo { get; set; }
        
        public string Descricao { get; set; }
        
        public string SubSistema { get; set; }
        
        public string InclusaoOuBaixa { get; set; }
        
        public string SemMovFinanceiro { get; set; } = "N";
        
        public int AtualizaBanco { get; set; }

        public int OperacaoBaixa { get; set; } = 0;
        
        public int OperacaoMovBco { get; set; } = 0;

        public int ReceitaDespesa { get; set; } = 9;
        
        public string CentrosDeCusto  { get; set; }
    }
}