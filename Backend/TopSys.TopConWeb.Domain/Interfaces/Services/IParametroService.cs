using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IParametroService
    {
        TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro;

        string ObterParametroN(string grupo, string chave);
        string ObterParametroIntegracoes(string integracaoTipo, string parametroNome);
        void AtualizarParametroN(string grupo, string chave, string valor);
    }
}
