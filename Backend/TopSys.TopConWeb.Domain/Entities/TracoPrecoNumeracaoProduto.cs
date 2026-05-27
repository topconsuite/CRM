namespace TopSys.TopConWeb.Domain.Entities
{
    public class TracoPrecoNumeracaoProduto
    {
        public int Numeracao { get; set; }
        public string UsoDescricao { get; set; }
        public int? UsinaBase { get; set; }
        public int? IdSegmentacao { get; set; }
        public int Status { get; set; }
    }
}
