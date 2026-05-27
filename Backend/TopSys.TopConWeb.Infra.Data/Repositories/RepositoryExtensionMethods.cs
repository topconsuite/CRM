using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public static class RepositoryExtensionMethods
    {
        public static PagedList<T> PagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize, Expression<Func<T, bool>> countFilter) where T : class
        {
            int _maxPageSize = 200;

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > _maxPageSize) pageSize = _maxPageSize;

            // Quando a query tem Includes que geram INNER JOINS, o Count() não gera os INNER JOINS a não ser que seja passado
            // como parametro um filtro que use propriedades da tabela agregada...
            // Então toda vez que o source tiver um Include(), é necessário passar uma clausula no paramentro "countFilter"
            // para forçar o Count a realizar o JOIN, evitando assim diferenças no recordCount...
            var count = countFilter!=null ? source.Count(countFilter) : source.Count();

            var pageCount = (count / pageSize);
            if ((count % pageSize) != 0) pageCount++;
            if (pageNumber > pageCount) pageNumber = pageCount;

            var skip = Math.Abs(pageNumber - 1) * pageSize;

            var records = source.AsNoTracking().Skip(skip).Take(pageSize).ToList();

            var result = new PagedList<T>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                RecordCount = count,
                PageCount = pageCount,
                Records = records
            };

            return result;
        }

        public static PagedList<IQueryResult> QueryPagedList<T>(this DbConnection cnn, string sql, int pageNumber, int pageSize, object param = null, IDbTransaction transaction = null) where T : IQueryResult
        {
            int _maxPageSize = 200;
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > _maxPageSize) pageSize = _maxPageSize;

            var count = cnn.QueryFirstOrDefault<int>($"SELECT COUNT(*) FROM ({sql.ToString()}) x", param);

            var pageCount = (count / pageSize);
            if ((count % pageSize) != 0) pageCount++;
            if (pageNumber > pageCount) pageNumber = pageCount;

            var skip = Math.Abs(pageNumber - 1) * pageSize;

            var result = (IEnumerable<IQueryResult>)cnn.Query<T>($"{sql} LIMIT {skip}, {pageSize}", param, transaction);

            return new PagedList<IQueryResult>
            {
                CurrentPage = pageNumber,
                PageCount = pageCount,
                PageSize = pageSize,
                RecordCount = count,
                Records = result
            };
        }

        public static IQueryable<T> Tracking<T>(this IQueryable<T> source, bool tracking) where T : class
        {
            if (tracking)
                return source;
            else
                return source.AsNoTracking();
        }
    }
}
