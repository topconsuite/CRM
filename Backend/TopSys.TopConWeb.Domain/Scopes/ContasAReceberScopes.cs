using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ContasAReceberScopes
    {
        public static bool VerificarContasAReceberJaCriado(this IEnumerable<ContasAReceber> contasAReceber)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(0, contasAReceber.Count(), "contasAReceber", "Contas a receber já inserido!")
            );
        }
        public static bool VerificarContasAReceberDaOperadoraParaGerarContasAReceberCliente(this IEnumerable<ContasAReceber> contasAReceber, ContratoPagamentoDetalheCartao contratoPagamentoDetalheCartao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(contratoPagamentoDetalheCartao.QuantidadeParcelas, contasAReceber.Count(), "contasAReceber", $"Quantidade de parcelas informada no detalhamento ({contratoPagamentoDetalheCartao.QuantidadeParcelas}) é diferente da quantidade de parcelas retornada pela operadora ({contasAReceber.Count()})!"),
                AssertionConcern.AssertAreEquals(contratoPagamentoDetalheCartao.Valor, (float)contasAReceber.Select(t => t.Valor).Sum(), "contasAReceber", $"Valor informado no detalhamento ({String.Format(new CultureInfo("pt-BR"), "{0:C}", contratoPagamentoDetalheCartao.Valor)}) difere da soma dos valores retornados pela operadora ({String.Format(new CultureInfo("pt-BR"), "{0:C}", contasAReceber.Select(t => t.Valor).Sum())})!")
            );
        }

        public static bool VerificarContasAReceberDaOperadoraParaGerarContasAReceberCliente(this IEnumerable<ContasAReceber> contasAReceber, ContratoPagamentoDetalheCartaoVersao contratoPagamentoDetalheCartao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(contratoPagamentoDetalheCartao.QuantidadeParcelas, contasAReceber.Count(), "contasAReceber", $"Quantidade de parcelas informada no detalhamento ({contratoPagamentoDetalheCartao.QuantidadeParcelas}) é diferente da quantidade de parcelas retornada pela operadora ({contasAReceber.Count()})!"),
                AssertionConcern.AssertAreEquals(contratoPagamentoDetalheCartao.Valor, (float)contasAReceber.Select(t => t.Valor).Sum(), "contasAReceber", $"Valor informado no detalhamento ({String.Format(new CultureInfo("pt-BR"), "{0:C}", contratoPagamentoDetalheCartao.Valor)}) difere da soma dos valores retornados pela operadora ({String.Format(new CultureInfo("pt-BR"), "{0:C}", contasAReceber.Select(t => t.Valor).Sum())})!")
            );
        }

        public static bool ChequeJaLiquidadoScope(this IEnumerable<TituloContasAReceber> tituloContasAReceber)
        {
            return AssertionConcern.IsSatisfiedBy
            (
               AssertionConcern.AssertIsGreaterOrEqualThan(tituloContasAReceber.Sum(t => t.SomaRecebimentos), 0, "ChequeLiquidado", "Cheque já liquidado no Financeiro, não permitida desaprovação!")
            );
        }

        public static bool CartaoDeCreditoJaVinculadoScope(this ContasAReceber contasAReceber)
        {
            return AssertionConcern.IsSatisfiedBy
            (
               AssertionConcern.AssertAreEquals(contasAReceber.SomaRecebimentos, 0, "CartaoVinculado", "Registros de Cartão Vinculado Já Liquidados ou Conciliados. Não Permitida Desaprovação!"),
               AssertionConcern.AssertAreEquals(contasAReceber.LoteConciliado, 0, "CartaoVinculado", "Registros de Cartão Vinculado Já Liquidados ou Conciliados. Não Permitida Desaprovação!")
            );
        }

        public static bool PagamentoJaCompensadoScope(this TituloContasAReceber contasAReceber)
        {
            return AssertionConcern.IsSatisfiedBy
            (
               AssertionConcern.AssertAreEquals(contasAReceber.SomaRecebimentos, 0, "PagamentoCompensado", "Registro de Cartão Já Compensado. Solicite o Cancelamento de Compensação Antes de Efetuar a Desaprovação!")
            );
        }

        public static bool VerificadoCartaoJaConciliadoScope(this TituloContasAReceber contasAReceber)
        {
            return AssertionConcern.IsSatisfiedBy
            (
               AssertionConcern.AssertTrue(contasAReceber.CartaoConciliado != "S", "CartaoConciliado", "Contas a Receber Referente ao Cartão já Conciliado!")
            );
        }

        public static bool ValidaMesFechadoScope(this TituloContasAReceber contasAReceber, DateTime competencia)
        {
            return AssertionConcern.IsSatisfiedBy
            (
               AssertionConcern.AssertIsGreaterThan(contasAReceber.DataEmissao, competencia, "MesAberto", $"Condição Possui Título Vínculado Que Pertence a Mês Fechado! Mês Aberto: {competencia}")
            );
        }
    }
}
