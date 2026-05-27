using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using System.Data.Entity;
using Dapper;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using System.Transactions;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ContratoTracoReajusteRepository : RepositoryBase<ContratoTracoReajuste>, IContratoTracoReajusteRepository
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IObraRepository _obraRepository;
        private readonly IIntervenienteRepository _intervenienteRepository;

        public ContratoTracoReajusteRepository(AppDataContext context, IContratoRepository contratoRepository, IIntervenienteRepository intervenienteRepository, IObraRepository obraRepository) : base(context)
        {
            _context = context;
            _contratoRepository = contratoRepository;
            _intervenienteRepository = intervenienteRepository;
            _obraRepository = obraRepository;
        }

        public IEnumerable<ContratoTracoReajuste> ListarContratoReajusteTracoPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia)
        {
            return _context.ContratoTracoReajustes
                .Include(t => t.ResistenciaTipo)
                .Include(t => t.Uso)
                .Include(t => t.Pedra)
                .Include(t => t.Slump)
                .Where(t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato && t.DataVigencia == dataVigencia);
        }

        public PagedList<ContratoTracoReajuste> ListarContratoReajusteTracoPorPagina(int pagina, int porPagina, string filter)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT reaj.usina {nameof(ContratoTracoReajuste.UsinaCodigo)}");
            sqlComando.Append($", reaj.ano_contrato {nameof(ContratoTracoReajuste.ContratoAno)}");
            sqlComando.Append($", reaj.num_contrato {nameof(ContratoTracoReajuste.ContratoNumero)}");
            sqlComando.Append($", reaj.dt_vigencia {nameof(ContratoTracoReajuste.DataVigencia)}");
            sqlComando.Append($", reaj.usina_principal {nameof(ContratoTracoReajuste.UsinaEntregaCodigo)}");
            sqlComando.Append($", reaj.data_carta {nameof(ContratoTracoReajuste.DataCarta)}");
            sqlComando.Append($", reaj.dt_confirmacao {nameof(ContratoTracoReajuste.DataConfirmacao)}");
            sqlComando.Append($", reaj.id_reprovacao {nameof(ContratoTracoReajuste.IdReprovacao)}");
            sqlComando.Append($" FROM con_reajuste_item reaj");
            sqlComando.Append($" INNER JOIN con_contrato cont");
            sqlComando.Append($" ON cont.usina=reaj.usina");
            sqlComando.Append($" AND cont.num_contrato=reaj.num_contrato");
            sqlComando.Append($" AND cont.ano_contrato=reaj.ano_contrato");
            sqlComando.Append($" INNER JOIN ger_interv interv");
            sqlComando.Append($" ON interv.cod=cont.interv");
            sqlComando.Append($" WHERE reaj.emite_carta='S'");
            sqlComando.Append($" AND ISNULL(cont.dt_encer_cont)");

            if (filter != null)
                sqlComando.Append($" AND {filter}");

            sqlComando.Append($" GROUP BY reaj.usina, reaj.num_contrato, reaj.ano_contrato");
            sqlComando.Append($" ORDER BY reaj.dt_vigencia");

            var contratoTracoReajustes = _context.Connection.QueryPagedList<ContratoTracoReajuste>(sqlComando.ToString(), pagina, porPagina);

            var contratoTracoReajusteLista = new List<ContratoTracoReajuste>();

            var pagedList = new PagedList<ContratoTracoReajuste>
            {
                CurrentPage = contratoTracoReajustes.CurrentPage,
                PageCount = contratoTracoReajustes.PageCount,
                PageSize = contratoTracoReajustes.PageSize,
                RecordCount = contratoTracoReajustes.RecordCount
            };

            foreach (var record in contratoTracoReajustes.Records)
            {
                var contratoTracoReajuste = (ContratoTracoReajuste)record;

                contratoTracoReajuste.Contrato = _contratoRepository.ObterPorId(contratoTracoReajuste.UsinaCodigo, contratoTracoReajuste.ContratoAno, contratoTracoReajuste.ContratoNumero);

                contratoTracoReajuste.Contrato.Interveniente = _intervenienteRepository.ObterPorId(contratoTracoReajuste.Contrato.IntervenienteCodigo);

                contratoTracoReajuste.Obra = _obraRepository.ObterObraPorContrato(contratoTracoReajuste.UsinaCodigo, contratoTracoReajuste.ContratoAno, contratoTracoReajuste.ContratoNumero);

                contratoTracoReajusteLista.Add(contratoTracoReajuste);
            }

            pagedList.Records = contratoTracoReajusteLista;

            return pagedList;
        }

        public IEnumerable<DateTime> ObterVigencias()
        {
            return _context.ContratoTracoReajustes
                .Include(t => t.Contrato)
                .Where(t => t.EmiteCartaSimNao == "S" && t.Contrato.DataEncerramento == null)
                .OrderBy(t => t.DataVigencia)
                .Select(t => t.DataVigencia)
                .Distinct()                
                .ToList();
        }
    }
}
