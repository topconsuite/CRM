using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Infra.Reports.AditivoReport
{
    public static class AditivoReportHelper
    {
        public static ReportClass GetByRptName(string rptName)
        {
            switch (rptName.ToLower())
            {
                case "aditivoreportconcrebras.rpt":
                    return new AditivoReportConcrebras();
                default:
                    return new AditivoReportDefault();
            }
        }

        private static ObraVersao CarregarDadosObraParaAditivoReport(this IObraService obraService, int versao, int idUsinaContrato, int contratoAno, int contratoNumero, IObraTaxaService obraTaxaService)
        {
            var obra = obraService.ListarFiltrados<ObraVersao>(
                // WHERE
                t => t.NumeroVersao == versao && t.UsinaCodigo == idUsinaContrato && t.AnoContrato == contratoAno && t.NumContrato == contratoNumero,
                // INCLUDES
                t => t.UsinaEntrega,
                t => t.Contrato.Vendedor,
                t => t.Concorrente,
                t => t.TipoCobranca,
                t => t.ObraTracos,
                t => t.ObraBombas,
                t => t.ObraBombas.Select(p => p.BombaTipo)
            ).FirstOrDefault();

            if (obra != null)
            {
                obra.ObraTracos = obraService.ListarFiltrados<ObraTracoVersao>(t => t.NumeroVersao == versao && t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero).ToList();
                obra.ObraBombas = obraService.ListarFiltrados<ObraBombaVersao>(t => t.NumeroVersao == versao && t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero, t => t.BombaTipo).ToList();
                obra.ObraTaxas = obraTaxaService.ListarByIdObraVersao(obra.NumeroVersao, obra.UsinaEntregaCodigo, obra.Numero);
                obra.ObraMensagensPadrao = obraService.ListarFiltrados<ObraMensagemPadraoVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraNumero == obra.Numero && t.SelecionadoSimNao == "S", t => t.MensagemPadrao).ToList();
            }

            return obra;
        }

        private static Empresa CarregarDadosEmpresaParaAditivoReport(this IEmpresaService empresaService, int empresaCodigo)
        {
            return empresaService.ListarFiltrados(
                t => t.Codigo == empresaCodigo,
                t => t.EnderecoMunicipio
            ).FirstOrDefault();
        }

        private static string GetMensagensPadraoString(ObraVersao obra)
        {
            int i = 0;

            return String.Join(" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & ",
                obra?.ObraMensagensPadrao?.Select(t => $"\"{++i})     {t.MensagemPadrao?.Mensagem?.Replace("\"", "\"\"")}\"") ?? Array.Empty<string>())
                .Replace("\r\n", "");
        }

        private static string GetEmpresaCpfCnpjString(Empresa empresa)
        {
            if (empresa?.CpfCnpj?.Length == 14)
            {
                var mascaraCNPJ = new System.ComponentModel.MaskedTextProvider("00,000,000/0000-00");
                mascaraCNPJ.Set(empresa?.CpfCnpj);
                return $"'{mascaraCNPJ.ToString().Replace(",", ".")}'";
            }
            else if (empresa?.CpfCnpj?.Length == 11)
            {
                var mascaraCPF = new System.ComponentModel.MaskedTextProvider("000,000,000-00");
                mascaraCPF.Set(empresa?.CpfCnpj);
                return $"'{mascaraCPF.ToString().Replace(",", ".")}'";
            }
            else
            {
                return $"'{empresa?.CpfCnpj}'";
            }
        }

        private static string GetEmpresaLogradouroString(Empresa empresa)
        {
            if (empresa != null)
            {
                var logradouro = $"'{empresa.EnderecoLogradouro} {empresa.EnderecoNumero} {empresa.EnderecoComplemento} - {empresa.EnderecoBairro}'";
                return logradouro.Substring(0, logradouro.Length > 200 ? 200 : logradouro.Length);
            }
            else
                return "''";
        }

        private static string GetEmpresaCepString(Empresa empresa)
        {
            if (empresa?.EnderecoCep?.Length == 8)
            {
                var mascaraCEP = new System.ComponentModel.MaskedTextProvider("00000-000");
                mascaraCEP.Set(empresa?.EnderecoCep);
                return $"'{mascaraCEP.ToString()}'";
            }
            else
                return "''";
        }

        private static string GetValorBombaString(ObraVersao obra)
        {
            return $"{obra?.Contrato?.ValorBomba ?? 0m}".Replace(",", ".");
        }

        public static void ConfigurarAditivoReport(ref ReportClass report, string rptName, int versao, int idUsinaContrato, int contratoAno, int contratoNumero, string[] diferencas, IObraService obraService, IObraTaxaService obraTaxaService, IEmpresaService empresaService, IParametroService parametroService, string tracosAlterados = "")
        {
            var obra = obraService.CarregarDadosObraParaAditivoReport(versao, idUsinaContrato, contratoAno, contratoNumero, obraTaxaService);

            var empresaCodigo = (obra?.UsinaEntrega?.FilialCodigo ?? 0) / 1000;
            var empresa = empresaService.CarregarDadosEmpresaParaAditivoReport(empresaCodigo);

            var parametroTaxaExtra = parametroService.ObterPorDataBase<ParametroTaxaExtra>(DateTime.Now);

            var parametroDadosFilial = parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            var utilizaDadosFilial = parametroDadosFilial.DadosFilialNaImpressaoProposta ? "1" : "0";

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_contrato_versao.num_versao} = " + (obra?.NumeroVersao ?? 0));
            report.RecordSelectionFormula += (" and {con_contrato_versao.usina} = " + (obra?.UsinaCodigo ?? 0));
            report.RecordSelectionFormula += (" and {con_contrato_versao.ano_contrato} = " + (obra?.AnoContrato ?? 0));
            report.RecordSelectionFormula += (" and {con_contrato_versao.num_contrato} = " + (obra?.NumContrato ?? 0));
            report.RecordSelectionFormula += (" and {ger_empresa.emp} = " + empresaCodigo);

            foreach (FormulaFieldDefinition formula in report.DataDefinition.FormulaFields)
            {
                switch (formula.Name.ToLower())
                {
                    case "men_desc_traco":
                        formula.Text = $"{diferencas[7]}";
                        break;
                    case "men_fck_consumo" :
                        formula.Text = $"'{diferencas[0]}'";
                        break;
                    case "men_pedra":
                        formula.Text = $"'{diferencas[1]}'";
                        break;
                    case "men_slump":
                        formula.Text = $"'{diferencas[2]}'";
                        break;
                    case "men_uso":
                        formula.Text = $"'{diferencas[3]}'";
                        break;
                    case "men_volume":
                        formula.Text = $"'{diferencas[4]}'";
                        break;
                    case "men_preco_unit":
                        formula.Text = $"'{diferencas[5]}'";
                        break;
                    case "men_vlr_total":
                        formula.Text = $"'{diferencas[6]}'";
                        break;
                    case "men_del_fck_consumo":
                        formula.Text = $"'{diferencas[8]}'";
                        break;
                    case "men_del_pedra":
                        formula.Text = $"'{diferencas[9]}'";
                        break;
                    case "men_del_slump":
                        formula.Text = $"'{diferencas[10]}'";
                        break;
                    case "men_del_uso":
                        formula.Text = $"'{diferencas[11]}'";
                        break;
                    case "men_del_volume":
                        formula.Text = $"'{diferencas[12]}'";
                        break;
                    case "men_del_preco_unit":
                        formula.Text = $"'{diferencas[13]}'";
                        break;
                    case "men_del_vlr_total":
                        formula.Text = $"'{diferencas[14]}'";
                        break;
                    case "men_bomba":
                        formula.Text = $"'{diferencas[15]}'";
                        break;
                    case "men_taxa":
                        formula.Text = $"'{diferencas[16]}'";
                        break;
                    case "men_pagamento":
                        formula.Text = $"'{diferencas[17]}'";
                        break;
                    case "men_endereco":
                        formula.Text = $"'{diferencas[18]}'";
                        break;
                    case "men_demais_servicos":
                        formula.Text = $"'{diferencas[19]}'";
                        break;
                    case "men_reajuste":
                        formula.Text = $"'{diferencas[20]}'";
                        break;
                    case "mes_padrao1":
                        formula.Text = $"{GetMensagensPadraoString(obra)}";
                        break;
                    case "obs_pagamento":
                        formula.Text = $"'{obra?.TipoCobranca?.Descricao}'";
                        break;
                    case "nome_vendedor":
                        formula.Text = $"'{obra?.Contrato?.Vendedor?.Nome}'";
                        break;
                    case "razao_vendedor":
                        formula.Text = $"'{obra?.Contrato?.Vendedor?.RazaoSocial}'";
                        break;
                    case "mostra_taxa":
                        formula.Text = "0";
                        break;
                    case "emp_razao":
                        formula.Text = $"'{empresa?.Razao}'";
                        break;
                    case "emp_cnpj":
                        formula.Text = $"{GetEmpresaCpfCnpjString(empresa)}";
                        break;
                    case "emp_endereco":
                        formula.Text = $"{GetEmpresaLogradouroString(empresa)}";
                        break;
                    case "emp_municipio":
                        formula.Text = $"'{empresa?.EnderecoMunicipio?.Nome}'";
                        break;
                    case "emp_uf":
                        formula.Text = $"'{empresa?.EnderecoMunicipio?.Uf}'";
                        break;
                    case "emp_cep":
                        formula.Text = $"{GetEmpresaCepString(empresa)}";
                        break;
                    case "vlr_bomba":
                        formula.Text = $"{GetValorBombaString(obra)}";
                        break;
                    case "utilizadadosfilial":
                        formula.Text = $"{utilizaDadosFilial}";
                        break;
                    case "tracos_alterados":
                        formula.Text = $"'{tracosAlterados}'";
                        break;
                }
            }
        }
    }
}
