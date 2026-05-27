using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IMunicipioTributacaoService : IServiceBase<Municipio>
    {
        ICollection<Municipio> Listar(string uf);
        Municipio ObterPorId(int id, bool tracking = false);
        Municipio ObterPorExternalId(string externalId, bool tracking = false);
        Municipio ObterPorIbgeCode(int ibgeCode, bool tracking = false);
        Municipio ObterPorMunicipioUf(string municipio, string uf, bool tracking = false);
        bool VerificaSeExisteInterveniente(int codigoInterveniente);
        int ObterProximoCodigo();
	}
}
