using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Application
{
    public class ParametroApplicationService : IParametroApplicationService
    {
        private readonly IParametroService _parametroService;

        public ParametroApplicationService(IParametroService parametroService)
        {
            _parametroService = parametroService;
        }

        public string ObterParametroIntegracoes(string integracaoTipo, string parametroNome)
        {
            return _parametroService.ObterParametroIntegracoes(integracaoTipo, parametroNome);
        }

        public string ObterParametroN(string group, string key)
        {
            return _parametroService.ObterParametroN(group, key);
        }

        public TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro
        {
            return _parametroService.ObterPorDataBase<TEntity>(dataBase);
        }

        public void AtualizarParametroN(string grupo, string chave, string valor)
        {
            _parametroService.AtualizarParametroN(grupo, chave, valor);
        }
    }
}
