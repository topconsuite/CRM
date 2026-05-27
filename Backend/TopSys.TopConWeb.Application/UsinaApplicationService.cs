using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{

    public class UsinaApplicationService : ApplicationServiceBase<Usina>, IUsinaApplicationService
    {

        private readonly IUsinaService _usinaService;

        public UsinaApplicationService(IUsinaService usinaService, IUnitOfWork unityOfWork) : base(usinaService, unityOfWork)
        {
            _usinaService = usinaService;
        }

        public IEnumerable<UsinaResponse> ListarPorEmpresa(int empresa)
        {
            return AutoMapper.Mapper.Map(_usinaService.ListarPorEmpresa(empresa), new List<UsinaResponse>());
        }

        public ParametroProgramacaoResponse ObterParametroProgramacao(int idUsina)
        {
            return AutoMapper.Mapper.Map(_usinaService.ObterPorId(idUsina), new ParametroProgramacaoResponse());
        }

        public float? ObterValorAdicionalM3PorUsinaKm(int idUsina, int km)
        {
            return _usinaService.ObterValorAdicionalM3PorUsinaKm(idUsina, km);
        }

        public bool UsinaAtendeKm(int idUsina, int km)
        {
            return _usinaService.UsinaAtendeKm(idUsina, km);
        }

        public IEnumerable<UsinaResponse> ListarUsinasPermitidasUsuario(string idUsuario)
        {
            return AutoMapper.Mapper.Map(_usinaService.ListarUsinasPermitidasUsuario(idUsuario), new List<UsinaResponse>());
        }

        public IEnumerable<UsinaResponse> ListarUsinasAtivas()
        {
            return AutoMapper.Mapper.Map(_usinaService.ListarUsinasAtivas(), new List<UsinaResponse>());
        }
    }
}
