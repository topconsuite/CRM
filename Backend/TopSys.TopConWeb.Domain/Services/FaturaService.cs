using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class FaturaService : ServiceBase<Fatura>, IFaturaService
    {
        IFaturaRepository _faturaRepository;

        public FaturaService(IFaturaRepository faturaRepository)
            : base(faturaRepository)
        {
            _faturaRepository = faturaRepository;
        }

        public Fatura ObterPorChave(Expression<Func<Fatura, bool>> filter, bool tracking = false)
        {
            return _faturaRepository.ObterPorChaveFatura(filter, tracking);
        }

        public ICollection<Fatura> Listar(Expression<Func<Fatura, bool>> filter, int page, int limit)
        {
            return _faturaRepository.ListarComPaginacao(filter, page, limit);
        }

        public PagedList<Fatura> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            return _faturaRepository.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit);
        }

        public Expression<Func<Fatura, bool>> CriarFiltroFatura(DateTime? dataFatura, int? filial, int? centroCusto, int? tipoDocumento, int? cliente)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(Fatura), "x");
            Expression body = Expression.Constant(true);

            if (dataFatura != null)
            {
                Expression dataNfIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(Fatura.DataNf)),
                    Expression.Constant(dataFatura.Value)
                );
                body = Expression.AndAlso(body, dataNfIgual);
            }

            if (filial != null)
            {
                Expression filialIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(Fatura.Filial)),
                    Expression.Constant(filial.Value)
                );
                body = Expression.AndAlso(body, filialIgual);
            }
            
            if (filial != centroCusto)
            {
                Expression centroCustoIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(Fatura.CentroCusto)),
                    Expression.Constant(centroCusto)
                );
                body = Expression.AndAlso(body, centroCustoIgual);
            }

            if (tipoDocumento != null)
            {
                Expression tipoDocumentoIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(Fatura.TipoDocumento)),
                    Expression.Constant(tipoDocumento.Value)
                );
                body = Expression.AndAlso(body, tipoDocumentoIgual);
            }

            if (cliente != null)
            {
                Expression clienteIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(Fatura.Cliente)),
                    Expression.Constant(cliente.Value)
                );
                body = Expression.AndAlso(body, clienteIgual);
            }

            return Expression.Lambda<Func<Fatura, bool>>(body, parameter);
        }
    }
}
