using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.Application
{
    public class ApplicationServiceBase<TEntity> : IDisposable, IApplicationServiceBase<TEntity> where TEntity : class
    {
        private IUnitOfWork _unitOfWork;

        protected IHandler<DomainNotification> _notifications;

        private readonly IServiceBase<TEntity> _serviceBase;

        public ApplicationServiceBase(IServiceBase<TEntity> serviceBase, IUnitOfWork unityOfWork)
        {
            _unitOfWork = unityOfWork;
            _serviceBase = serviceBase;
            _notifications = DomainEvent.Container.GetService<IHandler<DomainNotification>>();
        }

        public bool Commit()
        {
            if (_notifications.HasNotifications())
                return false;

            _unitOfWork.Commit();

            return true;
        }

        public void Adicionar(TEntity t)
        {
            _serviceBase.Adicionar(t);
            Commit();
        }

        public void Adicionar<TDto>(TDto t)
        {
            _serviceBase.Adicionar(AutoMapper.Mapper.Map<TEntity>(t));
            Commit();
        }

        public TEntity ObterPorId(params object[] keyvalues)
        {
            return _serviceBase.ObterPorId(keyvalues);
        }
        
        public IEnumerable<Dto> ListarTodos<Dto>()
        {
            return AutoMapper.Mapper.Map(_serviceBase.ListarTodos(), new List<Dto>());
        }

        public IEnumerable<TDto> ListarFiltrados<TDto>(Expression<Func<TEntity, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_serviceBase.ListarFiltrados(filter), new List<TDto>());
        }

        public IEnumerable<TDto> ListarFiltrados<TChildEntity, TDto>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class
        {
            return AutoMapper.Mapper.Map(_serviceBase.ListarFiltrados(filter), new List<TDto>());
        }

        public IEnumerable<TDto> ListarFiltrados<TDto>(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return AutoMapper.Mapper.Map(_serviceBase.ListarFiltrados(filter, includeProperties), new List<TDto>());
        }

        public IEnumerable<TDto> ListarFiltrados<TChildEntity, TDto>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class
        {
            return AutoMapper.Mapper.Map(_serviceBase.ListarFiltrados(filter, includeProperties), new List<TDto>());
        }

        public void Remover(TEntity t)
        {
            _serviceBase.Remover(t);
            Commit();
        }

        public void Remover<TDto>(TDto t)
        {
            _serviceBase.Remover(AutoMapper.Mapper.Map<TEntity>(t));
            Commit();
        }

        public void Atualizar(TEntity t)
        {
            _serviceBase.Atualizar(t);
            Commit();
        }

        public void Atualizar<TDto>(TDto t)
        {
            _serviceBase.Atualizar(AutoMapper.Mapper.Map<TEntity>(t));
            Commit();
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
            //TODO: REMOVER ESSAS UTILIZAÇÕES ´NÃO É NCESSÁRIO FORÇAR O GC
            //GC.SuppressFinalize(this);
        }
    }
}
