using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected AppDataContext _context;

        public RepositoryBase(AppDataContext context)
        {
            this._context = context;
        }

        public void Adicionar(TEntity t)
        {
            _context.Set<TEntity>().Add(t);
        }

        public void Adicionar<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _context.Set<TChildEntity>().Add(t);
        }

        public TEntity ObterPorId(params object[] keyvalues)
        {
            return _context.Set<TEntity>().Find(keyvalues);
        }

        public TChildEntity ObterPorId<TChildEntity>(params object[] keyvalues) where TChildEntity : class
        {
            return _context.Set<TChildEntity>().Find(keyvalues);
        }

        public IEnumerable<TEntity> ListarTodos()
        {
            return _context.Set<TEntity>().AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().Where(filter).AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().Where(filter).ToList();
        }

        public IEnumerable<TEntity> ListarFiltrados(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var set = _context.Set<TEntity>().AsQueryable();

            foreach (var includePropertie in includeProperties)
            {
                set = set.Include(includePropertie);
            }
            
            return set.Where(filter).AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> ListarFiltradosPaginado(Expression<Func<TEntity, bool>> filter, int page, int limit, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            if (limit > 100) limit = 10;

            var set = _context.Set<TEntity>().AsQueryable();

            foreach (var includePropertie in includeProperties)
            {
                set = set.Include(includePropertie);
            }

            return set.Where(filter).OrderBy(filter).Skip((page - 1) * limit).Take(limit).AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> ListarFiltradosTracking(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var set = _context.Set<TEntity>().AsQueryable();

            foreach (var includePropertie in includeProperties)
            {
                set = set.Include(includePropertie);
            }

            return set.Where(filter).ToList();
        }

        public IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class
        {
            return _context.Set<TChildEntity>().Where(filter).AsNoTracking().ToList();
        }
        public IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter) where TChildEntity : class
        {
            var set = _context.Set<TChildEntity>().Where(filter);
            return set.ToList();
        }

        public IEnumerable<TChildEntity> ListarFiltrados<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class
        {
            var set = _context.Set<TChildEntity>().AsQueryable();

            foreach (var includePropertie in includeProperties)
            {
                set = set.Include(includePropertie);
            }

            return set.Where(filter).AsNoTracking().ToList();
        }

        public IEnumerable<TChildEntity> ListarFiltradosTracking<TChildEntity>(Expression<Func<TChildEntity, bool>> filter, params Expression<Func<TChildEntity, object>>[] includeProperties) where TChildEntity : class
        {
            var set = _context.Set<TChildEntity>().AsQueryable();

            foreach (var includePropertie in includeProperties)
            {
                set = set.Include(includePropertie);
            }

            return set.Where(filter).ToList();
        }

        public void Remover(TEntity t)
        {
            _context.Set<TEntity>().Remove(t);
        }

        public void Remover<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _context.Set<TChildEntity>().Remove(t);
        }

        public void Atualizar(TEntity t)
        {
            _context.Entry(t).State = EntityState.Modified;
        }

        public void Atualizar<TChildEntity>(TChildEntity t) where TChildEntity : class
        {
            _context.Entry(t).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool IsOnTable(string value, string field, string table)
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.AppendLine($"SELECT * FROM {table} WHERE {field} = {value} limit 1");
            try
            {
                var queryResult = _context.Database.Connection.QuerySingle(sqlCommand.ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsOnTable(List<string> values, List<string> fields, string table)
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.AppendLine($"SELECT * FROM {table} WHERE");
            if (fields.Count > 1)
            {
                for (var i = 0; i < values.Count; i++)
                {
                    sqlCommand.Append($" {fields[i]} = {values[i]}");
                    if (i != values.Count - 1) sqlCommand.Append($" AND");
                }
                sqlCommand.Append($" limit 1");
            }
            else
            {
                sqlCommand.Append($" {fields[0]} = {values[0]} limit 1");
            }

            try
            {
                var queryResult = _context.Database.Connection.QuerySingle(sqlCommand.ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
