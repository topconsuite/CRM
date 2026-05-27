using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IBancoService : IServiceBase<Conta>
    {
        ICollection<Conta> Listar();
        Conta ObterPorId(int cod, int emp, bool tracking = false);
        bool EstaEmUsoBanco(int cod, int emp);
    }
}
