using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Infra.Reports.ContratoResidualReport
{
    public static class ContratoResidualReportHelper
    {
        public static ReportClass GetByRptName(string rptName)
        {
            switch (rptName.ToLower())
            {
                case "contratoresidualreportconcretize.rpt":
                    return new ContratoResidualReportConcretize();
                case "contratoresidualreportconcrebras.rpt":
                    return new ContratoResidualReportConcrebras();                
                default:
                    return new ContratoResidualReport();
            }
        }

        private static Obra CarregarDadosObraParaContratoReport(this IObraService obraService, int idUsinaContrato, int contratoAno, int contratoNumero, IObraTaxaService obraTaxaService)
        {
            var obra = obraService.ListarFiltrados(
                // WHERE
                t => t.UsinaCodigo == idUsinaContrato && t.AnoContrato == contratoAno && t.NumContrato == contratoNumero,
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
                obra.ObraTracos = obraService.ListarFiltrados<ObraTraco>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero).ToList();
                obra.ObraBombas = obraService.ListarFiltrados<ObraBomba>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero, t => t.BombaTipo).ToList();
                obra.ObraTaxas = obraTaxaService.ListarByIdObra(obra.UsinaEntregaCodigo, obra.Numero);
                obra.ObraMensagensPadrao = obraService.ListarFiltrados<ObraMensagemPadrao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraNumero == obra.Numero && t.SelecionadoSimNao == "S", t => t.MensagemPadrao).ToList();
            }

            return obra;
        }

        private static Empresa CarregarDadosEmpresaParaContratoReport(this IEmpresaService empresaService, int empresaCodigo)
        {
            return empresaService.ListarFiltrados(
                t => t.Codigo == empresaCodigo,
                t => t.EnderecoMunicipio
            ).FirstOrDefault();
        }

        private static string GetCondicaoBombaString(Obra obra, string formulaName)
        {
            var bombaIndex = int.Parse(formulaName.ElementAt(5).ToString());
            if (obra?.ObraBombas?.Count() >= bombaIndex)
            {
                var obraBomba = obra.ObraBombas.ElementAt(bombaIndex - 1);

                if (obraBomba.TipoCalculo != EBombaM3CalculoTipo.SemCobranca)
                {
                    var mensagemBomba = $"Serviço de bombeamento com {obraBomba.BombaTipo?.Descricao}:";
                    mensagemBomba += $" Para bombeamentos inferiores a {obraBomba.M3PropostoAte} m³ será cobrado taxa de R$ {string.Format("{0:#.00}", obraBomba.TaxaMinimaPrecoProposto)}";
                    mensagemBomba += $" e acima de {obraBomba.M3PropostoAte} m³ serão cobrados R$ {string.Format("{0:#.00}", obraBomba.M3PrecoProposto)} por m³ lançado";
                    mensagemBomba += $"{(obraBomba.TipoCalculo == Domain.Enums.EBombaM3CalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";

                    return $"'{mensagemBomba}'";
                }
                else
                {
                    var mensagemBomba = $"Serviço de bombeamento com {obraBomba.BombaTipo?.Descricao}:";
                    mensagemBomba += $" Para bombeamentos inferiores a {obraBomba.HoraPropostoAte} horas será cobrado taxa de R$ {string.Format("{0:#.00}", obraBomba.HoraTaxaMinimaPrecoProposto)}";
                    mensagemBomba += $" e acima de {obraBomba.HoraPropostoAte} horas serão cobrados R$ {string.Format("{0:#.00}", obraBomba.HoraPrecoProposto)} por hora lançada";
                    mensagemBomba += $"{(obraBomba.HoraTipoCalculo == Domain.Enums.EBombaHoraCalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";

                    return $"'{mensagemBomba}'";
                }

            }
            else
            {
                return "''";
            }
        }

        private static string GetDadosBombaString(Obra obra, string formulaName)
        {
            var bombaIndex = int.Parse(formulaName.ElementAt(10).ToString());
            if (obra?.ObraBombas?.Count() >= bombaIndex)
            {
                var obraBomba = obra.ObraBombas.ElementAt(bombaIndex - 1);

                var mensagemBomba = $"{obraBomba.BombaTipo?.Descricao};";
                mensagemBomba += $"{string.Format("{0:#.00}", obraBomba.TaxaMinimaPrecoProposto)};";
                mensagemBomba += $"{obraBomba.M3PropostoAte};";
                mensagemBomba += $"{string.Format("{0:#.00}", obraBomba.M3PrecoProposto)}";

                return $"'{mensagemBomba}'";
            }
            else
            {
                return "''";
            }
        }

        private static string GetTiposBombaString(Obra obra)
        {
            return $"'{String.Join("/", obra?.ObraBombas?.Select(t => t.BombaTipo?.Descricao) ?? Array.Empty<string>())}'";
        }

        private static string GetMensagensPadraoString(Obra obra)
        {
            int i = 0;

            return String.Join(" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & ",
                obra?.ObraMensagensPadrao?.Select(t => $"\"{++i})     {t.MensagemPadrao?.Mensagem?.Replace("\"", "\"\"")}\"") ?? Array.Empty<string>())
                .Replace("\r\n", "");
        }

        private static string GetConcorrenteString(Obra obra)
        {
            if (obra?.ObraNova == "S")
                return "'OBRA NOVA'";
            else if (obra?.ObraNova == "N")
            {
                if (obra.IniciadaPorConcorrenteSimNao == "S")
                    return $"'INICIADA POR: {obra.Concorrente?.Descricao}'";
                else
                    return "'OBRA PARALISA ANTERIORMENTE'";
            }
            else
                return "''";
        }

        private static string GetPrevisaoValorBombaString(Obra obra)
        {
            var quantidadeM3 = obra?.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0f;
            var valorBomba = obra?.ObraBombas?.Sum(t => {
                var result = 0f;

                if (quantidadeM3 > t.M3PropostoAte)
                    result += (quantidadeM3 * t.M3PrecoProposto);
                else
                    result += t.TaxaMinimaPrecoProposto;

                if (t.BombaPropria && t.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca)
                    result += t.HoraTaxaMinimaPrecoProposto;

                return result;
            }) ?? 0f;
            return $"{valorBomba.ToString().Replace(",", ".")}";
        }

        private static string GetValorBombaString(Obra obra)
        {
            return $"{obra?.Contrato?.ValorBomba ?? 0m}".Replace(",", ".");
        }

        private static string GetTaxaPedraString(Obra obra, ParametroTaxaExtra parametroTaxaExtra)
        {
            if (obra != null && parametroTaxaExtra != null)
            {
                var taxas = obra.ObraTaxas?.Where(t => t.Tipo == "ACRECIMO PARA ALTERAÇÃO DE PEDRAS" && t.Selecionada == "S") ?? Array.Empty<ObraTaxa>();
                return $"'{parametroTaxaExtra.MensagemAlteracaoPedra} {String.Join("  ", taxas.Select(t => t.Descricao.Replace("\n", "").Replace("\r", "")))}'";
            }
            else
                return "''";
        }

        private static string GetTaxaSlumpString(Obra obra, ParametroTaxaExtra parametroTaxaExtra)
        {
            if (obra != null && parametroTaxaExtra != null)
            {
                var taxas = obra.ObraTaxas?.Where(t => t.Tipo == "ACRECIMO PARA ALTERAÇÃO DE SLUMP" && t.Selecionada == "S") ?? Array.Empty<ObraTaxa>();
                return $"'{parametroTaxaExtra.MensagemAlteracaoSlump} {String.Join("  ", taxas.Select(t => t.Descricao.Replace("\n", "").Replace("\r", "")))}'";
            }
            else
                return "''";
        }

        private static List<KeyValuePair<string, string>> GetDadosTaxaExtra(Obra obra, string rptName)
        {
            var dadosTaxaExtra = new List<KeyValuePair<string, string>>();

            var relatoriosComDadosTaxaExtra = ";CONR6111SUPERCONCRETO.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111CONCREF.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111FM.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111CONCRETAR.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111CAMPEAO.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111IMPERMIX.RPT;";
            relatoriosComDadosTaxaExtra += ";CONR6111SANTAEFIGENIA.RPT;";

            if (relatoriosComDadosTaxaExtra.ToUpper().Contains($";{rptName.ToUpper()};"))
            {
                if (obra?.ObraTracos?.Count() > 0)
                {
                    dadosTaxaExtra.Add(new KeyValuePair<string, string>("3_campo1", string.Format("{0:#.00}", obra.ObraTracos.Sum(t => t.M3Quantidade))));
                }

                if (obra?.ObraTaxas?.Count() > 0)
                {
                    // FirstOrDefault por no Desktop só pega o primeiro registro
                    var taxa = obra.ObraTaxas.Where(t => t.Tipo == "ACRECIMO PARA ALTERAÇÃO DE PEDRAS" && t.Selecionada == "S").FirstOrDefault();
                    if (taxa != null)
                    {
                        var descricao = (taxa.ValorTipo == "R$"
                            ? $"R$ {string.Format("{0:#.00}", taxa.Valor)} por {taxa.ValorPor}"
                            : $"{string.Format("{0:#.00}", taxa.Valor)} {taxa.ValorTipo} por {taxa.ValorPor}");

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-2-1_campo1", descricao));
                    }

                    var taxas = obra.ObraTaxas.Where(t => t.Tipo == "ACRECIMO PARA ALTERAÇÃO DE SLUMP" && t.Selecionada == "S");
                    if (taxas?.Count() > 0)
                    {
                        foreach (var item in taxas)
                        {
                            var chave = "";

                            switch (item.SlumpPara)
                            {
                                case "12": chave = "4-2-2_campo2"; break;
                                case "14": chave = "4-2-2_campo3"; break;
                                case "15": chave = "4-2-2_campo4"; break;
                                case "18": chave = "4-2-2_campo5"; break;
                                case "20": chave = "4-2-2_campo6"; break;
                                default: continue;
                            }

                            var descricao = (item.ValorTipo == "R$"
                                ? $"R$ {string.Format("{0:#.00}", item.Valor)} por {item.ValorPor}"
                                : $"{string.Format("{0:#.00}", item.Valor)} {item.ValorTipo} por {item.ValorPor}");

                            dadosTaxaExtra.Add(new KeyValuePair<string, string>(chave, descricao));
                        }
                    }

                    taxa = obra.ObraTaxas.Where(t => t.Tipo == "ADICIONAL NOTURNO" && t.Selecionada == "S"
                        && t.QuandoDe.ToUpper() == "SEGUNDA FEIRA"
                        && t.QuandoOperacao.ToUpper() == "ATÉ"
                        && t.QuandoAte.ToUpper() == "SEXTA FEIRA").FirstOrDefault();
                    if (taxa != null)
                    {
                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-4_campo1", $"{string.Format("{0:00}", taxa.HorarioAposAs)}:00"));

                        var descricao = (taxa.ValorTipo == "R$"
                            ? $"R$ {string.Format("{0:#.00}", taxa.Valor)} por {taxa.ValorPor}"
                            : $"{string.Format("{0:#.00}", taxa.Valor)}{taxa.ValorTipo} por {taxa.ValorPor}");

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-4_campo2", descricao));
                    }

                    taxa = obra.ObraTaxas.Where(t => t.Tipo == "ADICIONAL NOTURNO" && t.Selecionada == "S"
                        && t.QuandoDe.ToUpper() == "SÁBADO"
                        && t.QuandoOperacao.ToUpper() == "ATÉ"
                        && t.QuandoAte.ToUpper() == "SÁBADO").FirstOrDefault();
                    if (taxa != null)
                    {
                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-4_campo3", $"{string.Format("{0:00}", taxa.HorarioAposAs)}:00"));

                        var descricao = (taxa.ValorTipo == "R$"
                            ? $"R$ {string.Format("{0:#.00}", taxa.Valor)} por {taxa.ValorPor}"
                            : $"{string.Format("{0:#.00}", taxa.Valor)} {taxa.ValorTipo} por {taxa.ValorPor}");

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-4_campo4", descricao));
                    }

                    taxa = obra.ObraTaxas.Where(t => t.Tipo == "ADICIONAL DOMINGOS E FERIANDOS" && t.Selecionada == "S"
                        && t.QuandoOperacao.ToUpper() == "E/OU"
                        && ((t.QuandoDe.ToUpper() == "DOMINGO" && t.QuandoAte.ToUpper() == "FERIADO")
                        || (t.QuandoDe.ToUpper() == "FERIADO" && t.QuandoAte.ToUpper() == "DOMINGO"))).FirstOrDefault();
                    if (taxa != null)
                    {
                        var descricao = (taxa.ValorTipo == "R$"
                            ? $"R$ {string.Format("{0:#.00}", taxa.Valor)} por {taxa.ValorPor}"
                            : $"{string.Format("{0:#.00}", taxa.Valor)}{taxa.ValorTipo} por {taxa.ValorPor}");

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-4_campo5", descricao));
                    }

                    taxa = obra.ObraTaxas.Where(t => t.Tipo == "M3 FALTANTE" && t.Selecionada == "S"
                        && t.CobrarVolume.ToUpper() == "VOLUME INFERIOR").FirstOrDefault();
                    if (taxa != null)
                    {
                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-6_campo1", $"{taxa.Volume} M3"));

                        var descricao = (taxa.ValorTipo == "R$"
                            ? $"R$ {string.Format("{0:#.00}", taxa.Valor)}"
                            : $"{string.Format("{0:#.00}", taxa.Valor)} {taxa.ValorTipo}");

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-6_campo2", descricao));
                    }
                }

                if (obra?.ObraBombas?.Count() > 0)
                {
                    foreach (var bomba in obra.ObraBombas)
                    {
                        if (bomba.BombaTipoCodigo != 6103)
                        {
                            dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-7_campo1", $"{string.Format("{0:#.00}", bomba.M3PrecoProposto)}"));
                            dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-7_campo2", $"R$ {string.Format("{0:#.00}", bomba.TaxaMinimaPrecoProposto)} por até {bomba.M3PropostoAte}"));
                        }
                        else
                        {
                            dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-8_campo1", $"{string.Format("{0:#.00}", bomba.M3PrecoProposto)}"));
                            dadosTaxaExtra.Add(new KeyValuePair<string, string>("4-8_campo2", $"R$ {string.Format("{0:#.00}", bomba.TaxaMinimaPrecoProposto)} por até {bomba.M3PropostoAte}"));
                        }

                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("vlrM3bombeado", $"R$ {string.Format("{0:#.00}", bomba.M3PrecoProposto)}"));
                        dadosTaxaExtra.Add(new KeyValuePair<string, string>("m3bombeadoAte", $"{bomba.M3PropostoAte}"));
                    }
                }
            }

            return dadosTaxaExtra;
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

        public static void ConfigurarContratoReport(ref ReportClass report, string rptName, int idUsinaContrato, int contratoAno, int contratoNumero, IObraService obraService, IObraTaxaService obraTaxaService, IEmpresaService empresaService, IParametroService parametroService, IContratoService contratoService, IContratoVersaoService contratoVersaoService)
        {
            var obra = obraService.CarregarDadosObraParaContratoReport(idUsinaContrato, contratoAno, contratoNumero, obraTaxaService);

            var empresaCodigo = (obra?.UsinaEntrega?.FilialCodigo ?? 0) / 1000;
            var empresa = empresaService.CarregarDadosEmpresaParaContratoReport(empresaCodigo);

            var parametroTaxaExtra = parametroService.ObterPorDataBase<ParametroTaxaExtra>(DateTime.Now);

            var parametroDadosFilial = parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            var utilizaDadosFilial = parametroDadosFilial.DadosFilialNaImpressaoProposta ? "1" : "0";

            if (report.RecordSelectionFormula.Trim() != "") report.RecordSelectionFormula += " and ";
            report.RecordSelectionFormula += ("{con_contrato.usina} = " + (obra?.UsinaCodigo ?? 0));
            report.RecordSelectionFormula += (" and {con_contrato.ano_contrato} = " + (obra?.AnoContrato ?? 0));
            report.RecordSelectionFormula += (" and {con_contrato.num_contrato} = " + (obra?.NumContrato ?? 0));
            report.RecordSelectionFormula += (" and {ger_empresa.emp} = " + empresaCodigo);

            var dadosTaxaExtra = GetDadosTaxaExtra(obra, rptName);

            var numProximaVersao = contratoService.GetUltimaVersaoContrato(idUsinaContrato, contratoAno, contratoNumero);
            DateTime? dataVersaoCriada = obra.Contrato.DataContrato;
         
            if (numProximaVersao > 0)
            {
                if (numProximaVersao > 1)
                {
                    if (contratoVersaoService.ExisteVersaoEmAberto(idUsinaContrato, contratoAno, contratoNumero))
                        numProximaVersao -= 1;
                }

                dataVersaoCriada = contratoService.GetDataCriacaoVersaoContrato(numProximaVersao, idUsinaContrato, contratoAno, contratoNumero);
                if (dataVersaoCriada == null || dataVersaoCriada == DateTime.MinValue)
                {
                    dataVersaoCriada = obra.Contrato.DataContrato;
                }

                if (report.ResourceName == "ContratoResidualReportConcrebras.rpt")
                    report.RecordSelectionFormula += " AND {con_proposta_item_versao.num_versao}=" + numProximaVersao;
            }

            foreach (FormulaFieldDefinition formula in report.DataDefinition.FormulaFields)
            {
                switch (formula.Name.ToLower())
                {
                    case "bomba1":
                    case "bomba2":
                    case "bomba3":
                    case "bomba4":
                        formula.Text = $"{GetCondicaoBombaString(obra, formula.Name)}";
                        break;
                    case "tp_bomba":
                        formula.Text = $"{GetTiposBombaString(obra)}";
                        break;
                    case "mes_padrao1":
                        formula.Text = $"{GetMensagensPadraoString(obra)}";
                        break;
                    case "concorrente":
                        formula.Text = $"{GetConcorrenteString(obra)}";
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
                    case "previsao_vlr_bomba":
                        formula.Text = $"{GetPrevisaoValorBombaString(obra)}";
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
                    case "dadosbomba1":
                    case "dadosbomba2":
                    case "dadosbomba3":
                    case "dadosbomba4":
                        formula.Text = $"{GetDadosBombaString(obra, formula.Name)}";
                        break;
                    case "utilizadadosfilial":
                        formula.Text = $"{utilizaDadosFilial}";
                        break;
                    case "dt_contrato_versao":
                        formula.Text = $"'{dataVersaoCriada.Value.ToString("dd/MM/yyyy")}'";
                        break;
                    default:
                        foreach (var taxa in dadosTaxaExtra)
                        {
                            if (formula.Name.ToLower() == taxa.Key.ToLower())
                            {
                                formula.Text = $"'{taxa.Value}'";
                                break;
                            }
                        }
                        break;
                }
            }

            var subReport = report.OpenSubreport("rpt_contrato_tx");

            if (subReport != null)
            {
                foreach (FormulaFieldDefinition formula in subReport.DataDefinition.FormulaFields)
                {
                    switch (formula.Name.ToLower())
                    {
                        case "slump":
                            formula.Text = $"{GetTaxaSlumpString(obra, parametroTaxaExtra)}";
                            break;
                        case "pedra":
                            formula.Text = $"{GetTaxaPedraString(obra, parametroTaxaExtra)}";
                            break;
                        default:
                            break;
                    }
                }
            }

        }
    }
}
