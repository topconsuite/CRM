using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IContasAReceberApplicationService
    {
        void GeraContasAReceberDaOperadora(string transactionId, string usuario = "AUTO");
        void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra);
        void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO");
        void DesaprovarCondicaoPagamento(int contratoUsina, int contratoAno, int contratoNumero,
            int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = false);
        void GerarContasAReceberOperadoraEAprovarPagamentoAtencipadoCartaoDeCredito(string transcaoId, int usina, int obra, int sequenciaPagamento, string usuario = "AUTO");
    }
}
