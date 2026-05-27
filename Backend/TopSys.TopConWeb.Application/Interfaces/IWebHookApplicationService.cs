using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.WebHook;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IWebHookApplicationService
    {

        void Adicionar(WebHookDesktop webHook);

        void AdicionarWebHookInterveniente(Interveniente interveniente, EWebHookTipoEvento evento);

        void AdicionarWebHookContratoPagamentoVersao(ContratoVersao contrato, List<ContratoPagamentoVersao> contratoPagamentos, EWebHookTipoEvento evento);

        void AdicionarWebHookContratoPagamento(Contrato contrato, List<ContratoPagamento> contratoPagamentos, EWebHookTipoEvento evento);

        void AdicionarWebhookContratoAprovado(Contrato contrato);
        void AdicionarWebhookContratoAprovadoVersao(ContratoVersao contrato);
        void AdicionarWebhookContratoPendenteAprovacaoFinanceira(Obra obra);
        void AdicionarWebhookContratoPendenteAprovacaoFinanceiraVersao(ObraVersao obra);
    }
}
