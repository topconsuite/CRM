using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class NotaFiscalDigitalService : ServiceBase<NotaFiscalDigital>, INotaFiscalDigitalService
    {
        INotaFiscalDigitalRepository _notaFiscalDigitalRepository;

        public NotaFiscalDigitalService(INotaFiscalDigitalRepository notaFiscalDigitalRepository)
            : base(notaFiscalDigitalRepository)
        {
            _notaFiscalDigitalRepository = notaFiscalDigitalRepository;
        }

        public NotaFiscalDigital ObterPorChave(Expression<Func<NotaFiscalDigital, bool>> filter, bool tracking = false)
        {
            return _notaFiscalDigitalRepository.ObterPorChaveNotaFiscalDigital(filter, tracking);
        }

        public ICollection<NotaFiscalDigital> Listar(Expression<Func<NotaFiscalDigital, bool>> filter, int page, int limit)
        {
            return _notaFiscalDigitalRepository.ListarComPaginacao(filter, page, limit);
        }

        public PagedList<NotaFiscalDigital> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            return _notaFiscalDigitalRepository.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit);
        }

        public Expression<Func<NotaFiscalDigital, bool>> CriarFiltroNotaFiscal(DateTime? dataNotaFiscal, int? filial, int? tipoDocumento, int? centroCusto, int? cliente)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(NotaFiscalDigital), "x");
            Expression body = Expression.Constant(true);

            if (dataNotaFiscal != null)
            {
                Expression dataNfIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(NotaFiscalDigital.DataNf)),
                    Expression.Constant(dataNotaFiscal.Value)
                );
                body = Expression.AndAlso(body, dataNfIgual);
            }

            if (filial != null)
            {
                Expression filialIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(NotaFiscalDigital.Filial)),
                    Expression.Constant(filial.Value)
                );
                body = Expression.AndAlso(body, filialIgual);
            }

            if (tipoDocumento != null)
            {
                Expression tipoDocumentoIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(NotaFiscalDigital.TipoDocumento)),
                    Expression.Constant(tipoDocumento.Value)
                );
                body = Expression.AndAlso(body, tipoDocumentoIgual);
            }

            if (centroCusto != null)
            {
                var property = Expression.Property(parameter, nameof(NotaFiscalDigital.CentroCusto));

                Expression centroCustoIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(NotaFiscalDigital.CentroCusto)),
                    Expression.Constant(centroCusto.Value, property.Type)
                );
                body = Expression.AndAlso(body, centroCustoIgual);
            }

            if (cliente != null)
            {
                Expression clienteIgual = Expression.Equal(
                    Expression.Property(parameter, nameof(NotaFiscalDigital.Cliente)),
                    Expression.Constant(cliente.Value)
                );
                body = Expression.AndAlso(body, clienteIgual);
            }

            return Expression.Lambda<Func<NotaFiscalDigital, bool>>(body, parameter);
        }
    }
}
