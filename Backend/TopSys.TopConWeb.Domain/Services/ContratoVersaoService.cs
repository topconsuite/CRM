using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Enums;
using System.Globalization;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ContratoVersaoService : IContratoVersaoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IParametroService _parametroService;
        private readonly IObraService _obraService;
        private readonly IPropostaRepository _propostaRepository;
        private readonly IArquivoService _arquivoService; 
        private readonly IObraTaxaRepository _obraTaxaRepository;

        private const int NUMERO_APLICACAO_PROPOSTA_LISTA = 6101;
        private string[] mensagem = new string[21];

        public ContratoVersaoService(IContratoRepository contratoRepository, IParametroService parametroService, 
            IObraService obraService, IPropostaRepository propostaRepository, IArquivoService arquivoService, 
            IObraTaxaRepository taxaExtraRepository)
        {
            _contratoRepository = contratoRepository;
            _parametroService = parametroService;
            _obraService = obraService;
            _propostaRepository = propostaRepository;
            _arquivoService = arquivoService; 
            _obraTaxaRepository = taxaExtraRepository;
        }

        public ICollection<ContratoVersao> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato)
        {

            var parametroGeraAditivoContratoSemAprovCadastro = _parametroService.ObterParametroN("web", "GeraAditivoContratoSemAprovCadastro").Equals("1");

            return _contratoRepository.ListarContratoVersoesAprovados(codUsina, anoContrato, numeroContrato, parametroGeraAditivoContratoSemAprovCadastro);
        }

        public string[] ObterAditivoReport(int versao, int codUsina, int anoProposta, int numeroProposta, out string tracosAlterados)
        {
            tracosAlterados = "";

            var propostas = _propostaRepository.ListarPropostaVersoes(versao, codUsina, anoProposta, numeroProposta);

            var propostaAtual = propostas.Where(x => x.NumeroVersao == versao).FirstOrDefault();
            var propostaAnterior = propostas.Where(x => x.NumeroVersao < versao).OrderByDescending(t => t.NumeroVersao).FirstOrDefault();

            var versionamentoTraco = _parametroService.ObterParametroN("web", "VersionamentoTraco").Contains("true");
            var versionamentoBomba = _parametroService.ObterParametroN("web", "VersionamentoBomba").Contains("true");
            var versionamentoTaxaExtra = _parametroService.ObterParametroN("web", "VersionamentoTaxaExtra").Contains("true");
            var versionamentoCondicaoPagamento = _parametroService.ObterParametroN("web", "VersionamentoCondicaoPagamento").Contains("true");
            var versionamentoEnderecoObra = _parametroService.ObterParametroN("web", "VersionamentoEnderecoObra").Contains("true");
            var versionamentoDemaisServicos = _parametroService.ObterParametroN("web", "VersionamentoDemaisServicos").Contains("true");
            var versionamentoReajusteContrato = _parametroService.ObterParametroN("web", "VersionamentoReajusteContrato").Contains("true");

            for(var contagem = 0;contagem < mensagem.Length;contagem++)
            {
                mensagem[contagem] = "";
            }

            if (versionamentoTraco)
            {
                HouveAlteracaoTraco(propostaAtual, propostaAnterior, out tracosAlterados) ;
            }
            if (versionamentoBomba)
            {
                mensagem[15] = HouveAlteracaoBomba(propostaAtual, propostaAnterior);
            }
            if (versionamentoTaxaExtra)
            {
                mensagem[16] = HouveAlteracaoTaxaExtra(propostaAtual, propostaAnterior);
            }
            if (versionamentoCondicaoPagamento)
            {
                mensagem[17] = HouveAlteracaoCondicaoPagamento(propostaAtual, propostaAnterior);
            }
            if (versionamentoEnderecoObra)
            {
                mensagem[18] = HouveAlteracaoEnderecoObra(propostaAtual, propostaAnterior);
            }
            if (versionamentoDemaisServicos)
            {
                mensagem[19] = HouveAlteracaoDemaisServicos(propostaAtual, propostaAnterior);
            }
            // || (parametro[6].ToString() == "true")

            return mensagem;
        }

        private string HouveAlteracaoEnderecoObra(PropostaVersao propostaAtual, PropostaVersao propostaAnterior)
        {
            string mensagem = "";

            if (propostaAtual.Obra.EnderecoLogradouro != propostaAnterior.Obra.EnderecoLogradouro ||
                propostaAtual.Obra.EnderecoNumero != propostaAnterior.Obra.EnderecoNumero ||
                propostaAtual.Obra.EnderecoCep != propostaAnterior.Obra.EnderecoCep ||
                propostaAtual.Obra.EnderecoComplemento != propostaAnterior.Obra.EnderecoComplemento ||
                propostaAtual.Obra.EnderecoBairro != propostaAnterior.Obra.EnderecoBairro ||
                propostaAtual.Obra.EnderecoMunicipioCodigo != propostaAnterior.Obra.EnderecoMunicipioCodigo)
            {
                mensagem = $"Houve alteração no endereço da obra";

                return mensagem;
            }
            else { return mensagem; }
        }

        private void HouveAlteracaoTraco(PropostaVersao propostaAtual, PropostaVersao propostaAnterior, out string tracosAlterados)
        {
            tracosAlterados = "";

            var quantidadeTraco = 0;
            var quantidadeTracoExcluido = 0;

            var customReportName = _parametroService.ObterParametroN("web", "RelAditivo");

            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaAtual.Obra.ObraTracos)
            {
                sequencias.Add(tDto.Sequencia);
                var tracoAnterior = _obraService.ListarFiltradosTracking<ObraTracoVersao>(t => t.NumeroVersao == propostaAnterior.NumeroVersao && t.UsinaCodigo == propostaAnterior.Obra.UsinaCodigo && t.ObraCodigo == tDto.ObraCodigo && t.Sequencia == tDto.Sequencia,
                                t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                                .FirstOrDefault();

                if (tracoAnterior != null)
                {
                    if (tDto.M3PrecoProposto != tracoAnterior.M3PrecoProposto || tDto.Uso.Codigo != tracoAnterior.UsoCodigo
                        || tDto.Pedra.Codigo != tracoAnterior.PedraCodigo || tDto.SlumpNominal.Codigo != tracoAnterior.SlumpNominalCodigo
                        || tDto.Slump.Codigo != tracoAnterior.SlumpCodigo || tDto.ResistenciaTipo.Codigo != tracoAnterior.ResistenciaTipoCodigo
                        || tDto.Fck != tracoAnterior.Fck || tDto.Consumo != tracoAnterior.Consumo || tDto.M3Quantidade != tracoAnterior.M3Quantidade
                        || tDto.M3QuantidadeBombeada != tracoAnterior.M3QuantidadeBombeada || tDto.Observacao != tracoAnterior.Observacao
                        || tDto.PecaConcretar != tracoAnterior.PecaConcretar || tDto.Justificativa != tracoAnterior.Justificativa
                        || tDto.PrecoConcorrencia != tracoAnterior.PrecoConcorrencia || tDto.PrecoReajustadoAtual != tracoAnterior.PrecoReajustadoAtual)
                    {
                        if (quantidadeTraco == 0) {
                            quantidadeTraco += 1;

                            tracosAlterados += ";" + tDto.Sequencia + ";";

                            mensagem[0] = $"{tDto.Resistencia}";

                            if (customReportName.Equals("AditivoReportConcrebras.rpt"))
                                if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                                    mensagem[0] += " MPA";

                            mensagem[1] = $"{tDto.Pedra.Descricao}";
                            mensagem[2] = $"{tDto.Slump.Descricao}";
                            mensagem[3] = $"{tDto.Uso.Descricao}";
                            mensagem[4] = $"{tDto.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                            mensagem[5] = $"{tDto.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                            mensagem[6] = $"{(tDto.M3Quantidade * tDto.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                            if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                            {
                                mensagem[7] = $"'{tDto.Uso.Descricao} {tDto.Resistencia} MPA {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                            }
                            else
                            {
                                mensagem[7] = $"'{tDto.Uso.Descricao} {tDto.Resistencia} {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                            }

                        } else if (quantidadeTraco > 0){
                            quantidadeTraco += 1;

                            tracosAlterados += ";" + tDto.Sequencia + ";";

                            mensagem[0] = mensagem[0] + $"ChrW(13){tDto.Resistencia}";

                            if (customReportName.Equals("AditivoReportConcrebras.rpt"))
                                if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                                    mensagem[0] += " MPA";

                            mensagem[1] = mensagem[1] + $"ChrW(13){tDto.Pedra.Descricao}";
                            mensagem[2] = mensagem[2] + $"ChrW(13){tDto.Slump.Descricao}";
                            mensagem[3] = mensagem[3] + $"ChrW(13){tDto.Uso.Descricao}";
                            mensagem[4] = mensagem[4] + $"ChrW(13){tDto.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                            mensagem[5] = mensagem[5] + $"ChrW(13){tDto.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                            mensagem[6] = mensagem[6] + $"ChrW(13){(tDto.M3Quantidade * tDto.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                            if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                            {
                                mensagem[7] = mensagem[7] + $" + ChrW(13) + '{tDto.Uso.Descricao} {tDto.Resistencia} MPA {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                            }
                            else
                            {
                                mensagem[7] = mensagem[7] + $" + ChrW(13) + '{tDto.Uso.Descricao} {tDto.Resistencia} {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                            }
                        }
                    }
                }
                else
                {
                    if (quantidadeTraco == 0)
                    {
                        quantidadeTraco += 1;

                        tracosAlterados += ";" + tDto.Sequencia + ";";

                        mensagem[0] = $"{tDto.Resistencia}";

                        if (customReportName.Equals("AditivoReportConcrebras.rpt"))
                            if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                                mensagem[0] += " MPA";

                        mensagem[1] = $"{tDto.Pedra.Descricao}";
                        mensagem[2] = $"{tDto.Slump.Descricao}";
                        mensagem[3] = $"{tDto.Uso.Descricao}";
                        mensagem[4] = $"{tDto.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                        mensagem[5] = $"{tDto.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                        mensagem[6] = $"{(tDto.M3Quantidade * tDto.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                        if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                        {
                            mensagem[7] = $"'{tDto.Uso.Descricao} {tDto.Resistencia} MPA {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                        }
                        else
                        {
                            mensagem[7] = $"'{tDto.Uso.Descricao} {tDto.Resistencia} {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                        }
                    }
                    else if (quantidadeTraco > 0)
                    {
                        quantidadeTraco += 1;

                        tracosAlterados += ";" + tDto.Sequencia + ";";

                        mensagem[0] = mensagem[0] + $"ChrW(13){tDto.Resistencia}";

                        if (customReportName.Equals("AditivoReportConcrebras.rpt"))
                            if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                                mensagem[0] += " MPA";

                        mensagem[1] = mensagem[1] + $"ChrW(13){tDto.Pedra.Descricao}";
                        mensagem[2] = mensagem[2] + $"ChrW(13){tDto.Slump.Descricao}";
                        mensagem[3] = mensagem[3] + $"ChrW(13){tDto.Uso.Descricao}";
                        mensagem[4] = mensagem[4] + $"ChrW(13){tDto.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                        mensagem[5] = mensagem[5] + $"ChrW(13){tDto.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                        mensagem[6] = mensagem[6] + $"ChrW(13){(tDto.M3Quantidade * tDto.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                        if (tDto.ResistenciaTipo.Vinculo == EResistenciaVinculoTipo.Mpa)
                        {
                            mensagem[7] = mensagem[7] + $" + ChrW(13) + '{tDto.Uso.Descricao} {tDto.Resistencia} MPA {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                        }
                        else
                        {
                            mensagem[7] = mensagem[7] + $" + ChrW(13) + '{tDto.Uso.Descricao} {tDto.Resistencia} {tDto.Pedra.Descricao} {tDto.Slump.Descricao}'";
                        }
                    }
                }
            }
            var seqs = sequencias.ToArray();
            var tracosExcluidos = _obraService.ListarFiltradosTracking<ObraTracoVersao>
                (t => t.NumeroVersao == propostaAtual.NumeroVersao - 1
                    && t.UsinaCodigo == propostaAtual.Usina.Codigo
                    && t.ObraCodigo == propostaAtual.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach( var tracoExcluido in tracosExcluidos)
            {
                if (quantidadeTracoExcluido == 0)
                {
                    quantidadeTracoExcluido += 1;

                    mensagem[8] = $"{tracoExcluido.Resistencia}";
                    mensagem[9] = $"{tracoExcluido.Pedra.Descricao}";
                    mensagem[10] = $"{tracoExcluido.Slump.Descricao}";
                    mensagem[11] = $"{tracoExcluido.Uso.Descricao}";
                    mensagem[12] = $"{tracoExcluido.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                    mensagem[13] = $"{tracoExcluido.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                    mensagem[14] = $"{(tracoExcluido.M3Quantidade * tracoExcluido.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                }
                else if (quantidadeTracoExcluido > 0)
                {
                    quantidadeTracoExcluido += 1;

                    mensagem[8] = mensagem[8] + $"ChrW(13){tracoExcluido.Resistencia}";
                    mensagem[9] = mensagem[9] + $"ChrW(13){tracoExcluido.Pedra.Descricao}";
                    mensagem[10] = mensagem[10] + $"ChrW(13){tracoExcluido.Slump.Descricao}";
                    mensagem[11] = mensagem[11] + $"ChrW(13){tracoExcluido.Uso.Descricao}";
                    mensagem[12] = mensagem[12] + $"ChrW(13){tracoExcluido.M3Quantidade.ToString("N2", new CultureInfo("pt-BR"))}";
                    mensagem[13] = mensagem[13] + $"ChrW(13){tracoExcluido.M3PrecoProposto.ToString("N2", new CultureInfo("pt-BR"))}";
                    mensagem[14] = mensagem[14] + $"ChrW(13){(tracoExcluido.M3Quantidade * tracoExcluido.M3PrecoProposto).ToString("N2", new CultureInfo("pt-BR"))}";
                }
            }
        }

        private string HouveAlteracaoBomba(PropostaVersao propostaAtual, PropostaVersao propostaAnterior)
        {
            var quantidadeBombasAlteradas = 0;
            var quantidadeBombasInseridas = 0;
            var quantidadeBombasExcluidas = 0;

            string mensagemAlteradas = "";
            string mensagemInseridas = "";
            string mensagemExcluidas = "";
            string mensagem = "";

            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaAtual.Obra.ObraBombas)
            {
                sequencias.Add(tDto.Sequencia);
                var bombaAnterior = _obraService.ObterPorId<ObraBombaVersao>
                        (propostaAnterior.NumeroVersao, tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                if (bombaAnterior != null)
                {
                    if (tDto.BombaPropria != bombaAnterior.BombaPropria || tDto.BombaTipoCodigo != bombaAnterior.BombaTipoCodigo
                        || tDto.TaxaMinimaPrecoProposto != bombaAnterior.TaxaMinimaPrecoProposto || tDto.M3PrecoProposto != bombaAnterior.M3PrecoProposto
                        || tDto.M3PropostoAte != bombaAnterior.M3PropostoAte || tDto.Justificativa != bombaAnterior.Justificativa
                        || tDto.DistanciaTubulacao != bombaAnterior.DistanciaTubulacao)
                    {
                        if (tDto.TipoCalculo != EBombaM3CalculoTipo.SemCobranca)
                        {
                            mensagemAlteradas = $"ChrW(13)  - Serviço de bombeamento com {tDto.BombaTipo?.Descricao}:";
                            mensagemAlteradas += $" Para bombeamentos inferiores a {tDto.M3PropostoAte} m³ será cobrado taxa de R$ {string.Format("{0:#.00}", tDto.TaxaMinimaPrecoProposto)}";
                            mensagemAlteradas += $" e acima de {tDto.M3PropostoAte} m³ serão cobrados R$ {string.Format("{0:#.00}", tDto.M3PrecoProposto)} por m³ lançado";
                            mensagemAlteradas += $"{(tDto.TipoCalculo == Domain.Enums.EBombaM3CalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";
                        }
                        else
                        {
                            mensagemAlteradas = $"ChrW(13)  - Serviço de bombeamento com {tDto.BombaTipo?.Descricao}:";
                            mensagemAlteradas += $" Para bombeamentos inferiores a {tDto.HoraPropostoAte} horas será cobrado taxa de R$ {string.Format("{0:#.00}", tDto.HoraTaxaMinimaPrecoProposto)}";
                            mensagemAlteradas += $" e acima de {tDto.HoraPropostoAte} horas serão cobrados R$ {string.Format("{0:#.00}", tDto.HoraPrecoProposto)} por hora lançada";
                            mensagemAlteradas += $"{(tDto.HoraTipoCalculo == Domain.Enums.EBombaHoraCalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";
                        }
                        quantidadeBombasAlteradas += 1;
                    }
                }
                else
                {
                    if (tDto.TipoCalculo != EBombaM3CalculoTipo.SemCobranca)
                    {
                        mensagemInseridas = $"ChrW(13)  - Serviço de bombeamento com {tDto.BombaTipo?.Descricao}:";
                        mensagemInseridas += $" Para bombeamentos inferiores a {tDto.M3PropostoAte} m³ será cobrado taxa de R$ {string.Format("{0:#.00}", tDto.TaxaMinimaPrecoProposto)}";
                        mensagemInseridas += $" e acima de {tDto.M3PropostoAte} m³ serão cobrados R$ {string.Format("{0:#.00}", tDto.M3PrecoProposto)} por m³ lançado";
                        mensagemInseridas += $"{(tDto.TipoCalculo == Domain.Enums.EBombaM3CalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";
                    }
                    else
                    {
                        mensagemInseridas = $"ChrW(13)  - Serviço de bombeamento com {tDto.BombaTipo?.Descricao}:";
                        mensagemInseridas += $" Para bombeamentos inferiores a {tDto.HoraPropostoAte} horas será cobrado taxa de R$ {string.Format("{0:#.00}", tDto.HoraTaxaMinimaPrecoProposto)}";
                        mensagemInseridas += $" e acima de {tDto.HoraPropostoAte} horas serão cobrados R$ {string.Format("{0:#.00}", tDto.HoraPrecoProposto)} por hora lançada";
                        mensagemInseridas += $"{(tDto.HoraTipoCalculo == Domain.Enums.EBombaHoraCalculoTipo.TaxaMinimaOuExcedente ? ", eliminado-se a taxa inicial." : ".")}";
                    }
                    quantidadeBombasInseridas += 1;
                }
            }

            var seqs = sequencias.ToArray();
            var bombasExcluidas = _obraService.ListarFiltradosTracking<ObraBombaVersao>
                    (t => t.NumeroVersao == propostaAnterior.NumeroVersao
                    && t.UsinaCodigo == propostaAtual.Usina.Codigo
                    && t.ObraCodigo == propostaAtual.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var bombaExcluida in bombasExcluidas)
            {
                if (bombaExcluida.TipoCalculo != EBombaM3CalculoTipo.SemCobranca)
                {
                    mensagemExcluidas = $"ChrW(13)  - Serviço de bombeamento com {bombaExcluida.BombaTipo?.Descricao}:";
                    mensagemExcluidas += $" Para bombeamentos inferiores a {bombaExcluida.M3PropostoAte} m³";
                    mensagemExcluidas += $" e acima de {bombaExcluida.M3PropostoAte} m³.";
                }
                else
                {
                    mensagemExcluidas = $"ChrW(13)  - Serviço de bombeamento com {bombaExcluida.BombaTipo?.Descricao}:";
                    mensagemExcluidas += $" Para bombeamentos inferiores a {bombaExcluida.HoraPropostoAte} horas";
                    mensagemExcluidas += $" e acima de {bombaExcluida.HoraPropostoAte} horas.";
                }
                quantidadeBombasExcluidas += 1;
            }

            if (quantidadeBombasAlteradas == 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alteração da bomba: " + mensagemAlteradas;
            }
            else if (quantidadeBombasAlteradas > 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alterações das bombas: " + mensagemAlteradas;
            }

            if (quantidadeBombasInseridas == 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Bomba adicionada: " + mensagemInseridas;
            }
            else if (quantidadeBombasInseridas > 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Bombas adicionadas: " + mensagemInseridas;
            }

            if (quantidadeBombasExcluidas == 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Bomba excluida: " + mensagemExcluidas;
            }
            else if (quantidadeBombasExcluidas > 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Bombas excluidas: " + mensagemExcluidas;
            }

            return mensagem;
        }

        private string HouveAlteracaoTaxaExtra(PropostaVersao propostaAtual, PropostaVersao propostaAnterior)
        {
            var quantidadeTaxasAlteradasAtivas = 0;
            var quantidadeTaxasAlteradasInativas = 0;
            var quantidadeTaxasAlteradas = 0;
            var quantidadeTaxasInseridas = 0;
            var quantidadeTaxasExcluidas = 0;

            string mensagemAlteradasAtivas = "";
            string mensagemAlteradasInativas = "";
            string mensagemAlteradas = "";
            string mensagemInseridas = "";
            string mensagemExcluidas = "";
            string mensagem = "";

            var taxasAnteriores = _obraTaxaRepository.ListarByIdObra(propostaAnterior.Obra.UsinaEntrega.Codigo, propostaAnterior.ObraCodigo, propostaAnterior.NumeroVersao, propostaAnterior.Segmentacao);

            foreach (var tDto in propostaAtual.Obra.ObraTaxas)
            {
                /* var taxaAnterior = _obraService.ObterPorId<ObraTaxaVersao>
                         (propostaAnterior.NumeroVersao, propostaAnterior.Obra.UsinaEntrega.Codigo, propostaAnterior.ObraCodigo, tDto.Sequencia);*/
                var taxaAnterior = taxasAnteriores.FirstOrDefault(t => t.Sequencia == tDto.Sequencia);

                if (taxaAnterior != null)
                {
                    if (tDto.Selecionada != taxaAnterior.Selecionada)
                    {
                        if (tDto.Selecionada == "S")
                        {
                            mensagemAlteradasAtivas += $"ChrW(13)  - {tDto.Descricao}";
                            quantidadeTaxasAlteradasAtivas += 1;
                        } else if (tDto.Selecionada == "N")
                        {
                            mensagemAlteradasInativas += $"ChrW(13)  - {tDto.Descricao}";
                            quantidadeTaxasAlteradasInativas += 1;
                        } 
                    }
                    if (tDto.IsPersonalizada && tDto.Selecionada == "S")
                    {
                        if (tDto.Descricao != taxaAnterior.Descricao)
                        {
                            mensagemAlteradas += $"ChrW(13)  - {tDto.Descricao}";
                            quantidadeTaxasAlteradas += 1;
                        }
                    }
                }
                else if (tDto.Selecionada == "S" && tDto.Nova)
                {
                    mensagemInseridas += $"ChrW(13)  - {tDto.Descricao}";
                    quantidadeTaxasInseridas += 1;
                }
            }

            if (quantidadeTaxasAlteradas == 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alteração da taxa: " + mensagemAlteradas;
            }
            else if (quantidadeTaxasAlteradas > 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alterações das taxas: " + mensagemAlteradas;
            }

            if (quantidadeTaxasAlteradasAtivas == 1 && mensagemAlteradasAtivas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxa ativada: " + mensagemAlteradasAtivas;
            }
            else if (quantidadeTaxasAlteradasAtivas > 1 && mensagemAlteradasAtivas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxas ativadas: " + mensagemAlteradasAtivas;
            }

            if (quantidadeTaxasAlteradasInativas == 1 && mensagemAlteradasInativas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxa inativada: " + mensagemAlteradasInativas;
            }
            else if (quantidadeTaxasAlteradasInativas > 1 && mensagemAlteradasInativas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxas Inativadas: " + mensagemAlteradasInativas;
            }

            if (quantidadeTaxasInseridas == 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxa adicionada: " + mensagemInseridas;
            }
            else if (quantidadeTaxasInseridas > 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxas adicionadas: " + mensagemInseridas;
            }

            if (quantidadeTaxasExcluidas == 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxa excluida: " + mensagemExcluidas;
            }
            else if (quantidadeTaxasExcluidas > 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Taxas excluidas: " + mensagemExcluidas;
            }

            return mensagem;
        }

        private string HouveAlteracaoDemaisServicos(PropostaVersao propostaAtual, PropostaVersao propostaAnterior)
        {
            string mensagemAlteradas = "";
            string mensagemInseridas = "";
            string mensagemExcluidas = "";
            string mensagem = "";

            List<int> sequencias = new List<int>();

            if (propostaAtual.Obra.VibradorQuantidade != propostaAnterior.Obra.VibradorQuantidade || propostaAtual.Obra.VibradorValorUnitario != propostaAnterior.Obra.VibradorValorUnitario)
            {
                mensagemAlteradas = $"ChrW(13)  - Vibrador com {propostaAtual.Obra.VibradorQuantidade} de Quantidade e Valor Unitário de R$ {string.Format("{0:#.00}", propostaAtual.Obra.VibradorValorUnitario)}";
            }

            foreach (var tDto in propostaAtual.Obra.ObraDemaisServicos)
            {
                sequencias.Add(tDto.Sequencia);
                var demaisServicosAnterior = _obraService.ObterPorId<ObraDemaisServicosVersao>
                        (propostaAnterior.NumeroVersao, propostaAtual.Obra.UsinaCodigo, propostaAtual.Obra.Numero, tDto.Sequencia);

                if (demaisServicosAnterior != null)
                {
                    if (tDto.Mercadoria.Codigo != demaisServicosAnterior.MercadoriaCodigo || tDto.PrecoProposto != demaisServicosAnterior.PrecoProposto
                        || tDto.Quantidade != demaisServicosAnterior.Quantidade || tDto.AtualizaEstoque != demaisServicosAnterior.AtualizaEstoque)
                    {
                        mensagemAlteradas += $"ChrW(13)  - {tDto.Mercadoria.Descricao}";
                    }
                }
                else
                {
                    mensagemInseridas += $"ChrW(13)  - {tDto.Mercadoria.Descricao}";
                }
            }
            var seqs = sequencias.ToArray();
            var demaisServicosExcluidos = _obraService.ListarFiltradosTracking<ObraDemaisServicosVersao>
                    (t => t.NumeroVersao == propostaAnterior.NumeroVersao
                        && t.UsinaCodigo == propostaAtual.Usina.Codigo
                        && t.ObraNumero == propostaAtual.Obra.Numero
                        && !seqs.Contains(t.Sequencia));

            foreach (var servicoExcluido in demaisServicosExcluidos)
            {
                mensagemExcluidas += $"ChrW(13)  - {servicoExcluido.Mercadoria.Descricao}.";
            }


            if (mensagemAlteradas != "")
            {
                mensagem += $"Alteração de Demais Serviços: " + mensagemAlteradas;
            }

            if (mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Demais Serviços adicionados: " + mensagemInseridas;
            }

            if (mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Demais Serviços Excluidos: " + mensagemExcluidas;
            }

            return mensagem;
        }

        private string HouveAlteracaoCondicaoPagamento(PropostaVersao propostaAtual, PropostaVersao propostaAnterior)
        {
            var quantidadeCondPagAlteradas = 0;
            var quantidadeCondPagInseridas = 0;
            var quantidadeCondPagExcluidas = 0;

            string mensagemAlteradas = "";
            string mensagemInseridas = "";
            string mensagemExcluidas = "";
            string mensagem = "";

            var sequencias = new List<int>();
            foreach (var tDto in propostaAtual.Obra.ObraPagamentos)
            {
                sequencias.Add(tDto.Sequencia);
                var contratoPagamentoAnterior = _obraService.ObterPorId<ContratoPagamentoVersao>
                            (propostaAnterior.NumeroVersao, propostaAtual.Usina.Codigo, propostaAtual.Obra.AnoContrato, propostaAtual.Obra.NumContrato, tDto.Sequencia);

                if (contratoPagamentoAnterior != null)
                {
                    if (contratoPagamentoAnterior.CondicaoPagamentoCodigo != tDto.CondicaoPagamento.Codigo || contratoPagamentoAnterior.Forma != tDto.TipoCobranca.Forma)
                    {
                        mensagemAlteradas += $"ChrW(13)  - {tDto.CondicaoPagamento.Descricao}";
                        quantidadeCondPagAlteradas += 1;
                    }
                }
                else
                {
                    mensagemInseridas += $"ChrW(13)  - {tDto.CondicaoPagamento.Descricao}";
                    quantidadeCondPagInseridas += 1;
                }

            }
            var seqs = sequencias.ToArray();
            var contratoPagamentosExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoVersao>
                    (t => t.NumeroVersao == propostaAnterior.NumeroVersao
                        && t.UsinaCodigo == propostaAtual.Usina.Codigo
                        && t.ContratoAno == (propostaAtual.Obra.AnoContrato ?? 0)
                        && t.ContratoNumero == (propostaAtual.Obra.NumContrato ?? 0)
                        && !seqs.Contains(t.Sequencia));

            foreach (var pagamentoExcluido in contratoPagamentosExcluidos)
            {
                mensagemExcluidas += $"ChrW(13)  - {pagamentoExcluido.CondicaoPagamento.Descricao}";
                quantidadeCondPagExcluidas += 1;
            }

            if (quantidadeCondPagAlteradas == 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alteração da Condição de Pagamento: " + mensagemAlteradas;
            }
            else if (quantidadeCondPagAlteradas > 1 && mensagemAlteradas != "")
            {
                mensagem = $"Alterações das Condições de Pagamento: " + mensagemAlteradas;
            }

            if (quantidadeCondPagInseridas == 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Condição de Pagamento adicionada: " + mensagemInseridas;
            }
            else if (quantidadeCondPagInseridas > 1 && mensagemInseridas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Condições de Pagamento adicionadas: " + mensagemInseridas;
            }

            if (quantidadeCondPagExcluidas == 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Condição de Pagamento excluida: " + mensagemExcluidas;
            }
            else if (quantidadeCondPagExcluidas > 1 && mensagemExcluidas != "")
            {
                if (mensagem != "")
                {
                    mensagem += $"ChrW(13)";
                }
                mensagem += $"Condições de Pagamento excluidas: " + mensagemExcluidas;
            }

            return mensagem;
        }

        public int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetUltimaVersaoContratoAprovado(codUsina, anoContrato, numeroContrato);
        }

        public void SalvarPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato, Stream contrato)
        {
            var chave = ObterChaveContratoVersao(versao, codUsina, anoContrato, numeroContrato);
            _arquivoService.SalvarArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave, contrato);
        }

        private string ObterChaveContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato)
        {
            return $"{codUsina};{numeroContrato};{anoContrato};{versao}";
        }

        public Stream ObterPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato)
        { 
            var chave = ObterChaveContratoVersao(versao, codUsina, anoContrato, numeroContrato);
            return _arquivoService.ObterArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave);
        }

        public bool ExisteVersaoEmAberto(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.ExisteVersaoEmAberto(codUsina, anoContrato, numeroContrato);
        }
    }
}
