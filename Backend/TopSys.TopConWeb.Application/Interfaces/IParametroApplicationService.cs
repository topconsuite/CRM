using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IParametroApplicationService
    {
        TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro;
        string ObterParametroN(string group, string key);
        string ObterParametroIntegracoes(string integracaoTipo, string parametroNome);
        void AtualizarParametroN(string grupo, string chave, string valor);
    }
}
