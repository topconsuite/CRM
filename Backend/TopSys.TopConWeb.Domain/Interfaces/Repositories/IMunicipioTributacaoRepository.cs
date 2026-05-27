using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IMunicipioTributacaoRepository : IRepositoryBase<Municipio>
    {
        ICollection<Municipio> ListarMunicipioTributacao(string uf);
        Municipio ObterPorId(int id, bool tracking = false);
        Municipio ObterPorExternalId(string externalId, bool tracking = false);
        Municipio ObterPorIbgeCode(int ibgeCode, bool tracking = false);
        Municipio ObterPorMunicipioUf(string municipio, string uf, bool tracking = false);
        int ObterProximoCodigo();
    }
}
