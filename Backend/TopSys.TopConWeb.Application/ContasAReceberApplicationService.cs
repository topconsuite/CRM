using System.Linq;
using System.Transactions;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.Application
{
    public class ContasAReceberApplicationService : IContasAReceberApplicationService
    {
        private readonly IContasAReceberService _contasAReceberService;
        private readonly IObraService _obraService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IObraApplicationService _obraApplicationService;
        private readonly IContratoService _contratoService;
        protected IHandler<DomainNotification> _notifications;

        public ContasAReceberApplicationService(IContasAReceberService contasAReceberService, IObraService obraService, IComercialLegacyService comercialLegacyService, IObraApplicationService obraApplicationService, IContratoService contratoService)
        {
            _contasAReceberService = contasAReceberService;
            _comercialLegacyService = comercialLegacyService;
            _obraService = obraService;
            _notifications = DomainEvent.Container.GetService<IHandler<DomainNotification>>();
            _obraApplicationService = obraApplicationService;
            _contratoService = contratoService;
        }

        public void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra)
        {
            using (var scope = new TransactionScope())
            {
                _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(usina, numeroObra);
                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }

        public void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO")
        {
            using (var scope = new TransactionScope())
            {
                _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(usina, numeroObra, sequenciaPagamento, usuario);

                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }

        public void GerarContasAReceberOperadoraEAprovarPagamentoAtencipadoCartaoDeCredito(string transactionId, int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO")
        {
            using (var scope = new TransactionScope())
            {
                _contasAReceberService.GeraContasAReceberDaOperadora(transactionId);
                _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(usina, numeroObra, sequenciaPagamento, usuario);

                var obra = _obraService.ObterPorId(usina, numeroObra);
                _obraService.AtualizarStatusFinanceiro(obra, usuario);

                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }

        }

        public void DesaprovarCondicaoPagamento(int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = true)
        {
            using (var scope = new TransactionScope())
            {
                var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(contratoUsina, contratoAno, contratoNumero);
                if (versaoAtual == 0)
                {
                    _comercialLegacyService.DesaprovarCondicaoPagamento(contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, usuario, verificaMovimentoDeBancoConciliado);

                    var obra = _obraService.ListarFiltrados<Obra>(t => t.UsinaCodigo == contratoUsina && t.AnoContrato == contratoAno && t.NumContrato == contratoNumero).FirstOrDefault();

                    var statusFinanceiroAnterior = obra.StatusFinanceiro;
                    _obraService.AtualizarStatusFinanceiro(obra, usuario);

                    _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(obra.Numero, obra.UsinaCodigo, statusFinanceiroAnterior);
                }
                else
                {
                    _comercialLegacyService.DesaprovarCondicaoPagamento(versaoAtual, contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, usuario, verificaMovimentoDeBancoConciliado);

                    var obra = _obraService.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == versaoAtual && t.UsinaCodigo == contratoUsina && t.AnoContrato == contratoAno && t.NumContrato == contratoNumero).FirstOrDefault();

                    var statusFinanceiroAnterior = obra.StatusFinanceiro;
                    _obraService.AtualizarStatusFinanceiro(obra, usuario);

                    _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceiraVersao(obra.Numero, obra.UsinaCodigo, versaoAtual, statusFinanceiroAnterior);
                }

                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }

        public void GeraContasAReceberDaOperadora(string transactionId, string usuario = "AUTO")
        {
            using (var scope = new TransactionScope())
            {
                if (_contasAReceberService.GeraContasAReceberDaOperadora(transactionId))
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }
    }
}
