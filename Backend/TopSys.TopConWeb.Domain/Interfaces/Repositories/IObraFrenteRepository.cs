using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IObraFrenteRepository : IRepositoryBase<ObraFrente>
    {

        IEnumerable<ObraFrente> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false);

        ObraFrente ObterPorObra(int obraUsina, int obraNumero, int sequencia, bool tracking = false);

        int ProximaSequenciaNaObra(int obraUsina, int obraNumero);

        bool VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia);

    }
}
