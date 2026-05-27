namespace TopSys.TopConWeb.Domain.Entities
{
    public class TipoDeCobranca
    {
        public int Codigo { get; set; }
        
        public string Forma { get; set; }
        
        public int Portador { get; set; }
        
        public int Situacao { get; set; }
        
        public string Obrigatorio { get; set; } = "N";

        public string Aprovacao { get; set; } = "N";
        
        public string Fixo { get; set; } = "N";
        
        public string UtilCap { get; set; } = "N";

        public string Descricao { get; set; } = "";
    }
}