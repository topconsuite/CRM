using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class BombaPrecoRepository : IBombaPrecoRepository
    {
        private AppDataContext _context;

        public BombaPrecoRepository(AppDataContext context)
        {
            _context = context;
        }

        public IEnumerable<CadastroGeral> ListarBombaTiposPorUsina(int idUsina)
        {
            var bombaTipos = _context
                .BombaPrecos
                .Include(t => t.BombaTipo)
                .Where(t => t.UsinaCodigo == idUsina)
                .GroupBy(t => t.BombaTipoCodigo)
                .Select(grupo => grupo.FirstOrDefault().BombaTipo)
                .AsNoTracking()
                .ToList();

            return bombaTipos;
        }

        public IEnumerable<Interveniente> ListarTerceirosPorBombaTipo(int idBombaTipo)
        {
            var terceiros = _context
                .BombaPrecosTerceiros
                .Include(t => t.Bombista)
                .Where(t => t.BombaTipoCodigo == idBombaTipo && t.Bombista.bombista.ToUpper().Equals("S"))
                .GroupBy(t => t.BombistaCodigo)
                .Select(grupo => grupo.FirstOrDefault().Bombista)
                .AsNoTracking()
                .ToList();

            return terceiros;
        }

        public float ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" ((@DISTANCIA_TUBULACAO - dist_tub_de) + 1) * vlr_adic_tub");
            sqlComando.Append(" FROM topsys.con_preco_bomba_tub");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina=@ID_USINA");
            sqlComando.Append(" AND tipo_bomba=@BOMBA_TIPO");
            sqlComando.Append(" AND dist_tub_de<=@DISTANCIA_TUBULACAO");
            sqlComando.Append(" AND dist_tub_ate>=@DISTANCIA_TUBULACAO");

            var result = _context.Database.Connection.Query<float?>(sqlComando.ToString(), new {
                ID_USINA = idUsina,
                BOMBA_TIPO = idBombaTipo,
                DISTANCIA_TUBULACAO = distanciaTubulacao
            }).FirstOrDefault();

            return result ?? 0f;
        }

        BombaPrecoTerceiro IBombaPrecoRepository.ObterPorBombistaBombaTipoData(int idBombista, int idBombaTipo, DateTime dataBase)
        {
            return _context
                .BombaPrecosTerceiros
                .Include(t => t.Bombista)
                .Include(t => t.BombaTipo)
                .Where(t => t.BombistaCodigo == idBombista
                    && t.BombaTipoCodigo == idBombaTipo
                    && t.DataInicioVigencia <= dataBase)
                .OrderByDescending(t => t.DataInicioVigencia)
                .AsNoTracking()
                .FirstOrDefault();
        }
        BombaPreco IBombaPrecoRepository.ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime dataBase)
        {
            return _context
                .BombaPrecos
                .Include(t => t.Usina)
                .Include(t => t.BombaTipo)
                .Where(t => t.UsinaCodigo == idUsina
                    && t.BombaTipoCodigo == idBombaTipo
                    && t.DataInicioVigencia <= dataBase)
                .OrderByDescending(t => t.DataInicioVigencia)
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
