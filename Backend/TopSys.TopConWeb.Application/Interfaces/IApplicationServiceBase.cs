using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IApplicationServiceBase <TEntity> where TEntity : class
    {

        void Adicionar(TEntity t);
        void Adicionar<TDto>(TDto t);

        TEntity ObterPorId(params object[] keyvalues);

        IEnumerable<TDto> ListarTodos<TDto>();

        IEnumerable<TDto> ListarFiltrados<TDto>(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TDto> ListarFiltrados<TChildEntity, TDto>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class;

        IEnumerable<TDto> ListarFiltrados<TDto>(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TDto> ListarFiltrados<TChildEntity, TDto>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class;

        void Atualizar(TEntity t);
        void Atualizar<TDto>(TDto t);

        void Remover(TEntity t);
        void Remover<TDto>(TDto t);

        void Dispose();
    }
}
