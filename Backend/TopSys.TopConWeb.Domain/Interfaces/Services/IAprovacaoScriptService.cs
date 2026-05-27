using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IAprovacaoScriptService : IServiceBase<AprovacaoScript>
    {
        void ExecutarAprovacao(string chave);

        void ExecutarReprovacao(string chave);
    }
}
