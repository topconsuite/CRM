using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Municipio
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Uf { get; set; }

        public int Pais { get; set; }

        public int IbgeCodigo { get; set; }

        public float AliquotaIss { get; set; }

        public EBaseCalculoIss BaseCalculo { get; set; }

        public string TributacaoIntegralBomba { get; set; }

        public bool BombaTribuatadaIntegralmente
        {
            get { return TributacaoIntegralBomba == "S"; }
        }

        public string TributacaoIntegralDemaisServicos { get; set; }

        public bool DemaisServicosTributadoIntegralmente
        {
            get { return TributacaoIntegralDemaisServicos == "S"; }
        }

        public float PorcentagemServico { get; set; }

        public float PorcentagemDeducaoMaterial { get; set; }

        public string NomeReduzido { get; set; }

        public int TributacaoIss { get; set; }

        public float PercentualIssRetido { get; set; }

        public string IssRetido { get; set; }

        public int IntervPrefeituraRetencao { get; set; }

        public int CodigoSiafi { get; set; }

        public string MensagemFiscal { get; set; }

        public string IdExterno { get; set; }

        public string TaxasTributadasIntegralmente { get; set; }

        public float ValorMinimoRetencaoIss { get; set; }
    }
}
