using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IDemaisAprovacaoService : IServiceBase<DemaisAprovacao>
    {
        void AtualizarAprovacoes(string usuario, ICollection<DemaisAprovacao> demaisAprovacoes);

        ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int usinaCodigo, int obraCodigo, string usuario);

        ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int numVersao, int usinaCodigo, int obraCodigo, string usuario);

        void RemoverAprovacoes(string id);
    }
}
