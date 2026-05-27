namespace TopSys.TopConWeb.Domain.Entities
{
    public class TipoDocumento
    {
        public int Codigo { get; set; }
        
        public string Abreviacao { get; set; }
        
        public string Descricao { get; set; }

        public string Fixo { get; set; } = "N";

        public string ModDoc { get; set; } = "";

        public string Nfse { get; set; } = "N";
        
        public int TpDocServ { get; set; }
    }
}