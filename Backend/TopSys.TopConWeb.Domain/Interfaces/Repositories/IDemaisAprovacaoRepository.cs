using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IDemaisAprovacaoRepository : IRepositoryBase<DemaisAprovacao>
    {
        ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int usinaCodigo, int obraCodigo);

        ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int numVersao, int usinaCodigo, int obraCodigo);

        void RemoverAprovacoes(string id);
    }
}
