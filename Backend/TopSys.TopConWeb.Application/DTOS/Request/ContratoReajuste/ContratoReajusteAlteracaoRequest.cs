using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.ContratoReajuste
{
    public class ContratoReajusteAlteracaoRequest
    {
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public DateTime DataVigencia { get; set; }
        public int Item { get; set; }
        public DateTime DataConfirmacao { get; set; }
        public string IdReprovacao { get; set; }
        public string IdAprovacaoVersao { get; set; }
    }
}
