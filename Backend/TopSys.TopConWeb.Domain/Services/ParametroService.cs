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
    public class ParametroService : IParametroService
    {
        private readonly IParametroRepository _parametroRepository;

        public ParametroService(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        public TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro
        {
            return _parametroRepository.ObterPorDataBase<TEntity>(dataBase);
        }

        public string ObterParametroN(string grupo, string chave)
        {
            return _parametroRepository.ObterParametroN(grupo, chave);
        }

        public string ObterParametroIntegracoes(string integracaoTipo, string parametroNome)
        {
            return _parametroRepository.ObterParametroIntegracoes(integracaoTipo, parametroNome);
        }

        public void AtualizarParametroN(string grupo, string chave, string valor)
        {
            _parametroRepository.AtualizarParametroN(grupo, chave, valor);
        }
    }
}
