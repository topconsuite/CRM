using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IUsinaApplicationService : IApplicationServiceBase<Usina>
    {
        bool UsinaAtendeKm(int idUsina, int km);
        float? ObterValorAdicionalM3PorUsinaKm(int idUsina, int km);
        ParametroProgramacaoResponse ObterParametroProgramacao(int idUsina);
        IEnumerable<UsinaResponse> ListarPorEmpresa(int empresa);
        IEnumerable<UsinaResponse> ListarUsinasPermitidasUsuario(string idUsuario);
        IEnumerable<UsinaResponse> ListarUsinasAtivas();
    }
}
