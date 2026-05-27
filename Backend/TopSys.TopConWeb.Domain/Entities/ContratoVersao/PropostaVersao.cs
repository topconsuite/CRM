namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaVersao : PropostaBase<PropostaVersao, ObraVersao, ContratoVersao, PropostaCobrancaVersao, PropostaFaturamentoVersao, PropostaResponsavelSolidarioVersao, ObraTracoVersao, ObraBombaVersao, ContratoTracoReajusteVersao, ContratoBombaReajusteVersao>
    {
        public int NumeroVersao { get; set; }
    }
}
