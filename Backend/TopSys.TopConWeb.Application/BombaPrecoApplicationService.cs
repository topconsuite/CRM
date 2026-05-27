using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Response.BombaPreco;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class BombaPrecoApplicationService : IBombaPrecoApplicationService
    {
        private readonly IBombaPrecoService _bombaPrecoService;

        public BombaPrecoApplicationService(IBombaPrecoService bombaPrecoService)
        {
            _bombaPrecoService = bombaPrecoService;
        }

        public IEnumerable<CadastroGeralDTO> ListarBombaTiposPorUsina(int idUsina)
        {
            return AutoMapper.Mapper.Map(_bombaPrecoService.ListarBombaTiposPorUsina(idUsina), new List<CadastroGeralDTO>());
        }

        public IEnumerable<IntervenienteDTO> ListarTerceirosPorBombaTipo(int idBombaTipo)
        {
            return AutoMapper.Mapper.Map(_bombaPrecoService.ListarTerceirosPorBombaTipo(idBombaTipo), new List<IntervenienteDTO>());
        }

        public BombaPrecoTerceiroResponse ObterPorBombistaBombaTipoData(int idBombista, int idBombaTipo, DateTime dataBase)
        {
            return AutoMapper.Mapper.Map(_bombaPrecoService.ObterPorBombistaBombaTipoData(idBombista, idBombaTipo, dataBase), new BombaPrecoTerceiroResponse());
        }

        public BombaPrecoResponse ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime dataBase)
        {
            return AutoMapper.Mapper.Map(_bombaPrecoService.ObterPorUsinaBombaTipoData(idUsina, idBombaTipo, dataBase), new BombaPrecoResponse());
        }

        public float ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao)
        {
            return _bombaPrecoService.ObterValorAdicional(idUsina, idBombaTipo, distanciaTubulacao);
        }
    }
}
