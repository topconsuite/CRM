using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IRepasseReajusteRepository : IRepositoryBase<RepasseReajuste>
    {
        RepasseReajuste ObterVigente(DateTime dataBase);
        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);
    }
}
