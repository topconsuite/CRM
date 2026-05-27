using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Infra.Reports.PropostaReport
{
    public static class PropostaReportHelper
    {
        public static ReportClass GetByRptName(string rptName)
        {
            switch (rptName.ToLower())
            {
                case "propostareportconcrebras.rpt":
                    return new PropostaReportConcrebras();

                case "propostareportconcreserv.rpt":
                    return new PropostaReportConcreserv();

                case "propostareportmaxmohr.rpt":
                    return new PropostaReportMaxMohr();

                default:
                    return new PropostaReport();
            }
        }    

        public static void ConfigurarPropostaReport(ref ReportClass report, string rptName, int idUsina, int propostaAno, int propostaNumero, IObraService obraService, IObraTaxaService obraTaxaService, IEmpresaService empresaService, IParametroService parametroService, IPropostaService propostaService, IContratoVersaoService contratoVersaoService, int sequenciaPDFProposta = 1, Guid? propagandaId = null)
        {
            var exibirColunaUsoNaProposta = parametroService.ObterParametroN("web", "ExibirColunaUsoNaProposta");
            report.DataDefinition.FormulaFields["ExibirColunaUsoNaProposta"].Text = exibirColunaUsoNaProposta;

            if (propagandaId != null)
                report.DataDefinition.FormulaFields["propagandaCodigo"].Text = $"\"{propagandaId.ToString()}\"";

            var parametroProposta = parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            report.DataDefinition.FormulaFields["utilizaDadosFilial"].Text = parametroProposta.DadosFilialNaImpressaoProposta ? "1" : "0";
            report.DataDefinition.FormulaFields["ocultarTaxaContrato"].Text = parametroProposta.OcultarTaxaProposta ? "1" : "0";
            

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_chtel.usina} = " + idUsina);
            report.RecordSelectionFormula += (" and {con_chtel.ano_chamada} = " + propostaAno);
            report.RecordSelectionFormula += (" and {con_chtel.num_chamada} = " + propostaNumero);
            report.RecordSelectionFormula += (" and floor({con_usina_entrega.emp_filial} / 1000) = {ger_empresa.emp}");

            var obra = obraService.ListarFiltrados(t => t.UsinaCodigo == idUsina && t.AnoChamada == propostaAno && t.NumChamada == propostaNumero).FirstOrDefault();
            
            var numProximaVersao = propostaService.GetUltimaVersaoProposta(idUsina, propostaAno, propostaNumero) + 1;
            var numVersao = "0";
            if (numProximaVersao == 1)
            {
                numVersao = Convert.ToString(numProximaVersao);
            }
            else
            {
                var versaoEmAberto = false;
                if (obra.NumContrato != 0)
                    if (contratoVersaoService.ExisteVersaoEmAberto(obra.UsinaCodigo, obra?.AnoContrato ?? 0, obra?.NumContrato ?? 0))
                        versaoEmAberto = true; 

                numVersao = Convert.ToString(numProximaVersao - (versaoEmAberto ? 2 : 1));

                if (report.ResourceName == "PropostaReportConcrebras.rpt" || report.ResourceName == "PropostaReportMaxMohr.rpt" || report.ResourceName == "PropostaReport.rpt")
                    report.RecordSelectionFormula += " AND {con_proposta_item_versao.num_versao}=" + numVersao;
            }

            foreach (FormulaFieldDefinition formula in report.DataDefinition.FormulaFields)
            {
                switch (formula.Name.ToLower())
                {
                    case "num_versao":
                        formula.Text = $"'{numVersao}'";
                        break;
                    case "seq_report":
                        formula.Text = $"'{sequenciaPDFProposta}'";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
