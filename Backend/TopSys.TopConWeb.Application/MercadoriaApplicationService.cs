using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class MercadoriaApplicationService : ApplicationServiceBase<Mercadoria>, IMercadoriaApplicationService
    {
        private readonly IMercadoriaService _mercadoriaService;
        private readonly IUsoService _usoService;

        public MercadoriaApplicationService(IMercadoriaService mercadoriaService, IUnitOfWork unityOfWork, IUsoService usoService)
            : base(mercadoriaService, unityOfWork)
        {
            _mercadoriaService = mercadoriaService;
            _usoService = usoService;
        }

        public MercadoriaResponse ObterTracoMercadoriaComDescricaoPersonalizada(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            if (!_usoService.PossuiDescricaoPersonalizada(idUso))
                return null;

            return AutoMapper.Mapper.Map<MercadoriaResponse>(_mercadoriaService.ObterTracoMercadoria(idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo));
        }
    }
}
