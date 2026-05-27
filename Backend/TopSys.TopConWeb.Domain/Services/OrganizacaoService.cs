using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class OrganizacaoService : IOrganizacaoService
    {
        private IParametroRepository _parametroRepository;

        public OrganizacaoService(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        public Organizacao ObterOrganizacaoReguaDeCobranca()
        {
            var result = new Organizacao();

            ulong.TryParse(_parametroRepository.ObterParametroN("Telluria", "ClienteCodigo"), out ulong clienteCodigo);
            result.Codigo = clienteCodigo;
            result.Nome = _parametroRepository.ObterParametroIntegracoes("regua_de_cobranca", "conta");

            return result;
        }
    }
}
