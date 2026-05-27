using System;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ContasAReceberService : IContasAReceberService
    {
        private readonly IContasAReceberRepository _contasAReceberRepository;
        private readonly ICartaoBandeiraRepository _cartaoBandeiraRepository;
        private readonly IObraRepository _obraRepository;
        private readonly IParametroFinanceiroRepository _parametroFinanceiroRepository;
        private readonly ICartaoTransacaoRepository _cartaoTransacaoRepository;
        private readonly ITipoCobrancaRepository _tipoCobrancaRepository;
        private readonly IContratoPagamentoRepository _contratoPagamentoRepository;

        public ContasAReceberService(IContasAReceberRepository contasAReceberRepository, ICartaoBandeiraRepository cartaoBandeiraRepository, IObraRepository obraRepository, IParametroFinanceiroRepository parametroFinanceiroRepository, ICartaoTransacaoRepository cartaoTransacaoRepository, ITipoCobrancaRepository tipoCobrancaRepository, IContratoPagamentoRepository contratoPagamentoRepository)
        {
            _contasAReceberRepository = contasAReceberRepository;
            _cartaoBandeiraRepository = cartaoBandeiraRepository;
            _obraRepository = obraRepository;
            _parametroFinanceiroRepository = parametroFinanceiroRepository;
            _cartaoTransacaoRepository = cartaoTransacaoRepository;
            _tipoCobrancaRepository = tipoCobrancaRepository;
            _contratoPagamentoRepository = contratoPagamentoRepository;
        }

        public void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra)
        {

            var obra = _obraRepository.ListarFiltradosTracking(t => t.UsinaCodigo == usina
                && t.Numero == numeroObra, t => t.Contrato, t=> t.Proposta, t => t.UsinaEntrega).FirstOrDefault();

            obra.ContratoPagamentos = _contratoPagamentoRepository
                .ListarContratoPagamentosDetalhados(obra.UsinaCodigo, obra.AnoContrato ?? 0, obra.NumContrato ?? 0, true)
                .ToList();

            if (!obra.AprovaPagamentoAntecipadoCartaoDeCreditoIsValid())
                return;

            var contratoPagamentosCartao = obra.ContratoPagamentos.Where(t =>
                (t.Forma == ("CC") || t.Forma == ("CD"))
                && t.IdAprovacao == ""
                && t.NecessitaAprovacaoSimNao == "S");

            foreach (var contratoPagamentoCartao in contratoPagamentosCartao)
            {
                var detalhePagamentoCartao = contratoPagamentoCartao.Detalhes.OfType<ContratoPagamentoDetalheCartao>().FirstOrDefault();
                if (!detalhePagamentoCartao.EncontrarContratoPagamentoDetalheCartaoScopeIsValid())
                    return;

                var contasAReceberDaOperadora = _contasAReceberRepository
                    .ListarContasAReceberPeloNumeroCartaoAutorizacaoEDataTransacao(detalhePagamentoCartao.NumeroCartaoAsString,
                    detalhePagamentoCartao.NumeroAutorizacao, detalhePagamentoCartao.DataTransacao);

                contasAReceberDaOperadora = contasAReceberDaOperadora.Where(t => t.Alocado != (int)EContasAReceberStatusAlocado.Vinculado);

                if (!contasAReceberDaOperadora.VerificarContasAReceberDaOperadoraParaGerarContasAReceberCliente(detalhePagamentoCartao))
                    return;

                var tipoCobranca = _tipoCobrancaRepository.ListarFiltrados(t => t.Codigo == contratoPagamentoCartao.TipoCobrancaCodigo).FirstOrDefault();
                if (!tipoCobranca.EncontrarTipoCobrancaScopeIsValid())
                    return;

                var operacaoRecebimentoCliente = _parametroFinanceiroRepository.ObterOperacaoRecebimentoDoClientePeloCodigoUsina(obra.UsinaEntrega.Codigo);

                var contasAReceberCliente = new ContasAReceber(obra, contratoPagamentoCartao, detalhePagamentoCartao, tipoCobranca, operacaoRecebimentoCliente);

                _contasAReceberRepository.InsereContasAReceber(contasAReceberCliente);

                foreach (var contaAReceber in contasAReceberDaOperadora)
                {
                    contaAReceber.Alocado = (int)EContasAReceberStatusAlocado.Vinculado;
                    _contasAReceberRepository.AtualizarAlocadoContasAReceber(contaAReceber);
                }

                //TODO: MODIFICAR PARA DAR COMMIT PELA ENTIDADE - REMOVER ESSE MÉTODO.
                contratoPagamentoCartao.IdAprovacao = $"AUTO - {DateTime.Now.ToString("dd/MM/y")}";
                _contratoPagamentoRepository.AtualizarIdAprovacao(contratoPagamentoCartao);

            }
            return;
        }


        public void AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO")
        {

            var obra = _obraRepository.ListarFiltradosTracking(t => t.UsinaCodigo == usina
                && t.Numero == numeroObra, t => t.Contrato, t => t.Proposta, t => t.UsinaEntrega).FirstOrDefault();

            obra.ContratoPagamentos = _contratoPagamentoRepository
                .ListarContratoPagamentosDetalhados(obra.UsinaCodigo, obra.AnoContrato ?? 0, obra.NumContrato ?? 0, true)
                .ToList();

            if (!obra.AprovaPagamentoAntecipadoCartaoDeCreditoIsValid())
                return;

            var contratoPagamento = obra.ContratoPagamentos.Where(t =>
                (t.Forma == ("CC") || t.Forma == ("CD"))
                && t.IdAprovacao == ""
                && t.NecessitaAprovacaoSimNao == "S"
                && t.Sequencia == sequenciaPagamento).FirstOrDefault();

            if (!contratoPagamento.AprovaPagamentoAntecipadoCartaoIsValid())
                return;

            var detalhePagamentoCartao = contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheCartao>().FirstOrDefault();
            if (!detalhePagamentoCartao.EncontrarContratoPagamentoDetalheCartaoScopeIsValid())
                return;

            var contasAReceberDaOperadora = _contasAReceberRepository
                .ListarContasAReceberPeloNumeroCartaoAutorizacaoEDataTransacao(detalhePagamentoCartao.NumeroCartaoAsString,
                detalhePagamentoCartao.NumeroAutorizacao, detalhePagamentoCartao.DataTransacao);

            contasAReceberDaOperadora = contasAReceberDaOperadora.Where(t => t.Alocado != (int)EContasAReceberStatusAlocado.Vinculado);

            if (!contasAReceberDaOperadora.VerificarContasAReceberDaOperadoraParaGerarContasAReceberCliente(detalhePagamentoCartao))
                return;

            var tipoCobranca = _tipoCobrancaRepository.ListarFiltrados(t => t.Codigo == contratoPagamento.TipoCobrancaCodigo).FirstOrDefault();
            if (!tipoCobranca.EncontrarTipoCobrancaScopeIsValid())
                return;

            var operacaoRecebimentoCliente = _parametroFinanceiroRepository.ObterOperacaoRecebimentoDoClientePeloCodigoUsina(obra.UsinaEntrega.Codigo);

            var contasAReceberCliente = new ContasAReceber(obra, contratoPagamento, detalhePagamentoCartao, tipoCobranca, operacaoRecebimentoCliente, usuario);

            _contasAReceberRepository.InsereContasAReceber(contasAReceberCliente);

            foreach (var contaAReceber in contasAReceberDaOperadora)
            {
                contaAReceber.Alocado = (int)EContasAReceberStatusAlocado.Vinculado;
                _contasAReceberRepository.AtualizarAlocadoContasAReceber(contaAReceber);
            }

            contratoPagamento.IdAprovacao = StringHelper.GetIDD(usuario);
            _contratoPagamentoRepository.AtualizarIdAprovacao(contratoPagamento);


            return;
        }

        public void AprovaPagamentoAntecipadoCartaoDeCredito(int numVersao, int usina, int numeroObra, int sequenciaPagamento, string usuario = "AUTO")
        {

            var obra = _obraRepository.ListarFiltradosTracking<ObraVersao>(t => t.NumeroVersao == numVersao && t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                && t.Numero == numeroObra, t => t.Contrato, t => t.Proposta, t => t.UsinaEntrega).FirstOrDefault();

            obra.ContratoPagamentos = _contratoPagamentoRepository
                .ListarContratoPagamentosDetalhados(numVersao, obra.UsinaCodigo, obra.AnoContrato ?? 0, obra.NumContrato ?? 0, true)
                .ToList();
            
            if (!obra.AprovaPagamentoAntecipadoCartaoDeCreditoIsValid())
                return;

            var contratoPagamento = obra.ContratoPagamentos.Where(t =>
                (t.Forma == ("CC") || t.Forma == ("CD"))
                && t.IdAprovacao == ""
                && t.NecessitaAprovacaoSimNao == "S"
                && t.Sequencia == sequenciaPagamento).FirstOrDefault();

            if (!contratoPagamento.AprovaPagamentoAntecipadoCartaoIsValid())
                return;

            var detalhePagamentoCartao = contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheCartaoVersao>().FirstOrDefault();
            if (!detalhePagamentoCartao.EncontrarContratoPagamentoDetalheCartaoScopeIsValid())
                return;

            var contasAReceberDaOperadora = _contasAReceberRepository
                .ListarContasAReceberPeloNumeroCartaoAutorizacaoEDataTransacao(detalhePagamentoCartao.NumeroCartaoAsString,
                detalhePagamentoCartao.NumeroAutorizacao, detalhePagamentoCartao.DataTransacao);

            contasAReceberDaOperadora = contasAReceberDaOperadora.Where(t => t.Alocado != (int)EContasAReceberStatusAlocado.Vinculado);

            if (!contasAReceberDaOperadora.VerificarContasAReceberDaOperadoraParaGerarContasAReceberCliente(detalhePagamentoCartao))
                return;

            var tipoCobranca = _tipoCobrancaRepository.ListarFiltrados(t => t.Codigo == contratoPagamento.TipoCobrancaCodigo).FirstOrDefault();
            if (!tipoCobranca.EncontrarTipoCobrancaScopeIsValid())
                return;

            var operacaoRecebimentoCliente = _parametroFinanceiroRepository.ObterOperacaoRecebimentoDoClientePeloCodigoUsina(obra.UsinaEntrega.Codigo);
            
            var contasAReceberCliente = new ContasAReceber(obra, contratoPagamento, detalhePagamentoCartao, tipoCobranca, operacaoRecebimentoCliente, usuario);

            _contasAReceberRepository.InsereContasAReceber(contasAReceberCliente);

            foreach (var contaAReceber in contasAReceberDaOperadora)
            {
                contaAReceber.Alocado = (int)EContasAReceberStatusAlocado.Vinculado;
                _contasAReceberRepository.AtualizarAlocadoContasAReceber(contaAReceber);
            }

            contratoPagamento.IdAprovacao = StringHelper.GetIDD(usuario);
            _contratoPagamentoRepository.AtualizarIdAprovacao(contratoPagamento);


            return;
        }

        public bool GeraContasAReceberDaOperadora(string transacaoId, string usuario = "AUTO")
        {

            var cartaoTransacao = _cartaoTransacaoRepository.ObterCartaoTransacaoPeloTransacaoId(transacaoId);
            if (!cartaoTransacao.CartaoTransacaoNaoProcessadoScopeIsValid())
                return false;

            var contasAReceber = _contasAReceberRepository.ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(cartaoTransacao.CartaoNumero, cartaoTransacao.AutorizacaoNumero, cartaoTransacao.TransacaoDataHora.Year);
            if (!contasAReceber.VerificarContasAReceberJaCriado())
            {
                _cartaoTransacaoRepository.AtualizaErroNaGeracaoContasAReceber(MensagensErro.ProcessoJaRealizado, transacaoId);
                return true;
            }

            var cartaoBandeira = _cartaoBandeiraRepository.ListarFiltrados(t => t.EstabelecimentoCod == cartaoTransacao.EstabelecimentoCod).FirstOrDefault();
            if (!cartaoBandeira.EncontrarCartaoBandeiraScopeIsValid())
            {
                _cartaoTransacaoRepository.AtualizaErroNaGeracaoContasAReceber(MensagensErro.BandeiraNaoEncontrada, transacaoId);
                return true;
            }

            var contratoPagamentoDetalheCartao = _contasAReceberRepository.ObterContratoPagamentoDetalheCartao(cartaoTransacao);
            //if (!contratoPagamentoDetalheCartao.EncontrarContratoPagamentoDetalheCartaoScopeIsValid())
            //{
            //    _cartaoTransacaoRepository.AtualizaErroNaGeracaoContasAReceber(MensagensErro.DetalhamentoNaoEncontrado, transacaoId);
            //    return true;
            //}
            Obra obra;
            if (contratoPagamentoDetalheCartao != null)
            {

                obra = _obraRepository.ListarFiltrados(t => t.UsinaCodigo == contratoPagamentoDetalheCartao.UsinaCodigo
               && t.Numero == contratoPagamentoDetalheCartao.ObraCodigo, t => t.Proposta, t => t.Contrato, t=> t.Contrato.Interveniente, t => t.UsinaEntrega, t => t.ContratoPagamentos, t => t.PropostaPagamentos).FirstOrDefault();
            }
            else
                obra = null;


            var operacaoCartao = _parametroFinanceiroRepository.ObterOperacaoCartaoPeloCodigoDaEmpresa(cartaoBandeira.EmpresaCod);


            if (cartaoTransacao.QuantidadeParcelas == 0)
                cartaoTransacao.QuantidadeParcelas = 1;

            for (int i = 1; i <= cartaoTransacao.QuantidadeParcelas; i++)
            {
                var contasAReceberDaOperadora = new ContasAReceber(obra, cartaoBandeira, cartaoTransacao, contratoPagamentoDetalheCartao, operacaoCartao, i, usuario);

                _contasAReceberRepository.InsereContasAReceber(contasAReceberDaOperadora);
            }

            _cartaoTransacaoRepository.AtualizaSucessoNaGeracaoContasAReceber(transacaoId);

            return true;
        }
    }

    internal static class MensagensErro
    {
        public const string ProcessoJaRealizado = "Ja existem contas a receber para este Cartao/Numero Autorizacao/Ano";
        public const string BandeiraNaoEncontrada = "Bandeira do cartao não encontrada";
        //public const string DetalhamentoNaoEncontrado = "Detalhamento não encontrado para este Cartao/Numero Autorizacao/Ano";
    }
}


