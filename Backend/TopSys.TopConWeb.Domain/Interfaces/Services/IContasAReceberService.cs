namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContasAReceberService
    {
        bool GeraContasAReceberDaOperadora(string transacaoId, string usuario = "AUTO");
        void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra);
        void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO");
        void AprovaPagamentoAntecipadoCartaoDeCredito(int numVersao, int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO");
    }
}
