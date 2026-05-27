using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IBancoRepository : IRepositoryBase<Conta>
    {
        ICollection<Conta> ListarBanco();
        Conta ObterPorIdBanco(int cod, int emp, bool tracking = false);
        bool EstaEmUsoBanco(int cod, int emp);
    }
}