using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContratoVersaoService : IServiceBase<ContratoVersao>
    {
        ICollection<ContratoVersao> ListarContratoVersoes(int codUsina, int anoContrato, int numeroContrato);
    }
}
