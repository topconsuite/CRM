using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Text;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class MunicipioTributacaoRepository : RepositoryBase<Municipio>, IMunicipioTributacaoRepository
    {
        public MunicipioTributacaoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Municipio> ListarMunicipioTributacao(string uf)
        {
            Expression<Func<Municipio, bool>> filter;

            if (uf != null)
                filter = t => t.Uf == uf;
            else
                filter = t => true;

            return _context
                .Municipios
                .Where(filter)
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public Municipio ObterPorId(int id, bool tracking = false)
        {
            return _context
                .Municipios
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Municipio ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _context
                .Municipios
                .Where(t => t.IdExterno == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Municipio ObterPorIbgeCode(int ibgeCode, bool tracking = false)
        {
            return _context
                .Municipios
                .Where(t => t.IbgeCodigo == ibgeCode)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Municipio ObterPorMunicipioUf(string municipio, string uf, bool tracking = false)
        {
            return _context
                .Municipios
                .Where(t => t.Nome == municipio && t.Uf == uf)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public int ObterProximoCodigo()
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("SELECT IFNULL(MAX(cod) + 1, 1) FROM ger_municipio");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }
    }
}
