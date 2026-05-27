using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ObraScopes
    {
        public static bool CepAtendidoScopeIsValid(float? valor)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(valor, "obraCepNaoAtendido", "Cep não atendido pela usina!")
            );
        }

        public static bool KmAtendidoScopeIsValid(float? valor)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(valor, "obraDistanciaUsinaNaoAtendido", "Distancia não atendida pela usina!")
            );
        }

        public static bool ValorTotalPagamentosScopeIsValid(this Obra obra, float valorExtras, bool NaoConsideraTodasBombas = false)
        {
            var valorTolerancia = 10f;
            var valorPagamentos = obra.ObraPagamentos?.Where(t => t.Ativo).Sum(t => t.Valor) ?? 0f;

            // acrescentando o valor de tolerancia ao valor dos pagamentos
            valorPagamentos = (float)Math.Round(valorPagamentos + valorTolerancia, 2);

            var valorTotal = (float)Math.Round(obra.CalcularValorConcreto() + obra.CalcularValorBomba(NaoConsideraTodasBombas) + (decimal)valorExtras, 2);

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(valorPagamentos, valorTotal, "obraPagamentos", $"Valor dos pagamentos inferior ao valor dos itens da proposta!")
            );
        }

        public static bool ValorTotalPagamentosScopeIsValid(this ObraVersao obra, IObraService obraService, float valorExtras, bool NaoConsideraTodasBombas = false)
        {
            var valorTolerancia = 10f;
            var valorPagamentos = obra.ObraPagamentos?.Where(t => t.Ativo).Sum(t => t.Valor) ?? 0f;
            float valorTotal;

            // acrescentando o valor de tolerancia ao valor dos pagamentos
            valorPagamentos = (float)Math.Round(valorPagamentos + valorTolerancia, 2);

            valorTotal = (float)Math.Round(obra.CalcularValorConcreto() + obra.CalcularValorBomba(NaoConsideraTodasBombas) + (decimal)valorExtras, 2);

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(valorPagamentos, valorTotal, "obraPagamentos", $"Valor dos pagamentos inferior ao valor dos itens da proposta!")
            );
        }

        public static bool ImpressaoPropostaIsValid(int usina, int anoChamada, int noChamada, IObraService obraService, IParametroService parametroService)
        {
            var parametroProposta = parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            bool aprovacaoPendente = false;

            if (parametroProposta.BloqueiaImpressaoPropostaContratoPendenteAprovacao)
            {
                var obra = obraService.ListarFiltrados(t => t.UsinaCodigo == usina && t.AnoChamada == anoChamada && t.NumChamada == noChamada)
                .FirstOrDefault();

                aprovacaoPendente = obraService.TemAprovacaoPendente(obra?.UsinaCodigo ?? 0, obra?.Numero ?? 0, obra?.AnoChamada ?? 0, obra?.NumChamada ?? 0);
            }

            var impressaoIsValid = !parametroProposta.BloqueiaImpressaoPropostaContratoPendenteAprovacao || !aprovacaoPendente;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(impressaoIsValid, "impressaoNaoPermitida", $"Não é possível imprimir pois há aprovações comerciais pendentes para essa proposta!")
            );
        }

        public static bool ImpressaoContratoIsValid(int usina, int anoContrato, int noContrato, IObraService obraService, IParametroService parametroService)
        {
            var parametroProposta = parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Today);
            bool aprovacaoPendente = false;

            if (parametroProposta.BloqueiaImpressaoPropostaContratoPendenteAprovacao)
            {
                var obra = obraService.ListarFiltrados(t => t.UsinaCodigo == usina && t.AnoContrato == anoContrato && t.NumContrato == noContrato)
                .FirstOrDefault();

                aprovacaoPendente = obraService.TemAprovacaoPendente(obra?.UsinaCodigo ?? 0, obra?.Numero ?? 0, obra?.AnoChamada ?? 0, obra?.NumChamada ?? 0);
            }

            var impressaoIsValid = !parametroProposta.BloqueiaImpressaoPropostaContratoPendenteAprovacao || !aprovacaoPendente;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(impressaoIsValid, "impressaoNaoPermitida", $"Não é possível imprimir pois há aprovações comerciais pendentes para este contrato!")
            );
        }

        public static bool AprovaPagamentoAntecipadoCartaoDeCreditoIsValid(this Obra obra)
        {
            var contratoPagamentos = obra.ContratoPagamentos?.Where(t =>
                (t.Forma == ("CC") || t.Forma == ("CD"))
                && t.IdAprovacao == ""
                && t.NecessitaAprovacaoSimNao == "S");

            var statusProposta = obra?.Proposta == null || obra?.Proposta.UsinaCodigo != 999 ? (int)Enums.EPropostaStatusCliente.Aprovado : (int)obra.Proposta.StatusProposta;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(statusProposta, (int)Enums.EPropostaStatusCliente.Aprovado, "contasAReceber-propostaStatus", $"Prospota não aprovada pelo Cliente!"),
                AssertionConcern.AssertNotNull(obra.ContratoPagamentos, "contasAReceber-Contrato Pagamentos", $"Contrato Pagamentos nulo!"),
                AssertionConcern.AssertIsGreaterThan(contratoPagamentos.Count(), 0,"contasAReceber-Contrato Pagamentos Cartao", $"Não foi encontrado nenhum pagamento com método Cartão que necessita aprovação!")
            );
        }

        public static bool AprovaPagamentoAntecipadoCartaoDeCreditoIsValid(this ObraVersao obra)
        {
            var contratoPagamentos = obra.ContratoPagamentos?.Where(t =>
                (t.Forma == ("CC") || t.Forma == ("CD"))
                && t.IdAprovacao == ""
                && t.NecessitaAprovacaoSimNao == "S");

            var statusProposta = obra?.Proposta == null || obra?.Proposta.UsinaCodigo != 999 ? (int)Enums.EPropostaStatusCliente.Aprovado : (int)obra.Proposta.StatusProposta;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(statusProposta, (int)Enums.EPropostaStatusCliente.Aprovado, "contasAReceber-propostaStatus", $"Prospota não aprovada pelo Cliente!"),
                AssertionConcern.AssertNotNull(obra.ContratoPagamentos, "contasAReceber-Contrato Pagamentos", $"Contrato Pagamentos nulo!"),
                AssertionConcern.AssertIsGreaterThan(contratoPagamentos.Count(), 0, "contasAReceber-Contrato Pagamentos Cartao", $"Não foi encontrado nenhum pagamento com método Cartão que necessita aprovação!")
            );
        }

        public static bool TributacaoMunicipalIsValid(this Obra obraAtual, IEnumerable<Obra> obrasMesmoCliente)
        {
            string chaveProposta = "";
            foreach (var obra in obrasMesmoCliente)
            {
                var obraTributacoesMunicipaisIntersecao = obra.ObraTributacoesMunicipais.Where(t => obraAtual.ObraTributacoesMunicipais.Any(y => t.CodigoObraPrefeitura == y.CodigoObraPrefeitura)).ToList();

                if (obraTributacoesMunicipaisIntersecao.Count == 0) continue;

                if (obraAtual.GetEnderecoString() != obra.GetEnderecoString())
                {
                    chaveProposta = obra.getChaveProposaToString();
                    break;
                }
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(chaveProposta == "", "obraTributacaoMunicipal", $"Código de Obra já cadastrada na proposta {chaveProposta}, não poderá ser confirmada a proposta com essa informação duplicada!")
            );
        }
    }
}
