using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.WebHook;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.WebHook;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using Topsys.TopConWeb.SharedKernel.Helpers;
using System.Linq;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;

namespace TopSys.TopConWeb.Application
{
    public class WebHookApplicationService : IWebHookApplicationService
    {

        private readonly IWebHookService _webHookService;
        private readonly IMunicipioService _municipioService;
        private readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly IContratoPagamentoRepository _contratoPagamentoRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly IPropostaRepository _propostaRepository;

        public WebHookApplicationService(IWebHookService webHookService, IIntervenienteService intervenienteService, IMunicipioService municipioService, ICondicaoPagamentoService condicaoPagamentoService, IContratoPagamentoRepository contratoPagamentoRepository, IParametroRepository parametroRepository, IPropostaRepository propostaRepository)
        {
            _webHookService = webHookService;
            _municipioService = municipioService;
            _condicaoPagamentoService = condicaoPagamentoService;
            _intervenienteService = intervenienteService;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _parametroRepository = parametroRepository;
            _propostaRepository = propostaRepository;
        }

        public void Adicionar(WebHookDesktop webHook)
        {
            _webHookService.Adicionar(webHook);
        }

        public void AdicionarWebHookContratoPagamentoVersao(ContratoVersao contrato, List<ContratoPagamentoVersao> contratoPagamentos, EWebHookTipoEvento evento)
        {
            foreach (var contratoPagamento in contratoPagamentos)
            {
                if (contratoPagamento.CondicaoPagamento is null)
                    contratoPagamento.CondicaoPagamento = _condicaoPagamentoService.ObterPeloId(contratoPagamento.CondicaoPagamentoCodigo);

                if (contratoPagamento.Detalhes == null)
                    contratoPagamento.Detalhes = new List<ContratoPagamentoDetalheVersao>();
            }

            if (contrato.ValorTotalContrato == 0 || (contrato.IntervenienteCodigo ?? 0) == 0)
            {
                var ctr = _condicaoPagamentoService.ObterPorId<Contrato>(contrato.Usina, contrato.Ano, contrato.Numero);
                contrato.ValorTotalContrato = ctr.ValorTotalContrato;
                contrato.IntervenienteCodigo = ctr.IntervenienteCodigo;
                contrato.Interveniente = ctr.Interveniente;
            }

            var dtos = AutoMapper.Mapper.Map(contratoPagamentos.Where(t => t.Detalhes.Count > 0), new List<ContratoPagamentoWebHookDTO>());

            if (contrato.Interveniente is null)
                contrato.Interveniente = _intervenienteService.ObterPorId(contrato.IntervenienteCodigo ?? 0);

            var usinaEntrega = _condicaoPagamentoService.ObterPorId<Contrato>(contrato.Usina, contrato.Ano, contrato.Numero).UsinaEntrega;
            var externalIdUsina = _condicaoPagamentoService.ObterPorId<Usina>(usinaEntrega).ExternalId;

            var payload = new ContratoPagamentosWebHookDTO()
            {
                ContratoUsina = contrato.Usina,
                ContratoAno = contrato.Ano,
                ContratoNumero = contrato.Numero,
                ExternalIdUsina = externalIdUsina,
                Pagamentos = dtos,
                ValorTotal = contrato.ValorTotalContrato,
                Interveniente = new IntervenienteContratoPagamentoWebHookDTO()
                {
                    CpfCnpj = contrato.Interveniente.CpfCnpj,
                    Razao = contrato.Interveniente.Razao,
                    IdExterno = contrato.Interveniente.IdExterno
                }

            };

            if (payload.Pagamentos.Count > 0)
            {
                var webHook = new WebHookDesktop()
                {
                    Payload = PayloadHelper.ConvertToJson(payload),
                    Evento = $"contract-payments-{evento.ToString()}".ToLower(),
                    Url = $"",
                    FilePath = $"",
                    Headers = $"",
                    AlertEmails = $"",
                    DtSendAfter = DateTime.Now
                };

                _webHookService.Adicionar(webHook);
            }
        }

        public void AdicionarWebHookContratoPagamento(Contrato contrato, List<ContratoPagamento> contratoPagamentos, EWebHookTipoEvento evento)
        {
            
            foreach(var contratoPagamento in contratoPagamentos)
            {
                if (contratoPagamento.CondicaoPagamento is null)
                    contratoPagamento.CondicaoPagamento = _condicaoPagamentoService.ObterPeloId(contratoPagamento.CondicaoPagamentoCodigo);

                if (contratoPagamento.Detalhes == null)
                    contratoPagamento.Detalhes = new List<ContratoPagamentoDetalhe>();
            }

            if(contrato.ValorTotalContrato == 0 || (contrato.IntervenienteCodigo ?? 0) == 0)
            {
                var ctr = _condicaoPagamentoService.ObterPorId<Contrato>(contrato.Usina, contrato.Ano, contrato.Numero);
                contrato.ValorTotalContrato = ctr.ValorTotalContrato;
                contrato.IntervenienteCodigo = ctr.IntervenienteCodigo;
                contrato.Interveniente = ctr.Interveniente;
            }

            var dtos = AutoMapper.Mapper.Map(contratoPagamentos.Where(t => t.Detalhes.Count > 0), new List<ContratoPagamentoWebHookDTO>());

            if (contrato.Interveniente is null)
                contrato.Interveniente = _intervenienteService.ObterPorId(contrato.IntervenienteCodigo ?? 0);

            var usinaEntrega = _condicaoPagamentoService.ObterPorId<Contrato>(contrato.Usina, contrato.Ano, contrato.Numero).UsinaEntrega;
            var externalIdUsina = _condicaoPagamentoService.ObterPorId<Usina>(usinaEntrega).ExternalId;

            var payload = new ContratoPagamentosWebHookDTO()
            {
                ContratoUsina = contrato.Usina,
                ContratoAno = contrato.Ano,
                ContratoNumero = contrato.Numero,
                ExternalIdUsina = externalIdUsina,
                Pagamentos = dtos,
                ValorTotal = contrato.ValorTotalContrato,
                Interveniente = new IntervenienteContratoPagamentoWebHookDTO()
                {
                    CpfCnpj = contrato.Interveniente.CpfCnpj,
                    Razao = contrato.Interveniente.Razao,
                    IdExterno = contrato.Interveniente.IdExterno
                }

            };

            if (payload.Pagamentos.Count > 0)
            {
                var webHook = new WebHookDesktop()
                {
                    Payload = PayloadHelper.ConvertToJson(payload),
                    Evento = $"contract-payments-{evento.ToString()}".ToLower(),
                    Url = $"",
                    FilePath = $"",
                    Headers = $"",
                    AlertEmails = $"",
                    DtSendAfter = DateTime.Now
                };

                _webHookService.Adicionar(webHook);
            }
        }

        public void AdicionarWebHookInterveniente(Interveniente interveniente, EWebHookTipoEvento evento)
        {

            if(interveniente.EnderecoMunicipio is null)
                interveniente.EnderecoMunicipio = _municipioService.ObterPorId(interveniente.EnderecoMunicipioCodigo ?? 0);

            var payload = AutoMapper.Mapper.Map(interveniente, new IntervenienteWebHookDTO());

            var webHook = new WebHookDesktop()
            {
                Payload = PayloadHelper.ConvertToJson(payload),
                Evento = $"customer-{evento.ToString()}".ToLower(),
                Url = $"",
                FilePath = $"",
                Headers = $"",
                AlertEmails = $"",
                DtSendAfter = DateTime.Now
            };

            _webHookService.Adicionar(webHook);

        }

        public void AdicionarWebhookContratoAprovado(Contrato contrato)
        {
            var webHookUrl = _parametroRepository.ObterParametroIntegracoes("webhook", $"contract-approved");
            if (string.IsNullOrEmpty(webHookUrl))
                return;

            var obra = _intervenienteService.ListarFiltrados<Obra>(t => t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero, t => t.ContratoPagamentos, t => t.EnderecoMunicipio).FirstOrDefault();

            var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.Usina, contrato.Ano, contrato.Numero);
            var obraMunicipo = _municipioService.ListarFiltrados<Municipio>(x => x.Codigo == obra.EnderecoMunicipioCodigo).FirstOrDefault();

            if(obra.ObraTributacoesMunicipais is null)
                obra.ObraTributacoesMunicipais = _intervenienteService.ListarFiltrados<ObraTributacaoMunicipal>(t => t.ObraNumero == obra.Numero && t.ObraUsinaCodigo == obra.UsinaCodigo).ToList();

            if (contrato.Vendedor == null)
            {
                var vendedor = _intervenienteService.ListarFiltrados<Vendedor>(t => t.Codigo == contrato.VendedorCodigo).FirstOrDefault();

                contrato.Vendedor = vendedor;
            }   

            var vendedorInterv = _intervenienteService.ListarFiltrados(t => t.Codigo ==contrato.Vendedor.Interveniente).FirstOrDefault();
            if (vendedorInterv != null)
                contrato.Vendedor.CpfCnpj = vendedorInterv.CpfCnpj;

            obra.ContratoPagamentos = contratoPagamentos.ToList();
            obra.EnderecoMunicipio = obraMunicipo;

            obra.Proposta = _propostaRepository.ObterPorId(obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

            obra.Proposta.Cobranca = _intervenienteService.ObterPorId<PropostaCobranca>(obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);
            obra.Proposta.Faturamento = _intervenienteService.ObterPorId<PropostaFaturamento>(obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);

            if (obra.Proposta.Faturamento != null)
                obra.Proposta.Faturamento.EnderecoMunicipio = _municipioService.ObterPorId(obra.Proposta.Faturamento.EnderecoMunicipioCodigo);

            if (obra.Proposta.Cobranca != null)
                obra.Proposta.Cobranca.EnderecoMunicipio = _municipioService.ObterPorId(obra.Proposta.Cobranca.EnderecoMunicipioCodigo);

            obra.CondicaoPagamento = _condicaoPagamentoService.ObterPorId<CondicaoPagamento>(obra.CondicaoPagamentoCodigo);
            obra.TipoCobranca = _condicaoPagamentoService.ObterPorId<TipoCobranca>(obra.TipoCobrancaCodigo);

            var payload = ContratoAprovadoWebHookDTO.FromContrato(contrato, obra);

            var webHook = new WebHookDesktop()
            {
                Payload = PayloadHelper.ConvertToJson(payload),
                Evento = $"contract-approved",
                Url = $"",
                FilePath = $"",
                Headers = $"",
                AlertEmails = $"",
                DtSendAfter = DateTime.Now
            };

            _webHookService.Adicionar(webHook);

        }

        public void AdicionarWebhookContratoAprovadoVersao(ContratoVersao contratoTrackeado)
        {
            var webHookUrl = _parametroRepository.ObterParametroIntegracoes("webhook", $"contract-approved");
            if (string.IsNullOrEmpty(webHookUrl))
                return;

            var contrato = _intervenienteService.ListarFiltrados<ContratoVersao>(t => t.Usina == contratoTrackeado.Usina && t.Ano == contratoTrackeado.Ano && t.Numero == contratoTrackeado.Numero, t=> t.Interveniente).FirstOrDefault();
            var obra = _intervenienteService.ListarFiltrados<ObraVersao>(t => t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero, t => t.ContratoPagamentos, t => t.EnderecoMunicipio).FirstOrDefault();

            var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.NumeroVersao, contrato.Usina, contrato.Ano, contrato.Numero);
            var obraMunicipo = _municipioService.ListarFiltrados<Municipio>(x => x.Codigo == obra.EnderecoMunicipioCodigo).FirstOrDefault();

            if (obra.ObraTributacoesMunicipais is null)
                obra.ObraTributacoesMunicipais = _intervenienteService.ListarFiltrados<ObraTributacaoMunicipalVersao>(t => t.ObraNumero == obra.Numero && t.ObraUsinaCodigo == obra.UsinaCodigo).ToList();

            if (contrato.Vendedor == null)
            {
                var vendedor = _intervenienteService.ListarFiltrados<Vendedor>(t => t.Codigo == contrato.VendedorCodigo).FirstOrDefault();

                contrato.Vendedor = vendedor;
            }

            var vendedorInterv = _intervenienteService.ListarFiltrados(t => t.Codigo == contrato.Vendedor.Interveniente).FirstOrDefault();
            if (vendedorInterv != null)
                contrato.Vendedor.CpfCnpj = vendedorInterv.CpfCnpj;

            obra.ContratoPagamentos = contratoPagamentos.ToList();
            obra.EnderecoMunicipio = obraMunicipo;

            obra.Proposta = _propostaRepository.ObterPorId<PropostaVersao>(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

            obra.Proposta.Cobranca = _intervenienteService.ObterPorId<PropostaCobrancaVersao>(obra.NumeroVersao, obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);
            obra.Proposta.Faturamento = _intervenienteService.ObterPorId<PropostaFaturamentoVersao>(obra.NumeroVersao, obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);

            if (obra.Proposta.Faturamento != null)
                obra.Proposta.Faturamento.EnderecoMunicipio = _municipioService.ObterPorId(obra.Proposta.Faturamento.EnderecoMunicipioCodigo);

            if (obra.Proposta.Cobranca != null)
                obra.Proposta.Cobranca.EnderecoMunicipio = _municipioService.ObterPorId(obra.Proposta.Cobranca.EnderecoMunicipioCodigo);

            obra.CondicaoPagamento = _condicaoPagamentoService.ObterPorId<CondicaoPagamento>(obra.CondicaoPagamentoCodigo);
            obra.TipoCobranca = _condicaoPagamentoService.ObterPorId<TipoCobranca>(obra.TipoCobrancaCodigo);

            var payload = ContratoAprovadoWebHookDTO.FromContrato(contrato, obra);

            var webHook = new WebHookDesktop()
            {
                Payload = PayloadHelper.ConvertToJson(payload),
                Evento = $"contract-approved",
                Url = $"",
                FilePath = $"",
                Headers = $"",
                AlertEmails = $"",
                DtSendAfter = DateTime.Now
            };

            _webHookService.Adicionar(webHook);

        }

        public void AdicionarWebhookContratoPendenteAprovacaoFinanceira(Obra obra)
        {
            var webHookUrl = _parametroRepository.ObterParametroIntegracoes("webhook", $"contract-pending-financial-approval");
            if (string.IsNullOrEmpty(webHookUrl))
                return;

            var contrato = _intervenienteService.ListarFiltrados<Contrato>(t => t.Ano == obra.AnoContrato && t.Numero == obra.NumContrato && t.Usina == obra.UsinaCodigo, c => c.Vendedor, c => c.Interveniente).FirstOrDefault();
            if (obra.ObraTributacoesMunicipais is null)
                obra.ObraTributacoesMunicipais = _intervenienteService.ListarFiltrados<ObraTributacaoMunicipal>(x => x.ObraNumero == obra.Numero && x.ObraUsinaCodigo == obra.UsinaCodigo).ToList();

            var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.Usina, contrato.Ano, contrato.Numero);
            var obraMunicipo = _intervenienteService.ListarFiltrados<Municipio>(x => x.Codigo == obra.EnderecoMunicipioCodigo).FirstOrDefault();

            var vendedor = _intervenienteService.ListarFiltrados<Vendedor>(t => t.Codigo == contrato.VendedorCodigo).FirstOrDefault();
            if (contrato.Vendedor != null)
                contrato.Vendedor = vendedor;

            if(contrato.Vendedor != null)
            {
                var vendedorInterv = _intervenienteService.ObterPorId(contrato.Vendedor.Interveniente);
                if (vendedorInterv != null)
                    contrato.Vendedor.CpfCnpj = vendedorInterv.CpfCnpj;
            }

            obra.ContratoPagamentos = contratoPagamentos.ToList();
            obra.EnderecoMunicipio = obraMunicipo;

            obra.Proposta = _propostaRepository.ObterPorId(obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

            obra.Proposta.Cobranca = _intervenienteService.ObterPorId<PropostaCobranca>(obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);
            obra.Proposta.Faturamento = _intervenienteService.ObterPorId<PropostaFaturamento>(obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);

            obra.CondicaoPagamento = _condicaoPagamentoService.ObterPorId<CondicaoPagamento>(obra.CondicaoPagamentoCodigo);
            obra.TipoCobranca = _condicaoPagamentoService.ObterPorId<TipoCobranca>(obra.TipoCobrancaCodigo);

            var payload = ContratoPendenteAprovacaoFinanceiraWebHookDTO.FromContrato(contrato, obra);

            var webHook = new WebHookDesktop()
            {
                Payload = PayloadHelper.ConvertToJson(payload),
                Evento = $"contract-pending-financial-approval",
                Url = $"",
                FilePath = $"",
                Headers = $"",
                AlertEmails = $"",
                DtSendAfter = DateTime.Now
            };

            _webHookService.Adicionar(webHook);
        }

        public void AdicionarWebhookContratoPendenteAprovacaoFinanceiraVersao(ObraVersao obra)
        {
            var webHookUrl = _parametroRepository.ObterParametroIntegracoes("webhook", $"contract-pending-financial-approval");
            if (string.IsNullOrEmpty(webHookUrl))
                return;

            var contrato = _intervenienteService.ListarFiltrados<ContratoVersao>(t => t.Ano == obra.AnoContrato && t.Numero == obra.NumContrato && t.Usina == obra.UsinaCodigo && t.NumeroVersao == obra.NumeroVersao, c => c.Vendedor, c => c.Interveniente).FirstOrDefault();
            if (obra.ObraTributacoesMunicipais is null)
                obra.ObraTributacoesMunicipais = _intervenienteService.ListarFiltrados<ObraTributacaoMunicipalVersao>(x => x.ObraNumero == obra.Numero && x.ObraUsinaCodigo == obra.UsinaCodigo).ToList();

            var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.NumeroVersao,contrato.Usina, contrato.Ano, contrato.Numero);
            var obraMunicipo = _intervenienteService.ListarFiltrados<Municipio>(x => x.Codigo == obra.EnderecoMunicipioCodigo).FirstOrDefault();

            var vendedor = _intervenienteService.ListarFiltrados<Vendedor>(t => t.Codigo == contrato.VendedorCodigo).FirstOrDefault();
            if (contrato.Vendedor != null)
                contrato.Vendedor = vendedor;

            var vendedorInterv = _intervenienteService.ObterPorId(contrato.Vendedor.Interveniente);
            if (vendedorInterv != null)
                contrato.Vendedor.CpfCnpj = vendedorInterv.CpfCnpj;

            obra.ContratoPagamentos = contratoPagamentos.ToList();
            obra.EnderecoMunicipio = obraMunicipo;

            obra.Proposta = _propostaRepository.ObterPorId<PropostaVersao>(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

            obra.Proposta.Cobranca = _intervenienteService.ObterPorId<PropostaCobrancaVersao>(obra.NumeroVersao, obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);
            obra.Proposta.Faturamento = _intervenienteService.ObterPorId<PropostaFaturamentoVersao>(obra.NumeroVersao, obra.Proposta.UsinaCodigo, obra.Proposta.Ano, obra.Proposta.Numero);

            obra.CondicaoPagamento = _condicaoPagamentoService.ObterPorId<CondicaoPagamento>(obra.CondicaoPagamentoCodigo);
            obra.TipoCobranca = _condicaoPagamentoService.ObterPorId<TipoCobranca>(obra.TipoCobrancaCodigo);

            var payload = ContratoPendenteAprovacaoFinanceiraWebHookDTO.FromContratoVersao(contrato, obra);

            var webHook = new WebHookDesktop()
            {
                Payload = PayloadHelper.ConvertToJson(payload),
                Evento = $"contract-pending-financial-approval",
                Url = $"",
                FilePath = $"",
                Headers = $"",
                AlertEmails = $"",
                DtSendAfter = DateTime.Now
            };

            _webHookService.Adicionar(webHook);
        }
    }
}
