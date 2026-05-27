using System;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ServiceBase<TEntity> : IDisposable, IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;
        
        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }
         
        public void Adicionar(TEntity t)
        {
            _repository.Adicionar(t);
        }

        public void Adicionar<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _repository.Adicionar(t);
        }

        public TEntity ObterPorId(params object[] keyvalues)
        {
            return _repository.ObterPorId(keyvalues);
        }

        public TChildEntity ObterPorId<TChildEntity>(params object[] keyvalues) where TChildEntity : class
        {
            return _repository.ObterPorId<TChildEntity>(keyvalues);
        }

        public IEnumerable<TEntity> ListarTodos()
        {
            return _repository.ListarTodos();
        }

        public IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.ListarFiltrados(filter);
        }
        public IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.ListarFiltradosTracking(filter);
        }

        public IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.ListarFiltrados(filter, includeProperties);
        }

        public IEnumerable<TEntity> ListarFiltradosPaginado(Expression<Func<TEntity, bool>> filter, int page, int limit, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.ListarFiltradosPaginado(filter, page, limit, includeProperties);
        }

        public IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.ListarFiltradosTracking(filter, includeProperties);
        }

        public IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class
        {
            return _repository.ListarFiltrados(filter);
        }
        public IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class
        {
            return _repository.ListarFiltradosTracking(filter);
        }

        public IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class
        {
            return _repository.ListarFiltrados(filter, includeProperties);
        }

        public IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class
        {
            return _repository.ListarFiltradosTracking(filter, includeProperties);
        }

        public void Remover(TEntity t)
        {
            _repository.Remover(t);
        }

        public void Remover<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _repository.Remover(t);
        }

        public void Atualizar(TEntity t)
        {
            _repository.Atualizar(t);
        }

        public void Atualizar<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _repository.Atualizar(t);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
