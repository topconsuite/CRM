using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.ClicksignConfiguracao;
using TopSys.TopConWeb.Application.DTOS.Response.ClicksignConfiguracao;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class ClicksignConfiguracaoApplicationService : ApplicationServiceBase<ClicksignConfiguracao>, IClicksignConfiguracaoApplicationService
    {
        private readonly IClicksignConfiguracaoService _clicksignConfiguracaoService;
        private readonly IClicksignConfiguracaoRepository _clicksignConfiguracaoRepository;

        public ClicksignConfiguracaoApplicationService(
            IClicksignConfiguracaoService clicksignConfiguracaoService,
            IClicksignConfiguracaoRepository clicksignConfiguracaoRepository,
            IUnitOfWork unitOfWork)
            : base(clicksignConfiguracaoService, unitOfWork)
        {
            _clicksignConfiguracaoService = clicksignConfiguracaoService;
            _clicksignConfiguracaoRepository = clicksignConfiguracaoRepository;
        }

        public IEnumerable<ClicksignConfiguracaoResponse> ListarTodos()
        {
            return AutoMapper.Mapper.Map(
                _clicksignConfiguracaoService.ListarTodos(),
                new List<ClicksignConfiguracaoResponse>());
        }

        public ClicksignConfiguracaoResponse ObterPorId(int id)
        {
            return AutoMapper.Mapper.Map(
                _clicksignConfiguracaoService.ObterPorId(id),
                new ClicksignConfiguracaoResponse());
        }

        public void Salvar(ClicksignConfiguracaoRequest request)
        {
            var entity = AutoMapper.Mapper.Map(request, new ClicksignConfiguracao());

            if (entity.Id == 0)
                _clicksignConfiguracaoService.Adicionar(entity);
            else
                _clicksignConfiguracaoService.Atualizar(entity);

            Commit();
        }

        public void Remover(int id)
        {
            var entity = _clicksignConfiguracaoService.ObterPorId(id);

            if (entity != null)
                _clicksignConfiguracaoService.Remover(entity);

            Commit();
        }

        public IEnumerable<UsinaResponse> ListarUsinasPorConfiguracao(int configuracaoId)
        {
            return AutoMapper.Mapper.Map(
                _clicksignConfiguracaoRepository.ListarUsinasPorConfiguracao(configuracaoId),
                new List<UsinaResponse>());
        }

        public void VincularUsina(int configuracaoId, int usinaId)
        {
            _clicksignConfiguracaoRepository.VincularUsina(configuracaoId, usinaId);
        }

        public void DesvincularUsina(int configuracaoId, int usinaId)
        {
            _clicksignConfiguracaoRepository.DesvincularUsina(usinaId);
        }

        public string ObterSha256SecretPorUsina(int usinaId)
        {
            var config = _clicksignConfiguracaoService.ObterConfiguracaoPorUsina(usinaId);

            if (config == null || string.IsNullOrEmpty(config.Sha256Secret))
                return null;

            return config.Sha256Secret;
        }
    }
}
