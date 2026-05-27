using System;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
