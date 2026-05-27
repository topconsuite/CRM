using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        void Adicionar(TEntity t);
        void Adicionar<TChildEntity>(TChildEntity t) where TChildEntity : class;

        TEntity ObterPorId(params object[] keyvalues);

        TChildEntity ObterPorId<TChildEntity>(params object[] keyvalues) where TChildEntity : class;

        IEnumerable<TEntity> ListarTodos();

        IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> ListarFiltradosPaginado(Expression<Func<TEntity, bool>> filter, int page, int limit, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class;
        IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class;

        IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class;
        IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class;

        void Atualizar(TEntity t);
        void Atualizar<TChildEntity>(TChildEntity t) where TChildEntity : class;

        void Remover(TEntity t);
        void Remover<TChildEntity>(TChildEntity t) where TChildEntity : class;

        void Dispose();
    }
}
