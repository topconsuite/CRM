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
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class PreTracoPrecoRepository : RepositoryBase<PreTracoPreco>, IPreTracoPrecoRepository
    {

        public PreTracoPrecoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public PagedList<PreTracoPreco> ListarAguardandoCienciaPorPagina(int pagina, int porPagina, int segmentacao, Expression<Func<PreTracoPreco, bool>> filter)
        {

            var pagedList = _context.PreTracoPreco
                .Include(tp => tp.Usina)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Where(filter)
                .Where(tp => tp.DataCiencia == new DateTime(2000, 1, 1, 0, 0, 0, 0))
                .Where(tp => segmentacao == -1 || tp.Uso.Segmentacao == segmentacao)
                .OrderByDescending(tp => tp.CreatedAt)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public IEnumerable<PreTracoPreco> ListarAguardandoCienciaPorTracoUsina(int usina, int uso, int pedra, int slump, int resistenciaTipo, float mpa, int consumo)
        {

            return _context.PreTracoPreco
                    .Include(tp => tp.Usina)
                    .Include(tp => tp.ResistenciaTipo)
                    .Include(tp => tp.Uso)
                    .Include(tp => tp.Pedra)
                    .Include(tp => tp.Slump)
                    .Where(tp => tp.DataCiencia == new DateTime(2000, 1, 1, 0, 0, 0, 0)
                                && tp.UsinaCodigo == usina
                                && tp.UsoCodigo == uso
                                && tp.PedraCodigo == pedra
                                && tp.SlumpCodigo == slump
                                && tp.ResistenciaTipoCodigo == resistenciaTipo
                                && tp.Mpa == mpa
                                && tp.Consumo == consumo)
                    .OrderByDescending(tp => tp.CreatedAt);

        }

        public PreTracoPreco ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistenciaTipo, float mpa, int consumo, bool tracking = false)
        {

            return _context.PreTracoPreco
                .Include(tp => tp.Usina)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Where(tp =>   tp.DataCiencia == new DateTime(2000, 1, 1, 0, 0, 0, 0)
                            && tp.UsinaCodigo == usina
                            && tp.UsoCodigo == uso
                            && tp.PedraCodigo == pedra
                            && tp.SlumpCodigo == slump
                            && tp.ResistenciaTipoCodigo == resistenciaTipo
                            && tp.Mpa == mpa
                            && tp.Consumo == consumo)
                .Tracking(tracking)
                .OrderByDescending(tp => tp.CreatedAt)
                .FirstOrDefault();

        }

        public PreTracoPreco ObterPorId(Guid id)
        {

            return _context.PreTracoPreco
                .Include(tp => tp.Usina)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Where(tp => tp.Id == id)
                .FirstOrDefault();

        }

    }
}
