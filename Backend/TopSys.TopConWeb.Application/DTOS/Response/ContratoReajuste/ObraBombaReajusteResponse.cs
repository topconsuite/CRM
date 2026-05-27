namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste
{
    public class ObraBombaReajusteResponse
    {
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public CadastroGeralDTO BombaTipo { get; set; }
        public int? BombaTipoCodigo { get; set; } = 0;
        public float ValorVigente { get; set; }
        public int VigenteAteM3 { get; set; }
        public float M3ExcedenteVigente { get; set; }
        public float ValorReajustado { get; set; }
        public int ReajustadoAteM3 { get; set; }
        public float M3ExcedenteReajustado { get; set; }
    }
}
