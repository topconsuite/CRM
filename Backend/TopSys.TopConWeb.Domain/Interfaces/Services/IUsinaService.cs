using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IUsinaService : IServiceBase<Usina>
    {
        bool UsinaAtendeKm(int idUsina, int km);
        float? ObterValorAdicionalM3PorUsinaKm(int idUsina, int km);
        IEnumerable<Usina> ListarPorEmpresa(int empresa);
        IEnumerable<Usina> ListarUsinasPermitidasUsuario(string idUsuario);
        IEnumerable<Usina> ListarUsinasAtivas();
    }
}
