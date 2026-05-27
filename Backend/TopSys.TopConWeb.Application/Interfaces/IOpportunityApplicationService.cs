using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Adicionar;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Opportunity;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IOpportunityApplicationService
    {

        OpportunityDTO Adicionar(OpportunityAdicionarRequest opportunity);

        OpportunityTypeDTO AdicionarOpportunityType(OpportunityTypeAdicionarRequest opportunityType);
        OpportunityFailureReasonDTO AdicionarOpportunityFailureReason(OpportunityFailureReasonAdicionarRequest opportunityFailureReason);
        OpportunityOriginDTO AdicionarOpportunityOrigin(OpportunityOriginAdicionarRequest opportunityOrigin);

        void Atualizar(OpportunityAlteracaoRequest opportunity);

        void AtualizarOpportunityType(OpportunityTypeAlteracaoRequest opportunityType);
        void AtualizarOpportunityFailureReason(OpportunityFailureReasonAlteracaoRequest opportunityFailureReason);
        void AtualizarOpportunityOrigin(OpportunityOriginAlteracaoRequest opportunityOrigin);


        void Deletar(Guid id);

        void DeletarOpportunityType(Guid id);
        void DeletarOpportunityFailureReason(Guid id);
        void DeletarOpportunityOrigin(Guid id);

        IEnumerable<OpportunityTypeDTO> ListarOpportunityTypes();
        IEnumerable<OpportunityFailureReasonDTO> ListarOpportunityFailureReasons();
        IEnumerable<OpportunityOriginDTO> ListarOpportunityOrigins();

        OpportunityTypeDTO ObterOpportunityTypePorId(Guid id);
        OpportunityFailureReasonDTO ObterOpportunityFailureReasonPorId(Guid id);
        OpportunityOriginDTO ObterOpportunityOriginPorId(Guid id);

        OpportunityDTO ObterPorId(Guid id);
        IEnumerable<OpportunityDTO> Listar();
        PagedList<OpportunityDTO> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter);

    }
}
