using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Reports.AditivoReport;
using TopSys.TopConWeb.Infra.Reports.ContratoReport;
using TopSys.TopConWeb.Infra.Reports.PropostaReport;
using TopSys.TopConWeb.Infra.Reports.ContratoResidualReport;
using TopSys.TopConWeb.Infra.Reports.FilterClasses;

namespace TopSys.TopConWeb.Infra.Reports
{
    public class ReportService
    {
        public readonly IParametroService _parametroService;
        public readonly IEmpresaService _empresaService;
        public readonly IObraService _obraService;
        public readonly IObraTaxaService _obraTaxaService;
        public readonly IAssinaturaEletronicaIntegrationService _assinaturaEletronicaIntegrationService;
        public readonly IClicksignRepository _clicksignRepository;
        public readonly IContratoService _contratoService;
        public readonly IPropostaService _propostaService;
        public readonly IContratoVersaoService _contratoVersaoService;
        public ReportService(IParametroService parametroService, 
            IEmpresaService empresaService, 
            IObraService obraService, 
            IObraTaxaService obraTaxaService, 
            IAssinaturaEletronicaIntegrationService assinaturaEletronicaIntegrationService, 
            IClicksignRepository clicksignRepository, 
            IContratoService contratoService, 
            IPropostaService propostaService, 
            IContratoVersaoService contratoVersaoService)
        {
            _parametroService = parametroService;
            _empresaService = empresaService;
            _obraService = obraService;
            _obraTaxaService = obraTaxaService;
            _assinaturaEletronicaIntegrationService = assinaturaEletronicaIntegrationService;
            _clicksignRepository = clicksignRepository;
            _contratoService = contratoService;
            _propostaService = propostaService;
            _contratoVersaoService = contratoVersaoService;
        }

        private Stream GetPdfStream<T>(ref T report) where T : ReportClass
        {
            report.Refresh();
            var result = report.ExportToStream(ExportFormatType.PortableDocFormat);
            report.Close();
            report.Dispose();
            return result;
        }

        public Stream GetContratoReport(int idUsina, int ano, int numero, int tipo, bool estaGerandoNovaVersao = false)
        {
            if (!ObraScopes.ImpressaoContratoIsValid(idUsina, ano, numero, _obraService, _parametroService)) return Stream.Null;

            var ultimoContratoAssinado = _clicksignRepository.ObterUltimoContratoAssinadoClicksignEnvio(idUsina,ano, numero);
            if (ultimoContratoAssinado != null)
            {
                var stream = GetContratoAssinadoClicksignReport(ultimoContratoAssinado);
                if (stream != null)
                    return stream;
            }

            var segmento = _contratoService.GetSegmentacaoContrato(idUsina, ano, numero);
            var parametro = "RelContrato";
            if (segmento != null)
            { 
                switch (segmento)
                {
                    case "CON":
                        parametro = "RelContrato";
                        break;
                    case "ARG":
                        parametro = "RelContratoArgamassa";
                        break;
                    case "EXP":
                        parametro = "RelContratoExpress";
                        break;
                }
            }

            var customReportName = _parametroService.ObterParametroN("TopCon", parametro);

            var obra = _obraService.ObterObraPorContrato(idUsina, ano, numero);
            var customReportNameUsina = _contratoService.ObterContratoUsina(obra.UsinaEntregaCodigo, segmento) ?? "";

            if (!customReportNameUsina.Equals("")) customReportName = customReportNameUsina;

            ReportClass report = ContratoReportHelper.GetByRptName(customReportName);

            ContratoReportHelper.ConfigurarContratoReport(ref report, customReportName, idUsina, ano, numero, _obraService, _obraTaxaService, _empresaService, _parametroService, _contratoService, _contratoVersaoService, tipo, estaGerandoNovaVersao);

            return GetPdfStream(ref report);
        }

        public Stream GetContratoVersaoReport(int idUsina, int ano, int numero, int tipo, int numeroVersao, bool estaGerandoNovaVersao = false)
        {
            if (!ObraScopes.ImpressaoContratoIsValid(idUsina, ano, numero, _obraService, _parametroService)) return Stream.Null;

            var segmento = _contratoService.GetSegmentacaoContrato(idUsina, ano, numero);
            var parametro = "RelContrato";
            if (segmento != null)
            {
                switch (segmento)
                {
                    case "CON":
                        parametro = "RelContrato";
                        break;
                    case "ARG":
                        parametro = "RelContratoArgamassa";
                        break;
                    case "EXP":
                        parametro = "RelContratoExpress";
                        break;
                }
            }

            var customReportName = _parametroService.ObterParametroN("TopCon", parametro).Split('.')[0] + "Versao.rpt";
            ReportClass report = ContratoReportHelper.GetByRptName(customReportName);

            ContratoReportHelper.ConfigurarContratoReport(ref report, customReportName, idUsina, ano, numero, _obraService, _obraTaxaService, _empresaService, _parametroService, _contratoService, _contratoVersaoService, tipo, estaGerandoNovaVersao, numeroVersao);

            return GetPdfStream(ref report);
        }

        public Stream GetContratoResidualReport(int idUsina, int ano, int numero)
        {
            if (!ObraScopes.ImpressaoContratoIsValid(idUsina, ano, numero, _obraService, _parametroService)) return Stream.Null;

            var ultimoContratoAssinado = _clicksignRepository.ObterUltimoContratoAssinadoClicksignEnvio(idUsina, ano, numero);
            if (ultimoContratoAssinado != null)
            {
                var stream = GetContratoAssinadoClicksignReport(ultimoContratoAssinado);
                if (stream != null)
                    return stream;
            }

            var customReportName = _parametroService.ObterParametroN("web", "RelContratoResidual");
            ReportClass report = ContratoResidualReportHelper.GetByRptName(customReportName);

            ContratoResidualReportHelper.ConfigurarContratoReport(ref report, customReportName, idUsina, ano, numero, _obraService, _obraTaxaService, _empresaService, _parametroService, _contratoService, _contratoVersaoService);

            return GetPdfStream(ref report);
        }

        public Stream GetAditivoReport(int versao, int idUsina, int ano, int numero, string[] diferencas, string tracosAlterados)
        {
            var customReportName = _parametroService.ObterParametroN("web", "RelAditivo");
            ReportClass report = AditivoReportHelper.GetByRptName(customReportName);

            AditivoReportHelper.ConfigurarAditivoReport(ref report, customReportName, versao, idUsina, ano, numero, diferencas, _obraService, _obraTaxaService, _empresaService, _parametroService, tracosAlterados);

            return GetPdfStream(ref report);
        }

        public Stream GetContratoAssinadoClicksignReport(ContratoClicksignEnvio contratoClicksignEnvio)
        {
            // Hierarquia: Contrato -> Obra -> UsinaEntrega -> ClicksignConfiguracao
            var obra = _obraService.ObterObraPorContrato(
                contratoClicksignEnvio.ContratonUsina,
                contratoClicksignEnvio.ContratoAno,
                contratoClicksignEnvio.ContratoNumero);

            _assinaturaEletronicaIntegrationService.ConfigurarContextoUsina(obra?.UsinaEntregaCodigo);

            var downloadUrl = _assinaturaEletronicaIntegrationService.GetClicksignUrlDocumentSigned(contratoClicksignEnvio.IdClicksign);
            var signedDocument = _assinaturaEletronicaIntegrationService.DownloadFile(downloadUrl);

            if (signedDocument == null) return null;

            return new MemoryStream(signedDocument);
        }


        public Stream GetPropostaReport(int idUsina, int ano, int numero, int sequenciaPDFProposta = 1, Guid? propagandaId = null)
        {
            if (!ObraScopes.ImpressaoPropostaIsValid(idUsina, ano, numero, _obraService, _parametroService)) return Stream.Null;

            var customReportName = _parametroService.ObterParametroN("web", "RelProposta");
            ReportClass report = PropostaReportHelper.GetByRptName(customReportName);

            PropostaReportHelper.ConfigurarPropostaReport(ref report, customReportName, idUsina, ano, numero, _obraService, _obraTaxaService, _empresaService, _parametroService, _propostaService, _contratoVersaoService, sequenciaPDFProposta, propagandaId);

            return GetPdfStream(ref report);
            /*var report = new PropostaReport();

            var exibirColunaUsoNaProposta = _parametroService.ObterParametroN("web", "ExibirColunaUsoNaProposta");
            report.DataDefinition.FormulaFields["ExibirColunaUsoNaProposta"].Text = exibirColunaUsoNaProposta;

            var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            report.DataDefinition.FormulaFields["utilizaDadosFilial"].Text = parametroProposta.DadosFilialNaImpressaoProposta ? "1" : "0";
            report.DataDefinition.FormulaFields["ocultarTaxaContrato"].Text = parametroProposta.OcultarTaxaProposta ? "1" : "0";

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_chtel.usina} = " + idUsina);
            report.RecordSelectionFormula += (" and {con_chtel.ano_chamada} = " + ano);
            report.RecordSelectionFormula += (" and {con_chtel.num_chamada} = " + numero);
            report.RecordSelectionFormula += (" and floor({con_usina_entrega.emp_filial} / 1000) = {ger_empresa.emp}");

            return GetPdfStream(ref report);*/
        }
        public Stream GetPropostaProgramacaoReport(int idUsina, int anoProposta, int numeroProposta, int sequenciaProgramacao)
        {
            if (!ObraScopes.ImpressaoPropostaIsValid(idUsina, anoProposta, numeroProposta, _obraService, _parametroService)) return Stream.Null;

            var report = new PropostaProgramacaoReport();

            var exibirColunaUsoNaProposta = _parametroService.ObterParametroN("web", "ExibirColunaUsoNaProposta");
            report.DataDefinition.FormulaFields["ExibirColunaUsoNaProposta"].Text = exibirColunaUsoNaProposta;

            var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            report.DataDefinition.FormulaFields["utilizaDadosFilial"].Text = parametroProposta.DadosFilialNaImpressaoProposta ? "1" : "0";
            report.DataDefinition.FormulaFields["ocultarTaxaContrato"].Text = parametroProposta.OcultarTaxaProposta ? "1" : "0";

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_chtel.usina} = " + idUsina);
            report.RecordSelectionFormula += (" and {con_chtel.ano_chamada} = " + anoProposta);
            report.RecordSelectionFormula += (" and {con_chtel.num_chamada} = " + numeroProposta);
            report.RecordSelectionFormula += (" and {con_programacao.seq_prog} = " + sequenciaProgramacao);
            report.RecordSelectionFormula += (" and floor({con_usina_entrega.emp_filial} / 1000) = {ger_empresa.emp}");

            return GetPdfStream(ref report);
        }

        public Stream GetProgramacaoReport(int idUsina, int obraNumero, int sequencia)
        {
            var report = new ProgramacaoReport();

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_programacao.usina} = " + idUsina);
            report.RecordSelectionFormula += (" and {con_programacao.no_obra} = " + obraNumero);
            report.RecordSelectionFormula += (" and {con_programacao.seq_prog} = " + sequencia);

            return GetPdfStream(ref report);
        }

        private Stream GetRelatorioProducaoReport<T>(RelatorioProducaoFilter filtro, bool detalharAdicionais, bool detalharViaCaptacao, T report) where T : ReportClass
        {

            var isVolumeReport = (report is RelatorioProducaoVolumeReport);

            if (report is RelatorioProducaoAnaliticoReport analiticoReport)
            {
                analiticoReport.DataDefinition.FormulaFields["detalharAdicionais"].Text = detalharAdicionais ? "1" : "0";
                analiticoReport.DataDefinition.FormulaFields["detalharViaCaptacaoIndicador"].Text = detalharViaCaptacao ? "1" : "0";
            }

            if (report is RelatorioProducaoPorProgramacaoReport programacaoReport)
            {
                programacaoReport.DataDefinition.FormulaFields["detalharViaCaptacaoIndicador"].Text = detalharViaCaptacao ? "1" : "0";
            }

            if (filtro.Usina != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                var campo = (isVolumeReport ? "con_programacao.usina_entrega" : "con_nf.usina");
                report.RecordSelectionFormula += ("{" + campo + "} = " + filtro.Usina);
            }

            if (filtro.Vendedor != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                if(isVolumeReport)
                    report.RecordSelectionFormula += ("(if {con_nf.tp_doc}=84 then {con_contrato.vendedor} = " + filtro.Vendedor + " else {con_nf.vendedor} = " + filtro.Vendedor + ")");
                else
                    report.RecordSelectionFormula += ("{con_contrato.vendedor} = " + filtro.Vendedor);

            }

            else if (!string.IsNullOrEmpty(filtro.VendedorIn))
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                var tabela = (isVolumeReport ? "con_contrato" : "con_nf");
                report.RecordSelectionFormula += ("{" + tabela + ".vendedor} IN[0," + filtro.VendedorIn + "]");
            }

            if (filtro.VendedorPadrinho != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                report.RecordSelectionFormula += ("{con_chtel.vend_padrinho} = " + filtro.VendedorPadrinho);
            }

            if (filtro.Cliente != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                var tabela = (isVolumeReport ? "con_contrato" : "con_nf");
                report.RecordSelectionFormula += ("{" + tabela + ".interv} = " + filtro.Cliente);
            }

            if (filtro.GrupoEconomico != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                report.RecordSelectionFormula += ("{ger_interv.grupo_economico} = " + filtro.GrupoEconomico);
            }

            if (filtro.AnoContrato != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                var tabela = (isVolumeReport ? "con_programacao" : "con_nf");
                report.RecordSelectionFormula += ("{" + tabela + ".ano_contrato} = " + filtro.AnoContrato);
            }

            if (filtro.NumContrato != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";

                var campo = (isVolumeReport ? "con_programacao.no_contrato" : "con_nf.num_contrato");
                report.RecordSelectionFormula += ("{" + campo + "} = " + filtro.NumContrato);
            }

            if (filtro.Segmentacao != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                report.RecordSelectionFormula += ("{con_contrato.segmentacao} = " + filtro.Segmentacao);

                report.DataDefinition.FormulaFields["exibe_segmentacao"].Text = "1";
            }
            else {
                report.DataDefinition.FormulaFields["exibe_segmentacao"].Text = "0";
            }

            if(filtro.ContratoFinalidade != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                report.RecordSelectionFormula += ("{con_contrato.finalidade_ctr} = " + filtro.ContratoFinalidade);
            }

            if(filtro.ViaCaptacao != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                report.RecordSelectionFormula += ("{con_obras.obra_captado_v} = " + filtro.ViaCaptacao);
            }

            if (filtro.DataDe != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                var d = filtro.DataDe.Value;
                var dateString = $"Date ({d.Year},{d.Month},{d.Day})";

                if(report is RelatorioProducaoVolumeReport)
                    report.RecordSelectionFormula += ("{con_contrato.dt_contrato} >= " + dateString);
                else
                    report.RecordSelectionFormula += ("{con_nf.data_remessa} >= " + dateString);

                report.DataDefinition.FormulaFields["dt1"].Text = dateString;
            }

            if (filtro.DataAte != null)
            {
                if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
                var d = filtro.DataAte.Value;
                var dateString = $"Date ({d.Year},{d.Month},{d.Day})";

                if (report is RelatorioProducaoVolumeReport)
                    report.RecordSelectionFormula += ("{con_contrato.dt_contrato} <= " + dateString);
                else
                    report.RecordSelectionFormula += ("{con_nf.data_remessa} <= " + dateString);

                report.DataDefinition.FormulaFields["dt2"].Text = dateString;
            }

            return GetPdfStream(ref report);
        }

        public Stream GetRelatorioProducaoAnaliticoReport(RelatorioProducaoFilter filtro, bool detalharAdicionais, bool detalharViaCaptacao)
        {
             return GetRelatorioProducaoReport(filtro, detalharAdicionais, detalharViaCaptacao, new RelatorioProducaoAnaliticoReport());
        }

        public Stream GetRelatorioProducaoSinteticoReport(RelatorioProducaoFilter filtro)
        {
            return GetRelatorioProducaoReport(filtro, false, false, new RelatorioProducaoSinteticoReport());
        }

        public Stream GetRelatorioProducaoPorProgramacaoReport(RelatorioProducaoFilter filtro, bool detalharViaCaptacao)
        {
            return GetRelatorioProducaoReport(filtro, false, detalharViaCaptacao, new RelatorioProducaoPorProgramacaoReport());
        }

        public Stream GetRelatorioVolumeReport(RelatorioProducaoFilter filtro)
        {
            return GetRelatorioProducaoReport(filtro, false, false, new RelatorioProducaoVolumeReport());
        }
    }
}
