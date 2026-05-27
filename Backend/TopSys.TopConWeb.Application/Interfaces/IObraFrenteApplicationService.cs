using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraFrenteResponse;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IObraFrenteApplicationService : IApplicationServiceBase<ObraFrente>
    {

        IEnumerable<ObraFrenteResponse> ListarPorObra(int obraUsina, int obraNumero);

        bool VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia);

    }
}
