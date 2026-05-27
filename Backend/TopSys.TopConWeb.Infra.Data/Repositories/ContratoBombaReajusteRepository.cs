using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ContratoBombaReajusteRepository : RepositoryBase<ContratoBombaReajuste>, IContratoBombaReajusteRepository
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IObraRepository _obraRepository;
        private readonly IIntervenienteRepository _intervenienteRepository;

        public ContratoBombaReajusteRepository(AppDataContext context, IContratoRepository contratoRepository, IIntervenienteRepository intervenienteRepository, IObraRepository obraRepository) : base(context)
        {
            _context = context;
            _contratoRepository = contratoRepository;
            _intervenienteRepository = intervenienteRepository;
            _obraRepository = obraRepository;
        }

        public IEnumerable<ContratoBombaReajuste> ListarContratoReajusteBombaPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia)
        {
            return _context.ContratoBombaReajustes
                .Include(t => t.BombaTipo)
                .Where(t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato && t.DataVigencia == dataVigencia);
        }

        public PagedList<ContratoBombaReajuste> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT reaj.usina {nameof(ContratoBombaReajuste.UsinaCodigo)}");
            sqlComando.Append($", reaj.ano_contrato {nameof(ContratoBombaReajuste.ContratoAno)}");
            sqlComando.Append($", reaj.num_contrato {nameof(ContratoBombaReajuste.ContratoNumero)}");
            sqlComando.Append($", reaj.dt_vigencia {nameof(ContratoBombaReajuste.DataVigencia)}");
            sqlComando.Append($", reaj.data_carta {nameof(ContratoBombaReajuste.DataCarta)}");
            sqlComando.Append($", reaj.dt_confirmacao {nameof(ContratoBombaReajuste.DataConfirmacao)}");
            sqlComando.Append($", reaj.id_reprovacao {nameof(ContratoBombaReajuste.IdReprovacao)}");
            sqlComando.Append($" FROM con_reaj_bomba reaj");
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

            var contratoBombaReajustes = _context.Connection.QueryPagedList<ContratoBombaReajuste>(sqlComando.ToString(), pagina, porPagina);

            var contratoBombaReajusteLista = new List<ContratoBombaReajuste>();

            var pagedList = new PagedList<ContratoBombaReajuste>
            {
                CurrentPage = contratoBombaReajustes.CurrentPage,
                PageCount = contratoBombaReajustes.PageCount,
                PageSize = contratoBombaReajustes.PageSize,
                RecordCount = contratoBombaReajustes.RecordCount
            };

            foreach (var record in contratoBombaReajustes.Records)
            {
                var contratoBombaReajuste = (ContratoBombaReajuste)record;

                contratoBombaReajuste.Contrato = _contratoRepository.ObterPorId(contratoBombaReajuste.UsinaCodigo, contratoBombaReajuste.ContratoAno, contratoBombaReajuste.ContratoNumero);

                contratoBombaReajuste.Contrato.Interveniente = _intervenienteRepository.ObterPorId(contratoBombaReajuste.Contrato.IntervenienteCodigo);

                contratoBombaReajuste.Obra = _obraRepository.ObterObraPorContrato(contratoBombaReajuste.UsinaCodigo, contratoBombaReajuste.ContratoAno, contratoBombaReajuste.ContratoNumero);

                contratoBombaReajusteLista.Add(contratoBombaReajuste);
            }

            pagedList.Records = contratoBombaReajusteLista;

            return pagedList;
        }

        public IEnumerable<DateTime> ObterVigencias()
        {
            return _context.ContratoBombaReajustes
                .Include(t => t.Contrato)
                .Where(t => t.EmiteCartaSimNao == "S" && t.Contrato.DataEncerramento == null)
                .OrderBy(t => t.DataVigencia)
                .Select(t => t.DataVigencia)
                .Distinct()
                .ToList();
        }
    }
}
