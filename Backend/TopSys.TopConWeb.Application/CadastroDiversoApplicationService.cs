using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroDiverso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CadastroDiversoApplicationService : ApplicationServiceBase<CadastroDiverso>, ICadastroDiversoApplicationService
    {
        private readonly ICadastroDiversoService _cadastroDiversoService;

        public CadastroDiversoApplicationService(ICadastroDiversoService cadastroDiversoService, IUnitOfWork unityOfWork)
            : base(cadastroDiversoService, unityOfWork)
        {
            _cadastroDiversoService = cadastroDiversoService;
        }

        public ICollection<CadastroDiversoResponse> ListarAndares()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarAndares(), new List<CadastroDiversoResponse>());
        }

        public ICollection<CadastroDiversoResponse> ListarCondicoesPagamento()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarCondicoesPagamento(), new List<CadastroDiversoResponse>());
        }

        public ICollection<CadastroDiversoResponse> ListarDiasDaSemanaFixo()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarDiasDaSemanaFixo(), new List<CadastroDiversoResponse>());
        }

        public ICollection<CadastroDiversoResponse> ListarOpcoesDeVencimentoEmDiaNaoUtil()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarOpcoesDeVencimentoEmDiaNaoUtil(), new List<CadastroDiversoResponse>());
        }

        public ICollection<CadastroDiversoResponse> ListarQuantidadeDeCorposDeProva()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarQuantidadeDeCorposDeProva(), new List<CadastroDiversoResponse>());
        }

        public ICollection<CadastroDiversoResponse> ListarModeloDocumentoRemessaConcreto()
        {
            return AutoMapper.Mapper.Map(_cadastroDiversoService.ListarModeloDocumentoRemessaConcreto(), new List<CadastroDiversoResponse>());
        }
    }
}
