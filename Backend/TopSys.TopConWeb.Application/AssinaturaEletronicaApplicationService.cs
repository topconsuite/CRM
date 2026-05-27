using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Application.DTOS.Response.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class AssinaturaEletronicaApplicationService : IAssinaturaEletronicaApplicationService
    {

        private readonly IClicksignRepository _clicksignRepository;
        private readonly ReportService _reportService;
        private readonly IAssinaturaEletronicaIntegrationService _assinaturaEletronicaIntegrationService;
        private readonly IObraRepository _obraRepository;

        public AssinaturaEletronicaApplicationService(
            IClicksignRepository clicksignRepository,
            ReportService reportService,
            IAssinaturaEletronicaIntegrationService assinaturaEletronicaIntegrationService,
            IObraRepository obraRepository)
        {
            _clicksignRepository = clicksignRepository;
            _reportService = reportService;
            _assinaturaEletronicaIntegrationService = assinaturaEletronicaIntegrationService;
            _obraRepository = obraRepository;
        }

        public void AtualizarParametrosClicksign(ClicksignParametrosRequest clicksignParametro)
        {
            var clicksignParametroEntity = AutoMapper.Mapper.Map(clicksignParametro, new ClicksignParametros());
            _clicksignRepository.AtualizarParametros(clicksignParametroEntity);

            return;
        }

        public void CancelarDocumentoClicksign(Guid idDocumento)
        {
            var documento = _clicksignRepository.ObterContratoClicksignEnvio(idDocumento);

            if (documento != null)
            {
                // Hierarquia: Contrato -> Obra -> UsinaEntrega -> ClicksignConfiguracao
                var obra = _obraRepository.ObterObraPorContrato(
                    documento.ContratonUsina,
                    documento.ContratoAno,
                    documento.ContratoNumero);

                _assinaturaEletronicaIntegrationService.ConfigurarContextoUsina(obra?.UsinaEntregaCodigo);
                _assinaturaEletronicaIntegrationService.RequestDocumentCancelClicksign(documento.IdClicksign);
            }

            _clicksignRepository.AtualizarDataCancelamento(documento.Id, DateTime.Now);
        }

        public IEnumerable<ContratoClicksignEnvioDTO> ListarContratosClicksignEnvios(int usina, int anoContrato, int numeroContrato)
        {
            return AutoMapper.Mapper.Map(_clicksignRepository.ListarContratoClicksignEnvios(usina, anoContrato, numeroContrato), new List<ContratoClicksignEnvioDTO>());

        }

        public ClicksignParametrosResponse ObterParametrosClicksign()
        {
            return AutoMapper.Mapper.Map(_clicksignRepository.ObterParametros(), new ClicksignParametrosResponse());
        }

        public ContratoClicksignEnvioDTO ObterUltimoContratoClicksignEnvio(int usina, int anoContrato, int numeroContrato)
        {
            return AutoMapper.Mapper.Map(_clicksignRepository.ObterUltimoContratoClicksignEnvio(usina, anoContrato, numeroContrato), new ContratoClicksignEnvioDTO());
        }

        public void ProcessarEventoClicksign(ClicksignEvento clicksignEvento)
        {
            var nomeEvento = clicksignEvento.Event.Name;

            if (nomeEvento.Contains("auto_close"))
            {
                // auto_close: contacted encerrado com todas as assinaturas. Data em occurred_at.
                _clicksignRepository.AtualizarDataAssinatura(
                    clicksignEvento.Document.Key,
                    clicksignEvento.Event.Occurred_at);
            }
            else if (nomeEvento.Contains("deadline"))
            {
                // deadline: prazo expirou, contrato encerrado com assinaturas parciais. Data em event.data.reached_at.
                _clicksignRepository.AtualizarDataAssinatura(
                    clicksignEvento.Document.Key,
                    clicksignEvento.Event.Data.Reached_at);
            }
        }

        public void SolicitarAssinaturaClicksign(SolicitacaoAssinaturaClicksignRequest solicitacaoAssinaturaClicksign)
        {
            var solicitacao = AutoMapper.Mapper.Map(solicitacaoAssinaturaClicksign, new SolicitacaoAssinaturaEletronicaClicksign());

            // --- Lógica de Fallback de Credenciais ---
            // Hierarquia: Contrato -> Obra -> UsinaEntrega -> ClicksignConfiguracao
            // O ContratoUsina identifica a Obra (via UsinaCodigo + AnoContrato + NumContrato).
            // A partir da Obra, obtém-se o UsinaEntregaCodigo real para resolução das credenciais.
            // Se a UsinaEntrega não possuir configuração vinculada ou ativa, as credenciais
            // globais (legado) serão utilizadas automaticamente (fallback transparente).
            var obra = _obraRepository.ObterObraPorContrato(
                solicitacao.ContratoUsina,
                solicitacao.ContratoAno,
                solicitacao.ContratoNumero);

            _assinaturaEletronicaIntegrationService.ConfigurarContextoUsina(obra?.UsinaEntregaCodigo);
            // -----------------------------------------

            var parametro = _clicksignRepository.ObterParametros();
            int qtdEnvioAssinaturaWhatsApp = 0;

            if (solicitacao.MetodoEnvioAssinatura == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp && !parametro.PermiteAssinaturaWhatsApp)
            {
                AssertionConcern.Notify("Parâmetro Permitir Assinatura WhatsApp", "O envio da assinatura via Whatsapp está desativada nas configurações da Assinatura Eletrônica.");
                return;
            }

            solicitacao.ContratoPdf = _reportService.GetContratoReport(solicitacao.ContratoUsina, solicitacao.ContratoAno, solicitacao.ContratoNumero, 1);

            var solicitacoesAnteriores = _clicksignRepository.ListarContratoClicksignEnvios(solicitacao.ContratoUsina, solicitacao.ContratoAno, solicitacao.ContratoNumero);
            if (!solicitacoesAnteriores.ContratoClicksignEnvioEmProcessamentoIsValid()) return;

            List<ClicksignSigner> clicksignSigners = new List<ClicksignSigner>();
            foreach (var dadosPessoais in solicitacao.DadosPessoaisAssinatura)
            {
                var clicksignSigner = GetClicksignSigner(dadosPessoais, parametro);
                clicksignSigner = _assinaturaEletronicaIntegrationService.CreateSigner(clicksignSigner);
                if (clicksignSigner == null)
                    return;

                clicksignSigners.Add(clicksignSigner);
            }

            ClicksignSigner clicksignSignerHired = null;
            if (parametro.EnviaAssinaturaContratada)
            {
                clicksignSignerHired = GetClicksignSignerHired(parametro);
                clicksignSignerHired = _assinaturaEletronicaIntegrationService.CreateSignerHired(clicksignSignerHired);
                if (clicksignSignerHired == null)
                    return;
            }

            ClicksignSigner clicksignCoResponsible = null;
            if (parametro.EnviaAssinaturaResponsavelSolidario && solicitacao.EmailResponsavelSolidario != "")
            {
                clicksignCoResponsible = GetClicksignCoResponsible(solicitacao, parametro);
                clicksignCoResponsible = _assinaturaEletronicaIntegrationService.CreateSignerCoResponsible(clicksignCoResponsible);
                if (clicksignCoResponsible == null)
                    return;
            }

            ClicksignSigner clicksignFirstWitness = null;
            if (parametro.PrimeiraTestemunha != EOpcoesTestemunhasAssinaturaEletronicaClicksign.NaoEnvia && solicitacao.EmailPrimeiraTestemunha != "")
            {
                clicksignFirstWitness = GetClicksignFirstWitness(solicitacao, parametro);
                clicksignFirstWitness = _assinaturaEletronicaIntegrationService.CreateSigner(clicksignFirstWitness);
                if (clicksignFirstWitness == null)
                    return;
            }

            ClicksignSigner clicksignSecondWitness = null;
            if (parametro.SegundaTestemunha != EOpcoesTestemunhasAssinaturaEletronicaClicksign.NaoEnvia && solicitacao.EmailSegundaTestemunha != "")
            {
                clicksignSecondWitness = GetClicksignSecondWitness(solicitacao, parametro);
                clicksignSecondWitness = _assinaturaEletronicaIntegrationService.CreateSigner(clicksignSecondWitness);
                if (clicksignSecondWitness == null)
                    return;
            }

            var clicksignDocument = new ClicksignDocument(solicitacao, parametro);
            clicksignDocument = _assinaturaEletronicaIntegrationService.CreateDocument(clicksignDocument);
            if (clicksignDocument == null)
                return;

            foreach (var signer in clicksignSigners)
            {
                var clicksignSigner = _assinaturaEletronicaIntegrationService.AddSignerToDocument(clicksignDocument, signer, false, false, false);
                if (clicksignSigner == null)
                    return;

                if (clicksignSigner.MetodoEnvioAssinatura == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp)
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesWhatsApp(clicksignSigner);
                    qtdEnvioAssinaturaWhatsApp += 1;
                }
                else
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesEmail(clicksignSigner);
                }
            }

            if (clicksignSignerHired != null)
            {
                clicksignSignerHired = _assinaturaEletronicaIntegrationService.AddSignerToDocument(clicksignDocument, clicksignSignerHired, parametro.EnviaAssinaturaContratada, false, false);
                if (clicksignDocument == null)
                    return;

                if (parametro.MetodoEnvioAssinaturaContratada == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp)
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesWhatsApp(clicksignSignerHired);
                    qtdEnvioAssinaturaWhatsApp += 1;
                }
                else
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesEmail(clicksignSignerHired);
                }
            }

            if (clicksignCoResponsible != null)
            {
                clicksignCoResponsible = _assinaturaEletronicaIntegrationService.AddSignerCoResponsibleToDocument(clicksignDocument, clicksignCoResponsible);
                if (clicksignCoResponsible == null)
                    return;

                if (solicitacaoAssinaturaClicksign.MetodoEnvioAssinaturaResponsavelSolidario == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp)
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesWhatsApp(clicksignCoResponsible);
                    qtdEnvioAssinaturaWhatsApp += 1;
                }
                else
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesEmail(clicksignCoResponsible);
                }

            }

            if (clicksignFirstWitness != null)
            {
                clicksignFirstWitness = _assinaturaEletronicaIntegrationService.AddSignerToDocument(clicksignDocument, clicksignFirstWitness, false
                    , parametro.PrimeiraTestemunha == EOpcoesTestemunhasAssinaturaEletronicaClicksign.Vendedor
                    , parametro.PrimeiraTestemunha == EOpcoesTestemunhasAssinaturaEletronicaClicksign.Testemunha);
                if (clicksignFirstWitness == null)
                    return;

                if (solicitacaoAssinaturaClicksign.MetodoEnvioAssinaturaPrimeiraTestemunha == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp)
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesWhatsApp(clicksignFirstWitness);
                    qtdEnvioAssinaturaWhatsApp += 1;
                }
                else
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesEmail(clicksignFirstWitness);
                }
            }

            if (clicksignSecondWitness != null)
            {
                clicksignSecondWitness = _assinaturaEletronicaIntegrationService.AddSignerToDocument(clicksignDocument, clicksignSecondWitness, false
                    , parametro.SegundaTestemunha == EOpcoesTestemunhasAssinaturaEletronicaClicksign.Vendedor
                    , parametro.SegundaTestemunha == EOpcoesTestemunhasAssinaturaEletronicaClicksign.Testemunha);
                if (clicksignSecondWitness == null)
                    return;

                if (solicitacaoAssinaturaClicksign.MetodoEnvioAssinaturaSegundaTestemunha == EMetodoEnvioAssinaturaEletronicaClicksign.Whatsapp)
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesWhatsApp(clicksignSecondWitness);
                    qtdEnvioAssinaturaWhatsApp += 1;
                }
                else
                {
                    _assinaturaEletronicaIntegrationService.RequestSignaturesEmail(clicksignSecondWitness);
                }
            }

            _clicksignRepository.SalvarIdClicksignDocument(clicksignDocument.Id, solicitacao, qtdEnvioAssinaturaWhatsApp);
        }

        private ClicksignSigner GetClicksignSigner(DadosPessoaisAssinatura dadosPessoais, ClicksignParametros parametro)
        {
            ClicksignSigner clicksignSigner = new ClicksignSigner
            {
                Email = dadosPessoais.Email,
                MetodoAutenticacao = dadosPessoais.MetodoAutenticacao,
                MetodoEnvioAssinatura = dadosPessoais.MetodoEnvioAssinatura,
                Nome = dadosPessoais.NomeCompleto,
                Cpf = dadosPessoais.Cpf,
                DataNascimento = dadosPessoais.DataNascimento,
                Telefone = dadosPessoais.Telefone,
                ObrigaDocumentoOficial = parametro.ObrigaDocumentoOficial,
                ObrigaSelfie = parametro.ObrigaSelfie,
                ObrigaAssinaturaManuscrita = parametro.ObrigaAssinaturaManuscrita,
                ObrigaReconhecimentoFacial = parametro.ObrigaReconhecimentoFacial,
                NotificaClienteNaConfirmacaoDeAssinatura = parametro.NotificaClienteNaConfirmacaoDeAssinatura,
                MessageEmail = parametro.CorpoEmail
            };

            return clicksignSigner;
        }

        private ClicksignSigner GetClicksignSignerHired(ClicksignParametros parametro)
        {
            ClicksignSigner clicksignSignerHired = new ClicksignSigner
            {
                Email = parametro.EmailContratada,
                MetodoAutenticacao = parametro.MetodoAutenticacaoContratada,
                MetodoEnvioAssinatura = parametro.MetodoEnvioAssinaturaContratada,
                Telefone = $"{parametro.DDDContratada}{parametro.TelefoneContratada}",
                ObrigaDocumentoOficial = parametro.ObrigaDocumentoOficial,
                ObrigaSelfie = parametro.ObrigaSelfie,
                ObrigaAssinaturaManuscrita = parametro.ObrigaAssinaturaManuscrita,
                ObrigaReconhecimentoFacial = parametro.ObrigaReconhecimentoFacial,
                NotificaClienteNaConfirmacaoDeAssinatura = parametro.NotificaClienteNaConfirmacaoDeAssinatura,
                MessageEmail = parametro.CorpoEmail
            };

            return clicksignSignerHired;
        }

        private ClicksignSigner GetClicksignCoResponsible(SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinatura, ClicksignParametros parametro)
        {
            ClicksignSigner clicksignCoResponsible = new ClicksignSigner
            {
                Email = solicitacaoAssinatura.EmailResponsavelSolidario,
                MetodoAutenticacao = solicitacaoAssinatura.MetodoAutenticacaoResponsavelSolidario,
                MetodoEnvioAssinatura = solicitacaoAssinatura.MetodoEnvioAssinaturaResponsavelSolidario,
                Nome = solicitacaoAssinatura.NomeCompletoResponsavelSolidario,
                Cpf = solicitacaoAssinatura.CpfResponsavelSolidario,
                DataNascimento = solicitacaoAssinatura.DataNascimentoResponsavelSolidario,
                Telefone = solicitacaoAssinatura.TelefoneResponsavelSolidario,
                ObrigaDocumentoOficial = parametro.ObrigaDocumentoOficial,
                ObrigaSelfie = parametro.ObrigaSelfie,
                ObrigaAssinaturaManuscrita = parametro.ObrigaAssinaturaManuscrita,
                ObrigaReconhecimentoFacial = parametro.ObrigaReconhecimentoFacial,
                NotificaClienteNaConfirmacaoDeAssinatura = parametro.NotificaClienteNaConfirmacaoDeAssinatura,
                MessageEmail = parametro.CorpoEmail
            };

            return clicksignCoResponsible;
        }

        private ClicksignSigner GetClicksignFirstWitness(SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinatura, ClicksignParametros parametro)
        {
            ClicksignSigner clicksignFirstWitness = new ClicksignSigner
            {
                Email = solicitacaoAssinatura.EmailPrimeiraTestemunha,
                MetodoAutenticacao = solicitacaoAssinatura.MetodoAutenticacaoPrimeiraTestemunha,
                MetodoEnvioAssinatura = solicitacaoAssinatura.MetodoEnvioAssinaturaPrimeiraTestemunha,
                Nome = solicitacaoAssinatura.NomeCompletoPrimeiraTestemunha,
                Cpf = solicitacaoAssinatura.CpfPrimeiraTestemunha,
                DataNascimento = solicitacaoAssinatura.DataNascimentoPrimeiraTestemunha,
                Telefone = solicitacaoAssinatura.TelefonePrimeiraTestemunha,
                ObrigaDocumentoOficial = parametro.ObrigaDocumentoOficial,
                ObrigaSelfie = parametro.ObrigaSelfie,
                ObrigaAssinaturaManuscrita = parametro.ObrigaAssinaturaManuscrita,
                ObrigaReconhecimentoFacial = parametro.ObrigaReconhecimentoFacial,
                NotificaClienteNaConfirmacaoDeAssinatura = parametro.NotificaClienteNaConfirmacaoDeAssinatura,
                MessageEmail = parametro.CorpoEmail
            };

            return clicksignFirstWitness;
        }

        private ClicksignSigner GetClicksignSecondWitness(SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinatura, ClicksignParametros parametro)
        {
            ClicksignSigner clicksignSecondWitness = new ClicksignSigner
            {
                Email = solicitacaoAssinatura.EmailSegundaTestemunha,
                MetodoAutenticacao = solicitacaoAssinatura.MetodoAutenticacaoSegundaTestemunha,
                MetodoEnvioAssinatura = solicitacaoAssinatura.MetodoEnvioAssinaturaSegundaTestemunha,
                Nome = solicitacaoAssinatura.NomeCompletoSegundaTestemunha,
                Cpf = solicitacaoAssinatura.CpfSegundaTestemunha,
                DataNascimento = solicitacaoAssinatura.DataNascimentoSegundaTestemunha,
                Telefone = solicitacaoAssinatura.TelefoneSegundaTestemunha,
                ObrigaDocumentoOficial = parametro.ObrigaDocumentoOficial,
                ObrigaSelfie = parametro.ObrigaSelfie,
                ObrigaAssinaturaManuscrita = parametro.ObrigaAssinaturaManuscrita,
                ObrigaReconhecimentoFacial = parametro.ObrigaReconhecimentoFacial,
                NotificaClienteNaConfirmacaoDeAssinatura = parametro.NotificaClienteNaConfirmacaoDeAssinatura,
                MessageEmail = parametro.CorpoEmail
            };

            return clicksignSecondWitness;
        }

    }
}
