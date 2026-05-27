using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Adicionar;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Opportunity;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class OpportunityApplicationService : ApplicationServiceBase<Opportunity>, IOpportunityApplicationService
    {

        private readonly IOpportunityService _opportunityService;

        public OpportunityApplicationService(IOpportunityService opportunityService, IUnitOfWork unityOfWork) : base(opportunityService, unityOfWork)
        {
            _opportunityService = opportunityService;
        }

        public OpportunityDTO Adicionar(OpportunityAdicionarRequest opportunity)
        {

            var newOpportunity = AutoMapper.Mapper.Map<OpportunityAdicionarRequest, Opportunity>(opportunity);

            newOpportunity.Id = Guid.NewGuid();
            newOpportunity.CreatedAt = DateTime.UtcNow;

            _opportunityService.Adicionar(newOpportunity);

            Commit();

            return AutoMapper.Mapper.Map<Opportunity, OpportunityDTO>(newOpportunity);

        }

        public OpportunityTypeDTO AdicionarOpportunityType(OpportunityTypeAdicionarRequest opportunityType)
        {
            var newOpportunityType = AutoMapper.Mapper.Map<OpportunityTypeAdicionarRequest, OpportunityType>(opportunityType);

            newOpportunityType.Id = Guid.NewGuid();

            _opportunityService.AdicionarOpportunityType(newOpportunityType);

            Commit();

            return AutoMapper.Mapper.Map<OpportunityType, OpportunityTypeDTO>(newOpportunityType);
        }

        public OpportunityOriginDTO AdicionarOpportunityOrigin(OpportunityOriginAdicionarRequest opportunityOrigin)
        {
            var newOpportunityOrigin = AutoMapper.Mapper.Map<OpportunityOriginAdicionarRequest, OpportunityOrigin>(opportunityOrigin);

            newOpportunityOrigin.Id = Guid.NewGuid();

            _opportunityService.AdicionarOpportunityOrigin(newOpportunityOrigin);

            Commit();

            return AutoMapper.Mapper.Map<OpportunityOrigin, OpportunityOriginDTO>(newOpportunityOrigin);
        }

        public OpportunityFailureReasonDTO AdicionarOpportunityFailureReason(OpportunityFailureReasonAdicionarRequest opportunityFailureReason)
        {
            var newOpportunityFailureReason = AutoMapper.Mapper.Map<OpportunityFailureReasonAdicionarRequest, OpportunityFailureReason>(opportunityFailureReason);

            newOpportunityFailureReason.Id = Guid.NewGuid();

            _opportunityService.AdicionarOpportunityFailureReason(newOpportunityFailureReason);

            Commit();

            return AutoMapper.Mapper.Map<OpportunityFailureReason, OpportunityFailureReasonDTO>(newOpportunityFailureReason);
        }

        public void Atualizar(OpportunityAlteracaoRequest opportunity)
        {
            var opportunityOld = _opportunityService.ObterPorId(opportunity.Id);

            if (opportunityOld == null)
            {
                AssertionConcern.Notify("Opportunity", "Registro referente ao ID informado é inválido.");
                return;
            }

            if (opportunityOld.Deleted)
            {
                AssertionConcern.Notify("Opportunity", "Registro informado foi deletado.");
                return;
            }

            AutoMapper.Mapper.Map<OpportunityAlteracaoRequest, Opportunity>(opportunity, opportunityOld);

            opportunityOld.UpdatedAt = DateTime.UtcNow;

            Commit();

        }

        public void AtualizarOpportunityFailureReason(OpportunityFailureReasonAlteracaoRequest opportunityFailureReason)
        {
            var opportunityFailureReasonOld = _opportunityService.ObterOpportunityFailureReasonPorId(opportunityFailureReason.Id);

            if(opportunityFailureReasonOld == null)
            {
                AssertionConcern.Notify("OpportunityFailureReason", "Registro referente ao ID informado é inválido.");
                return;
            }

            if(opportunityFailureReasonOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityFailureReason", "Registro informado foi deletado.");
                return;
            }

            opportunityFailureReasonOld.Description = opportunityFailureReason.Description;

            Commit();

        }

        public void AtualizarOpportunityOrigin(OpportunityOriginAlteracaoRequest opportunityOrigin)
        {
            var opportunityOriginOld = _opportunityService.ObterOpportunityOriginPorId(opportunityOrigin.Id);

            if (opportunityOriginOld == null)
            {
                AssertionConcern.Notify("OpportunityOrigin", "Registro referente ao ID informado não existe.");
                return;
            }

            if (opportunityOriginOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityOrigin", "Registro informado foi deletado.");
                return;
            }

            opportunityOriginOld.Description = opportunityOrigin.Description;

            Commit();
        }

        public void AtualizarOpportunityType(OpportunityTypeAlteracaoRequest opportunityType)
        {

            var opportunityTypeOld = _opportunityService.ObterOpportunityTypePorId(opportunityType.Id);

            if (opportunityTypeOld == null)
            {
                AssertionConcern.Notify("OpportunityType", "Registro referente ao ID informado não existe.");
                return;
            }

            if (opportunityTypeOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityType", "Registro informado foi deletado.");
                return;
            }

            opportunityTypeOld.Description = opportunityType.Description;

            Commit();

        }

        public void Deletar(Guid id)
        {
            var opportunity = _opportunityService.ObterPorId(id);

            if (opportunity == null)
            {
                AssertionConcern.Notify("Opportunity", "Registro referente ao ID informado é inválido.");
                return;
            }

            if (opportunity.Deleted)
            {
                AssertionConcern.Notify("Opportunity", "Registro informado já foi deletado.");
                return;
            }

            opportunity.Deleted = true;
            opportunity.DeletedAt = DateTime.UtcNow;

            Commit();
        }

        public void DeletarOpportunityFailureReason(Guid id)
        {
            var opportunityFailureReasonOld = _opportunityService.ObterOpportunityFailureReasonPorId(id);

            if (opportunityFailureReasonOld == null)
            {
                AssertionConcern.Notify("OpportunityFailureReason", "Registro referente ao ID informado é inválido.");
                return;
            }

            if (opportunityFailureReasonOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityFailureReason", "Registro informado já foi deletado.");
                return;
            }

            opportunityFailureReasonOld.Deleted = true;
            opportunityFailureReasonOld.DeletedAt = DateTime.UtcNow;

            Commit();
        }

        public void DeletarOpportunityOrigin(Guid id)
        {
            var opportunityOriginOld = _opportunityService.ObterOpportunityOriginPorId(id);

            if (opportunityOriginOld == null)
            {
                AssertionConcern.Notify("OpportunityOrigin", "Registro referente ao ID informado não existe.");
                return;
            }

            if (opportunityOriginOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityOrigin", "Registro informado já foi deletado.");
                return;
            }

            opportunityOriginOld.Deleted = true;
            opportunityOriginOld.DeletedAt = DateTime.UtcNow;

            Commit();
        }

        public void DeletarOpportunityType(Guid id)
        {
            var opportunityTypeOld = _opportunityService.ObterOpportunityTypePorId(id);

            if (opportunityTypeOld == null)
            {
                AssertionConcern.Notify("OpportunityType", "Registro referente ao ID informado não existe.");
                return;
            }

            if (opportunityTypeOld.Deleted)
            {
                AssertionConcern.Notify("OpportunityType", "Registro informado já foi deletado.");
                return;
            }

            opportunityTypeOld.Deleted = true;
            opportunityTypeOld.DeletedAt = DateTime.UtcNow;

            Commit();
        }

        public IEnumerable<OpportunityDTO> Listar()
        {
            return AutoMapper.Mapper.Map<IEnumerable<Opportunity>, IEnumerable<OpportunityDTO>>(_opportunityService.Listar());
        }

        public IEnumerable<OpportunityFailureReasonDTO> ListarOpportunityFailureReasons()
        {
            return AutoMapper.Mapper.Map<IEnumerable<OpportunityFailureReason>, IEnumerable<OpportunityFailureReasonDTO>>(_opportunityService.ListarOpportunityFailureReasons());
        }

        public IEnumerable<OpportunityOriginDTO> ListarOpportunityOrigins()
        {
            return AutoMapper.Mapper.Map<IEnumerable<OpportunityOrigin>, IEnumerable<OpportunityOriginDTO>>(_opportunityService.ListarOpportunityOrigins());
        }

        public IEnumerable<OpportunityTypeDTO> ListarOpportunityTypes()
        {
            return AutoMapper.Mapper.Map<IEnumerable<OpportunityType>, IEnumerable<OpportunityTypeDTO>>(_opportunityService.ListarOpportunityTypes());
        }

        public PagedList<OpportunityDTO> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter)
        {
            return AutoMapper.Mapper.Map<PagedList<Opportunity>, PagedList<OpportunityDTO>>(_opportunityService.ListarPorPagina(pagina, porPagina, filter));
        }

        public OpportunityFailureReasonDTO ObterOpportunityFailureReasonPorId(Guid id)
        {
            return AutoMapper.Mapper.Map<OpportunityFailureReason, OpportunityFailureReasonDTO>(_opportunityService.ObterOpportunityFailureReasonPorId(id));
        }

        public OpportunityOriginDTO ObterOpportunityOriginPorId(Guid id)
        {
            return AutoMapper.Mapper.Map<OpportunityOrigin, OpportunityOriginDTO>(_opportunityService.ObterOpportunityOriginPorId(id));
        }

        public OpportunityTypeDTO ObterOpportunityTypePorId(Guid id)
        {
            return AutoMapper.Mapper.Map<OpportunityType, OpportunityTypeDTO>(_opportunityService.ObterOpportunityTypePorId(id));
        }

        public OpportunityDTO ObterPorId(Guid id)
        {
            return AutoMapper.Mapper.Map<Opportunity, OpportunityDTO>(_opportunityService.ObterPorId(id));
        }
    }
}
