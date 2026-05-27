using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IParametroRepository
    {
        TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro;

        string ObterParametroN(string grupo, string chave);

        string ObterParametroIntegracoes(string integracaoTipo, string parametroNome);

        void AtualizarParametroN(string grupo, string chave, string valor);

        void ApagarParametroN(string grupo, string chave);
    }
}
