using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Unidade;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class UnidadeApplicationService : ApplicationServiceBase<Unidade>, IUnidadeApplicationService
    {
        private readonly IUnidadeService _unidadeService;

        public UnidadeApplicationService(IUnidadeService unidadeService, IUnitOfWork unityOfWork)
            : base(unidadeService, unityOfWork)
        {
            _unidadeService = unidadeService;
        }

        public IEnumerable<UnidadeResponse> ListarTodos()
        {
            return AutoMapper.Mapper.Map(_unidadeService.ListarTodos(), new List<UnidadeResponse>());
        }
    }
}
