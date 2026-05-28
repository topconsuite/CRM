using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using Topsys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Infra.Legacy.Filters;
using TopSys.TopConWeb.Infra.Legacy.QueryResults;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Infra.Legacy.Services
{
    public class ComercialLegacyService : IComercialLegacyService
    {
        private readonly AppDataContext _context;
        private readonly IParametroRepository _parametroRepository;
        private readonly IIntervenienteRepository _intervenienteRepository;
        private readonly IObraTaxaService _obraTaxaService;
        private readonly ITituloContasAReceberRepository _tituloContasAReceberRepository;
        private readonly IContasAReceberRepository _contasAReceberRepository;
        private readonly IContratoPagamentoRepository _contratoPagamentoRepository;
        private readonly ICartaoTransacaoRepository _cartaoTransacaoRepository;
        private readonly IMovimentoBancoRepository _movimentoBancoRepository;
        private readonly IUsinaService _usinaService;
        private readonly IObraRepository _obraRepository;
        private readonly IdentityHelperService _identityHelperService;
        private readonly IProgramacaoRepository _programacaoRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IIntervenienteSequenceControlService _intervenienteSequenceControl;
        private readonly IAprovacaoComercialUsinaRepository _aprovacaoComercialUsinaRepository;
        private readonly IAprovacaoComercialHierarquiaRepository _aprovacaoComercialHierarquiaRepository;
        private readonly IAprovacaoComercialPendenteRepository _aprovacaoComercialPendenteRepository;
        private readonly IClicksignRepository _clicksignRepository;
        private readonly IWebHookApplicationService _webHookApplicationService;
        private readonly ITracoPrecoService _tracoPrecoService;

        private readonly string _host;
        private readonly string _port;
        private readonly string _database;
        private readonly string _user;
        private readonly string _password;

        public ComercialLegacyService(AppDataContext context,
            IParametroRepository parametroRepository,
            IIntervenienteRepository intervenienteRepository,
            IObraTaxaService obraTaxaService,
            ITituloContasAReceberRepository tituloContasAReceberRepository,
            IContasAReceberRepository contasAReceberRepository,
            IContratoPagamentoRepository contratoPagamentoRepository,
            ICartaoTransacaoRepository cartaoTransacaoRepository,
            IMovimentoBancoRepository movimentoBancoRepository,
            IProgramacaoRepository programacaoRepository,
            IContratoRepository contratoRepository,
            IObraRepository obraRepository,
            IUsinaService usinaService,
            IAprovacaoComercialUsinaRepository aprovacaoComercialUsinaRepository,
            IAprovacaoComercialHierarquiaRepository aprovacaoComercialHierarquiaRepository,
            IAprovacaoComercialPendenteRepository aprovacaoComercialPendenteRepository,
            IClicksignRepository clicksignRepository,
            ITracoPrecoService tracoPrecoService,
            IdentityHelperService identityHelperService,
            IIntervenienteSequenceControlService intervenienteSequenceControl,
            IWebHookApplicationService webHookApplicationService)
        {
            _context = context;
            _parametroRepository = parametroRepository;
            _intervenienteRepository = intervenienteRepository;
            _obraTaxaService = obraTaxaService;
            _tituloContasAReceberRepository = tituloContasAReceberRepository;
            _contasAReceberRepository = contasAReceberRepository;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _cartaoTransacaoRepository = cartaoTransacaoRepository;
            _movimentoBancoRepository = movimentoBancoRepository;
            _usinaService = usinaService;
            _identityHelperService = identityHelperService;
            _programacaoRepository = programacaoRepository;
            _contratoRepository = contratoRepository;
            _obraRepository = obraRepository;
            _intervenienteSequenceControl = intervenienteSequenceControl;
            _aprovacaoComercialUsinaRepository = aprovacaoComercialUsinaRepository;
            _aprovacaoComercialHierarquiaRepository = aprovacaoComercialHierarquiaRepository;
            _aprovacaoComercialPendenteRepository = aprovacaoComercialPendenteRepository;
            _clicksignRepository = clicksignRepository;
            _webHookApplicationService = webHookApplicationService;
            _tracoPrecoService = tracoPrecoService;
            // _obraService = obraService;

            //conectar no banco
            var stringConexao = ConfigurationManager.ConnectionStrings["AppCnnStr"].ConnectionString.Split(';');

            //Realizando o parse dos dados de conexão
            _host = stringConexao[0].Split('=')[1];
            _port = stringConexao[1].Split('=')[1];
            _database = stringConexao[2].Split('=')[1];
            _user = stringConexao[3].Split('=')[1];
            _password = stringConexao[4].Split('=')[1];
            _intervenienteSequenceControl = intervenienteSequenceControl;
        }

        public PagedList<IQueryResult> ConsultarObras(IFilter filtro, int pagina, int porPagina, string Usuario, string ordenacao = "")
        {
            var f = (ConsultarObraFilter)filtro;

            var cnn = _context.Connection;
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();

            var limiteCreditoPorGrupoEconomico = _parametroRepository.ObterParametroN("web", "LimiteCreditoPorGrupoEconomico") == "1";

            var minMaxProgramacao = "MIN";
            if (f.ProgramacaoDataHoraAte != null)
                minMaxProgramacao = "MAX";

            var filtroTmpTable = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(f.StatusCadastroIn))
                filtroTmpTable.Append($" AND COALESCE(ov.status_cadastro, o.status_cadastro) IN({f.StatusCadastroIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusComercialIn))
                filtroTmpTable.Append($" AND COALESCE(ov.status_comercial, o.status_comercial) IN({f.StatusComercialIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusEngenhariaIn))
                filtroTmpTable.Append($" AND COALESCE(ov.status_engenharia, o.status_engenharia) IN({f.StatusEngenhariaIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusFinanceiroIn))
                filtroTmpTable.Append($" AND COALESCE(ov.status_financeiro, o.status_financeiro) IN({f.StatusFinanceiroIn})");


            if (!f.ConsiderarEncerrados)
                filtroTmpTable.Append($" AND COALESCE(cv.dt_encer_cont, c.dt_encer_cont) IS NULL");

            if (f.Vendedor > 0)
                filtroTmpTable.Append($" AND COALESCE(cv.vendedor, c.vendedor) = {f.Vendedor}");
            else if (f.VendedoresPermitidos != "*" && !string.IsNullOrWhiteSpace(f.VendedoresPermitidos))
                filtroTmpTable.Append($" AND COALESCE(cv.vendedor, c.vendedor) in ({f.VendedoresPermitidos})");

            if (f.Cliente > 0)
                filtroTmpTable.Append($" AND COALESCE(cv.interv, c.interv)={f.Cliente}");

            if (!string.IsNullOrWhiteSpace(f.UsinaEntregaIn))
            {
                filtroTmpTable.Append($" AND (COALESCE(cv.usina_principal, c.usina_principal) IN({f.UsinaEntregaIn})");
                filtroTmpTable.Append($" OR r.usina_entrega IN({f.UsinaEntregaIn}))");
            }
            else
            {
                var usinasPermitidasUsuario = _usinaService.ListarUsinasPermitidasUsuario(Usuario).Select(t => t.Codigo);
                filtroTmpTable.Append($" AND (COALESCE(cv.usina_principal, c.usina_principal) IN({String.Join(",", usinasPermitidasUsuario)})");
                filtroTmpTable.Append($" OR r.usina_entrega IN({String.Join(",", usinasPermitidasUsuario)}))");
            }

            if (f.Segmentacao > 0)
                filtroTmpTable.Append($" AND COALESCE(cv.segmentacao, c.segmentacao)={f.Segmentacao}");

            if (f.ContratoFinalidade > 0)
                filtroTmpTable.Append($" AND COALESCE(cv.finalidade_ctr, c.finalidade_ctr)={f.ContratoFinalidade}");

            var sql = new StringBuilder();

            sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_prog_superior (PRIMARY KEY(usina,ano_contrato,no_contrato)) ENGINE=MyISAM AS");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,r.dt_concretagem,{minMaxProgramacao}(r.seq_prog) seq_prog");
            sql.Append($" FROM (");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,{minMaxProgramacao}(r.dt_concretagem) dt_concretagem");
            sql.Append($" FROM con_programacao r");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND r.no_obra = o.numero");
            sql.Append($" LEFT JOIN con_contrato_versao cv");
            sql.Append($" ON r.usina=cv.usina AND r.ano_contrato=cv.ano_contrato AND r.no_contrato=cv.num_contrato");
            sql.Append($" LEFT JOIN con_obras_versao ov ON cv.usina=ov.usina AND o.numero = ov.numero AND cv.num_versao=ov.num_versao");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem>={(f.ProgramacaoDataHoraDe != null ? $"'{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'" : "CURDATE()")}");

            if (f.ProgramacaoDataHoraAte != null)
                sql.Append($" AND r.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");
            sql.Append($" ) a");
            sql.Append($" INNER JOIN con_programacao r");
            sql.Append($" ON a.usina=r.usina AND a.ano_contrato=r.ano_contrato AND a.no_contrato=r.no_contrato AND a.dt_concretagem=r.dt_concretagem");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND r.no_obra = o.numero");
            sql.Append($" LEFT JOIN con_contrato_versao cv");
            sql.Append($" ON r.usina=cv.usina AND r.ano_contrato=cv.ano_contrato AND r.no_contrato=cv.num_contrato");
            sql.Append($" LEFT JOIN con_obras_versao ov ON cv.usina=ov.usina AND o.numero = ov.numero AND cv.num_versao=ov.num_versao");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem>={(f.ProgramacaoDataHoraDe != null ? $"'{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'" : "CURDATE()")}");

            if (f.ProgramacaoDataHoraAte != null)
                sql.Append($" AND r.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");

            cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_prog_superior");
            cnn.Execute(sql.ToString());

            sql.Clear();
            sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_prog_inferior (PRIMARY KEY(usina,ano_contrato,no_contrato)) ENGINE=MyISAM AS");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,r.dt_concretagem,MAX(r.seq_prog) seq_prog");
            sql.Append($" FROM (");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,MAX(r.dt_concretagem) dt_concretagem");
            sql.Append($" FROM con_programacao r");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND r.no_obra = o.numero");
            sql.Append($" LEFT JOIN con_contrato_versao cv");
            sql.Append($" ON r.usina=cv.usina AND r.ano_contrato=cv.ano_contrato AND r.no_contrato=cv.num_contrato");
            sql.Append($" LEFT JOIN con_obras_versao ov ON cv.usina=ov.usina AND o.numero = ov.numero AND cv.num_versao=ov.num_versao");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem<CURDATE()");

            if (f.ProgramacaoDataHoraDe != null)
                sql.Append($" AND r.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");
            sql.Append($" ) a");
            sql.Append($" INNER JOIN con_programacao r");
            sql.Append($" ON a.usina=r.usina AND a.ano_contrato=r.ano_contrato AND a.no_contrato=r.no_contrato AND a.dt_concretagem=r.dt_concretagem");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND r.no_obra = o.numero");
            sql.Append($" LEFT JOIN con_contrato_versao cv");
            sql.Append($" ON r.usina=cv.usina AND r.ano_contrato=cv.ano_contrato AND r.no_contrato=cv.num_contrato");
            sql.Append($" LEFT JOIN con_obras_versao ov ON cv.usina=ov.usina AND o.numero = ov.numero AND cv.num_versao=ov.num_versao");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem<CURDATE()");

            if (f.ProgramacaoDataHoraDe != null)
                sql.Append($" AND r.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");

            cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_prog_inferior");
            cnn.Execute(sql.ToString());

            var temFiltroTabelaPagamentos = (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn) || !string.IsNullOrWhiteSpace(f.PortadorIn) || !string.IsNullOrWhiteSpace(f.BandeiraIn));

            if (temFiltroTabelaPagamentos)
            {
                sql.Clear();
                sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_contrato_pagamento_detalhe");
                sql.Append($" (KEY pagamento(usina,ano_contrato,num_contrato, seq_pgto)) ENGINE=MyISAM AS");
                sql.Append($" (SELECT COALESCE(dv.usina, d.usina) as usina, COALESCE(dv.ano_contrato, d.ano_contrato) as ano_contrato, COALESCE(dv.num_contrato, d.num_contrato) as num_contrato, COALESCE(dv.seq_pgto, d.seq_pgto) as seq_pgto, COALESCE(dv.portador, d.portador) as portador, null bandeira");
                sql.Append($" FROM con_contrato_dep d");
                sql.Append($" LEFT JOIN con_contrato_versao cv");
                sql.Append($" on d.num_contrato = cv.num_contrato and d.ano_contrato = cv.ano_contrato");
                sql.Append($" and d.usina = cv.usina");
                sql.Append($" and cv.num_versao = (SELECT MAX(cv2.num_versao) FROM con_contrato_versao cv2 WHERE cv2.usina = d.usina AND cv2.ano_contrato = d.ano_contrato AND cv2.num_contrato = d.num_contrato)");
                sql.Append($" and cv.status not in (9133, 9136)");
                sql.Append($" LEFT JOIN con_contrato_dep_versao dv");
                sql.Append($" on cv.num_contrato = dv.num_contrato and cv.ano_contrato = dv.ano_contrato");
                sql.Append($" and cv.usina = dv.usina)");
                sql.Append($" UNION");
                sql.Append($" (SELECT COALESCE(dv.usina, d.usina) as usina, COALESCE(dv.ano_contrato, d.ano_contrato) as ano_contrato, COALESCE(dv.num_contrato, d.num_contrato) as num_contrato, COALESCE(dv.seq_pgto, d.seq_pgto) as seq_pgto, b.portador, COALESCE(dv.bandeira, d.bandeira) as bandeira");
                sql.Append($" FROM con_contrato_ccredit d");
                sql.Append($" LEFT JOIN con_bandeira b ON d.bandeira=b.cod_bandeira");
                sql.Append($" LEFT JOIN con_contrato_versao cv");
                sql.Append($" on d.num_contrato = cv.num_contrato and d.ano_contrato = cv.ano_contrato");
                sql.Append($" and d.usina = cv.usina");
                sql.Append($" and cv.num_versao = (SELECT MAX(cv2.num_versao) FROM con_contrato_versao cv2 WHERE cv2.usina = d.usina AND cv2.ano_contrato = d.ano_contrato AND cv2.num_contrato = d.num_contrato)");
                sql.Append($" and cv.status not in (9133, 9136)");
                sql.Append($" LEFT JOIN con_contrato_ccredit_versao dv");
                sql.Append($" on cv.num_contrato = dv.num_contrato and cv.ano_contrato = dv.ano_contrato and cv.usina = dv.usina");
                sql.Append($" LEFT JOIN con_bandeira bv ON dv.bandeira = bv.cod_bandeira)");

                cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_contrato_pagamento_detalhe");
                cnn.Execute(sql.ToString());

                sql.Clear();
                sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_contrato_pagamento");
                sql.Append($" (KEY contrato(usina,ano_contrato,num_contrato), KEY(tp_cobranca), KEY(bandeira), KEY(portador)) ENGINE=MyISAM AS");
                sql.Append($" SELECT COALESCE(pv.usina, p.usina) as usina, COALESCE(pv.ano_contrato, p.ano_contrato) as ano_contrato, COALESCE(pv.num_contrato, p.num_contrato) as num_contrato,");
                sql.Append($" COALESCE(pv.tp_cobranca, p.tp_cobranca) as tp_cobranca, IFNULL(d.bandeira, 0) bandeira");
                sql.Append($", IFNULL(IFNULL(d.portador,COALESCE(tcv.portador, tc.portador)), 0) portador");
                sql.Append($" FROM con_contrato_pag p");
                sql.Append($" LEFT JOIN con_tipo_cobranca tc ON p.tp_cobranca=tc.tipo_cobranca");
                sql.Append($" LEFT JOIN tmp_contrato_pagamento_detalhe d");
                sql.Append($" ON p.usina=d.usina AND p.ano_contrato=d.ano_contrato AND p.num_contrato=d.num_contrato AND p.seq=d.seq_pgto");
                sql.Append($" LEFT JOIN con_contrato_versao cv");
                sql.Append($" ON p.num_contrato = cv.num_contrato AND p.ano_contrato = cv.ano_contrato");
                sql.Append($" AND p.usina = cv.usina");
                sql.Append($" AND cv.num_versao = (SELECT MAX(cv2.num_versao) FROM con_contrato_versao cv2 WHERE cv2.usina = p.usina AND cv2.ano_contrato = p.ano_contrato AND cv2.num_contrato = p.num_contrato)");
                sql.Append($" AND cv.status not in (9133, 9136)");
                sql.Append($" LEFT JOIN con_contrato_pag_versao pv ON pv.num_contrato = cv.num_contrato AND pv.ano_contrato = cv.ano_contrato AND pv.num_versao=cv.num_versao");
                sql.Append($" AND pv.usina = cv.usina");
                sql.Append($" LEFT JOIN con_tipo_cobranca tcv ON pv.tp_cobranca = tcv.tipo_cobranca");
                sql.Append($" WHERE true");

                if (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn))
                    sql.Append($" AND COALESCE(pv.tp_cobranca, p.tp_cobranca) IN({f.TipoCobrancaIn})");

                if (!string.IsNullOrWhiteSpace(f.BandeiraIn))
                    sql.Append($" AND d.bandeira IN({f.BandeiraIn})");

                if (!string.IsNullOrWhiteSpace(f.PortadorIn))
                    sql.Append($" AND IFNULL(d.portador,COALESCE(tcv.portador, tc.portador)) IN({f.PortadorIn})");

                cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_contrato_pagamento");
                cnn.Execute(sql.ToString());
            }

            sql.Clear();
            sql.Append($"SELECT b.* FROM (SELECT COALESCE(cv.usina, c.usina) {nameof(ConsultarObraQueryResult.Usina)}");
            sql.Append($", COALESCE(cv.ano_contrato, c.ano_contrato) {nameof(ConsultarObraQueryResult.ContratoAno)}");
            sql.Append($", COALESCE(cv.num_contrato, c.num_contrato) {nameof(ConsultarObraQueryResult.ContratoNumero)}");
            sql.Append($", COALESCE(cv.dt_contrato, c.dt_contrato) {nameof(ConsultarObraQueryResult.ContratoData)}");
            sql.Append($", COALESCE(cv.vendedor, c.vendedor) {nameof(ConsultarObraQueryResult.VendedorCodigo)}");
            sql.Append($", COALESCE(cv.interv, c.interv) {nameof(ConsultarObraQueryResult.ClienteCodigo)}");
            sql.Append($", COALESCE(cv.total_m3, c.total_m3) {nameof(ConsultarObraQueryResult.VolumeTotal)}");
            sql.Append($", COALESCE(cv.status, c.status) {nameof(ConsultarObraQueryResult.Status)}");
            sql.Append($", COALESCE(cv.analista, c.analista) {nameof(ConsultarObraQueryResult.AnalistaCodigo)}");
            sql.Append($", COALESCE(cv.vlr_total_ctr, c.vlr_total_ctr) {nameof(ConsultarObraQueryResult.ContratoValorTotal)}");//***
            sql.Append($", COALESCE(cv.usina_principal, c.usina_principal) UsinaPrincipal");
            sql.Append($", COALESCE(iv.razao, i.razao) {nameof(ConsultarObraQueryResult.ClienteRazao)}");
            sql.Append($", COALESCE(iv.DDD, i.DDD) {nameof(ConsultarObraQueryResult.ClienteTelefoneDdd)}");
            sql.Append($", COALESCE(iv.Tel, i.Tel) {nameof(ConsultarObraQueryResult.ClienteTelefoneNumero)}");
            sql.Append($", COALESCE(iv.ddd_celular, i.ddd_celular) {nameof(ConsultarObraQueryResult.ClienteCelularDdd)}");
            sql.Append($", COALESCE(iv.celular, i.celular) {nameof(ConsultarObraQueryResult.ClienteCelularNumero)}");
            sql.Append($", COALESCE(iv.ddd_com, i.ddd_com) {nameof(ConsultarObraQueryResult.ClienteTelefoneComercialDdd)}");
            sql.Append($", COALESCE(iv.tel_com, i.tel_com) {nameof(ConsultarObraQueryResult.ClienteTelefoneComercialNumero)}");

            if (!limiteCreditoPorGrupoEconomico)
            {
                sql.Append($", COALESCE(iv.lim_cred_val, i.lim_cred_val) {nameof(ConsultarObraQueryResult.ClienteLimiteData)}");
                sql.Append($", COALESCE(iv.limite_cred, i.limite_cred) {nameof(ConsultarObraQueryResult.ClienteLimiteValor)}");
            }
            else
            {
                sql.Append($", IF(NOT ISNULL(COALESCE(gev.limite_cred, ge.limite_cred)), COALESCE(gev.lim_cred_val, ge.lim_cred_val), COALESCE(iv.lim_cred_val, i.lim_cred_val)) {nameof(ConsultarObraQueryResult.ClienteLimiteData)}");
                sql.Append($", IFNULL(COALESCE(gev.limite_cred, ge.limite_cred), COALESCE(iv.limite_cred, i.limite_cred)) {nameof(ConsultarObraQueryResult.ClienteLimiteValor)}");
            }
                
            sql.Append($", COALESCE(ov.tipo_cobranca, o.tipo_cobranca) {nameof(ConsultarObraQueryResult.TipoCobrancaCodigo)}");
            sql.Append($", COALESCE(vv.nome, v.nome) {nameof(ConsultarObraQueryResult.VendedorNome)}");
            sql.Append($", COALESCE(sv.descr, s.descr) {nameof(ConsultarObraQueryResult.StatusDescricao)}");
            sql.Append($", COALESCE(av.nome_reduzido, a.nome_reduzido) {nameof(ConsultarObraQueryResult.AnalistaNomeReduzido)}");
            sql.Append($", COALESCE(tcv.Descr, tc.Descr) {nameof(ConsultarObraQueryResult.TipoCobrancaDescricao)}");
            sql.Append($", COALESCE(ov.numero, o.numero) {nameof(ConsultarObraQueryResult.ObraNumero)}");
            sql.Append($", COALESCE(ov.ano_chamada, o.ano_chamada) {nameof(ConsultarObraQueryResult.PropostaAno)}");
            sql.Append($", COALESCE(ov.no_chamada, o.no_chamada) {nameof(ConsultarObraQueryResult.PropostaNumero)}");
            sql.Append($", COALESCE(ov.obra_contato, o.obra_contato) {nameof(ConsultarObraQueryResult.ObraContato)}");
            sql.Append($", COALESCE(ov.obra_nome, o.obra_nome) {nameof(ConsultarObraQueryResult.ObraNome)}");
            sql.Append($", COALESCE(omv.mun, om.mun) {nameof(ConsultarObraQueryResult.ObraMunicipio)}");
            sql.Append($", COALESCE(IFNULL(cv.representante,cv.vendedor), IFNULL(c.representante,c.vendedor)) VendedorRepresentante");
            sql.Append($", COALESCE(iv.CNPJ_CPF, i.CNPJ_CPF) {nameof(ConsultarObraQueryResult.ClienteCpfCnpj)}");
            sql.Append($", COALESCE(IFNULL(upv.sigla,IFNULL(uv.sigla,u1v.sigla)), IFNULL(up.sigla,IFNULL(u.sigla,u1.sigla))) {nameof(ConsultarObraQueryResult.UsinaEntregaSigla)}");
            sql.Append($", COALESCE(IFNULL(pv.usina_entrega,IFNULL(p1v.usina_entrega,cv.usina_principal)), IFNULL(p.usina_entrega,IFNULL(p1.usina_entrega,c.usina_principal))) {nameof(ConsultarObraQueryResult.UsinaEntrega)}");
            sql.Append($", COALESCE(IFNULL(upv.emp_filial,IFNULL(uv.emp_filial,u1v.emp_filial)), IFNULL(up.emp_filial,IFNULL(u.emp_filial,u1.emp_filial))) {nameof(ConsultarObraQueryResult.UsinaEntregaFilial)}");
            sql.Append($", COALESCE(IFNULL(pv.dt_concretagem, p1v.dt_concretagem), IFNULL(p.dt_concretagem, p1.dt_concretagem)) {nameof(ConsultarObraQueryResult.DataConcretagem)}");
            sql.Append($", COALESCE(IFNULL(pv.horario, p1v.horario), IFNULL(p.horario, p1.horario)) {nameof(ConsultarObraQueryResult.Horario)}");
            sql.Append($", COALESCE(ov.status_cadastro, o.status_cadastro) {nameof(ConsultarObraQueryResult.StatusCadastro)}");
            sql.Append($", COALESCE(ov.status_comercial, o.status_comercial) {nameof(ConsultarObraQueryResult.StatusComercial)}");
            sql.Append($", COALESCE(ov.status_engenharia, o.status_engenharia) {nameof(ConsultarObraQueryResult.StatusEngenharia)}");
            sql.Append($", COALESCE(ov.status_financeiro, o.status_financeiro) {nameof(ConsultarObraQueryResult.StatusFinanceiro)}");
            sql.Append($", COALESCE(ov.cond_pgto, o.cond_pgto) {nameof(ConsultarObraQueryResult.CondicaoPagamentoCodigo)}");
            sql.Append($", COALESCE(cpv.descr, cp.descr) {nameof(ConsultarObraQueryResult.CondicaoPagamentoDescricao)}");
            sql.Append($", o.volume_status_comercial {nameof(ConsultarObraQueryResult.VolumeStatusComercial)}");

            sql.Append($", COALESCE(gev.codigo, ge.codigo) {nameof(ConsultarObraQueryResult.GrupoEconomicoCodigo)}");
            sql.Append($", COALESCE(gev.descricao, ge.descricao) {nameof(ConsultarObraQueryResult.GrupoEconomicoDescricao)}");

            sql.Append(" FROM con_obras as o");
            sql.Append(" LEFT JOIN con_contrato as c ON o.usina=c.usina AND o.ano_contrato=c.ano_contrato AND o.no_contrato=c.num_contrato");
            sql.Append(" LEFT JOIN con_vendedor AS v ON c.vendedor=v.cod");
            sql.Append(" LEFT JOIN ger_interv AS i ON c.interv=i.cod");

            sql.Append(" LEFT JOIN ger_grupo_economico ge ON ge.codigo=i.grupo_economico");

            sql.Append(" LEFT JOIN ger_geral AS s ON c.status=s.cod");
            sql.Append(" LEFT JOIN con_funcionario AS a ON c.analista=a.cod");

            sql.Append(" LEFT JOIN ger_municipio om ON o.obra_mun=om.cod");
            sql.Append(" LEFT JOIN tmp_prog_superior ptmp");
            sql.Append(" ON c.usina=ptmp.usina");
            sql.Append(" AND c.ano_contrato=ptmp.ano_contrato");
            sql.Append(" AND c.num_contrato=ptmp.no_contrato");
            sql.Append(" LEFT JOIN con_programacao p");
            sql.Append(" ON c.usina=p.usina");
            sql.Append(" AND c.ano_contrato=p.ano_contrato");
            sql.Append(" AND c.num_contrato=p.no_contrato");
            sql.Append(" AND p.seq_prog=ptmp.seq_prog");
            sql.Append(" LEFT JOIN tmp_prog_inferior p1tmp");
            sql.Append(" ON c.usina=p1tmp.usina");
            sql.Append(" AND c.ano_contrato=p1tmp.ano_contrato");
            sql.Append(" AND c.num_contrato=p1tmp.no_contrato");
            sql.Append(" LEFT JOIN con_programacao p1");
            sql.Append(" ON c.usina=p1.usina");
            sql.Append(" AND c.ano_contrato=p1.ano_contrato");
            sql.Append(" AND c.num_contrato=p1.no_contrato");
            sql.Append(" AND p1.seq_prog=p1tmp.seq_prog");
            sql.Append(" LEFT JOIN con_usina AS u ON p.usina_entrega=u.cod");
            sql.Append(" LEFT JOIN con_usina AS u1 ON p1.usina_entrega=u1.cod");
            sql.Append(" LEFT JOIN con_usina AS up ON c.usina_principal=up.cod");
            sql.Append(" LEFT JOIN ger_geral AS tc ON o.tipo_cobranca=tc.cod");
            sql.Append(" LEFT JOIN ger_cond_pag AS cp ON o.cond_pgto=cp.cod");
            sql.Append(" LEFT JOIN con_contrato_versao AS cv");
            sql.Append(" ON c.usina=cv.usina AND c.ano_contrato=cv.ano_contrato AND c.num_contrato=cv.num_contrato");
            sql.Append(" AND cv.num_versao = (SELECT MAX(cv2.num_versao) FROM con_contrato_versao cv2 WHERE cv2.usina = c.usina AND cv2.ano_contrato = c.ano_contrato AND cv2.num_contrato = c.num_contrato)");
            sql.Append(" AND cv.status not in (9133, 9136)");
            sql.Append(" LEFT JOIN con_vendedor AS vv ON cv.vendedor=vv.cod");
            sql.Append(" LEFT JOIN ger_interv AS iv ON cv.interv=iv.cod");

            sql.Append(" LEFT JOIN ger_grupo_economico gev ON gev.codigo=iv.grupo_economico");

            sql.Append(" LEFT JOIN ger_geral AS sv ON cv.status=sv.cod");
            sql.Append(" LEFT JOIN con_funcionario AS av ON cv.analista=av.cod");
            sql.Append(" LEFT JOIN con_obras_versao as ov ON cv.usina=ov.usina AND cv.ano_contrato=ov.ano_contrato AND cv.num_contrato=ov.no_contrato AND cv.num_versao=ov.num_versao");
            sql.Append(" LEFT JOIN ger_municipio omv ON ov.obra_mun=omv.cod");
            sql.Append(" LEFT JOIN con_programacao pv");
            sql.Append(" ON cv.usina=pv.usina");
            sql.Append(" AND cv.ano_contrato=pv.ano_contrato");
            sql.Append(" AND cv.num_contrato=pv.no_contrato");
            sql.Append(" AND pv.seq_prog=ptmp.seq_prog");
            sql.Append(" LEFT JOIN con_programacao p1v");
            sql.Append(" ON cv.usina=p1v.usina");
            sql.Append(" AND cv.ano_contrato=p1v.ano_contrato");
            sql.Append(" AND cv.num_contrato=p1v.no_contrato");
            sql.Append(" AND p1v.seq_prog=p1tmp.seq_prog");
            sql.Append(" LEFT JOIN con_usina AS uv ON pv.usina_entrega=uv.cod");
            sql.Append(" LEFT JOIN con_usina AS u1v ON p1v.usina_entrega=u1v.cod");
            sql.Append(" LEFT JOIN con_usina AS upv ON cv.usina_principal=upv.cod");
            sql.Append(" LEFT JOIN ger_geral AS tcv ON ov.tipo_cobranca=tcv.cod");
            sql.Append(" LEFT JOIN ger_cond_pag AS cpv ON ov.cond_pgto=cpv.cod");

            if (temFiltroTabelaPagamentos)
            {
                sql.Append(" LEFT JOIN tmp_contrato_pagamento cpag");
                sql.Append(" ON c.usina=cpag.usina");
                sql.Append(" AND c.ano_contrato=cpag.ano_contrato");
                sql.Append(" AND c.num_contrato=cpag.num_contrato");
            }

            sql.Append(" WHERE COALESCE(cv.status, c.status)<>0");

            if (!string.IsNullOrWhiteSpace(f.StatusCadastroIn))
                sql.Append($" AND COALESCE(ov.status_cadastro, o.status_cadastro) IN({f.StatusCadastroIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusComercialIn))
                sql.Append($" AND COALESCE(ov.status_comercial, o.status_comercial) IN({f.StatusComercialIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusEngenhariaIn))
                sql.Append($" AND COALESCE(ov.status_engenharia, o.status_engenharia) IN({f.StatusEngenhariaIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusFinanceiroIn))
                sql.Append($" AND COALESCE(ov.status_financeiro, o.status_financeiro) IN({f.StatusFinanceiroIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusGeracaoContratoIn)) 
            {
                if (f.StatusGeracaoContratoIn == "0")
                    sql.Append($" AND COALESCE(cv.num_contrato, c.num_contrato) = 0");
                else if (f.StatusGeracaoContratoIn == "1")
                    sql.Append($" AND COALESCE(cv.num_contrato, c.num_contrato) <> 0");
            }

            if (!f.ConsiderarEncerrados)
                sql.Append($" AND COALESCE(cv.dt_encer_cont, c.dt_encer_cont) IS NULL");

            if (f.Vendedor > 0)
                sql.Append($" AND COALESCE(cv.vendedor, c.vendedor) = {f.Vendedor}");
            else if (f.VendedoresPermitidos != "*" && !string.IsNullOrWhiteSpace(f.VendedoresPermitidos))
                sql.Append($" AND COALESCE(cv.vendedor, c.vendedor) in ({f.VendedoresPermitidos})");

            if (!string.IsNullOrWhiteSpace(f.CpfCnpj))
                sql.Append($" AND COALESCE(iv.CNPJ_CPF, i.CNPJ_CPF)='{f.CpfCnpj}'");

            if (f.Cliente > 0)
                sql.Append($" AND COALESCE(cv.interv, c.interv)={f.Cliente}");

            if (f.GrupoEconomico > 0)
                sql.Append($" AND COALESCE(iv.grupo_economico, i.grupo_economico)={f.GrupoEconomico}");

            if (!string.IsNullOrWhiteSpace(f.UsinaEntregaIn))
            {
                sql.Append($" AND (COALESCE(cv.usina_principal, c.usina_principal) IN({f.UsinaEntregaIn})");
                sql.Append($" OR COALESCE(pv.usina_entrega, p.usina_entrega) IN({f.UsinaEntregaIn})");
                sql.Append($" OR COALESCE(p1v.usina_entrega, p1.usina_entrega) IN({f.UsinaEntregaIn}))");
            }
            else
            {
                var usinasPermitidasUsuario = _usinaService.ListarUsinasPermitidasUsuario(Usuario).Select(t => t.Codigo);
                sql.Append($" AND (COALESCE(cv.usina_principal, c.usina_principal) IN({String.Join(",", usinasPermitidasUsuario)})");
                sql.Append($" OR COALESCE(pv.usina_entrega, p.usina_entrega) IN({String.Join(",", usinasPermitidasUsuario)})");
                sql.Append($" OR COALESCE(p1v.usina_entrega, p1.usina_entrega) IN({String.Join(",", usinasPermitidasUsuario)}))");
            }


            if (!string.IsNullOrWhiteSpace(f.AnalistaIn))
                sql.Append($" AND COALESCE(cv.analista, c.analista) IN({f.AnalistaIn})");

            if (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn))
                sql.Append($" AND cpag.tp_cobranca IN({f.TipoCobrancaIn})");

            if (!string.IsNullOrWhiteSpace(f.BandeiraIn))
                sql.Append($" AND cpag.bandeira IN({f.BandeiraIn})");

            if (!string.IsNullOrWhiteSpace(f.PortadorIn))
                sql.Append($" AND cpag.portador IN({f.PortadorIn})");

            if (f.ProgramacaoDataHoraDe != null)
            {
                sql.Append($" AND COALESCE(pv.dt_concretagem, p.dt_concretagem)>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");
                sql.Append($" AND COALESCE(pv.horario, p.horario)>='{f.ProgramacaoDataHoraDe?.ToString("HHmm")}'");
            }

            if (f.ProgramacaoDataHoraAte != null)
            {
                sql.Append($" AND COALESCE(pv.dt_concretagem, p.dt_concretagem)<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");
                sql.Append($" AND COALESCE(pv.horario, p.horario)<='{f.ProgramacaoDataHoraAte?.ToString("HHmm")}'");
            }

            if (f.PeriodoDe != null)
                sql.Append($" and COALESCE(cv.dt_contrato, c.dt_contrato)>='{f.PeriodoDe?.ToString("yyyy-MM-dd")}'");

            if (f.PeriodoAte != null)
                sql.Append($" and COALESCE(cv.dt_contrato, c.dt_contrato)<='{f.PeriodoAte?.ToString("yyyy-MM-dd")}'");

            if (f.AnoContrato > 0)
                sql.Append($" and COALESCE(cv.ano_contrato, c.ano_contrato)={f.AnoContrato}");

            if (f.NumeroContrato > 0)
                sql.Append($" and COALESCE(cv.num_contrato, c.num_contrato)={f.NumeroContrato}");

            if (f.AnoProposta > 0)
                sql.Append($" and COALESCE(ov.ano_chamada, o.ano_chamada)={f.AnoProposta}");

            if (f.NumeroProposta > 0)
                sql.Append($" and COALESCE(ov.no_chamada, o.no_chamada)={f.NumeroProposta}");

            if (f.Segmentacao > 0)
                sql.Append($" AND COALESCE(cv.segmentacao, c.segmentacao)={f.Segmentacao}");

            if (f.ContratoFinalidade > 0)
                sql.Append($" AND COALESCE(cv.finalidade_ctr, c.finalidade_ctr)={f.ContratoFinalidade}");


            var possuiFiltroParaContrato = !string.IsNullOrWhiteSpace(f.AnalistaIn);
            possuiFiltroParaContrato = possuiFiltroParaContrato || !string.IsNullOrWhiteSpace(f.TipoCobrancaIn);
            possuiFiltroParaContrato = possuiFiltroParaContrato || !string.IsNullOrWhiteSpace(f.BandeiraIn);
            possuiFiltroParaContrato = possuiFiltroParaContrato || !string.IsNullOrWhiteSpace(f.PortadorIn);
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.ProgramacaoDataHoraDe != null;
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.ProgramacaoDataHoraAte != null;
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.PeriodoDe != null;
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.PeriodoAte != null;
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.AnoContrato > 0;
            possuiFiltroParaContrato = possuiFiltroParaContrato || f.NumeroContrato > 0;
            possuiFiltroParaContrato = possuiFiltroParaContrato || (!string.IsNullOrWhiteSpace(f.StatusGeracaoContratoIn) && !f.StatusGeracaoContratoIn.Contains("0"));

            if(!possuiFiltroParaContrato)
            {

                sql.Append($" UNION SELECT ");
                sql.Append($"COALESCE(ov.usina, o.usina) AS Usina, ");
                sql.Append($"COALESCE(ov.ano_contrato, o.ano_contrato) AS ContratoAno, ");
                sql.Append($"o.no_contrato AS ContratoNumero, ");
                sql.Append($"NULL AS ContratoData, ");
                sql.Append($"COALESCE(ch.vendedor, chv.vendedor) AS VendedorCodigo, ");
                sql.Append($"COALESCE(ch.cod_cliente, chv.cod_cliente) AS ClienteCodigo, ");
                sql.Append($"COALESCE(ch.total_m3, chv.total_m3) AS VolumeTotal, ");
                sql.Append($"COALESCE(ch.`status`, chv.`status`) AS Status, ");
                sql.Append($"NULL AS AnalistaCodigo, ");
                sql.Append($"COALESCE(ch.vlr_total_ctr, chv.vlr_total_ctr) AS ContratoValorTotal, ");
                sql.Append($"o.obra_usina AS UsinaPrincipal, ");
                sql.Append($"COALESCE(ch.cliente, chv.cliente) AS ClienteRazao, ");
                sql.Append($"COALESCE(ch.ddd, chv.ddd) AS ClienteTelefoneDdd, ");
                sql.Append($"COALESCE(ch.tel, chv.tel) AS ClienteTelefoneNumero, ");
                sql.Append($"COALESCE(ch.ddd_celular, chv.ddd_celular) AS ClienteCelularDdd, ");
                sql.Append($"COALESCE(ch.celular, chv.celular) AS ClienteCelularNumero, ");
                sql.Append($"COALESCE(ch.ddd_com, chv.ddd_com) AS ClienteTelefoneComercialDdd, ");
                sql.Append($"COALESCE(ch.tel_com, chv.tel_com) AS ClienteTelefoneComercialNumero, ");

                if (!limiteCreditoPorGrupoEconomico)
                {
                    sql.Append($"cli.lim_cred_val AS ClienteLimiteData, ");
                    sql.Append($"cli.Limite_Cred AS ClienteLimiteValor, ");
                }
                else
                {
                    sql.Append($"IF(NOT ISNULL(ge.Limite_Cred), ge.lim_cred_val, cli.lim_cred_val) AS ClienteLimiteData, ");
                    sql.Append($"IFNULL(ge.Limite_Cred, cli.Limite_Cred) AS ClienteLimiteValor, ");
                }
                

                sql.Append($"COALESCE(ov.tipo_cobranca, o.tipo_cobranca) AS TipoCobrancaCodigo, ");
                sql.Append($"v.nome AS VendedorNome, ");
                sql.Append($"'' AS StatusDescricao, ");
                sql.Append($"'' AS AnalistaNomeReduzido, ");
                sql.Append($" COALESCE(tcv.Descr, tc.Descr) AS TipoCobrancaDescricao, ");
                sql.Append($"o.numero AS ObraNumero, ");
                sql.Append($"o.ano_chamada AS PropostaAno, ");
                sql.Append($"o.no_chamada AS PropostaNumero, ");
                sql.Append($"o.obra_contato AS ObraContato, ");
                sql.Append($"o.obra_nome AS ObraNome, ");
                sql.Append($"omv.mun AS ObraMunicipio, ");
                sql.Append($"COALESCE(ch.represent, chv.represent) AS VendedorRepresentante, ");
                sql.Append($"COALESCE(ch.cnpj_cpf, chv.cnpj_cpf) AS ClienteCpfCnpj, ");
                sql.Append($"uent.sigla AS UsinaEntregaSigla, ");
                sql.Append($"uent.cod AS UsinaEntrega, ");
                sql.Append($"uent.emp_filial AS UsinaEntregaFilial, ");
                sql.Append($"NULL AS DataConcretagem, ");
                sql.Append($"NULL AS Horario, ");
                sql.Append($"o.status_cadastro AS StatusCadastro, ");
                sql.Append($"o.status_comercial AS StatusComercial, ");
                sql.Append($"o.status_engenharia AS StatusEngenharia, ");
                sql.Append($"o.status_financeiro AS StatusFinanceiro, ");
                sql.Append($"COALESCE(ov.cond_pgto, o.cond_pgto) AS CondicaoPagamentoCodigo, ");
                sql.Append($"COALESCE(cpv.descr, cp.descr) CondicaoPagamentoDescricao, ");
                sql.Append($"o.volume_status_comercial VolumeStatusComercial, ");

                sql.Append($"COALESCE(ge.codigo, 0) GrupoEconomicoCodigo, ");
                sql.Append($"COALESCE(ge.descricao, '') GrupoEconomicoDescricao ");

                sql.Append($"FROM ");
                sql.Append($"con_obras AS o ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"ger_municipio omv ON o.obra_mun=omv.cod ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"con_obras_versao AS ov ON o.usina=ov.usina AND o.numero=ov.numero AND o.no_chamada=ov.no_chamada AND o.ano_chamada=ov.ano_chamada ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"con_chtel AS ch ON ch.usina = o.usina AND ch.ano_chamada=o.ano_chamada AND ch.num_chamada=o.no_chamada AND ch.no_obra=o.numero ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"con_chtel_versao AS chv ON chv.usina = o.usina AND chv.ano_chamada=o.ano_chamada AND chv.num_chamada=o.no_chamada AND chv.no_obra=o.numero AND chv.num_versao = ov.num_versao ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"con_vendedor AS v on v.cod = COALESCE(chv.vendedor, ch.vendedor) ");
                sql.Append($"LEFT JOIN ");
                sql.Append($"ger_interv AS cli on cli.cod = COALESCE(chv.cod_cliente, ch.cod_cliente) ");

                sql.Append($"LEFT JOIN ger_grupo_economico ge ON ge.codigo=cli.grupo_economico ");

                sql.Append($"LEFT JOIN ");
                sql.Append($"con_usina AS uent ON uent.cod = o.obra_usina ");
                sql.Append($"LEFT JOIN ger_cond_pag AS cpv ON ov.cond_pgto=cpv.cod ");
                sql.Append($"LEFT JOIN ger_cond_pag AS cp ON o.cond_pgto=cp.cod ");
                sql.Append($"LEFT JOIN ger_geral AS tc ON o.tipo_cobranca=tc.cod ");
                sql.Append($"LEFT JOIN ger_geral AS tcv ON ov.tipo_cobranca=tcv.cod ");
                sql.Append($"WHERE ");
                sql.Append($"(o.usina, o.ano_contrato, o.no_contrato) NOT IN ");
                sql.Append($"( ");
                sql.Append($"SELECT ");
                sql.Append($"usina, ano_contrato, num_contrato ");
                sql.Append($"FROM ");
                sql.Append($"con_contrato ");
                sql.Append($")");
                sql.Append($" AND NOT ISNULL(ch.ano_chamada) ");

                // Filtros
                if (!string.IsNullOrWhiteSpace(f.StatusCadastroIn))
                    sql.Append($" AND COALESCE(ov.status_cadastro, o.status_cadastro) IN({f.StatusCadastroIn}) ");

                if (!string.IsNullOrWhiteSpace(f.StatusComercialIn))
                    sql.Append($" AND COALESCE(ov.status_comercial, o.status_comercial) IN({f.StatusComercialIn}) ");

                if (!string.IsNullOrWhiteSpace(f.StatusEngenhariaIn))
                    sql.Append($" AND COALESCE(ov.status_engenharia, o.status_engenharia) IN({f.StatusEngenhariaIn}) ");

                if (!string.IsNullOrWhiteSpace(f.StatusFinanceiroIn))
                    sql.Append($" AND COALESCE(ov.status_financeiro, o.status_financeiro) IN({f.StatusFinanceiroIn}) ");

                if (f.Vendedor > 0)
                    sql.Append($" AND v.cod = {f.Vendedor}");
                else if (f.VendedoresPermitidos != "*" && !string.IsNullOrWhiteSpace(f.VendedoresPermitidos))
                    sql.Append($" AND v.cod in ({f.VendedoresPermitidos}) ");

                if (!string.IsNullOrWhiteSpace(f.CpfCnpj))
                    sql.Append($" AND cli.CNPJ_CPF='{f.CpfCnpj}' ");

                if (f.GrupoEconomico > 0)
                    sql.Append($" AND cli.grupo_economico={f.GrupoEconomico}");

                if (f.Cliente > 0)
                    sql.Append($" AND cli.cod={f.Cliente} ");

                if (!string.IsNullOrWhiteSpace(f.UsinaEntregaIn))
                {
                    sql.Append($" AND COALESCE(ov.obra_usina, o.obra_usina) IN({f.UsinaEntregaIn}) ");
                }
                else
                {
                    var usinasPermitidasUsuario = _usinaService.ListarUsinasPermitidasUsuario(Usuario).Select(t => t.Codigo);
                    sql.Append($" AND COALESCE(ov.obra_usina, o.obra_usina) IN({String.Join(",", usinasPermitidasUsuario)}) ");
                }

                if (f.AnoProposta > 0)
                    sql.Append($" AND o.ano_chamada={f.AnoProposta} ");

                if (f.NumeroProposta > 0)
                    sql.Append($" AND o.no_chamada={f.NumeroProposta} ");

                if (f.Segmentacao > 0)
                    sql.Append($" AND COALESCE(chv.segmentacao, ch.segmentacao)={f.Segmentacao}");

                if (f.ContratoFinalidade > 0)
                    sql.Append($" AND COALESCE(chv.finalidade_ctr, ch.finalidade_ctr)={f.ContratoFinalidade}");

            }

            sql.Append(") AS b");

            sql.Append($" GROUP BY b.{nameof(ConsultarObraQueryResult.Usina)}");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.ObraNumero)}");

            if (ordenacao.Equals(""))
            {
                sql.Append($" ORDER BY b.{nameof(ConsultarObraQueryResult.DataConcretagem)}");
                sql.Append($", CONVERT(b.{nameof(ConsultarObraQueryResult.Horario)},UNSIGNED)");
                sql.Append($", b.{nameof(ConsultarObraQueryResult.Usina)}");
                sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoAno)}");
                sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoNumero)}");
            }
            else
                sql.Append($" ORDER BY {ordenacao}");

            var queryResult = cnn.Query<ConsultarObraQueryResult>(sql.ToString()).ToList<IQueryResult>();

            foreach (var record in queryResult.Where(x => ((ConsultarObraQueryResult)x).StatusComercial == EObraStatusComercial.Aguardando))
            {
                var item = (ConsultarObraQueryResult)record;
                var aprovUsina = _aprovacaoComercialUsinaRepository.ObterPorUsina(item.UsinaEntrega);

                if (aprovUsina == null)
                    item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                else
                {
                    if (!aprovUsina.Ativo)
                        item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                    else
                    {

                        var aprovUsuario = _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorUsuarioUsina(Usuario, aprovUsina.Id);
                        if (aprovUsuario != null)
                        {
                            var aprovHierarquia = _aprovacaoComercialHierarquiaRepository.ObterPorId(aprovUsuario.AprovacaoComercialHierarquiaId);
                            var ultimaVersao = _obraRepository.ObterUltimaVersaoObra(item.Usina, item.ObraNumero);

                            var aprovPendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(item.Usina, item.ObraNumero, ultimaVersao);

                            if (aprovPendentes.Count() == 0)
                                item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                            else
                            {

                                var aprovPendente = aprovPendentes.FirstOrDefault(x => x.NivelHierarquia == aprovHierarquia.NivelAutoridade);

                                if (aprovPendente == null)
                                    item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel;
                                else if (aprovPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                                    item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacao;
                                else
                                    item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel;

                            }
                        }

                    }
                }
            }

            if (f.AguardandoOutroNivel)
            {
                queryResult = queryResult
                 .Where(x => ((ConsultarObraQueryResult)x).StatusAprovComAlcada == ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel)
                 .ToList<IQueryResult>();
            }

            if (!string.IsNullOrWhiteSpace(f.StatusClicksignDocumentoIn))
            {
                foreach (var record in queryResult)
                {
                    var item = (ConsultarObraQueryResult)record;

                    if (item.ContratoNumero != 0)
                        item.ClicksignEnvio = _clicksignRepository.ListarContratoClicksignEnvios(item.Usina, item.ContratoAno, item.ContratoNumero);
                }

                queryResult = queryResult
                 .Where(x => f.StatusClicksignDocumentoIn.Contains(((int)((ConsultarObraQueryResult)x).StatusClicksignDocumento).ToString()))
                 .ToList<IQueryResult>();
            }

            var pagedItems = queryResult.Skip((pagina - 1) * porPagina).Take(porPagina).ToList();

            var result = new PagedList<IQueryResult>
            {
                CurrentPage = pagina,
                PageSize = porPagina,
                RecordCount = queryResult.Count,
                PageCount = (int)Math.Ceiling((double)queryResult.Count / porPagina),
                Records = pagedItems
            };

            if (f.AnalisarLimiteCredito)
            {
                var valorDifencaLimiteCliente = new Dictionary<int, float>();

                foreach (var record in result.Records)
                {
                    var item = (ConsultarObraQueryResult)record;

                    var intervenientesNoGrupo = "";
                    if (item.GrupoEconomicoCodigo != 0 && limiteCreditoPorGrupoEconomico)
                    {
                        sql.Clear();
                        sql.Append($"SELECT GROUP_CONCAT(cod) FROM ger_interv WHERE grupo_economico={item.GrupoEconomicoCodigo}");

                        intervenientesNoGrupo = cnn.QueryFirstOrDefault<string>(sql.ToString());
                    }

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(sal),0) saldo");
                    sql.Append($" FROM fin_car");
                    sql.Append($" WHERE tp_doc<>8");

                    if (intervenientesNoGrupo.Equals(""))
                        sql.Append($" AND cli={item.ClienteCodigo}");
                    else
                        sql.Append($" AND cli IN({intervenientesNoGrupo})");

                    var saldoCarSemCheque = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(if(ifnull(datediff(current_date(),liq_dt),dias_lib_cred)>dias_lib_cred,sal,if(desdo=0,if(isnull(liq_dt),sal,liq_vl_rec),liq_vl_rec))),0) saldo");
                    sql.Append($" FROM fin_car as car");
                    sql.Append($", (SELECT dias_lib_cred,inicio_validade FROM fin_parametro WHERE emp={(item.UsinaEntregaFilial / 1000)} AND inicio_validade<=current_date ORDER BY inicio_validade DESC LIMIT 1) pm");
                    sql.Append($" WHERE tp_doc=8");

                    if (intervenientesNoGrupo.Equals(""))
                        sql.Append($" AND cli={item.ClienteCodigo}");
                    else
                        sql.Append($" AND cli IN({intervenientesNoGrupo})");

                    var saldoCarCheque = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(comp.vlr_total_cobranca),0) saldo");
                    sql.Append($" FROM con_nf nf");
                    sql.Append($" LEFT JOIN con_nf_complemento comp");
                    sql.Append($" ON comp.filial=nf.filial AND comp.interv=nf.interv");
                    sql.Append($" AND comp.tp_doc=nf.tp_doc AND comp.num_nf=nf.num_nf");
                    sql.Append($" AND comp.serie=nf.serie AND comp.seq_nf=nf.seq_nf");
                    sql.Append($" WHERE ISNULL(nf.dt_fatura)");
                    sql.Append($" AND nf.motivo_cancel=0");

                    if (intervenientesNoGrupo.Equals(""))
                        sql.Append($" AND nf.interv={item.ClienteCodigo}");
                    else
                        sql.Append($" AND nf.interv IN({intervenientesNoGrupo})");

                    sql.Append($" AND nf.data_remessa<=curdate()");

                    var valorNfNaoFaturada = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    item.ClienteSaldoContasAReceber = (saldoCarSemCheque + saldoCarCheque);
                    item.ClienteValorTotalNfsNaoFaturadas = valorNfNaoFaturada;

                    var calculaLimite = (item.DataConcretagem >= DateTime.Today);

                    sql.Clear();
                    sql.Append($"SELECT usina ContratoUsina, ano_contrato ContratoAno, no_contrato ContratoNumero, dt_concretagem DataConcretagem");
                    sql.Append($", SUM(totalProgramacao) ValorTotalProgramacao, SUM(vlrTotalNF) ValorTotalNF");
                    sql.Append($" FROM");
                    sql.Append($" (SELECT p.usina, p.ano_contrato, p.no_contrato, COALESCE(cv.interv,c.interv) as interv, p.dt_concretagem");
                    sql.Append($", IFNULL(p.vlr_total_prog, 0) totalProgramacao, IFNULL(nf.vlrTotalNF, 0) vlrTotalNF");
                    sql.Append($" FROM con_programacao p");
                    sql.Append($" INNER JOIN con_contrato c ON p.usina=c.usina AND p.no_contrato=c.num_contrato AND p.ano_contrato=c.ano_contrato");
                    sql.Append($" LEFT JOIN con_contrato_versao cv ON p.usina = cv.usina");
                    sql.Append($" AND p.no_contrato = cv.num_contrato AND p.ano_contrato = cv.ano_contrato");
                    sql.Append($" AND cv.num_versao = (SELECT MAX(cv2.num_versao) FROM con_contrato_versao cv2 WHERE cv2.usina = p.usina AND cv2.ano_contrato = p.ano_contrato AND cv2.num_contrato = p.no_contrato)");
                    sql.Append($" AND cv.status not in (9133, 9136)");
                    sql.Append($" LEFT JOIN (SELECT usina_contrato, num_contrato, ano_contrato, seq_prog");
                    sql.Append($", SUM(vlr_total_cobranca) vlrTotalNF");
                    sql.Append($" FROM con_nf nf");
                    sql.Append($" LEFT JOIN con_nf_complemento comp");
                    sql.Append($" ON comp.filial=nf.filial AND comp.interv=nf.interv");
                    sql.Append($" AND comp.tp_doc=nf.tp_doc AND comp.num_nf=nf.num_nf");
                    sql.Append($" AND comp.serie=nf.serie AND comp.seq_nf=nf.seq_nf");
                    sql.Append($" WHERE ISNULL(dt_fatura)");
                    sql.Append($" AND motivo_cancel=0 AND data_remessa=curdate()");
                    sql.Append($" GROUP BY usina_contrato, num_contrato, ano_contrato, seq_prog) nf");
                    sql.Append($" ON p.usina=nf.usina_contrato");
                    sql.Append($" AND p.no_contrato=nf.num_contrato");
                    sql.Append($" AND p.ano_contrato=nf.ano_contrato");
                    sql.Append($" AND p.seq_prog=nf.seq_prog");
                    sql.Append($" WHERE (p.dt_concretagem>=curdate()");
                    if (f.ProgramacaoDataHoraDe != null && f.ProgramacaoDataHoraAte != null)
                    {
                        sql.Append($" AND (p.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");
                        sql.Append($" AND p.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}')");
                    }
                    sql.Append($")");
                    sql.Append($" AND p.status<>{(int)EProgramacaoStatus.Cancelada}");

                    if (intervenientesNoGrupo.Equals(""))
                        sql.Append($" AND COALESCE(cv.interv, c.interv)={item.ClienteCodigo}");
                    else
                        sql.Append($" AND COALESCE(cv.interv, c.interv) IN({intervenientesNoGrupo})");

                    sql.Append($" ) a");
                    sql.Append($" GROUP BY usina, ano_contrato, no_contrato, dt_concretagem");
                    sql.Append($" ORDER BY interv, dt_concretagem");

                    var programacoes = cnn.Query(sql.ToString());

                    item.LimiteCreditoDisponivel = item.ClienteLimiteValor - (saldoCarSemCheque + saldoCarCheque) - valorNfNaoFaturada;
                    item.LimiteCreditoSaldo = item.LimiteCreditoDisponivel;

                    foreach (var programacao in programacoes)
                    {
                        var carregaLimite = false;

                        var valorDiferencaLimite = 0f;

                        if (calculaLimite && programacao.DataConcretagem >= DateTime.Today)
                        {
                            carregaLimite = true;

                            if (!valorDifencaLimiteCliente.TryGetValue(item.ClienteCodigo, out valorDiferencaLimite))
                            {
                                valorDiferencaLimite = item.ClienteLimiteValor - (saldoCarSemCheque + saldoCarCheque) - valorNfNaoFaturada;
                                valorDifencaLimiteCliente.Add(item.ClienteCodigo, valorDiferencaLimite);
                            }

                            if (programacao.DataConcretagem <= DateTime.Today)
                            {
                                valorDiferencaLimite += programacao.ValorTotalNF;
                                if (f.AnaliseLimiteConsiderarPrevisao)
                                    valorDifencaLimiteCliente[item.ClienteCodigo] += (programacao.ValorTotalNF - programacao.ValorTotalProgramacao);
                            }
                            else
                                valorDifencaLimiteCliente[item.ClienteCodigo] -= programacao.ValorTotalProgramacao;
                        }

                        if (f.AnaliseLimiteConsiderarPrevisao)
                        {
                            item.ValorProgramadoCliente += programacao.ValorTotalProgramacao;

                            if (item.Usina == programacao.ContratoUsina && item.ContratoAno == programacao.ContratoAno && item.ContratoNumero == programacao.ContratoNumero)
                                item.ValorProgramado += programacao.ValorTotalProgramacao;
                        }
                        

                        if (carregaLimite && item.DataConcretagem == programacao.DataConcretagem)
                        {
                            //item.LimiteCreditoDisponivel = valorDiferencaLimite;
                            item.LimiteCreditoSaldo = valorDifencaLimiteCliente[item.ClienteCodigo];

                            var diferencaMaximaPermitida = -9.99f;

                            if (item.LimiteCreditoSaldo < diferencaMaximaPermitida)
                                item.LimiteCreditoAnalise = "Limite de Crédito Insuficiente";
                            else if (item.ClienteLimiteData != null)
                            {
                                if (item.ClienteLimiteData < item.DataConcretagem)
                                    item.LimiteCreditoAnalise = $"Data Limite Vencida: {item.ClienteLimiteData?.ToString("dd/MM/yyyy")}";
                                else
                                    item.LimiteCreditoAnalise = "Crédito OK";
                            }
                            else
                                item.LimiteCreditoAnalise = "Crédito OK";

                        }
                    }
                    valorDifencaLimiteCliente.Clear();
                }
            }

            return result;
        }

        public PagedList<IQueryResult> ConsultarObrasVersao(IFilter filtro, int pagina, int porPagina, string Usuario)
        {
            var f = (ConsultarObraFilter)filtro;

            var cnn = _context.Connection;
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();

            var minMaxProgramacao = "MIN";
            if (f.ProgramacaoDataHoraAte != null)
                minMaxProgramacao = "MAX";

            var filtroTmpTable = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(f.StatusCadastroIn))
                filtroTmpTable.Append($" AND o.status_cadastro IN({f.StatusCadastroIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusComercialIn))
                filtroTmpTable.Append($" AND o.status_comercial IN({f.StatusComercialIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusEngenhariaIn))
                filtroTmpTable.Append($" AND o.status_engenharia IN({f.StatusEngenhariaIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusFinanceiroIn))
                filtroTmpTable.Append($" AND o.status_financeiro IN({f.StatusFinanceiroIn})");


            if (!f.ConsiderarEncerrados)
                filtroTmpTable.Append($" AND c.dt_encer_cont IS NULL");

            if (f.Vendedor > 0)
                filtroTmpTable.Append($" AND c.vendedor = {f.Vendedor}");
            else if (f.VendedoresPermitidos != "*" && !string.IsNullOrWhiteSpace(f.VendedoresPermitidos))
                filtroTmpTable.Append($" AND c.vendedor in ({f.VendedoresPermitidos})");

            if (f.Cliente > 0)
                filtroTmpTable.Append($" AND c.interv={f.Cliente}");

            if (!string.IsNullOrWhiteSpace(f.UsinaEntregaIn))
            {
                filtroTmpTable.Append($" AND (c.usina_principal IN({f.UsinaEntregaIn})");
                filtroTmpTable.Append($" OR r.usina_entrega IN({f.UsinaEntregaIn}))");
            }
            else
            {
                var usinasPermitidasUsuario = _usinaService.ListarUsinasPermitidasUsuario(Usuario).Select(t => t.Codigo);
                filtroTmpTable.Append($" AND (c.usina_principal IN({String.Join(",", usinasPermitidasUsuario)})");
                filtroTmpTable.Append($" OR r.usina_entrega IN({String.Join(",", usinasPermitidasUsuario)}))");
            }

            if (f.Segmentacao > 0)
                filtroTmpTable.Append($" AND c.segmentacao={f.Segmentacao}");

            if (f.ContratoFinalidade > 0)
                filtroTmpTable.Append($" AND c.finalidade_ctr={f.ContratoFinalidade}");

            var sql = new StringBuilder();

            sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_prog_superior (PRIMARY KEY(num_versao,usina,ano_contrato,no_contrato)) ENGINE=MyISAM AS");
            sql.Append($" SELECT c.num_versao, r.usina,r.ano_contrato,r.no_contrato,r.dt_concretagem,{minMaxProgramacao}(r.seq_prog) seq_prog");
            sql.Append($" FROM (");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,{minMaxProgramacao}(r.dt_concretagem) dt_concretagem");
            sql.Append($" FROM con_programacao r");
            sql.Append($" INNER JOIN con_contrato_versao c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras_versao o ON c.num_versao=o.num_versao AND c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem>={(f.ProgramacaoDataHoraDe != null ? $"'{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'" : "CURDATE()")}");

            if (f.ProgramacaoDataHoraAte != null)
                sql.Append($" AND r.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY c.num_versao, r.usina,r.ano_contrato,r.no_contrato");
            sql.Append($" ) a");
            sql.Append($" INNER JOIN con_programacao r");
            sql.Append($" ON a.usina=r.usina AND a.ano_contrato=r.ano_contrato AND a.no_contrato=r.no_contrato AND a.dt_concretagem=r.dt_concretagem");
            sql.Append($" INNER JOIN con_contrato_versao c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras_versao o ON c.num_versao=o.num_versao c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem>={(f.ProgramacaoDataHoraDe != null ? $"'{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'" : "CURDATE()")}");

            if (f.ProgramacaoDataHoraAte != null)
                sql.Append($" AND r.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY c.num_versao,r.usina,r.ano_contrato,r.no_contrato");

            cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_prog_superior");
            cnn.Execute(sql.ToString());

            sql.Clear();
            sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_prog_inferior (PRIMARY KEY(usina,ano_contrato,no_contrato)) ENGINE=MyISAM AS");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,r.dt_concretagem,MAX(r.seq_prog) seq_prog");
            sql.Append($" FROM (");
            sql.Append($" SELECT r.usina,r.ano_contrato,r.no_contrato,MAX(r.dt_concretagem) dt_concretagem");
            sql.Append($" FROM con_programacao r");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem<CURDATE()");

            if (f.ProgramacaoDataHoraDe != null)
                sql.Append($" AND r.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");
            sql.Append($" ) a");
            sql.Append($" INNER JOIN con_programacao r");
            sql.Append($" ON a.usina=r.usina AND a.ano_contrato=r.ano_contrato AND a.no_contrato=r.no_contrato AND a.dt_concretagem=r.dt_concretagem");
            sql.Append($" INNER JOIN con_contrato c");
            sql.Append($" ON r.usina=c.usina AND r.ano_contrato=c.ano_contrato AND r.no_contrato=c.num_contrato");
            sql.Append($" INNER JOIN con_obras o ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append($" WHERE r.status<>{(int)EProgramacaoStatus.Cancelada}");
            sql.Append($" AND r.dt_concretagem<CURDATE()");

            if (f.ProgramacaoDataHoraDe != null)
                sql.Append($" AND r.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");

            sql.Append(filtroTmpTable);
            sql.Append($" GROUP BY r.usina,r.ano_contrato,r.no_contrato");

            cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_prog_inferior");
            cnn.Execute(sql.ToString());

            var temFiltroTabelaPagamentos = (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn) || !string.IsNullOrWhiteSpace(f.PortadorIn) || !string.IsNullOrWhiteSpace(f.BandeiraIn));

            if (temFiltroTabelaPagamentos)
            {
                sql.Clear();
                sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_contrato_pagamento_detalhe");
                sql.Append($" (KEY pagamento(usina,ano_contrato,num_contrato, seq_pgto)) ENGINE=MyISAM AS");
                sql.Append($" (SELECT d.usina, d.ano_contrato, d.num_contrato, d.seq_pgto, d.portador, null bandeira");
                sql.Append($" FROM con_contrato_dep d)");
                sql.Append($" UNION");
                sql.Append($" (SELECT d.usina, d.ano_contrato, d.num_contrato, d.seq_pgto, b.portador, d.bandeira");
                sql.Append($" FROM con_contrato_ccredit d");
                sql.Append($" LEFT JOIN con_bandeira b ON d.bandeira=b.cod_bandeira)");

                cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_contrato_pagamento_detalhe");
                cnn.Execute(sql.ToString());

                sql.Clear();
                sql.Append($"CREATE TEMPORARY TABLE IF NOT EXISTS tmp_contrato_pagamento");
                sql.Append($" (KEY contrato(usina,ano_contrato,num_contrato), KEY(tp_cobranca), KEY(bandeira), KEY(portador)) ENGINE=MyISAM AS");
                sql.Append($" SELECT p.usina, p.ano_contrato, p.num_contrato, p.tp_cobranca, IFNULL(d.bandeira, 0) bandeira");
                sql.Append($", IFNULL(IFNULL(d.portador,tc.portador), 0) portador");
                sql.Append($" FROM con_contrato_pag p");
                sql.Append($" LEFT JOIN con_tipo_cobranca tc ON p.tp_cobranca=tc.tipo_cobranca");
                sql.Append($" LEFT JOIN tmp_contrato_pagamento_detalhe d");
                sql.Append($" ON p.usina=d.usina AND p.ano_contrato=d.ano_contrato AND p.num_contrato=d.num_contrato AND p.seq=d.seq_pgto");
                sql.Append($" WHERE true");

                if (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn))
                    sql.Append($" AND p.tp_cobranca IN({f.TipoCobrancaIn})");

                if (!string.IsNullOrWhiteSpace(f.BandeiraIn))
                    sql.Append($" AND d.bandeira IN({f.BandeiraIn})");

                if (!string.IsNullOrWhiteSpace(f.PortadorIn))
                    sql.Append($" AND IFNULL(d.portador,tc.portador) IN({f.PortadorIn})");

                cnn.Execute("DROP TEMPORARY TABLE IF EXISTS tmp_contrato_pagamento");
                cnn.Execute(sql.ToString());
            }

            sql.Clear();
            sql.Append($"SELECT b.* FROM (SELECT c.usina {nameof(ConsultarObraQueryResult.Usina)}");
            sql.Append($", c.ano_contrato {nameof(ConsultarObraQueryResult.ContratoAno)}");
            sql.Append($", c.num_contrato {nameof(ConsultarObraQueryResult.ContratoNumero)}");
            sql.Append($", c.dt_contrato {nameof(ConsultarObraQueryResult.ContratoData)}");
            sql.Append($", c.vendedor {nameof(ConsultarObraQueryResult.VendedorCodigo)}");
            sql.Append($", c.interv {nameof(ConsultarObraQueryResult.ClienteCodigo)}");
            sql.Append($", c.total_m3 {nameof(ConsultarObraQueryResult.VolumeTotal)}");
            sql.Append($", c.status {nameof(ConsultarObraQueryResult.Status)}");
            sql.Append($", c.analista {nameof(ConsultarObraQueryResult.AnalistaCodigo)}");
            sql.Append($", vlr_total_ctr {nameof(ConsultarObraQueryResult.ContratoValorTotal)}");
            sql.Append($", c.usina_principal UsinaPrincipal");
            sql.Append($", i.razao {nameof(ConsultarObraQueryResult.ClienteRazao)}");
            sql.Append($", i.DDD {nameof(ConsultarObraQueryResult.ClienteTelefoneDdd)}");
            sql.Append($", i.Tel {nameof(ConsultarObraQueryResult.ClienteTelefoneNumero)}");
            sql.Append($", i.ddd_celular {nameof(ConsultarObraQueryResult.ClienteCelularDdd)}");
            sql.Append($", i.celular {nameof(ConsultarObraQueryResult.ClienteCelularNumero)}");
            sql.Append($", i.ddd_com {nameof(ConsultarObraQueryResult.ClienteTelefoneComercialDdd)}");
            sql.Append($", i.tel_com {nameof(ConsultarObraQueryResult.ClienteTelefoneComercialNumero)}");
            sql.Append($", i.lim_cred_val {nameof(ConsultarObraQueryResult.ClienteLimiteData)}");
            sql.Append($", i.limite_cred {nameof(ConsultarObraQueryResult.ClienteLimiteValor)}");
            sql.Append($", o.tipo_cobranca {nameof(ConsultarObraQueryResult.TipoCobrancaCodigo)}");
            sql.Append($", v.nome {nameof(ConsultarObraQueryResult.VendedorNome)}");
            sql.Append($", s.descr {nameof(ConsultarObraQueryResult.StatusDescricao)}");
            sql.Append($", a.nome_reduzido {nameof(ConsultarObraQueryResult.AnalistaNomeReduzido)}");
            sql.Append($", tc.Descr {nameof(ConsultarObraQueryResult.TipoCobrancaDescricao)}");
            sql.Append($", o.numero {nameof(ConsultarObraQueryResult.ObraNumero)}");
            sql.Append($", o.ano_chamada {nameof(ConsultarObraQueryResult.PropostaAno)}");
            sql.Append($", o.no_chamada {nameof(ConsultarObraQueryResult.PropostaNumero)}");
            sql.Append($", o.obra_contato {nameof(ConsultarObraQueryResult.ObraContato)}");
            sql.Append($", o.obra_nome {nameof(ConsultarObraQueryResult.ObraNome)}");
            sql.Append($", om.mun {nameof(ConsultarObraQueryResult.ObraMunicipio)}");
            sql.Append($", IFNULL(c.representante,c.vendedor) VendedorRepresentante");
            sql.Append($", i.CNPJ_CPF {nameof(ConsultarObraQueryResult.ClienteCpfCnpj)}");
            sql.Append($", IFNULL(up.sigla,IFNULL(u.sigla,u1.sigla)) {nameof(ConsultarObraQueryResult.UsinaEntregaSigla)}");
            sql.Append($", IFNULL(p.usina_entrega,IFNULL(p1.usina_entrega,c.usina_principal)) {nameof(ConsultarObraQueryResult.UsinaEntrega)}");
            sql.Append($", IFNULL(up.emp_filial,IFNULL(u.emp_filial,u1.emp_filial)) {nameof(ConsultarObraQueryResult.UsinaEntregaFilial)}");
            sql.Append($", IFNULL(p.dt_concretagem, p1.dt_concretagem) {nameof(ConsultarObraQueryResult.DataConcretagem)}");
            sql.Append($", IFNULL(p.horario, p1.horario) {nameof(ConsultarObraQueryResult.Horario)}");
            sql.Append($", o.status_cadastro {nameof(ConsultarObraQueryResult.StatusCadastro)}");
            sql.Append($", o.status_comercial {nameof(ConsultarObraQueryResult.StatusComercial)}");
            sql.Append($", o.status_engenharia {nameof(ConsultarObraQueryResult.StatusEngenharia)}");
            sql.Append($", o.status_financeiro {nameof(ConsultarObraQueryResult.StatusFinanceiro)}");
            sql.Append($", o.cond_pgto {nameof(ConsultarObraQueryResult.CondicaoPagamentoCodigo)}");
            sql.Append($", cp.descr {nameof(ConsultarObraQueryResult.CondicaoPagamentoDescricao)}");
            sql.Append(" FROM con_contrato AS c");
            sql.Append(" LEFT JOIN con_vendedor AS v ON c.vendedor=v.cod");
            sql.Append(" LEFT JOIN ger_interv AS i ON c.interv=i.cod");
            sql.Append(" LEFT JOIN ger_geral AS s ON c.status=s.cod");
            sql.Append(" LEFT JOIN con_funcionario AS a ON c.analista=a.cod");
            sql.Append(" INNER JOIN con_obras as o ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append(" LEFT JOIN ger_municipio om ON o.obra_mun=om.cod");
            sql.Append(" LEFT JOIN tmp_prog_superior ptmp");
            sql.Append(" ON c.usina=ptmp.usina");
            sql.Append(" AND c.ano_contrato=ptmp.ano_contrato");
            sql.Append(" AND c.num_contrato=ptmp.no_contrato");
            sql.Append(" LEFT JOIN con_programacao p");
            sql.Append(" ON c.usina=p.usina");
            sql.Append(" AND c.ano_contrato=p.ano_contrato");
            sql.Append(" AND c.num_contrato=p.no_contrato");
            sql.Append(" AND p.seq_prog=ptmp.seq_prog");
            sql.Append(" LEFT JOIN tmp_prog_inferior p1tmp");
            sql.Append(" ON c.usina=p1tmp.usina");
            sql.Append(" AND c.ano_contrato=p1tmp.ano_contrato");
            sql.Append(" AND c.num_contrato=p1tmp.no_contrato");
            sql.Append(" LEFT JOIN con_programacao p1");
            sql.Append(" ON c.usina=p1.usina");
            sql.Append(" AND c.ano_contrato=p1.ano_contrato");
            sql.Append(" AND c.num_contrato=p1.no_contrato");
            sql.Append(" AND p1.seq_prog=p1tmp.seq_prog");
            sql.Append(" LEFT JOIN con_usina AS u ON p.usina_entrega=u.cod");
            sql.Append(" LEFT JOIN con_usina AS u1 ON p1.usina_entrega=u1.cod");
            sql.Append(" LEFT JOIN con_usina AS up ON c.usina_principal=up.cod");
            sql.Append(" LEFT JOIN ger_geral AS tc ON o.tipo_cobranca=tc.cod");
            sql.Append(" LEFT JOIN ger_cond_pag AS cp ON o.cond_pgto=cp.cod");

            if (temFiltroTabelaPagamentos)
            {
                sql.Append(" LEFT JOIN tmp_contrato_pagamento cpag");
                sql.Append(" ON c.usina=cpag.usina");
                sql.Append(" AND c.ano_contrato=cpag.ano_contrato");
                sql.Append(" AND c.num_contrato=cpag.num_contrato");
            }

            sql.Append(" WHERE c.status<>0");

            if (!string.IsNullOrWhiteSpace(f.StatusCadastroIn))
                sql.Append($" AND o.status_cadastro IN({f.StatusCadastroIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusComercialIn))
                sql.Append($" AND o.status_comercial IN({f.StatusComercialIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusEngenhariaIn))
                sql.Append($" AND o.status_engenharia IN({f.StatusEngenhariaIn})");

            if (!string.IsNullOrWhiteSpace(f.StatusFinanceiroIn))
                sql.Append($" AND o.status_financeiro IN({f.StatusFinanceiroIn})");


            if (!f.ConsiderarEncerrados)
                sql.Append($" AND c.dt_encer_cont IS NULL");

            if (f.Vendedor > 0)
                sql.Append($" AND c.vendedor = {f.Vendedor}");
            else if (f.VendedoresPermitidos != "*" && !string.IsNullOrWhiteSpace(f.VendedoresPermitidos))
                sql.Append($" AND c.vendedor in ({f.VendedoresPermitidos})");

            if (!string.IsNullOrWhiteSpace(f.CpfCnpj))
                sql.Append($" AND i.CNPJ_CPF='{f.CpfCnpj}'");

            if (f.GrupoEconomico > 0)
                sql.Append($" AND i.grupo_economico={f.GrupoEconomico}");

            if (f.Cliente > 0)
                sql.Append($" AND c.interv={f.Cliente}");

            if (!string.IsNullOrWhiteSpace(f.UsinaEntregaIn))
            {
                sql.Append($" AND (c.usina_principal IN({f.UsinaEntregaIn})");
                sql.Append($" OR p.usina_entrega IN({f.UsinaEntregaIn})");
                sql.Append($" OR p1.usina_entrega IN({f.UsinaEntregaIn}))");
            }
            else
            {
                var usinasPermitidasUsuario = _usinaService.ListarUsinasPermitidasUsuario(Usuario).Select(t => t.Codigo);
                sql.Append($" AND (c.usina_principal IN({String.Join(",", usinasPermitidasUsuario)})");
                sql.Append($" OR p.usina_entrega IN({String.Join(",", usinasPermitidasUsuario)})");
                sql.Append($" OR p1.usina_entrega IN({String.Join(",", usinasPermitidasUsuario)}))");
            }


            if (!string.IsNullOrWhiteSpace(f.AnalistaIn))
                sql.Append($" AND c.analista IN({f.AnalistaIn})");

            if (!string.IsNullOrWhiteSpace(f.TipoCobrancaIn))
                sql.Append($" AND cpag.tp_cobranca IN({f.TipoCobrancaIn})");

            if (!string.IsNullOrWhiteSpace(f.BandeiraIn))
                sql.Append($" AND cpag.bandeira IN({f.BandeiraIn})");

            if (!string.IsNullOrWhiteSpace(f.PortadorIn))
                sql.Append($" AND cpag.portador IN({f.PortadorIn})");

            if (f.ProgramacaoDataHoraDe != null)
            {
                sql.Append($" AND p.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");
                sql.Append($" AND p.horario>='{f.ProgramacaoDataHoraDe?.ToString("HHmm")}'");
            }

            if (f.ProgramacaoDataHoraAte != null)
            {
                sql.Append($" AND p.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}'");
                sql.Append($" AND p.horario<='{f.ProgramacaoDataHoraAte?.ToString("HHmm")}'");
            }

            if (f.PeriodoDe != null)
                sql.Append($" and c.dt_contrato>='{f.PeriodoDe?.ToString("yyyy-MM-dd")}'");

            if (f.PeriodoAte != null)
                sql.Append($" and c.dt_contrato<='{f.PeriodoAte?.ToString("yyyy-MM-dd")}'");

            if (f.AnoContrato > 0)
                sql.Append($" and c.ano_contrato={f.AnoContrato}");

            if (f.NumeroContrato > 0)
                sql.Append($" and c.num_contrato={f.NumeroContrato}");

            if (f.AnoProposta > 0)
                sql.Append($" and o.ano_chamada={f.AnoProposta}");

            if (f.NumeroProposta > 0)
                sql.Append($" and o.no_chamada={f.NumeroProposta}");

            if (f.Segmentacao > 0)
                sql.Append($" AND c={f.Segmentacao}");

            if (f.ContratoFinalidade > 0)
                sql.Append($" AND c.finalidade_ctr={f.ContratoFinalidade}");

            sql.Append(") AS b");

            sql.Append($" GROUP BY b.{nameof(ConsultarObraQueryResult.Usina)}");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoAno)}");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoNumero)}");

            sql.Append($" ORDER BY b.{nameof(ConsultarObraQueryResult.DataConcretagem)}");
            sql.Append($", CONVERT(b.{nameof(ConsultarObraQueryResult.Horario)},UNSIGNED)");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.Usina)}");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoAno)}");
            sql.Append($", b.{nameof(ConsultarObraQueryResult.ContratoNumero)}");

            var result = cnn.QueryPagedList<ConsultarObraQueryResult>(sql.ToString(), pagina, porPagina);

            foreach(var record in result.Records.Where(x => ((ConsultarObraQueryResult)x).StatusComercial == EObraStatusComercial.Aguardando))
            {
                var item = (ConsultarObraQueryResult)record;
                var aprovUsina = _aprovacaoComercialUsinaRepository.ObterPorUsina(item.UsinaEntrega);

                if (aprovUsina == null)
                    item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                else
                {
                    if (!aprovUsina.Ativo)
                        item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                    else
                    {

                        var aprovUsuario = _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorUsuarioUsina(Usuario, aprovUsina.Id);
                        var aprovHierarquia = _aprovacaoComercialHierarquiaRepository.ObterPorId(aprovUsuario.AprovacaoComercialHierarquiaId);
                        var ultimaVersao = _obraRepository.ObterUltimaVersaoObra(item.Usina, item.ObraNumero);

                        var aprovPendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(item.Usina, item.ObraNumero, ultimaVersao);

                        if (aprovPendentes.Count() == 0)
                            item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;
                        else
                        {

                            var aprovPendente = aprovPendentes.FirstOrDefault(x => x.NivelHierarquia == aprovHierarquia.NivelAutoridade);

                            if(aprovPendente == null)
                                item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel;
                            else if (aprovPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                                item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacao;
                            else
                                item.StatusAprovComAlcada = ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel;

                        }

                    }
                }
            }

            if (f.AnalisarLimiteCredito)
            {
                var valorDifencaLimiteCliente = new Dictionary<int, float>();

                foreach (var record in result.Records)
                {
                    var item = (ConsultarObraQueryResult)record;

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(sal),0) saldo");
                    sql.Append($" FROM fin_car");
                    sql.Append($" WHERE tp_doc<>8");
                    sql.Append($" AND cli={item.ClienteCodigo}");

                    var saldoCarSemCheque = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(if(ifnull(datediff(current_date(),liq_dt),dias_lib_cred)>dias_lib_cred,sal,if(desdo=0,if(isnull(liq_dt),sal,liq_vl_rec),liq_vl_rec))),0) saldo");
                    sql.Append($" FROM fin_car as car");
                    sql.Append($", (SELECT dias_lib_cred,inicio_validade FROM fin_parametro WHERE emp={(item.UsinaEntregaFilial / 1000)} AND inicio_validade<=current_date ORDER BY inicio_validade DESC LIMIT 1) pm");
                    sql.Append($" WHERE tp_doc=8");
                    sql.Append($" AND cli={item.ClienteCodigo}");

                    var saldoCarCheque = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    sql.Clear();
                    sql.Append($"SELECT IFNULL(SUM(comp.vlr_total_cobranca),0) saldo");
                    sql.Append($" FROM con_nf nf");
                    sql.Append($" LEFT JOIN con_nf_complemento comp");
                    sql.Append($" ON comp.filial=nf.filial AND comp.interv=nf.interv");
                    sql.Append($" AND comp.tp_doc=nf.tp_doc AND comp.num_nf=nf.num_nf");
                    sql.Append($" AND comp.serie=nf.serie AND comp.seq_nf=nf.seq_nf");
                    sql.Append($" WHERE ISNULL(nf.dt_fatura)");
                    sql.Append($" AND nf.motivo_cancel=0");
                    sql.Append($" AND nf.interv={item.ClienteCodigo}");
                    sql.Append($" AND nf.data_remessa<=curdate()");

                    var valorNfNaoFaturada = cnn.QueryFirstOrDefault<float>(sql.ToString());

                    item.ClienteSaldoContasAReceber = (saldoCarSemCheque + saldoCarCheque);
                    item.ClienteValorTotalNfsNaoFaturadas = valorNfNaoFaturada;

                    var calculaLimite = (item.DataConcretagem >= DateTime.Today);

                    sql.Clear();
                    sql.Append($"SELECT usina ContratoUsina, ano_contrato ContratoAno, no_contrato ContratoNumero, dt_concretagem DataConcretagem");
                    sql.Append($", SUM(totalProgramacao) ValorTotalProgramacao, SUM(vlrTotalNF) ValorTotalNF");
                    sql.Append($" FROM");
                    sql.Append($" (SELECT p.usina, p.ano_contrato, p.no_contrato, c.interv, p.dt_concretagem");
                    sql.Append($", IFNULL(p.vlr_total_prog, 0) totalProgramacao, IFNULL(nf.vlrTotalNF, 0) vlrTotalNF");
                    sql.Append($" FROM con_programacao p");
                    sql.Append($" INNER JOIN con_contrato c ON p.usina=c.usina AND p.no_contrato=c.num_contrato AND p.ano_contrato=c.ano_contrato");
                    sql.Append($" LEFT JOIN (SELECT usina_contrato, num_contrato, ano_contrato, seq_prog");
                    sql.Append($", SUM(vlr_total_cobranca) vlrTotalNF");
                    sql.Append($" FROM con_nf nf");
                    sql.Append($" LEFT JOIN con_nf_complemento comp");
                    sql.Append($" ON comp.filial=nf.filial AND comp.interv=nf.interv");
                    sql.Append($" AND comp.tp_doc=nf.tp_doc AND comp.num_nf=nf.num_nf");
                    sql.Append($" AND comp.serie=nf.serie AND comp.seq_nf=nf.seq_nf");
                    sql.Append($" WHERE ISNULL(dt_fatura)");
                    sql.Append($" AND motivo_cancel=0 AND data_remessa=curdate()");
                    sql.Append($" GROUP BY usina_contrato, num_contrato, ano_contrato, seq_prog) nf");
                    sql.Append($" ON p.usina=nf.usina_contrato");
                    sql.Append($" AND p.no_contrato=nf.num_contrato");
                    sql.Append($" AND p.ano_contrato=nf.ano_contrato");
                    sql.Append($" AND p.seq_prog=nf.seq_prog");
                    sql.Append($" WHERE (p.dt_concretagem>=curdate()");
                    if (f.ProgramacaoDataHoraDe != null && f.ProgramacaoDataHoraAte != null)
                    {
                        sql.Append($" OR (p.dt_concretagem>='{f.ProgramacaoDataHoraDe?.ToString("yyyy-MM-dd")}'");
                        sql.Append($" AND p.dt_concretagem<='{f.ProgramacaoDataHoraAte?.ToString("yyyy-MM-dd")}')");
                    }
                    sql.Append($")");
                    sql.Append($" AND p.status<>{(int)EProgramacaoStatus.Cancelada}");
                    sql.Append($" AND c.interv={item.ClienteCodigo}");
                    sql.Append($" ) a");
                    sql.Append($" GROUP BY usina, ano_contrato, no_contrato, dt_concretagem");
                    sql.Append($" ORDER BY interv, dt_concretagem");

                    var programacoes = cnn.Query(sql.ToString());

                    foreach (var programacao in programacoes)
                    {
                        var carregaLimite = false;

                        var valorDiferencaLimite = 0f;

                        if (calculaLimite && programacao.DataConcretagem >= DateTime.Today)
                        {
                            carregaLimite = true;

                            if (!valorDifencaLimiteCliente.TryGetValue(item.ClienteCodigo, out valorDiferencaLimite))
                            {
                                valorDiferencaLimite = item.ClienteLimiteValor - (saldoCarSemCheque + saldoCarCheque) - valorNfNaoFaturada;
                                valorDifencaLimiteCliente.Add(item.ClienteCodigo, valorDiferencaLimite);
                            }

                            if (programacao.DataConcretagem <= DateTime.Today)
                            {
                                valorDiferencaLimite += programacao.ValorTotalNF;
                                if (f.AnaliseLimiteConsiderarPrevisao)
                                    valorDifencaLimiteCliente[item.ClienteCodigo] += (programacao.ValorTotalNF - programacao.ValorTotalProgramacao);
                            }
                            else
                                valorDifencaLimiteCliente[item.ClienteCodigo] -= programacao.ValorTotalProgramacao;
                        }

                        if (item.Usina == programacao.ContratoUsina && item.ContratoAno == programacao.ContratoAno && item.ContratoNumero == programacao.ContratoNumero)
                        {
                            item.ValorProgramadoCliente += programacao.ValorTotalProgramacao;

                            if (carregaLimite && item.DataConcretagem == programacao.DataConcretagem)
                            {
                                item.LimiteCreditoDisponivel = valorDiferencaLimite;
                                item.LimiteCreditoSaldo = valorDifencaLimiteCliente[item.ClienteCodigo];

                                var diferencaMaximaPermitida = -9.99f;

                                if (item.LimiteCreditoSaldo < diferencaMaximaPermitida)
                                    item.LimiteCreditoAnalise = "Limite de Crédito Insuficiente";
                                else if (item.ClienteLimiteData != null)
                                {
                                    if (item.ClienteLimiteData < item.DataConcretagem)
                                        item.LimiteCreditoAnalise = $"Data Limite Vencida: {item.ClienteLimiteData?.ToString("dd/MM/yyyy")}";
                                    else
                                        item.LimiteCreditoAnalise = "Crédito OK";
                                }
                                else
                                    item.LimiteCreditoAnalise = "Crédito OK";

                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool ValidarContrato(Contrato contrato, string usuario, out string mensagensRetorno, bool aprovarAutomaticamente = false)
        {
            var queries = new List<string>();
            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var parametroDesativarObrigatoriedadeAprovacaoComercial = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoComercial)
                aprovarAutomaticamente = true;

            var limiteCreditoPorGrupoEconomico = _parametroRepository.ObterParametroN("web", "LimiteCreditoPorGrupoEconomico") == "1";

            var inconsistencias = "";
            mensagensRetorno = "";

            if (string.IsNullOrEmpty(contrato.IdAprovacaoVendedor))
                inconsistencias += "1;";

            if (string.IsNullOrEmpty(contrato.IdAprovacaoCadastro) && !parametroDesativarObrigatoriedadeAprovacaoComercial)
            {
                inconsistencias += "2;";
                mensagensRetorno += "Será Necessária Aprovação do Cadastro Antes da Aprovação Final!\n";
            }

            if (contrato.AprovaEngenharia == "S" && string.IsNullOrEmpty(contrato.IdAprovacaoEngenharia))
                inconsistencias += "3;";

            if (contrato.CadastroAprovado == "N")
            {
                inconsistencias += "4;";
                mensagensRetorno += "Cadastro Reprovado!\n";
            }

            var obra = _intervenienteRepository
                .ListarFiltradosTracking<Obra>(t => t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            sql.Clear();
            sql.AppendLine("SELECT status_comercial Comercial, status_cadastro Cadastro, status_financeiro Financeiro, status_engenharia Engenharia");
            sql.AppendLine("FROM con_obras WHERE usina = @Usina AND numero = @Numero");

            var obraStatus = cnn.QueryFirstOrDefault(sql.ToString(), new { Usina = obra.UsinaCodigo, Numero = obra.Numero });

            if(obra.StatusComercial != EObraStatusComercial.Aprovado)
                obra.StatusComercial = (EObraStatusComercial)(int)obraStatus.Comercial;

            if(obra.StatusCadastro != EObraStatusCadastro.Aprovado)
                obra.StatusCadastro = (EObraStatusCadastro)(int)obraStatus.Cadastro;

            if (obra.StatusFinanceiro != EObraStatusFinanceiro.Aprovado)
                obra.StatusFinanceiro = (EObraStatusFinanceiro)(int)obraStatus.Financeiro;

            if (obra.StatusEngenharia != EObraStatusEngenharia.Aprovado)
                obra.StatusEngenharia = (EObraStatusEngenharia)(int)obraStatus.Engenharia;

            if (obra == null)
            {
                inconsistencias += "5;";
                mensagensRetorno += "Nenhuma Obra Cadastrada Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.* FROM con_proposta_item AS p");
            sql.Append(" INNER JOIN con_obras AS o");
            sql.Append(" ON p.usina = o.usina");
            sql.Append(" AND p.no_obra=o.numero");
            sql.Append(" INNER JOIN con_contrato AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" WHERE (p.obs_aprov='ADMIN' or p.obs_aprov='')");
            sql.Append(" AND p.aprov_verbal='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");

            var rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Existem Descontos a Serem Aprovados Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.* FROM con_prop_bomba AS p");
            sql.Append(" INNER JOIN con_obras AS o");
            sql.Append(" ON p.usina = o.usina");
            sql.Append(" AND p.no_obra=o.numero");
            sql.Append(" INNER JOIN con_contrato AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" WHERE (obs_aprov='ADMIN' or obs_aprov='')");
            sql.Append(" AND aprov_verbal='S'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Existem Descontos a Serem Aprovados Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.*");
            sql.Append(" FROM con_obras o");
            sql.Append(" INNER JOIN view_obras_pendentes_aprov p ON o.usina=p.usina AND o.numero=p.numero");
            sql.Append($" WHERE o.usina={contrato.Usina}");
            sql.Append($" AND o.ano_contrato={contrato.Ano}");
            sql.Append($" AND o.no_contrato={contrato.Numero}");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Exitem pendencias de aprovações comerciais para este contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) FROM con_obras_tx AS ctx");
            sql.Append(" INNER JOIN con_obras AS o");
            sql.Append(" ON ctx.usina = o.usina");
            sql.Append(" AND ctx.obra=o.numero");
            sql.Append(" INNER JOIN con_contrato AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" WHERE ctx.aprov_desc='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");

            var qtdTaxasSemAprovacao = cnn.QueryFirstOrDefault<int>(sql.ToString());

            if (qtdTaxasSemAprovacao > 0)
            {
                inconsistencias += "7;";
                mensagensRetorno += "Existem Alterações em Condições de Contrato(s) a Serem Aprovadas!\n";
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) FROM con_obras_mp mp");
            sql.Append(" INNER JOIN con_obras AS o");
            sql.Append(" ON mp.usina = o.usina");
            sql.Append(" AND mp.obra=o.numero");
            sql.Append(" INNER JOIN con_contrato AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" WHERE mp.seleciona='N'");
            sql.Append(" AND mp.aprov_desc='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");

            var qtdMensagensSemAprovacao = cnn.QueryFirstOrDefault<int>(sql.ToString());

            if (qtdMensagensSemAprovacao > 0)
            {
                inconsistencias += "8;";
                mensagensRetorno += "Existem Alterações em Mensagens Padrões a Serem Aprovadas!\n";
            }

            inconsistencias += ValidarPagamentosContrato(contrato, true, out string mensagens);
            if (mensagens != "")
                mensagensRetorno += $"{mensagens}\n";

            if (ValidarCodigoObraPrefeitura(obra.UsinaCodigo, "", contrato.Numero, contrato.Ano))
            {
                inconsistencias += "13;";
                mensagensRetorno += "É Necessário Informar o Código Obra Na Prefeitura Deste Contrato!\n";
            }

            var cliente = _intervenienteRepository.ObterPorId(contrato.IntervenienteCodigo);

            if (cliente.GrupoEconomico != null && limiteCreditoPorGrupoEconomico)
            {
                if (cliente.GrupoEconomico.BloqueioMotivoCodigo != 0)
                {
                    inconsistencias += "14;";
                    mensagensRetorno += "Não é Possível Aprovar, Contrato Possui Interveniente em um Grupo Econômico Bloqueado!\n";
                }
            }
            else
            {
                if (cliente.BloqueioMotivoCodigo != 0)
                {
                    inconsistencias += "14;";
                    mensagensRetorno += "Não é Possível Aprovar, Contrato Possui Interveniente Bloqueado!\n";
                }
            }

            // Atualizando a lista de inconsistências do contrato
            sql.Clear();
            sql.Append("UPDATE con_contrato");
            sql.Append($" SET inconsistencias='{inconsistencias}'");

            if ((contrato.IsCadastroAprovado() && obra.StatusCadastro == EObraStatusCadastro.Aprovado) || parametroDesativarObrigatoriedadeAprovacaoComercial)
            {
                if (obra.StatusComercial == EObraStatusComercial.Aguardando)
                    contrato.Status = EContratoStatus.AguardandoAprovacaoComercial;
                else if (obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoConfirmacao)
                    contrato.Status = EContratoStatus.AguardandoConfirmacaoPagamento;
                else if (obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoDadosPagamento)
                    contrato.Status = EContratoStatus.AguardandoDadosPagamento;

                    sql.Append($", status={(int)contrato.Status}");
            }

            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            cnn.Execute(sql.ToString());
            queries.Add(sql.ToString());
            cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato", sql.ToString());

            // Caso não existam inconsistências e o parâmetro "aprovarAutomaticamente"=True aprova automáticamente o contrato
            if (aprovarAutomaticamente && inconsistencias == ""
                // acrescentadas essas validações para o processo ficar mais consistente
                && ((contrato.IsCadastroAprovado() && obra.StatusCadastro == EObraStatusCadastro.Aprovado) || parametroDesativarObrigatoriedadeAprovacaoComercial)
                && (obra.StatusComercial == EObraStatusComercial.NaoNecessita || obra.StatusComercial == EObraStatusComercial.Aprovado)
                && (obra.StatusEngenharia == EObraStatusEngenharia.NaoNecessita || obra.StatusEngenharia == EObraStatusEngenharia.Aprovado)
                && (obra.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita || obra.StatusFinanceiro == EObraStatusFinanceiro.Aprovado))
            {
                sql.Clear();
                sql.AppendLine("SELECT status");
                sql.AppendLine("FROM con_contrato WHERE usina = @Usina AND num_contrato = @Numero and ano_contrato=@Ano");

                var statusAnterior = cnn.QueryFirstOrDefault(sql.ToString(), new { contrato.Usina, contrato.Numero, contrato.Ano});

                contrato.Status = EContratoStatus.Aprovado;

                sql.Clear();
                sql.Append("UPDATE con_contrato");
                sql.Append($" SET id_aprov_dir='{StringHelper.GetIDD(usuario)}'");
                sql.Append(", fechado='S'");
                sql.Append($", status={(int)contrato.Status}");
                sql.Append($" WHERE usina={contrato.Usina}");
                sql.Append($" AND ano_contrato={contrato.Ano}");
                sql.Append($" AND num_contrato={contrato.Numero}");
                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato", sql.ToString());

                if (statusAnterior.status != (int)EContratoStatus.Aprovado)
                {
                    _webHookApplicationService.AdicionarWebhookContratoAprovado(contrato);
                }
                

                if (contrato.Status != EContratoStatus.Aprovado)
                {
                    var statusDe = _intervenienteRepository.ObterPorId<CadastroGeral>((int)contrato.Status);
                    var statusPara = _intervenienteRepository.ObterPorId<CadastroGeral>((int)EContratoStatus.Aprovado);

                    _intervenienteRepository.Adicionar(new ObraLog
                    {
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        AnoChamada = obra.AnoChamada ?? 0,
                        NumChamada = obra.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Sequencia = 1,
                        Evento = "ALTERAÇÃO STATUS",
                        Complemento = $"DE: {statusDe?.Descricao} PARA: {statusPara?.Descricao}",
                        Usuario = usuario,
                        Observacao = ""
                    });
                    _intervenienteRepository.SaveChanges();
                }
            }

            //if (inconsistencias != "" && inconsistencias != "1;")
            if (inconsistencias != "" && inconsistencias != "1;" && inconsistencias != "3;")
                return true;

            _aprovacaoComercialUsinaRepository.AdicionarLog(
                new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, 0, "", "ComerciaLegacyService.ValidarContrato", 
                    PayloadHelper.ConvertToJson(queries),
                    PayloadHelper.ConvertToJson(new { inconsistencias })
                    )
                );

            return false;
        }

        public bool ValidarContrato(ContratoVersao contrato, string usuario, out string mensagensRetorno, bool aprovarAutomaticamente = false)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var parametroDesativarObrigatoriedadeAprovacaoComercial = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoComercial)
                aprovarAutomaticamente = true;

            var queries = new List<string>();

            var limiteCreditoPorGrupoEconomico = _parametroRepository.ObterParametroN("web", "LimiteCreditoPorGrupoEconomico") == "1";

            var inconsistencias = "";
            mensagensRetorno = "";

            if (string.IsNullOrEmpty(contrato.IdAprovacaoVendedor))
                inconsistencias += "1;";

            if (string.IsNullOrEmpty(contrato.IdAprovacaoCadastro) && !parametroDesativarObrigatoriedadeAprovacaoComercial)
            {
                inconsistencias += "2;";
                mensagensRetorno += "Será Necessária Aprovação do Cadastro Antes da Aprovação Final!\n";
            }

            if (contrato.AprovaEngenharia == "S" && string.IsNullOrEmpty(contrato.IdAprovacaoEngenharia))
                inconsistencias += "3;";

            if (contrato.CadastroAprovado == "N")
            {
                inconsistencias += "4;";
                mensagensRetorno += "Cadastro Reprovado!\n";
            }

            var obra = _intervenienteRepository
                .ListarFiltradosTracking<ObraVersao>(t => t.NumeroVersao == contrato.NumeroVersao && t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            sql.Clear();
            sql.AppendLine("SELECT status_comercial Comercial, status_cadastro Cadastro, status_financeiro Financeiro, status_engenharia Engenharia");
            sql.AppendLine("FROM con_obras_versao WHERE num_versao = @Versao AND usina = @Usina AND numero = @Numero");

            var obraStatus = cnn.QueryFirstOrDefault(sql.ToString(), new { Versao = obra.NumeroVersao, Usina = obra.UsinaCodigo, Numero = obra.Numero });

            if (obra.StatusComercial != EObraStatusComercial.Aprovado)
                obra.StatusComercial = (EObraStatusComercial)(int)obraStatus.Comercial;

            if (obra.StatusCadastro != EObraStatusCadastro.Aprovado)
                obra.StatusCadastro = (EObraStatusCadastro)(int)obraStatus.Cadastro;

            if (obra.StatusFinanceiro != EObraStatusFinanceiro.Aprovado)
                obra.StatusFinanceiro = (EObraStatusFinanceiro)(int)obraStatus.Financeiro;

            if (obra.StatusEngenharia != EObraStatusEngenharia.Aprovado)
                obra.StatusEngenharia = (EObraStatusEngenharia)(int)obraStatus.Engenharia;

            if (obra == null)
            {
                inconsistencias += "5;";
                mensagensRetorno += "Nenhuma Obra Cadastrada Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.* FROM con_proposta_item_versao AS p");
            sql.Append(" INNER JOIN con_obras_versao AS o");
            sql.Append(" ON p.usina = o.usina");
            sql.Append(" AND p.no_obra=o.numero");
            sql.Append(" AND p.num_versao=o.num_versao");
            sql.Append(" INNER JOIN con_contrato_versao AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" AND c.num_versao=o.num_versao");
            sql.Append(" WHERE (p.obs_aprov='ADMIN' or p.obs_aprov='')");
            sql.Append(" AND p.aprov_verbal='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");
            sql.Append($" AND c.num_versao={contrato.NumeroVersao}");

            var rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Existem Descontos a Serem Aprovados Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.* FROM con_prop_bomba_versao AS p");
            sql.Append(" INNER JOIN con_obras_versao AS o");
            sql.Append(" ON p.usina = o.usina");
            sql.Append(" AND p.no_obra=o.numero");
            sql.Append(" AND p.num_versao=o.num_versao");
            sql.Append(" INNER JOIN con_contrato_versao AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" AND c.num_versao=o.num_versao");
            sql.Append(" WHERE (obs_aprov='ADMIN' or obs_aprov='')");
            sql.Append(" AND aprov_verbal='S'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");
            sql.Append($" AND c.num_versao={contrato.NumeroVersao}");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Existem Descontos a Serem Aprovados Para Este Contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT p.*");
            sql.Append(" FROM con_obras_versao o");
            sql.Append(" INNER JOIN view_obras_pendentes_aprov_versao p ON o.usina=p.usina AND o.numero=p.numero AND o.num_versao=p.versao");
            sql.Append($" WHERE o.usina={contrato.Usina}");
            sql.Append($" AND o.ano_contrato={contrato.Ano}");
            sql.Append($" AND o.no_contrato={contrato.Numero}");
            sql.Append($" AND o.num_versao={contrato.NumeroVersao}");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                inconsistencias += "6;";
                mensagensRetorno += "Exitem pendencias de aprovações comerciais para este contrato!\n";
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) FROM con_obras_tx_versao AS ctx");
            sql.Append(" INNER JOIN con_obras_versao AS o");
            sql.Append(" ON ctx.usina = o.usina");
            sql.Append(" AND ctx.obra=o.numero");
            sql.Append(" AND ctx.num_versao=o.num_versao");
            sql.Append(" INNER JOIN con_contrato_versao AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" AND c.num_versao=o.num_versao");
            sql.Append(" WHERE ctx.aprov_desc='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");
            sql.Append($" AND c.num_versao={contrato.NumeroVersao}");

            var qtdTaxasSemAprovacao = cnn.QueryFirstOrDefault<int>(sql.ToString());

            if (qtdTaxasSemAprovacao > 0)
            {
                inconsistencias += "7;";
                mensagensRetorno += "Existem Alterações em Condições de Contrato(s) a Serem Aprovadas!\n";
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) FROM con_obras_mp_versao mp");
            sql.Append(" INNER JOIN con_obras_versao AS o");
            sql.Append(" ON mp.usina = o.usina");
            sql.Append(" AND mp.obra=o.numero");
            sql.Append(" AND mp.num_versao=o.num_versao");
            sql.Append(" INNER JOIN con_contrato_versao AS c");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.num_contrato=o.no_contrato");
            sql.Append(" AND c.ano_contrato=o.ano_contrato");
            sql.Append(" AND c.num_versao=o.num_versao");
            sql.Append(" WHERE mp.seleciona='N'");
            sql.Append(" AND mp.aprov_desc='N'");
            sql.Append($" AND c.usina={contrato.Usina}");
            sql.Append($" AND c.ano_contrato={contrato.Ano}");
            sql.Append($" AND c.num_contrato={contrato.Numero}");
            sql.Append($" AND c.num_versao={contrato.NumeroVersao}");

            var qtdMensagensSemAprovacao = cnn.QueryFirstOrDefault<int>(sql.ToString());

            if (qtdMensagensSemAprovacao > 0)
            {
                inconsistencias += "8;";
                mensagensRetorno += "Existem Alterações em Mensagens Padrões a Serem Aprovadas!\n";
            }

            inconsistencias += ValidarPagamentosContrato(contrato, true, out string mensagens);
            if (mensagens != "")
                mensagensRetorno += $"{mensagens}\n";

            if (ValidarCodigoObraPrefeitura(obra.UsinaCodigo, "", contrato.Numero, contrato.Ano))
            {
                inconsistencias += "13;";
                mensagensRetorno += "É Necessário Informar o Código Obra Na Prefeitura Deste Contrato!\n";
            }

            var cliente = _intervenienteRepository.ObterPorId(contrato.IntervenienteCodigo);

            if (cliente.GrupoEconomico != null && limiteCreditoPorGrupoEconomico)
            {

                if (cliente.GrupoEconomico.BloqueioMotivoCodigo != 0)
                {
                    inconsistencias += "14;";
                    mensagensRetorno += "Não é Possível Aprovar, Contrato Possui Interveniente em um Grupo Econômico Bloqueado!\n";
                }
            }
            else
            {
                if (cliente.BloqueioMotivoCodigo != 0)
                {
                    inconsistencias += "14;";
                    mensagensRetorno += "Não é Possível Aprovar, Contrato Possui Interveniente Bloqueado!\n";
                }
            }

            // Atualizando a lista de inconsistências do contrato
            sql.Clear();
            sql.Append("UPDATE con_contrato_versao");
            sql.Append($" SET inconsistencias='{inconsistencias}'");

            if ((contrato.IsCadastroAprovado() && obra.StatusCadastro == EObraStatusCadastro.Aprovado) || parametroDesativarObrigatoriedadeAprovacaoComercial)
            {
                if (obra.StatusComercial == EObraStatusComercial.Aguardando)
                    contrato.Status = EContratoStatus.AguardandoAprovacaoComercial;
                else if (obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoConfirmacao)
                    contrato.Status = EContratoStatus.AguardandoConfirmacaoPagamento;
                else if (obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoDadosPagamento)
                    contrato.Status = EContratoStatus.AguardandoDadosPagamento;

                sql.Append($", status={(int)contrato.Status}");
            }

            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append($" AND num_versao={contrato.NumeroVersao}");
            cnn.Execute(sql.ToString());
            queries.Add(sql.ToString());
            cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_versao", sql.ToString());

            // Caso não existam inconsistências e o parâmetro "aprovarAutomaticamente"=True aprova automáticamente o contrato
            if (aprovarAutomaticamente && inconsistencias == ""
                // acrescentadas essas validações para o processo ficar mais consistente
                && ((contrato.IsCadastroAprovado() && obra.StatusCadastro == EObraStatusCadastro.Aprovado) || parametroDesativarObrigatoriedadeAprovacaoComercial)
                && (obra.StatusComercial == EObraStatusComercial.NaoNecessita || obra.StatusComercial == EObraStatusComercial.Aprovado)
                && (obra.StatusEngenharia == EObraStatusEngenharia.NaoNecessita || obra.StatusEngenharia == EObraStatusEngenharia.Aprovado)
                && (obra.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita || obra.StatusFinanceiro == EObraStatusFinanceiro.Aprovado))
            {
                contrato.Status = EContratoStatus.Aprovado;

                sql.Clear();
                sql.AppendLine("SELECT status");
                sql.AppendLine("FROM con_contrato_versao WHERE usina = @Usina AND num_contrato = @Numero and ano_contrato=@Ano and num_versao=@NumeroVersao");

                var statusAnterior = cnn.QueryFirstOrDefault(sql.ToString(), new { contrato.Usina, contrato.Numero, contrato.Ano, contrato.NumeroVersao });

                sql.Clear();
                sql.Append("UPDATE con_contrato_versao");
                sql.Append($" SET id_aprov_dir='{StringHelper.GetIDD(usuario)}'");
                sql.Append(", fechado='S'");
                sql.Append($", status={(int)contrato.Status}");
                sql.Append($" WHERE usina={contrato.Usina}");
                sql.Append($" AND ano_contrato={contrato.Ano}");
                sql.Append($" AND num_contrato={contrato.Numero}");
                sql.Append($" AND num_versao={contrato.NumeroVersao}");
                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_versao", sql.ToString());

                if (statusAnterior.status != (int)EContratoStatus.Aprovado)
                {
                    _webHookApplicationService.AdicionarWebhookContratoAprovadoVersao(contrato);
                }

                if (contrato.Status != EContratoStatus.Aprovado)
                {
                    var statusDe = _intervenienteRepository.ObterPorId<CadastroGeral>((int)contrato.Status);
                    var statusPara = _intervenienteRepository.ObterPorId<CadastroGeral>((int)EContratoStatus.Aprovado);

                    _intervenienteRepository.Adicionar(new ObraLogVersao
                    {
                        NumeroVersao = obra.NumeroVersao,
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        AnoChamada = obra.AnoChamada ?? 0,
                        NumChamada = obra.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Sequencia = 1,
                        Evento = "ALTERAÇÃO STATUS",
                        Complemento = $"DE: {statusDe?.Descricao} PARA: {statusPara?.Descricao}",
                        Usuario = usuario,
                        Observacao = ""
                    });
                    _intervenienteRepository.SaveChanges();
                }
            }

            //if (inconsistencias != "" && inconsistencias != "1;")
            if (inconsistencias != "" && inconsistencias != "1;" && inconsistencias != "3;")
                return true;

            _aprovacaoComercialUsinaRepository.AdicionarLog(
                new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, obra.NumeroVersao, "", "ComerciaLegacyService.ValidarContrato",
                    PayloadHelper.ConvertToJson(queries),
                    PayloadHelper.ConvertToJson(new { inconsistencias })
                    )
                );

            return false;
        }

        public bool ValidarAprovacaoCadastro(Contrato contrato, out string mensagens)
        {
            mensagens = "";

            var clienteCodigo = contrato.IntervenienteCodigo ?? 0;

            if (contrato.Usina == 0 || contrato.Ano == 0 || contrato.Numero == 0 || contrato.IntervenienteCodigo == 0)
            {
                mensagens = "Dados do Contrato Inválidos!";
                return true;
            }

            if (contrato.StatusClicksignDocumento == EStatusClicksignDocumento.Processando)
            {
                mensagens = "Contrato não aprovado devido a Assinatura do ClickSign estar em Processamento!";
                return true;
            }

            var sql = new StringBuilder();

            var cnn = _context.Connection;

            var cliente = _intervenienteRepository.ObterPorId(clienteCodigo);

            var obrigarNomeMae = _parametroRepository.ObterParametroN("Topcon", "NaoObrigarNomeMae").Trim() != "1";

            // VERIFICA MÃE
            if (obrigarNomeMae && (cliente.CpfCnpj ?? "").Length <= 11 && cliente.NomeMae.Trim() == "")
            {
                sql.Clear();
                sql.Append("SELECT o.cond_pgto,p.analise_fraude FROM con_obras AS o");
                sql.Append(" LEFT JOIN ger_cond_pag AS p");
                sql.Append(" ON o.cond_pgto=p.cod");
                sql.Append($" WHERE o.usina={contrato.Usina}");
                sql.Append($" AND o.ano_contrato={contrato.Ano}");
                sql.Append($" AND o.no_contrato={contrato.Numero}");

                var rs2 = cnn.QueryFirstOrDefault(sql.ToString());

                if (rs2 != null && rs2.analise_fraude == "S")
                {
                    mensagens = "Aprovação Não Permitida! Campo 'Nome da Mãe' Não Informado No Cadastro do Cliente.";
                    return true;
                }
            }

            if (cliente.EnderecoCep.Trim() == "0" || cliente.EnderecoCep.Trim() == "")
            {
                mensagens = "Cliente Sem Cep Cadastrado! Será Necessário Cadastrar o Cep Para Aprovar o Cadastro!";
                return true;
            }

            if (cliente.Email.Trim() != "")
            {
                var emails = cliente.Email.Split(';');
                foreach (var email in emails)
                {
                    if (!StringHelper.EmailIsValid(email.Trim()))
                    {
                        mensagens = "Cliente Possui Endereço de Email Inválido!\nSerá Necessário Corrigi-lo Para Aprovar o Cadastro!";
                        return true;
                    }
                }
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) AS nRegistros");
            sql.Append(" FROM con_contrato_pag AS cpag");
            sql.Append($" WHERE cpag.usina={contrato.Usina}");
            sql.Append($" AND cpag.ano_contrato={contrato.Ano}");
            sql.Append($" AND cpag.num_contrato={contrato.Numero}");
            sql.Append($" AND cpag.necessita_aprov<>'S'");

            var quantidadeCondicoesPagamentoNaoAntecipado = cnn.QueryFirstOrDefault<int>(sql.ToString());

            // AprovaCoincidencia 
            if (quantidadeCondicoesPagamentoNaoAntecipado > 0)
            {
                if (contrato.AguardandoAprovacao == "S")
                {
                    mensagens = "Existem Coincidências no Contrato! Solicite Aprovação.";
                    return true;
                }
                else if (contrato.AguardandoAprovacao == "R")
                {
                    mensagens = "Contrato Reprovado Devido Coincidências!";
                    return true;
                }
            }

            if (ValidarCodigoObraPrefeitura(contrato.Usina, "", contrato.Numero, contrato.Ano))
            {
                mensagens = "Código Obra na Prefeitura Deste Contrato Não Foi Informado.\nNão Será Possível Aprová-lo!";
                return true;
            }

            var parametros = _parametroRepository.ObterPorDataBase<ParametroProposta>(DateTime.Now);

            var obra = _intervenienteRepository
                .ListarFiltrados<Obra>(t => t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            if (obra != null && parametros.ObrigaAprovacaoDistanciaUsinaEntrega)
            {
                if (ValidarAprovacaoDistanciaUsinaEntregaCep(obra.UsinaEntregaCodigo, obra.EnderecoCep))
                {
                    mensagens = "Aprovação Não Permitida!, Distância Usina Entrega/CEP Obra Não Aprovada.";
                    return true;
                }
            }

            if (obra != null && obra.AnoChamada > 0 && obra.NumChamada > 0)
            {
                var proposta = _intervenienteRepository.ObterPorId<Proposta>(obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

                if (proposta.Status == EPropostaStatus.Reprovada || proposta.Status == EPropostaStatus.ReprovadaComercialmente)
                {
                    mensagens = "Aprovação não permitida! Proposta Reprovada!";
                    return true;
                }
            }

            if (ValidarPagamentosContrato(contrato, false, out string mensagensPagamentos) != "")
            {
                mensagens = mensagensPagamentos;
                return true;
            }

            return false;
        }

        public bool ValidarAprovacaoCadastro(ContratoVersao contrato, out string mensagens)
        {
            mensagens = "";

            var clienteCodigo = contrato.IntervenienteCodigo ?? 0;

            if (contrato.Usina == 0 || contrato.Ano == 0 || contrato.Numero == 0 || contrato.IntervenienteCodigo == 0)
            {
                mensagens = "Dados do Contrato Inválidos!";
                return true;
            }

            if (contrato.StatusClicksignDocumento == EStatusClicksignDocumento.Processando)
            {
                mensagens = "Contrato não aprovado devido a Assinatura do ClickSign estar em Processamento!";
                return true;
            }

            var sql = new StringBuilder();

            var cnn = _context.Connection;

            var cliente = _intervenienteRepository.ObterPorId(clienteCodigo);

            var obrigarNomeMae = _parametroRepository.ObterParametroN("Topcon", "NaoObrigarNomeMae").Trim() != "1";

            // VERIFICA MÃE
            if (obrigarNomeMae && (cliente.CpfCnpj ?? "").Length <= 11 && cliente.NomeMae.Trim() == "")
            {
                sql.Clear();
                sql.Append("SELECT o.cond_pgto,p.analise_fraude FROM con_obras_versao AS o");
                sql.Append(" LEFT JOIN ger_cond_pag AS p");
                sql.Append(" ON o.cond_pgto=p.cod");
                sql.Append($" WHERE o.usina={contrato.Usina}");
                sql.Append($" AND o.ano_contrato={contrato.Ano}");
                sql.Append($" AND o.no_contrato={contrato.Numero}");
                sql.Append($" AND o.num_versao={contrato.NumeroVersao}");

                var rs2 = cnn.QueryFirstOrDefault(sql.ToString());

                if (rs2 != null && rs2.analise_fraude == "S")
                {
                    mensagens = "Aprovação Não Permitida! Campo 'Nome da Mãe' Não Informado No Cadastro do Cliente.";
                    return true;
                }
            }

            if (cliente.EnderecoCep.Trim() == "0" || cliente.EnderecoCep.Trim() == "")
            {
                mensagens = "Cliente Sem Cep Cadastrado! Será Necessário Cadastrar o Cep Para Aprovar o Cadastro!";
                return true;
            }

            if (cliente.Email.Trim() != "")
            {
                var emails = cliente.Email.Split(';');
                foreach (var email in emails)
                {
                    if (!StringHelper.EmailIsValid(email.Trim()))
                    {
                        mensagens = "Cliente Possui Endereço de Email Inválido!\nSerá Necessário Corrigi-lo Para Aprovar o Cadastro!";
                        return true;
                    }
                }
            }

            sql.Clear();
            sql.Append("SELECT COUNT(*) AS nRegistros");
            sql.Append(" FROM con_contrato_pag_versao AS cpag");
            sql.Append($" WHERE cpag.usina={contrato.Usina}");
            sql.Append($" AND cpag.ano_contrato={contrato.Ano}");
            sql.Append($" AND cpag.num_contrato={contrato.Numero}");
            sql.Append($" AND cpag.num_versao={contrato.NumeroVersao}");
            sql.Append($" AND cpag.necessita_aprov<>'S'");

            var quantidadeCondicoesPagamentoNaoAntecipado = cnn.QueryFirstOrDefault<int>(sql.ToString());

            // AprovaCoincidencia 
            if (quantidadeCondicoesPagamentoNaoAntecipado > 0)
            {
                if (contrato.AguardandoAprovacao == "S")
                {
                    mensagens = "Existem Coincidências no Contrato! Solicite Aprovação.";
                    return true;
                }
                else if (contrato.AguardandoAprovacao == "R")
                {
                    mensagens = "Contrato Reprovado Devido Coincidências!";
                    return true;
                }
            }

            if (ValidarCodigoObraPrefeitura(contrato.Usina, "", contrato.Numero, contrato.Ano))
            {
                mensagens = "Código Obra na Prefeitura Deste Contrato Não Foi Informado.\nNão Será Possível Aprová-lo!";
                return true;
            }

            var parametros = _parametroRepository.ObterPorDataBase<ParametroProposta>(DateTime.Now);

            var obra = _intervenienteRepository
                .ListarFiltrados<ObraVersao>(t => t.NumeroVersao == contrato.NumeroVersao && t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            if (obra != null && parametros.ObrigaAprovacaoDistanciaUsinaEntrega)
            {
                if (ValidarAprovacaoDistanciaUsinaEntregaCep(obra.UsinaEntregaCodigo, obra.EnderecoCep))
                {
                    mensagens = "Aprovação Não Permitida!, Distância Usina Entrega/CEP Obra Não Aprovada.";
                    return true;
                }
            }

            if (obra != null && obra.AnoChamada > 0 && obra.NumChamada > 0)
            {
                var proposta = _intervenienteRepository.ObterPorId<PropostaVersao>(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoChamada, obra.NumChamada);

                if (proposta.Status == EPropostaStatus.Reprovada || proposta.Status == EPropostaStatus.ReprovadaComercialmente)
                {
                    mensagens = "Aprovação não permitida! Proposta Reprovada!";
                    return true;
                }
            }

            if (ValidarPagamentosContrato(contrato, false, out string mensagensPagamentos) != "")
            {
                mensagens = mensagensPagamentos;
                return true;
            }

            return false;
        }

        public string ValidarPagamentosContrato(Contrato contrato, bool verificaAprovacaoFinanceira, out string mensagens)
        {
            // Constante que define o valor máximo de diferença entre o total das condições de pagamento e o total do contrato
            const float DIFERENCA_VALOR_MAXIMA = 9.99f;

            var retorno = "";

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            mensagens = "";

            // Verificando se o total do contrato diverge da soma das condições de pagamento
            sql.Clear();
            sql.Append("SELECT ccon.vlr_total_ctr, cpag.valor");
            sql.Append(" FROM con_contrato AS ccon");
            sql.Append(" INNER JOIN");
            sql.Append(" (SELECT usina, ano_contrato, num_contrato, sum(valor) as valor ");
            sql.Append(" FROM con_contrato_pag");
            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append(" AND ativo='S') AS cpag");
            sql.Append(" ON cpag.usina=ccon.usina");
            sql.Append(" AND cpag.ano_contrato=ccon.ano_contrato");
            sql.Append(" AND cpag.num_contrato=ccon.num_contrato");
            sql.Append($" WHERE ccon.usina={contrato.Usina}");
            sql.Append($" AND ccon.ano_contrato={contrato.Ano}");
            sql.Append($" AND ccon.num_contrato={contrato.Numero}");
            sql.Append(" AND ccon.vlr_total_ctr <> cpag.valor");

            var rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null && Math.Round(rs1.valor, 2) < Math.Round(rs1.vlr_total_ctr, 2))
            {
                if ((Math.Round(rs1.vlr_total_ctr, 2) - Math.Round(rs1.valor, 2)) > Math.Round(DIFERENCA_VALOR_MAXIMA, 2))
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Soma das Condições de Pagamento Inferior ao Valor Total do Contrato.";
                    retorno += "10;";
                }
            }
            else if (rs1 != null && Math.Round(rs1.valor, 2) > Math.Round(rs1.vlr_total_ctr, 2))
            {
                var validaTotalCondPgto = _parametroRepository.ObterParametroN("Topcon", "NValidaTotalCondPgto").Trim() != "1";

                if (validaTotalCondPgto)
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Soma das Condições de Pagamento Superior ao Valor Total do Contrato.";
                    retorno += "10;";
                }
            }

            if (verificaAprovacaoFinanceira)
            {
                // Verificando se existem condições de pagamento que precisam de aprovação e não estão aprovadas
                sql.Clear();
                sql.Append("SELECT * FROM con_contrato_pag");
                sql.Append($" WHERE usina={contrato.Usina}");
                sql.Append($" AND ano_contrato={contrato.Ano}");
                sql.Append($" AND num_contrato={contrato.Numero}");
                sql.Append(" AND (necessita_aprov='S' OR necessita_aprov='')");
                sql.Append(" AND id_aprovacao=''");
                sql.Append(" AND ativo='S'");

                rs1 = cnn.QueryFirstOrDefault(sql.ToString());

                if (rs1 != null)
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Existem Condições de Pagamento Sem Aprovação. Será Necessário Aprová-la(s) Antes de Aprovar o Contrato";
                    retorno += "11;";
                }
            }

            // Verificando se existem totais de condições de pagamento CC/CD/DN/DP divergentes dos totais informados no contrato
            sql.Clear();
            sql.Append("SELECT cpag.valor, ccond.valor, cpag.forma");
            sql.Append(" FROM con_contrato_pag AS cpag");
            sql.Append(" INNER JOIN (");
            sql.Append(" SELECT usina, ano_contrato , num_contrato, seq_pgto, sum(valor) as valor");
            sql.Append(" FROM con_contrato_ccredit");
            sql.Append($" WHERE Usina ={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append(" GROUP BY seq_pgto");
            sql.Append(" UNION");
            sql.Append(" SELECT usina, ano_contrato, num_contrato, seq_pgto, sum(valor_dep) AS valor");
            sql.Append(" FROM con_contrato_dep");
            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append(" GROUP BY seq_pgto");
            sql.Append(" UNION");
            sql.Append(" SELECT usina, ano_contrato , num_contrato, seq_pgto, sum(valor) as valor");
            sql.Append(" FROM con_contrato_dinheir");
            sql.Append($" WHERE Usina ={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append(" GROUP BY seq_pgto) AS ccond");
            sql.Append(" ON ccond.usina= cpag.usina");
            sql.Append(" AND ccond.ano_contrato= cpag.ano_contrato");
            sql.Append(" AND ccond.num_contrato= cpag.num_contrato");
            sql.Append(" AND ccond.seq_pgto= cpag.seq");
            sql.Append($" WHERE cpag.Usina ={contrato.Usina}");
            sql.Append($" AND cpag.ano_contrato={contrato.Ano}");
            sql.Append($" AND cpag.num_contrato={contrato.Numero}");
            sql.Append(" AND cpag.forma in ('CC','CD','DN','DP')");
            sql.Append(" AND ccond.valor <> cpag.valor;");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                mensagens += "Existem Detalhamentos de Condições de Pagamento Que Não Conferem Com o Valor Total da Condição. Favor alterá-las.";
                retorno += "12;";
            }

            return retorno;
        }

        public string ValidarPagamentosContrato(ContratoVersao contrato, bool verificaAprovacaoFinanceira, out string mensagens)
        {
            // Constante que define o valor máximo de diferença entre o total das condições de pagamento e o total do contrato
            const float DIFERENCA_VALOR_MAXIMA = 9.99f;

            var retorno = "";

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            mensagens = "";

            // Verificando se o total do contrato diverge da soma das condições de pagamento
            sql.Clear();
            sql.Append("SELECT ccon.vlr_total_ctr, cpag.valor");
            sql.Append(" FROM con_contrato_versao AS ccon");
            sql.Append(" INNER JOIN");
            sql.Append(" (SELECT num_versao, usina, ano_contrato, num_contrato, sum(valor) as valor ");
            sql.Append(" FROM con_contrato_pag_versao");
            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append($" AND num_versao={contrato.NumeroVersao}");
            sql.Append(" AND ativo='S') AS cpag");
            sql.Append(" ON cpag.usina=ccon.usina");
            sql.Append(" AND cpag.ano_contrato=ccon.ano_contrato");
            sql.Append(" AND cpag.num_contrato=ccon.num_contrato");
            sql.Append(" AND cpag.num_versao=ccon.num_versao");
            sql.Append($" WHERE ccon.usina={contrato.Usina}");
            sql.Append($" AND ccon.ano_contrato={contrato.Ano}");
            sql.Append($" AND ccon.num_contrato={contrato.Numero}");
            sql.Append($" AND ccon.num_versao={contrato.NumeroVersao}");
            sql.Append(" AND ccon.vlr_total_ctr <> cpag.valor");

            var rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null && Math.Round(rs1.valor, 2) < Math.Round(rs1.vlr_total_ctr, 2))
            {
                if ((Math.Round(rs1.vlr_total_ctr, 2) - Math.Round(rs1.valor, 2)) > Math.Round(DIFERENCA_VALOR_MAXIMA, 2))
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Soma das Condições de Pagamento Inferior ao Valor Total do Contrato.";
                    retorno += "10;";
                }
            }
            else if (rs1 != null && Math.Round(rs1.valor, 2) > Math.Round(rs1.vlr_total_ctr, 2))
            {
                var validaTotalCondPgto = _parametroRepository.ObterParametroN("Topcon", "NValidaTotalCondPgto").Trim() != "1";

                if (validaTotalCondPgto)
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Soma das Condições de Pagamento Superior ao Valor Total do Contrato.";
                    retorno += "10;";
                }
            }

            if (verificaAprovacaoFinanceira)
            {
                // Verificando se existem condições de pagamento que precisam de aprovação e não estão aprovadas
                sql.Clear();
                sql.Append("SELECT * FROM con_contrato_pag_versao");
                sql.Append($" WHERE usina={contrato.Usina}");
                sql.Append($" AND ano_contrato={contrato.Ano}");
                sql.Append($" AND num_contrato={contrato.Numero}");
                sql.Append($" AND num_versao={contrato.NumeroVersao}");
                sql.Append(" AND (necessita_aprov='S' OR necessita_aprov='')");
                sql.Append(" AND id_aprovacao=''");
                sql.Append(" AND ativo='S'");

                rs1 = cnn.QueryFirstOrDefault(sql.ToString());

                if (rs1 != null)
                {
                    if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                    mensagens += "Existem Condições de Pagamento Sem Aprovação. Será Necessário Aprová-la(s) Antes de Aprovar o Contrato";
                    retorno += "11;";
                }
            }

            // Verificando se existem totais de condições de pagamento CC/CD/DN/DP divergentes dos totais informados no contrato
            sql.Clear();
            sql.Append("SELECT cpag.valor, ccond.valor, cpag.forma");
            sql.Append(" FROM con_contrato_pag_versao AS cpag");
            sql.Append(" INNER JOIN (");
            sql.Append(" SELECT num_versao, usina, ano_contrato , num_contrato, seq_pgto, sum(valor) as valor");
            sql.Append(" FROM con_contrato_ccredit_versao");
            sql.Append($" WHERE Usina ={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append($" AND num_versao={contrato.NumeroVersao}");
            sql.Append(" GROUP BY seq_pgto");
            sql.Append(" UNION");
            sql.Append(" SELECT num_versao, usina, ano_contrato, num_contrato, seq_pgto, sum(valor_dep) AS valor");
            sql.Append(" FROM con_contrato_dep_versao");
            sql.Append($" WHERE usina={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append($" AND num_versao={contrato.NumeroVersao}");
            sql.Append(" GROUP BY seq_pgto");
            sql.Append(" UNION");
            sql.Append(" SELECT num_versao, usina, ano_contrato , num_contrato, seq_pgto, sum(valor) as valor");
            sql.Append(" FROM con_contrato_dinheir_versao");
            sql.Append($" WHERE Usina ={contrato.Usina}");
            sql.Append($" AND ano_contrato={contrato.Ano}");
            sql.Append($" AND num_contrato={contrato.Numero}");
            sql.Append($" AND num_versao={contrato.NumeroVersao}");
            sql.Append(" GROUP BY seq_pgto) AS ccond");
            sql.Append(" ON ccond.usina= cpag.usina");
            sql.Append(" AND ccond.ano_contrato= cpag.ano_contrato");
            sql.Append(" AND ccond.num_contrato= cpag.num_contrato");
            sql.Append(" AND ccond.seq_pgto= cpag.seq");
            sql.Append(" AND ccond.num_versao= cpag.num_versao");
            sql.Append($" WHERE cpag.Usina ={contrato.Usina}");
            sql.Append($" AND cpag.ano_contrato={contrato.Ano}");
            sql.Append($" AND cpag.num_contrato={contrato.Numero}");
            sql.Append($" AND cpag.num_versao={contrato.NumeroVersao}");
            sql.Append(" AND cpag.forma in ('CC','CD','DN','DP')");
            sql.Append(" AND ccond.valor <> cpag.valor;");

            rs1 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs1 != null)
            {
                if (!string.IsNullOrEmpty(mensagens)) mensagens += "\n";
                mensagens += "Existem Detalhamentos de Condições de Pagamento Que Não Conferem Com o Valor Total da Condição. Favor alterá-las.";
                retorno += "12;";
            }

            return retorno;
        }

        public bool ValidarAprovacaoDistanciaUsinaEntregaCep(int usinaEntrega, string cep)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("SELECT id_aprov");
            sql.Append(" FROM con_dist_usina_cep");
            sql.Append($" WHERE usina_entrega={usinaEntrega}");
            sql.Append($" AND cep='{cep.Trim()}'");

            var idAprovacao = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (string.IsNullOrEmpty(idAprovacao))
                return true;

            return false;
        }

        public bool ValidarCodigoObraPrefeitura(int usina, string codigoObraPrefeitura, int contratoNumero = 0, int contratoAno = 0)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (contratoNumero > 0 && contratoAno > 0)
            {
                sql.Clear();
                sql.Append("SELECT u.dig_cod_obra, c.no_obra");
                sql.Append(" FROM con_contrato as c");
                sql.Append(" INNER JOIN con_usina u ON c.usina = u.cod");
                sql.Append($" WHERE cod={usina}");
                sql.Append($" AND num_contrato={contratoNumero}");
                sql.Append($" AND ano_contrato={contratoAno}");

                var result = cnn.QueryFirstOrDefault(sql.ToString());
                return (result != null && result.dig_cod_obra == "S" && (result.no_obra == "0" || result.no_obra == ""));
            }
            else
            {
                sql.Clear();
                sql.Append("SELECT dig_cod_obra");
                sql.Append(" FROM con_usina");
                sql.Append($" WHERE cod={usina}");

                var result = cnn.QueryFirstOrDefault(sql.ToString());
                return (result != null && result.dig_cod_obra == "S" && (codigoObraPrefeitura == "0" || codigoObraPrefeitura == ""));
            }
        }

        public bool VerificarFraude(int intervenienteCodigo, string enderecoLogradouro, int enderecoNumero,
            string enderecoCobrancaLogradouro, int enderecoCobrancaNumero, string enderecoFaturamentoLogradouro,
            int enderecoFaturamentoNumero, string enderecoObraLogradouro, int enderecoObraNumero, int usina,
            int obraNumero, out string aguardandoAprovacao, out string mensagemRetorno)
        {
            string Mensagem = "";

            var cnn = _context.Connection;

            var sql = new StringBuilder();

            aguardandoAprovacao = "";

            mensagemRetorno = "";

            // buscando os dados do interveniente
            var intervenienteDoContrato = _intervenienteRepository.ObterPorId(intervenienteCodigo);

            // verificação dos sócios
            sql.Append($"SELECT interv, nome FROM con_socios WHERE interv <> {intervenienteCodigo} ");
            sql.Append(" AND cpf_cnpj IN (select cpf_cnpj FROM con_socios ");
            sql.Append($" where interv= {intervenienteCodigo} )");

            var result = cnn.Query(sql.ToString());

            foreach (var socio in result)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {socio.interv}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");

                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());

                if (rs1 > 0)
                {
                    Mensagem += $"Sócio: {socio.nome} cadastrado para a empresa: {socio.interv} \n ";
                }

            }

            sql.Clear();
            sql.Append("SELECT cod, razao FROM ger_interv WHERE CNPJ_CPF IN");
            sql.Append("(SELECT cpf_cnpj FROM con_socios ");
            sql.Append($" WHERE interv= {intervenienteCodigo} )");
            result = cnn.Query(sql.ToString());

            foreach (var socio in result)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {socio.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)
                {
                    Mensagem += $"Sócio {socio.razao} cadastrado como cliente no: {socio.cod} \n ";
                }

            }

            // validando se existe interveniente que está cadastado em um endereço próximo
            sql.Clear();
            sql.Append($"select cod, nome from ger_interv where end='{enderecoLogradouro}'");
            sql.Append($" and Num between  {enderecoNumero - 100} and {enderecoNumero + 100}");
            sql.Append($" and cod<> {intervenienteCodigo}");
            var intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.cod} - {interveniente.nome} cadastrado com o mesmo endereço.\n";
            }

            // validando se existe interveniente que está cadastado em um endereço de cobrança próximo
            sql.Clear();
            sql.Append($"select cod, nome from ger_interv where end='{enderecoCobrancaLogradouro}'");
            sql.Append($" and Num between  {enderecoCobrancaNumero - 100} and {enderecoCobrancaNumero + 100}");
            sql.Append($" and cod<> {intervenienteCodigo}");
            sql.Append(" and end<>''");

            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.cod} - {interveniente.nome} cadastrado com o mesmo endereço de cobrança.\n";
            }

            // validando endereço de faturamento 
            sql.Clear();
            sql.Append($"select cod, nome from ger_interv where end='{enderecoFaturamentoLogradouro}'");
            sql.Append($" and Num between  {enderecoFaturamentoNumero - 100} and {enderecoFaturamentoNumero + 100}");
            sql.Append($" and cod<> {intervenienteCodigo}");
            sql.Append(" and end<>''");

            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.cod} - {interveniente.nome} cadastrado com o mesmo endereço de faturamento.\n";
            }

            // valida se existe outro interveniente com endereço próximo ao da obra
            sql.Clear();
            sql.Append($"select cod, nome from ger_interv where end='{enderecoObraLogradouro}'");
            sql.Append($" and Num between  {enderecoObraNumero - 100} and {enderecoObraNumero + 100}");
            sql.Append($" and cod<> {intervenienteCodigo}");
            sql.Append(" and end<>''");

            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.cod} - {interveniente.nome} cadastrado com o mesmo endereço de obra.\n";
            }

            // Verificação de endereços da obra

            // Endereço residencial do interveniente
            sql.Clear();
            sql.Append($"SELECT 'Residencial' tipo, numero, IF(no_contrato <> 0, CONCAT('Contrato ', no_contrato, '/', ano_contrato), CONCAT('Proposta ', no_chamada, '/', ano_chamada)) label from con_obras where obra_end='{enderecoLogradouro}'");
            sql.Append($" AND obra_num BETWEEN { enderecoNumero - 100} AND { enderecoNumero + 100}");
            sql.Append($" AND ( numero <> {obraNumero}  OR usina <> {usina} )");
            sql.Append($" AND obra_end<>''");
            
            // endereço de cobrança
            sql.Append($" UNION ");
            sql.Append($"SELECT 'Cobrança' tipo, numero, IF(no_contrato <> 0, CONCAT('Contrato ', no_contrato, '/', ano_contrato), CONCAT('Proposta ', no_chamada, '/', ano_chamada)) label from con_obras where obra_end='{enderecoCobrancaLogradouro}'");
            sql.Append($" AND obra_num BETWEEN { enderecoCobrancaNumero - 100} AND { enderecoCobrancaNumero + 100}");
            sql.Append($" AND ( numero <> {obraNumero}  OR usina <> {usina} )");
            sql.Append($" AND obra_end<>''");
            
            // endereço de faturamento
            sql.Append($" UNION ");
            sql.Append($"SELECT 'Faturamento' tipo, numero, IF(no_contrato <> 0, CONCAT('Contrato ', no_contrato, '/', ano_contrato), CONCAT('Proposta ', no_chamada, '/', ano_chamada)) label from con_obras where obra_end='{enderecoFaturamentoLogradouro}'");
            sql.Append($" AND obra_num BETWEEN { enderecoFaturamentoNumero - 100} AND { enderecoFaturamentoNumero + 100}");
            sql.Append($" AND ( numero <> {obraNumero}  OR usina <> {usina} )");
            sql.Append($" AND obra_end<>''");

            // endereço de obra
            sql.Append($" UNION ");
            sql.Append($"SELECT 'Obra' tipo, numero, IF(no_contrato <> 0, CONCAT('Contrato ', no_contrato, '/', ano_contrato), CONCAT('Proposta ', no_chamada, '/', ano_chamada)) label from con_obras where obra_end='{enderecoObraLogradouro}'");
            sql.Append($" AND obra_num BETWEEN { enderecoObraNumero - 100} AND { enderecoObraNumero + 100}");
            sql.Append($" AND ( numero <> {obraNumero}  OR usina <> {usina} )");
            sql.Append($" AND obra_end<>''");

            sql.Append($" ORDER BY label");

            var label = "";

            var obras = cnn.Query(sql.ToString());
            foreach (var obra in obras)
            {
                if (obra.label != label)
                {
                    label = obra.label;

                    Mensagem += $"{obra.label} \n";
                }
                    
                if (obra.tipo == "Residencial")
                    Mensagem += $"Obra {obra.numero} cadastrada no endereço do interveniente \n";
                else if (obra.tipo == "Cobrança")
                    Mensagem += $"Obra {obra.numero} cadastrada no endereço do cobrança \n";
                else if (obra.tipo == "Faturamento")
                    Mensagem += $"Obra {obra.numero} cadastrada no endereço do faturamento \n";
                else if (obra.tipo == "Obra")
                    Mensagem += $"Obra {obra.numero} cadastrada no endereço \n";
            }

            // verificação de endereços de cobraça/faturamento
            // endereço residencial
            // dar join em interv para trazer o nome
            sql.Clear();
            sql.Append($"SELECT local.interv interv, interv.razao razao");
            sql.Append(" FROM ger_local local");
            sql.Append(" INNER JOIN ger_interv interv");
            sql.Append(" ON local.interv = interv.cod");
            sql.Append($" WHERE local.end='{ enderecoLogradouro }'");
            sql.Append($" AND local.Num between {enderecoNumero - 100 }  and { enderecoNumero + 100}");
            sql.Append($" AND local.interv<> {intervenienteCodigo}");
            sql.Append(" AND local.end <> ''");
            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.interv}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.interv} - {interveniente.razao} cadastrado no endereço de cobrança faturamento.\n";
            }

            // endereço de cobrança
            sql.Clear();
            sql.Append($"SELECT local.interv interv, interv.razao razao");
            sql.Append(" FROM ger_local local");
            sql.Append(" INNER JOIN ger_interv interv");
            sql.Append(" ON local.interv = interv.cod");
            sql.Append($" WHERE local.end='{ enderecoCobrancaLogradouro }'");
            sql.Append($" AND local.Num between {enderecoCobrancaNumero - 100 }  and { enderecoCobrancaNumero + 100}");
            sql.Append($" AND local.interv<> {intervenienteCodigo}");
            sql.Append(" AND local.end <> ''");
            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.interv}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.interv} - {interveniente.razao} possui mesmo endereço de cobrança/faturamento.\n";
            }


            // endereço de faturamento
            sql.Clear();
            sql.Append($"SELECT local.interv interv, interv.razao razao");
            sql.Append(" FROM ger_local local");
            sql.Append(" INNER JOIN ger_interv interv");
            sql.Append(" ON local.interv = interv.cod");
            sql.Append($" WHERE local.end='{ enderecoFaturamentoLogradouro }'");
            sql.Append($" AND local.Num between {enderecoFaturamentoNumero - 100 }  and { enderecoFaturamentoNumero + 100}");
            sql.Append($" AND local.interv<> {intervenienteCodigo}");
            sql.Append(" AND local.end <> ''");
            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.interv}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.interv} - {interveniente.razao} possui mesmo endereço de cobrança/faturamento.\n";
            }

            // endereço da obra
            sql.Clear();
            sql.Append($"SELECT local.interv interv, interv.razao razao");
            sql.Append(" FROM ger_local local");
            sql.Append(" INNER JOIN ger_interv interv");
            sql.Append(" ON local.interv = interv.cod");
            sql.Append($" WHERE local.end='{ enderecoObraLogradouro }'");
            sql.Append($" AND local.Num between {enderecoObraNumero - 100 }  and { enderecoObraNumero + 100}");
            sql.Append($" AND local.interv<> {intervenienteCodigo}");
            sql.Append(" AND local.end <> ''");
            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.interv}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.interv} - {interveniente.razao} com endereço de cobrança/faturamento igual ao desta obra .\n";
            }

            // Verificação de telefones de clientes
            sql.Clear();
            sql.Append("SELECT cod, nome FROM ger_interv WHERE tel");
            sql.Append($" IN(SELECT Tel from ger_interv WHERE cod = {intervenienteCodigo} )");
            sql.Append($" AND cod <> { intervenienteCodigo}");
            sql.Append(" AND tel <> 0");
            intervenientes = cnn.Query(sql.ToString());

            foreach (var interveniente in intervenientes)
            {
                sql.Clear();
                sql.Append($"select count(*) from fin_car where cli = {interveniente.cod}");
                sql.Append(" and (isnull(liq_dt) or DATEDIFF(liq_dt,dt_vcto)>30)");
                var rs1 = cnn.QueryFirstOrDefault<int>(sql.ToString());
                if (rs1 > 0)

                    Mensagem += $"Interveniente {interveniente.cod} - {interveniente.nome} com mesmo telefone .\n";
            }

            if (!Mensagem.Equals(""))
            {
                aguardandoAprovacao = "S";
                mensagemRetorno = Mensagem;
                return true;
            }

            return false;
        }

        private int ObterStatusContrato(int usina, int contratoAno, int contratoNumero)
        {
            var cnn = _context.Connection;
            var sql = new StringBuilder();

            sql.Clear();
            sql.Append("SELECT IFNULL(status,0) AS statusContrato from con_contrato");
            sql.Append(" WHERE usina=" + usina);
            sql.Append(" AND ano_contrato=" + contratoAno);
            sql.Append(" AND num_contrato=" + contratoNumero);

            return cnn.QueryFirstOrDefault<int>(sql.ToString());
        }

        private int ObterStatusContrato(int numVersao, int usina, int contratoAno, int contratoNumero)
        {
            var cnn = _context.Connection;
            var sql = new StringBuilder();

            sql.Clear();
            sql.Append("SELECT IFNULL(status,0) AS statusContrato from con_contrato_versao");
            sql.Append(" WHERE usina=" + usina);
            sql.Append(" AND ano_contrato=" + contratoAno);
            sql.Append(" AND num_contrato=" + contratoNumero);
            sql.Append(" AND num_versao=" + numVersao);

            return cnn.QueryFirstOrDefault<int>(sql.ToString());
        }

        private bool AtualizarProposta(DadosAprovacaoProposta dadosAprovacao, string usuario)
        {
            var sql = new StringBuilder();
            var queries = new List<string>();
            var cnn = _context.Connection;
            var parametroDesativarObrigatoriedadeAprovacaoComercial = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            EPropostaStatus propostaStatus;

            if (dadosAprovacao.NumVersao == 0)
            {
                var propReprovada = TemItensReprovados(dadosAprovacao.Usina, dadosAprovacao.ObraNumero);
                var propTemContrato = PropostaTemContrato(dadosAprovacao.Usina, dadosAprovacao.NumChamada, dadosAprovacao.AnoChamada, dadosAprovacao.ObraNumero);

                propostaStatus = (dadosAprovacao.AprovacaoProposta == "X" || propReprovada
                    ? EPropostaStatus.ReprovadaComercialmente
                    : EPropostaStatus.Andamento);

                if (propostaStatus == EPropostaStatus.Andamento && propTemContrato)
                {
                    propostaStatus = EPropostaStatus.ContratoGerado;
                }

                sql.Clear();
                sql.Append("UPDATE con_chtel");
                sql.Append(" SET status=" + (propostaStatus == EPropostaStatus.Andamento
                    ? $"IF(status_anterior=0 OR status_anterior={(int)EPropostaStatus.ReprovadaComercialmente},{(int)EPropostaStatus.Andamento},status_anterior)"
                    : $"{(int)propostaStatus}"));
                sql.Append(", status_anterior=0");
                sql.Append(" WHERE usina=" + dadosAprovacao.Usina);
                sql.Append(" AND ano_chamada=" + dadosAprovacao.AnoChamada);
                sql.Append(" AND num_chamada=" + dadosAprovacao.NumChamada);

                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_chtel", sql.ToString());

                for (int i = 0; i < dadosAprovacao.LogsAprovacaoProposta.Count; i++)
                {
                    if (dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta != "N" && dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta != "")
                    {
                        propostaStatus = (dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta == "X" || propReprovada ? EPropostaStatus.ReprovadaComercialmente : EPropostaStatus.Andamento);

                        var evento = dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta == "X" ? "REPROVADA CML" : "ANÁLISE COMERCIAL";
                        InserirObraLog(dadosAprovacao.Usina, dadosAprovacao.ObraNumero, usuario, evento, dadosAprovacao.LogsAprovacaoProposta[i].Complemento, dadosAprovacao.LogsAprovacaoProposta[i].ObservacaoProposta ?? "");
                    }
                }
            }
            else
            {
                var propReprovada = TemItensReprovados(dadosAprovacao.NumVersao, dadosAprovacao.Usina, dadosAprovacao.ObraNumero);
                var propTemContrato = PropostaTemContrato(dadosAprovacao.NumVersao, dadosAprovacao.Usina, dadosAprovacao.NumChamada, dadosAprovacao.AnoChamada, dadosAprovacao.ObraNumero);

                propostaStatus = (dadosAprovacao.AprovacaoProposta == "X" || propReprovada
                    ? EPropostaStatus.ReprovadaComercialmente
                    : EPropostaStatus.Andamento);

                if (propostaStatus == EPropostaStatus.Andamento && propTemContrato)
                {
                    propostaStatus = EPropostaStatus.ContratoGerado;
                }

                sql.Clear();
                sql.Append("UPDATE con_chtel_versao");
                sql.Append(" SET status=" + (propostaStatus == EPropostaStatus.Andamento
                    ? $"IF(status_anterior=0 OR status_anterior={(int)EPropostaStatus.ReprovadaComercialmente},{(int)EPropostaStatus.Andamento},status_anterior)"
                    : $"{(int)propostaStatus}"));
                sql.Append(", status_anterior=0");
                sql.Append(" WHERE usina=" + dadosAprovacao.Usina);
                sql.Append(" AND ano_chamada=" + dadosAprovacao.AnoChamada);
                sql.Append(" AND num_chamada=" + dadosAprovacao.NumChamada);
                sql.Append(" AND num_versao=" + dadosAprovacao.NumVersao);

                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_chtel_versao", sql.ToString());

                for (int i = 0; i < dadosAprovacao.LogsAprovacaoProposta.Count; i++)
                {
                    if (dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta != "N" && dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta != "")
                    {
                        propostaStatus = (dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta == "X" || propReprovada ? EPropostaStatus.ReprovadaComercialmente : EPropostaStatus.Andamento);

                        var evento = dadosAprovacao.LogsAprovacaoProposta[i].LogAprovacaoProposta == "X" ? "REPROVADA CML" : "ANÁLISE COMERCIAL";
                        InserirObraLog(dadosAprovacao.NumVersao, dadosAprovacao.Usina, dadosAprovacao.ObraNumero, usuario, evento, dadosAprovacao.LogsAprovacaoProposta[i].Complemento, dadosAprovacao.LogsAprovacaoProposta[i].ObservacaoProposta);
                    }
                }
            }

            _aprovacaoComercialUsinaRepository.AdicionarLog(
                new AprovacaoComercialLog(dadosAprovacao.Usina, dadosAprovacao.ObraNumero, dadosAprovacao.NumVersao, "", "ComerciaLegacyService.AtualizarProposta",
                    PayloadHelper.ConvertToJson(queries),
                    PayloadHelper.ConvertToJson(new { dadosAprovacao })
                    )
                );

            return false;
        }

        private bool TemItensReprovados(int Usina, int Obra)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("SELECT status_aprov FROM con_proposta_item");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND no_obra=" + Obra);
            sql.Append(" AND status_aprov='X'");

            var statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT status_aprov FROM con_prop_bomba");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND no_obra=" + Obra);
            sql.Append(" AND status_aprov='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT reaj_sts_aprov FROM con_obras");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND numero=" + Obra);
            sql.Append(" AND reaj_sts_aprov='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT aprov_desc FROM con_obras_tx");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND obra=" + Obra);
            sql.Append(" AND aprov_desc='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT aprov_desc FROM con_obras_mp");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND obra=" + Obra);
            sql.Append(" AND aprov_desc='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            return false;
        }

        private bool TemItensReprovados(int NumVersao, int Usina, int Obra)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("SELECT status_aprov FROM con_proposta_item_versao");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND no_obra=" + Obra);
            sql.Append(" AND num_versao=" + NumVersao);
            sql.Append(" AND status_aprov='X'");

            var statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT status_aprov FROM con_prop_bomba_versao");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND no_obra=" + Obra);
            sql.Append(" AND num_versao=" + NumVersao);
            sql.Append(" AND status_aprov='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT reaj_sts_aprov FROM con_obras_versao");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND numero=" + Obra);
            sql.Append(" AND num_versao=" + NumVersao);
            sql.Append(" AND reaj_sts_aprov='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT aprov_desc FROM con_obras_tx_versao");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND obra=" + Obra);
            sql.Append(" AND num_versao=" + NumVersao);
            sql.Append(" AND aprov_desc='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            sql.Clear();
            sql.Append("SELECT aprov_desc FROM con_obras_mp_versao");
            sql.Append(" WHERE usina=" + Usina);
            sql.Append(" AND obra=" + Obra);
            sql.Append(" AND num_versao=" + NumVersao);
            sql.Append(" AND aprov_desc='X'");

            statusAprov = cnn.QueryFirstOrDefault<string>(sql.ToString());

            if (statusAprov != null)
            {
                return true;
            }

            return false;
        }

        private bool PropostaTemContrato(int usinaProposta, int numProposta, int anoProposta, int NumObra)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("SELECT c.num_chamada FROM con_chtel AS c");
            sql.Append(" LEFT JOIN con_obras AS o");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.no_obra=o.numero");
            sql.Append(" AND c.num_chamada=o.no_chamada");
            sql.Append(" AND c.ano_chamada=o.ano_chamada");
            sql.Append(" WHERE c.usina=" + usinaProposta);
            sql.Append(" AND c.num_chamada=" + numProposta);
            sql.Append(" AND c.ano_chamada=" + anoProposta);
            sql.Append(" AND c.no_obra=" + NumObra);
            sql.Append(" AND o.no_contrato>0");

            var result = cnn.QueryFirstOrDefault(sql.ToString());

            return (result != null);

        }

        private bool PropostaTemContrato(int numVersao, int usinaProposta, int numProposta, int anoProposta, int NumObra)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("SELECT c.num_chamada FROM con_chtel_versao AS c");
            sql.Append(" LEFT JOIN con_obras_versao AS o");
            sql.Append(" ON c.usina=o.usina");
            sql.Append(" AND c.no_obra=o.numero");
            sql.Append(" AND c.num_chamada=o.no_chamada");
            sql.Append(" AND c.num_chamada=o.no_chamada");
            sql.Append(" AND c.num_versao=o.num_versao");
            sql.Append(" WHERE c.usina=" + usinaProposta);
            sql.Append(" AND c.num_chamada=" + numProposta);
            sql.Append(" AND c.ano_chamada=" + anoProposta);
            sql.Append(" AND c.no_obra=" + NumObra);
            sql.Append(" AND c.num_versao=" + numVersao);
            sql.Append(" AND o.no_contrato>0");

            var result = cnn.QueryFirstOrDefault(sql.ToString());

            return (result != null);

        }

        public void FinalizarAprovacoesComerciais(string usuario, string chaveObra, List<ObraLogDado> logs)
        {
            var obraChave = chaveObra.Split('-');
            int usina = short.Parse(obraChave[0]);
            int obraNumero = int.Parse(obraChave[1]);

            var queries = new List<string>();

            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var obra = _obraRepository.ObterPorId(usina, obraNumero);

            if (obra == null) throw new Exception($"Obra não encontrada: {usina}-{obraNumero}");

            var anoContrato = obra.AnoContrato ?? 0;
            var numContrato = obra.NumContrato ?? 0;
            var anoChamada = obra.AnoChamada ?? 0;
            var numChamada = obra.NumChamada ?? 0;

            if (anoContrato != 0 && numContrato != 0)
            {
                sql.Clear();
                sql.Append("UPDATE con_contrato");
                sql.Append(" SET vlr_concreto=(");
                sql.Append("   SELECT SUM(qtde_m3*preco_unit_prop) VlrTotal");
                sql.Append("   FROM con_proposta_item");
                sql.Append("   WHERE usina=" + usina);
                sql.Append("   AND no_obra=" + obraNumero);
                sql.Append(" )");
                sql.Append(" ,vlr_total_ctr=vlr_concreto+vlr_bomba+vlr_extras");
                sql.Append(" WHERE usina=" + usina);
                sql.Append(" AND ano_contrato=" + anoContrato);
                sql.Append(" AND num_contrato=" + numContrato);

                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());

                ////'********************************************************************************************
                ////'Verifica Status atual do contrato
                ////'********************************************************************************************
                var contratoStatus = ObterStatusContrato(usina, anoContrato, numContrato);

                if (contratoStatus == (int)EContratoStatus.AguardandoAprovacaoComercial || contratoStatus == (int)EContratoStatus.Aprovado)
                {
                    Contrato contrato = _contratoRepository.ObterPorId(usina, anoContrato, numContrato);

                    //Verifica se pode aprovar cadastro
                    if (!ValidarAprovacaoCadastro(contrato, out string mensagem))
                    {
                        //Atualiza con_contrato
                        sql.Clear();
                        sql.Append("UPDATE con_contrato");
                        sql.Append(" SET id_aprov_cad='" + StringHelper.GetIDD(usuario) + "'");
                        sql.Append(" ,cad_aprovado='S'");
                        sql.Append(" WHERE usina=" + usina);
                        sql.Append(" AND ano_contrato=" + anoContrato);
                        sql.Append(" AND num_contrato=" + numContrato);

                        cnn.Execute(sql.ToString());
                        queries.Add(sql.ToString());
                        cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());
                    }
                    else
                    {
                        sql.Clear();
                        sql.Append("UPDATE con_contrato");
                        sql.Append(" SET status=" + (int)EContratoStatus.EmAnalise);
                        sql.Append(" WHERE usina=" + usina);
                        sql.Append(" AND ano_contrato=" + anoContrato);
                        sql.Append(" AND num_contrato=" + numContrato);

                        cnn.Execute(sql.ToString());
                        queries.Add(sql.ToString());
                        cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());

                        var complementoLog = (contratoStatus == (int)EContratoStatus.AguardandoAprovacaoComercial)
                            ? "DE: AG.APROV.COMERCIA PARA: EM ANÁLISE"
                            : "DE: APROVADO PARA: EM ANÁLISE";
                        var evento = ObterEventoConObra(EContratoStatus.AguardandoDadosPagamento);
                        InserirObraLog(usina, obraNumero, usuario, evento, complementoLog, "");
                    }
                }

                sql.Clear();
                sql.Append("SELECT status,status_anterior,id_aprov_cad,cad_aprovado");
                sql.Append(" FROM con_contrato");
                sql.Append(" WHERE usina=" + usina);
                sql.Append(" AND ano_contrato=" + anoContrato);
                sql.Append(" AND num_contrato=" + numContrato);

                var contratoResult = cnn.QueryFirstOrDefault(sql.ToString());
                if (contratoResult != null)
                {
                    if ((contratoResult.status == (int)EContratoStatus.Aprovado || contratoResult.status == (int)EContratoStatus.AguardandoAprovacaoComercial) && contratoResult.id_aprov_cad != "")
                    {
                        //********************************************************************************************
                        //A Função 'ValidarContrato' verifica se o contrato tem alguma pendencia, e realiza
                        //a aprovação do mesmo caso não existam pendencias
                        //********************************************************************************************
                        Contrato contrato = _contratoRepository.ObterPorId(usina, anoContrato, numContrato);
                        ValidarContrato(contrato, usuario, out string mensagem, aprovContratoDirAuto);
                    }
                }

            }

            if (anoChamada != 0 && numChamada != 0)
            {
                DadosLogAprovacaoProposta[] obraLogs = new DadosLogAprovacaoProposta[logs.Count];

                for (int i = 0; i < logs.Count(); i++)
                {
                    obraLogs[i] = new DadosLogAprovacaoProposta
                    {
                        LogAprovacaoProposta = logs[i].Operacao,
                        Complemento = logs[i].Complemento,
                        ObservacaoProposta = logs[i].Observacao
                    };
                }

                var dadosAprovacao = new DadosAprovacaoProposta
                {
                    NumVersao = 0,
                    Usina = usina,
                    AnoChamada = anoChamada,
                    NumChamada = numChamada,
                    ObraNumero = obraNumero,
                    LogsAprovacaoProposta = obraLogs.ToList()
                };

                if (logs.Count > 0)
                {
                    dadosAprovacao.AprovacaoProposta = obraLogs[obraLogs.Length - 1].LogAprovacaoProposta;
                }

                _aprovacaoComercialUsinaRepository.AdicionarLog(
                new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, 0, "", "ComerciaLegacyService.FinalizarAprovacoesComerciais",
                    PayloadHelper.ConvertToJson(queries), ""
                    )
                );

                AtualizarProposta(dadosAprovacao, usuario);
            }
        }

        public void FinalizarAprovacoesComerciaisVersao(string usuario, string chaveObra, List<ObraLogDado> logs)
        {
            var obraChave = chaveObra.Split('-');
            int numVersao = short.Parse(obraChave[0]);
            int usina = short.Parse(obraChave[1]);
            int obraNumero = int.Parse(obraChave[2]);

            var queries = new List<string>();

            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var obra = _obraRepository.ObterPorId(usina, obraNumero);

            if (obra == null) throw new Exception($"Obra não encontrada: {usina}-{obraNumero}");

            var anoContrato = obra.AnoContrato ?? 0;
            var numContrato = obra.NumContrato ?? 0;
            var anoChamada = obra.AnoChamada ?? 0;
            var numChamada = obra.NumChamada ?? 0;

            if (anoContrato != 0 && numContrato != 0)
            {
                sql.Clear();
                sql.Append("UPDATE con_contrato_versao");
                sql.Append(" SET vlr_concreto=(");
                sql.Append("   SELECT SUM(qtde_m3*preco_unit_prop) VlrTotal");
                sql.Append("   FROM con_proposta_item_versao");
                sql.Append("   WHERE usina=" + usina);
                sql.Append("   AND no_obra=" + obraNumero);
                sql.Append("   AND num_versao=" + numVersao);
                sql.Append(" )");
                sql.Append(" ,vlr_total_ctr=vlr_concreto+vlr_bomba+vlr_extras");
                sql.Append(" WHERE usina=" + usina);
                sql.Append(" AND ano_contrato=" + anoContrato);
                sql.Append(" AND num_contrato=" + numContrato);
                sql.Append(" AND num_versao=" + numVersao);

                cnn.Execute(sql.ToString());
                queries.Add(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_contrato_versao", sql.ToString());

                ////'********************************************************************************************
                ////'Verifica Status atual do contrato
                ////'********************************************************************************************
                var contratoStatus = ObterStatusContrato(numVersao, usina, anoContrato, numContrato);

                if (contratoStatus == (int)EContratoStatus.AguardandoAprovacaoComercial || contratoStatus == (int)EContratoStatus.Aprovado)
                {
                    ContratoVersao contrato = _contratoRepository.ContratoVersaoObterPorId(numVersao, usina, anoContrato, numContrato);

                    //Verifica se pode aprovar cadastro
                    if (!ValidarAprovacaoCadastro(contrato, out string mensagem))
                    {
                        //Atualiza con_contrato
                        sql.Clear();
                        sql.Append("UPDATE con_contrato_versao");
                        sql.Append(" SET id_aprov_cad='" + StringHelper.GetIDD(usuario) + "'");
                        sql.Append(" ,cad_aprovado='S'");
                        sql.Append(" WHERE usina=" + usina);
                        sql.Append(" AND ano_contrato=" + anoContrato);
                        sql.Append(" AND num_contrato=" + numContrato);
                        sql.Append(" AND num_versao=" + numVersao);

                        cnn.Execute(sql.ToString());
                        queries.Add(sql.ToString());
                        cnn.GravarLogGeral(usuario, "con_contrato_versao", sql.ToString());
                    }
                    else
                    {
                        sql.Clear();
                        sql.Append("UPDATE con_contrato_versao");
                        sql.Append(" SET status=" + (int)EContratoStatus.EmAnalise);
                        sql.Append(" WHERE usina=" + usina);
                        sql.Append(" AND ano_contrato=" + anoContrato);
                        sql.Append(" AND num_contrato=" + numContrato);
                        sql.Append(" AND num_versao=" + numVersao);

                        cnn.Execute(sql.ToString());
                        queries.Add(sql.ToString());
                        cnn.GravarLogGeral(usuario, "con_contrato_versao", sql.ToString());

                        var complementoLog = (contratoStatus == (int)EContratoStatus.AguardandoAprovacaoComercial)
                            ? "DE: AG.APROV.COMERCIA PARA: EM ANÁLISE"
                            : "DE: APROVADO PARA: EM ANÁLISE";
                        var evento = ObterEventoConObra(EContratoStatus.AguardandoDadosPagamento);
                        InserirObraLog(numVersao, usina, obraNumero, usuario, evento, complementoLog, "");
                    }
                }

                sql.Clear();
                sql.Append("SELECT status,status_anterior,id_aprov_cad,cad_aprovado");
                sql.Append(" FROM con_contrato_versao");
                sql.Append(" WHERE usina=" + usina);
                sql.Append(" AND ano_contrato=" + anoContrato);
                sql.Append(" AND num_contrato=" + numContrato);
                sql.Append(" AND num_versao=" + numVersao);

                var contratoResult = cnn.QueryFirstOrDefault(sql.ToString());
                if (contratoResult != null)
                {
                    if ((contratoResult.status == (int)EContratoStatus.Aprovado || contratoResult.status == (int)EContratoStatus.AguardandoAprovacaoComercial) && contratoResult.id_aprov_cad != "")
                    {
                        //********************************************************************************************
                        //A Função 'ValidarContrato' verifica se o contrato tem alguma pendencia, e realiza
                        //a aprovação do mesmo caso não existam pendencias
                        //********************************************************************************************
                        ContratoVersao contrato = _contratoRepository.ContratoVersaoObterPorId(numVersao, usina, anoContrato, numContrato);
                        ValidarContrato(contrato, usuario, out string mensagem, aprovContratoDirAuto);
                    }
                }

            }

            if (anoChamada != 0 && numChamada != 0)
            {
                DadosLogAprovacaoProposta[] obraLogs = new DadosLogAprovacaoProposta[logs.Count];

                for (int i = 0; i < logs.Count(); i++)
                {
                    obraLogs[i] = new DadosLogAprovacaoProposta
                    {
                        LogAprovacaoProposta = logs[i].Operacao,
                        Complemento = logs[i].Complemento,
                        ObservacaoProposta = logs[i].Observacao
                    };
                }

                var dadosAprovacao = new DadosAprovacaoProposta
                {
                    NumVersao = numVersao,
                    Usina = usina,
                    AnoChamada = anoChamada,
                    NumChamada = numChamada,
                    ObraNumero = obraNumero,
                    LogsAprovacaoProposta = obraLogs.ToList()
                };

                if (logs.Count > 0)
                {
                    dadosAprovacao.AprovacaoProposta = obraLogs[obraLogs.Length - 1].LogAprovacaoProposta;
                }

                _aprovacaoComercialUsinaRepository.AdicionarLog(
                new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, numVersao, "", "ComerciaLegacyService.FinalizarAprovacoesComerciais",
                    PayloadHelper.ConvertToJson(queries), ""
                    )
                );

                AtualizarProposta(dadosAprovacao, usuario);
            }
        }

        public string FinalizarRevalidacaoCadastro(string usuario, Contrato contrato, string observacaoLog)
        {
            int contratoStatus = 0;
            int contratoAnalista = 0;
            int obraNumero = 0;
            string mensagem;

            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            sql.Clear();
            sql.Append("SELECT c.analista,c.status,o.ano_chamada,o.no_chamada,o.numero num_obra");
            sql.Append(" FROM con_contrato c");
            sql.Append(" LEFT JOIN con_obras o ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato");
            sql.Append(" WHERE c.usina=" + contrato.Usina);
            sql.Append(" AND c.ano_contrato=" + contrato.Ano);
            sql.Append(" AND c.num_contrato=" + contrato.Numero);

            var queryResult = cnn.QueryFirstOrDefault(sql.ToString());

            if (queryResult != null)
            {
                contratoAnalista = (int)queryResult.analista;
                contratoStatus = (int)queryResult.status;
                obraNumero = (int)queryResult.num_obra;
            }

            if (!ValidarAprovacaoCadastro(contrato, out mensagem))
            {
                //Atualiza con_contrato
                sql.Clear();
                sql.Append("UPDATE con_contrato");
                sql.Append(" SET id_aprov_cad='" + StringHelper.GetIDD(usuario) + "'");
                sql.Append(" ,cad_aprovado='S'");
                sql.Append(" WHERE usina=" + contrato.Usina);
                sql.Append(" AND ano_contrato=" + contrato.Ano);
                sql.Append(" AND num_contrato=" + contrato.Numero);

                cnn.Execute(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());
            }
            else
            {
                return mensagem;
            }

            if (ValidarContrato(contrato, usuario, out mensagem, aprovContratoDirAuto))
            {
                //Atualiza con_contrato
                sql.Clear();
                sql.Append("UPDATE con_contrato");
                sql.Append(" SET id_aprov_cad=''");
                sql.Append(" ,cad_aprovado=''");
                sql.Append(" WHERE usina=" + contrato.Usina);
                sql.Append(" AND ano_contrato=" + contrato.Ano);
                sql.Append(" AND num_contrato=" + contrato.Numero);
                sql.Append(" AND status<>" + (int)EContratoStatus.AguardandoAprovacaoComercial);

                cnn.Execute(sql.ToString());
                cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());

                return mensagem;
            }

            sql.Clear();
            sql.Append("SELECT cod");
            sql.Append(" FROM con_funcionario");
            sql.Append(" WHERE usuario_sist='" + usuario + "'");

            var analista = cnn.QueryFirstOrDefault<int>(sql.ToString());

            //'Atualiza Status e Analista con_contrato
            sql.Clear();
            sql.Append("UPDATE con_contrato");
            sql.Append(" SET status=" + (int)EContratoStatus.Aprovado);
            if (analista != 0) sql.Append(" ,analista=" + analista);
            sql.Append(" WHERE usina=" + contrato.Usina);
            sql.Append(" AND ano_contrato=" + contrato.Ano);
            sql.Append(" AND num_contrato=" + contrato.Numero);

            cnn.Execute(sql.ToString());
            cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString());

            //'Insere log mudança status
            if (contratoStatus != (int)EContratoStatus.Aprovado)
            {
                sql.Clear();
                sql.Append("SELECT descr_reduzida");
                sql.Append(" FROM topsys.ger_geral");
                sql.Append(" WHERE cod=" + contratoStatus);

                var StatusDe = cnn.QueryFirstOrDefault<string>(sql.ToString());

                sql.Clear();
                sql.Append("SELECT descr_reduzida");
                sql.Append(" FROM topsys.ger_geral");
                sql.Append(" WHERE cod=" + (int)EContratoStatus.Aprovado);

                var StatusPara = cnn.QueryFirstOrDefault<string>(sql.ToString());

                var evento = ObterEventoConObra(EContratoStatus.AguardandoDadosPagamento);
                var complemento = $"DE: {StatusDe} PARA: {StatusPara}";
                InserirObraLog(contrato.Usina, obraNumero, usuario, evento, complemento, observacaoLog);
            }

            //'Insere log mudança analista
            if (contratoAnalista != analista)
            {
                sql.Clear();
                sql.Append("SELECT nome_reduzido");
                sql.Append(" FROM topsys.con_funcionario");
                sql.Append(" WHERE cod=" + contratoAnalista);

                var AnalistaDe = cnn.QueryFirstOrDefault<string>(sql.ToString());

                sql.Clear();
                sql.Append("SELECT nome_reduzido");
                sql.Append(" FROM topsys.con_funcionario");
                sql.Append(" WHERE cod=" + analista);

                var AnalistaPara = cnn.QueryFirstOrDefault<string>(sql.ToString());

                var evento = ObterEventoConObra(9142);
                var complemento = $"DE: {AnalistaDe} PARA: {AnalistaPara}";
                InserirObraLog(contrato.Usina, obraNumero, usuario, evento, complemento, observacaoLog);
            }

            //'Insere log Acompanhamento
            if (contratoStatus == (int)EContratoStatus.Aprovado && contratoAnalista == analista)
            {
                var evento = ObterEventoConObra(9143);
                InserirObraLog(contrato.Usina, obraNumero, usuario, evento, "", observacaoLog);
            }

            return mensagem;
        }

        public bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out Contrato contrato, out string mensagem)
        {
            mensagem = "";
            contrato = null;

            var cnn = _context.Connection;
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            var transacao = cnn.BeginTransaction();
            var transacaoAberta = true;

            var idd = StringHelper.GetIDD(usuario);

            try
            {
                var sql = new StringBuilder();

                sql.Clear();
                sql.Append("SELECT ger_ctr_analise");
                sql.Append(" FROM con_parametro");
                sql.Append(" WHERE data_vigencia<=curdate()");

                var contratoAnalise = cnn.QueryFirstOrDefault<bool>(sql.ToString(), transaction: transacao);

                var chaveProposta = new { propostaUsina, propostaAno, propostaNumero };

                sql.Clear();
                sql.Append($"SELECT ");

                var columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Obra>(_context, "o");
                sql.Append(columns);

                sql.Append(" FROM con_obras o");
                sql.Append($" WHERE o.usina=@{nameof(propostaUsina)}");
                sql.Append($" AND o.ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND o.no_chamada=@{nameof(propostaNumero)}");
                sql.Append(" FOR UPDATE");

                var obra = cnn.QueryFirstOrDefault<Obra>(sql.ToString(), chaveProposta, transacao);

                if (obra == null)
                    throw new Exception($"Não encontrada obra para a proposta {propostaUsina}-{propostaNumero.ToString().PadLeft(6, '0')}/{propostaAno}");

                var tracos = _intervenienteRepository
                    .ListarFiltrados<ObraTraco>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero);

                var STATUS_CUSTO_VIRTUAL = 7105;

                foreach(var traco in tracos)
                {

                    var status = 0;

                    if ((traco.NumeracaoProduto ?? 0) == 0)
                    {
                        var numeracaoFamilia = _tracoPrecoService.ObterNumeroTabelaVigentePorDataBaseUsina(DateTime.Now, obra.UsinaEntregaCodigo);
                        var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                            obra.UsinaEntregaCodigo,
                            traco.UsoCodigo,
                            traco.PedraCodigo,
                            traco.SlumpCodigo,
                            traco.ResistenciaTipoCodigo,
                            traco.Fck,
                            traco.Consumo, obra);

                        if (tracoPreco is null)
                            continue;

                        status = _tracoPrecoService.ObterStatusPorTracoPreco(tracoPreco);

                    }
                    else
                    {
                        status = _tracoPrecoService.ObterStatusPorNumeracaoProduto(obra.UsinaEntregaCodigo, (traco.NumeracaoProduto ?? 0), obra);
                    }

                    if (status == STATUS_CUSTO_VIRTUAL)
                        throw new Exception($"Proposta {propostaUsina}-{propostaNumero.ToString().PadLeft(6, '0')}/{propostaAno} possui traço com status de Custo Virtual.");

                }

                obra.AtualizarStatusAprovacao(usuario);

                var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

                sql.Clear();

                sql.Append($"SELECT status FROM con_chtel");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                var statusProposta = cnn.QueryFirstOrDefault<int>(sql.ToString(), chaveProposta, transacao);

                if (statusProposta != (int)EPropostaStatus.AprovadaPeloCliente && statusProposta != (int)EPropostaStatus.ContratoGerado 
                    && parametroDesativarObrigatoriedadeAprovacaoCadastro)
                    throw new Exception($"Contrato não pode ser gerado, proposta não esta aprovada.");

                contrato = new Contrato
                {
                    Usina = propostaUsina,
                    Ano = obra.AnoContrato ?? 0,
                    Numero = obra.NumContrato ?? 0
                };

                if (contrato.Numero > 0)
                    throw new Exception($"Contrato já gerado: {contrato.Usina}-{contrato.Numero.ToString().PadLeft(6, '0')}/{contrato.Ano}");

                var obraAprovadaComercialmente = (obra.StatusComercial == EObraStatusComercial.Aprovado || obra.StatusComercial == EObraStatusComercial.NaoNecessita);

                //  Quando parâmetro estiver ativo, contrato só pode ser gerado se estiver aprovado comercialmente
                if (parametroDesativarObrigatoriedadeAprovacaoCadastro && !obraAprovadaComercialmente)
                    throw new Exception("Contrato não pode ser gerado pois há pendências de aprovação comercial.");

                    sql.Clear();
                sql.Append($"SELECT ");

                columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Proposta>(_context, "p");
                sql.Append(columns);

                sql.Append(" FROM con_chtel p");
                sql.Append($" WHERE p.usina=@{nameof(propostaUsina)}");
                sql.Append($" AND p.ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND p.num_chamada=@{nameof(propostaNumero)}");

                var proposta = cnn.QueryFirstOrDefault<Proposta>(sql.ToString(), chaveProposta, transacao);

                if (proposta == null)
                    throw new Exception($"Proposta não encontrada: {propostaUsina}-{propostaNumero.ToString().PadLeft(6, '0')}/{propostaAno}");

                proposta.Cobranca = _intervenienteRepository.ObterPorId<PropostaCobranca>(proposta.UsinaCodigo, proposta.Ano, proposta.Numero);
                proposta.Faturamento = _intervenienteRepository.ObterPorId<PropostaFaturamento>(proposta.UsinaCodigo, proposta.Ano, proposta.Numero);


                var contratoAno = cnn.QueryFirstOrDefault<int>("SELECT CAST(RIGHT(YEAR(curdate()),2) as unsigned);", transaction: transacao);

                sql.Clear();
                sql.Append($"INSERT INTO con_contrato");
                sql.Append($" SET usina=@{nameof(propostaUsina)}");
                sql.Append($", ano_contrato=@{nameof(contratoAno)}");

                cnn.Execute(sql.ToString(), new { propostaUsina, contratoAno }, transacao);

                var contratoNumero = cnn.QueryFirstOrDefault<int>("SELECT @NUMERO_CONTRATO_INSERIDO;", transaction: transacao);

                if (contratoNumero == 0)
                    throw new Exception($"Falha ao inserir contrato");

                contrato.Ano = contratoAno;
                contrato.Numero = contratoNumero;

                sql.Clear();
                sql.Append($"UPDATE con_obras");
                sql.Append($" SET ano_contrato=@{nameof(contratoAno)}");
                sql.Append($", no_contrato=@{nameof(contratoNumero)}");
                sql.Append($", ganhou='S'");
                sql.Append($", intinerante='N'");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND numero=@{nameof(Proposta.ObraCodigo)}");

                object parametros = new { contratoAno, contratoNumero, propostaUsina, proposta.ObraCodigo };
                cnn.Execute(sql.ToString(), parametros, transacao);
                cnn.GravarLogGeral(usuario, "con_obras", sql.ToString(), parametros, transacao);

                // Verificar Se Necessário Cadastrar Cliente ou apenas atualizar Vendedor Exclusivo
                sql.Clear();
                sql.Append($"SELECT ");

                columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Interveniente>(_context, "i");
                sql.Append(columns);

                var filtraPorIE = proposta.CpfCnpj.Length < 14;

                sql.Append($" FROM ger_interv i");
                sql.Append($" WHERE i.CNPJ_CPF=@{nameof(Proposta.CpfCnpj)}");
                sql.Append($" AND ((!{filtraPorIE} OR i.ie = @{nameof(Proposta.InscricaoEstadual)}) OR i.ie = 'ISENTO' OR i.ie = '')");

                var sqlInterveniente = sql.ToString();

                var interveniente = cnn.QueryFirstOrDefault<Interveniente>(sqlInterveniente, new { proposta.CpfCnpj, proposta.InscricaoEstadual }, transacao);

                var intervenienteCodigo = interveniente?.Codigo ?? 0;

                var utilizaVendedorExclusivo = _parametroRepository.ObterParametroN("web", "ValidaVendedorExclusivo").Equals("1");

                if (intervenienteCodigo != 0)
                {
                    sql.Clear();
                    sql.Append($"UPDATE ger_interv");
                    sql.Append($" SET vend=@{nameof(Proposta.VendedorCodigo)}");
                    sql.Append($", id_atual=@{nameof(idd)}");
                    sql.Append($", cli='S'");
                    sql.Append($" WHERE cod=@{nameof(intervenienteCodigo)}");
                    sql.Append($" AND vend=0");

                    parametros = new { proposta.VendedorCodigo, idd, intervenienteCodigo };
                    cnn.Execute(sql.ToString(), parametros, transacao);
                    cnn.GravarLogGeral(usuario, "ger_interv", sql.ToString(), parametros, transacao);
                    _webHookApplicationService.AdicionarWebHookInterveniente(interveniente, EWebHookTipoEvento.Update);
                }
                else
                {
                    var controlaIntervientePorfaixa = _parametroRepository.ObterParametroN("FeatureFlags", "ControlaIntervenientePorfaixa") == "1";

                    if (controlaIntervientePorfaixa)
                    {
                        proposta.IntervenienteCodigo = _intervenienteSequenceControl.GerarProximaSequencia();
                        sql.Clear();
                        sql.Append("UPDATE con_chtel");
                        sql.Append($" SET cod_cliente={proposta.IntervenienteCodigo}");
                        sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                        sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                        sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                        cnn.Execute(sql.ToString(), chaveProposta, transacao);
                        cnn.GravarLogGeral(usuario, "con_chtel", sql.ToString(), chaveProposta, transacao);
                    }

                    sql.Clear();
                    sql.Append($"INSERT INTO ger_interv");
                    sql.Append($" SET");
                    sql.Append($" nome=@{nameof(Proposta.IntervenienteNome)}");
                    sql.Append(controlaIntervientePorfaixa ? $", cod=@{nameof(Proposta.IntervenienteCodigo)}" : "");
                    sql.Append($", razao=@{nameof(Proposta.IntervenienteRazao)}");
                    sql.Append($", cli='S', forn='N', transpo='N', prest_serv='N'");
                    sql.Append($", org_publ='N', outro='N'");
                    sql.Append($", cep=@{nameof(Proposta.EnderecoCep)}");
                    sql.Append($", end=@{nameof(Proposta.EnderecoLogradouro)}");
                    sql.Append($", num=@{nameof(Proposta.EnderecoNumero)}");
                    sql.Append($", compl=@{nameof(Proposta.EnderecoComplemento)}");
                    sql.Append($", bairro=@{nameof(Proposta.EnderecoBairro)}");
                    sql.Append($", cod_munic=@{nameof(Proposta.EnderecoMunicipioCodigo)}");
                    sql.Append($", CNPJ_CPF=@{nameof(Proposta.CpfCnpj)}");
                    sql.Append($", IE=@{nameof(Proposta.InscricaoEstadual)}");
                    sql.Append($", rg=@{nameof(Proposta.Rg)}");
                    sql.Append($", org_uf_emi=@{nameof(Proposta.OrgaoExpedidor)}");
                    sql.Append($", ccm=@{nameof(Proposta.InscricaoMunicipal)}");
                    sql.Append($", ddd=@{nameof(Proposta.TelefoneDdd)}");
                    sql.Append($", tel=@{nameof(Proposta.TelefoneNumero)}");
                    sql.Append($", ramal=@{nameof(Proposta.Ramal)}");
                    sql.Append($", ddd_celular=@{nameof(Proposta.CelularDdd)}");
                    sql.Append($", celular=@{nameof(Proposta.CelularNumero)}");
                    sql.Append($", email=@{nameof(Proposta.Email)}");
                    sql.Append($", email_cobranca=@{nameof(Proposta.EmailCobranca)}");
                    sql.Append($", contato=@{nameof(Proposta.Contato)}");
                    sql.Append($", ativ=0, tp_cobranca=0, port_cobranca=0");
                    sql.Append($", Bloq=0");
                    sql.Append(", vend=");
                    sql.Append(utilizaVendedorExclusivo ? $"@{nameof(Proposta.VendedorCodigo)}" : "0");
                    sql.Append($", Limite_Cred=0, Sal_Atual=0, Maior_Sal=0");
                    sql.Append($", Dt_Maior_Sal=null, Pct_Desco=0, Dt_Ult_Op=null");
                    sql.Append($", Cond_Pag=0, In86='N', ctb_cta_contab=''");
                    sql.Append($", obs=@{nameof(Proposta.Observacao)}");
                    sql.Append($", id_cadast='{idd}'");
                    sql.Append($", id_atual='', bombista='N', forn_mp='N'");
                    sql.Append($", regiao=0, rot=0, seq_rot=0, transp=0");
                    sql.Append($", tp_cliente=@{nameof(Proposta.IntervenienteTipo)}");
                    sql.Append($", ret_iss='X'");
                    sql.Append($", local_entrega='N', especificacao=''");
                    sql.Append($", nome_mae=@{nameof(Proposta.NomeMae)}");
                    sql.Append($", conjuge=@{nameof(Proposta.NomeConjuge)}");
                    sql.Append($", id_aprov_iss='', func='N', mala_direta='N'");
                    sql.Append($", site='', aprov_eng='N'");
                    sql.Append($", profissao=@{nameof(Proposta.Profissao)}");
                    sql.Append($", ddd_com=@{nameof(Proposta.TelefoneComercialDdd)}");
                    sql.Append($", tel_com=@{nameof(Proposta.TelefoneComercialNumero)}");
                    sql.Append($", emp_trabalho=@{nameof(Proposta.EmpresaTrabalho)}");

                    cnn.Execute(sql.ToString(), proposta, transacao);

                    if (controlaIntervientePorfaixa)
                        intervenienteCodigo = (int)proposta.IntervenienteCodigo;
                    else
                        intervenienteCodigo = cnn.QueryFirstOrDefault<int>("SELECT last_insert_id()", transaction: transacao);

                    cnn.GravarLogGeral(usuario, "ger_interv", sql.ToString(), proposta, transacao);
                    interveniente = cnn.QueryFirstOrDefault<Interveniente>(sqlInterveniente, new { proposta.CpfCnpj, proposta.InscricaoEstadual }, transacao);
                    _webHookApplicationService.AdicionarWebHookInterveniente(interveniente, EWebHookTipoEvento.Insert);

                    if (intervenienteCodigo != 0)
                    {
                        sql.Clear();
                        sql.Append($"UPDATE ger_interv_anex");
                        sql.Append($" SET interv=@{nameof(intervenienteCodigo)}");
                        sql.Append($", ano_chamada=0");
                        sql.Append($", num_chamada=0");
                        sql.Append($" WHERE ano_chamada=@{nameof(propostaAno)}");
                        sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                        cnn.Execute(sql.ToString(), new { intervenienteCodigo, propostaAno, propostaNumero });
                    }
                }

                if (intervenienteCodigo == 0)
                    throw new Exception($"Falha ao inserir interveniente");

                if (intervenienteCodigo != proposta.IntervenienteCodigo)
                {
                    sql.Clear();
                    sql.Append("UPDATE con_chtel");
                    sql.Append($" SET cod_cliente={intervenienteCodigo}");
                    sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                    sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                    sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                    cnn.Execute(sql.ToString(), chaveProposta, transacao);
                    cnn.GravarLogGeral(usuario, "con_chtel", sql.ToString(), chaveProposta, transacao);
                }

                sql.Clear();
                sql.Append("SELECT IFNULL(MAX(seq),0)+1");
                sql.Append(" FROM ger_local");
                sql.Append($" WHERE interv=@{nameof(intervenienteCodigo)}");

                var localProximaSequencia = cnn.QueryFirstOrDefault<int>(sql.ToString(), new { intervenienteCodigo }, transacao);

                // Declaração de função interna para lógica dos locais,
                // para que não seja preciso repetir o código 3 vezes
                // optei por usar a função interna e não externa para
                // reutilizar várias variáveis do escopo interno desta função
                int obterLocalSequencia<T>() where T : class, IPropostaDadosPessoais
                {
                    sql.Clear();
                    sql.Append("SELECT ");
                    columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<T>(_context, "l");
                    sql.Append(columns);
                    sql.Append($" FROM {EntityMapHelper.GetTableName<T>(_context)} l");
                    sql.Append(" WHERE l.usina=@propostaUsina");
                    sql.Append(" AND l.ano_chamada=@propostaAno");
                    sql.Append(" AND l.num_chamada=@propostaNumero");

                    var dadosLocal = cnn.QueryFirstOrDefault<T>(sql.ToString(), chaveProposta, transacao);

                    if (dadosLocal == null)
                        return 0;

                    sql.Clear();
                    sql.Append("SELECT ");
                    columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<IntervenienteLocal>(_context, "l");
                    sql.Append(columns);
                    sql.Append(" FROM ger_local l");
                    sql.Append($" WHERE l.interv={intervenienteCodigo}");
                    sql.Append($" AND l.cnpj_cpf=@{nameof(IPropostaDadosPessoais.CpfCnpj)}");
                    sql.Append($" AND l.cep=@{nameof(IPropostaDadosPessoais.EnderecoCep)}");
                    sql.Append($" AND l.end=@{nameof(IPropostaDadosPessoais.EnderecoLogradouro)}");
                    sql.Append($" AND l.num=@{nameof(IPropostaDadosPessoais.EnderecoNumero)}");
                    sql.Append($" AND l.compl=@{nameof(IPropostaDadosPessoais.EnderecoComplemento)}");
                    sql.Append($" AND l.bairro=@{nameof(IPropostaDadosPessoais.EnderecoBairro)}");
                    sql.Append($" AND l.mun=@{nameof(IPropostaDadosPessoais.EnderecoMunicipioCodigo)}");

                    if (typeof(T).Equals(typeof(PropostaFaturamento)))
                    {
                        sql.Append(" AND l.loc_fatur='S'");
                    }
                    else if (typeof(T).Equals(typeof(PropostaCobranca)))
                    {
                        sql.Append(" AND l.loc_cobr='S'");
                    }

                    var local = cnn.QueryFirstOrDefault<IntervenienteLocal>(sql.ToString(), dadosLocal, transacao);

                    if (local == null)
                    {
                        // VALIDAÇÃO ADICIONADA DEVIDO AO TICKET 507837
                        // ONDE ESTAVA OCORRENDO UMA SITUAÇÃO ESTRANHA
                        // EM QUE A GER LOCAL ESTA COM DADOS ZERADOS
                        if(dadosLocal.CpfCnpj == "" || dadosLocal.Nome == "" || dadosLocal.Razao == ""
                            || dadosLocal.EnderecoCep == "" || dadosLocal.EnderecoLogradouro == "" 
                            || dadosLocal.EnderecoBairro == "" || dadosLocal.EnderecoMunicipioCodigo == 0)
                        {
                            return 0;
                        }

                        local = new IntervenienteLocal
                        {
                            IntervenienteCodigo = intervenienteCodigo,
                            Sequencia = localProximaSequencia++,
                            CpfCnpj = dadosLocal.CpfCnpj,
                            Nome = dadosLocal.Nome,
                            Razao = dadosLocal.Razao,
                            Rg = dadosLocal.Rg,
                            OrgaoExpedidor = dadosLocal.OrgaoExpedidor,
                            InscricaoEstadual = dadosLocal.InscricaoEstadual,
                            InscricaoMunicipal = dadosLocal.InscricaoMunicipal,
                            EnderecoCep = dadosLocal.EnderecoCep,
                            EnderecoLogradouro = dadosLocal.EnderecoLogradouro,
                            EnderecoNumero = dadosLocal.EnderecoNumero,
                            EnderecoComplemento = dadosLocal.EnderecoComplemento,
                            EnderecoBairro = dadosLocal.EnderecoBairro,
                            EnderecoMunicipioCodigo = dadosLocal.EnderecoMunicipioCodigo,
                            Email = dadosLocal.Email,
                            LocalCobrancaSimNao = "S",
                            LocalFaturamentoSimNao = "S",
                            LocalEntregaSimNao = "S",
                            IdCadastro = idd,
                        };

                        var comando = local.MontarSqlInsert(_context);
                        cnn.Execute(comando, transaction: transacao);
                        cnn.GravarLogGeral(usuario, "ger_local", comando, transaction: transacao);
                    }

                    return local.Sequencia;
                }

                var localFaturamentoSequencia = obterLocalSequencia<PropostaFaturamento>();
                var localCobrancaSequencia = obterLocalSequencia<PropostaCobranca>();
                var localResponsavelSolidarioSequencia = obterLocalSequencia<PropostaResponsavelSolidario>();

                // VALIDAÇÂO DE FRAUDE (INCONSISTÊNCIAS)
                sql.Clear();
                sql.Append("(SELECT cp.analise_fraude");
                sql.Append(" FROM con_obras o");
                sql.Append(" INNER JOIN ger_cond_pag cp ON o.cond_pgto=cp.cod");
                sql.Append($" WHERE o.usina={obra.UsinaCodigo}");
                sql.Append($" AND o.numero={obra.Numero}");
                sql.Append($" AND o.ano_chamada={obra.AnoChamada}");
                sql.Append($" AND o.no_chamada={obra.NumChamada}");
                sql.Append(" AND cp.analise_fraude='S')");
                sql.Append(" UNION");
                sql.Append(" (SELECT cp.analise_fraude");
                sql.Append(" FROM con_chtel_pag o");
                sql.Append(" INNER JOIN ger_cond_pag cp ON o.cond_pgto=cp.cod");
                sql.Append($" WHERE o.usina={obra.UsinaCodigo}");
                sql.Append($" AND o.ano_chamada={obra.AnoChamada}");
                sql.Append($" AND o.num_chamada={obra.NumChamada}");
                sql.Append(" AND cp.analise_fraude='S')");

                var analiseFraude = cnn.QueryFirstOrDefault<string>(sql.ToString());

                var aguardandoAprovacaoCadastro = "";
                var mensagemCoincidencias = "";

                if (!string.IsNullOrEmpty(analiseFraude))
                    VerificarFraude(intervenienteCodigo, proposta.EnderecoLogradouro, proposta.EnderecoNumero,
                        proposta.Cobranca?.EnderecoLogradouro ?? "", proposta.Cobranca?.EnderecoNumero ?? 0,
                        proposta.Faturamento?.EnderecoLogradouro ?? "", proposta.Faturamento?.EnderecoNumero ?? 0,
                        obra.EnderecoLogradouro, obra.EnderecoNumero, obra.UsinaCodigo, obra.Numero,
                        out aguardandoAprovacaoCadastro, out mensagemCoincidencias);

                // VALIDAÇÃO DE APROVAÇÃO DE ENGENHARIA
                var aprovaEngenharia = "";

                foreach (var traco in tracos)
                {
                    if (aprovaEngenharia == "S")
                        break;

                    aprovaEngenharia = (VerificaRegrasAprovacaoEngenharia(traco, intervenienteCodigo) ? "S" : "N");
                }

                var statusContrato = (int)(contratoAnalise ? EContratoStatus.EmAnalise : EContratoStatus.PreAnalise);
                if(parametroDesativarObrigatoriedadeAprovacaoCadastro)
                {
                    statusContrato = (int)EContratoStatus.Aprovado;
                }

                sql.Clear();
                sql.Append($"REPLACE INTO con_contrato");
                sql.Append($" SET usina=@{nameof(propostaUsina)}");
                sql.Append($", ano_contrato=@{nameof(contratoAno)}");
                sql.Append($", num_contrato=@{nameof(contratoNumero)}");
                sql.Append($", interv=@{nameof(intervenienteCodigo)}");
                sql.Append($", dt_contrato=curdate()");
                sql.Append($", dt_encer_cont=null");
                sql.Append($", representante=@{nameof(Proposta.RepresentanteCodigo)}");
                sql.Append($", vendedor=@{nameof(Proposta.VendedorCodigo)}");
                sql.Append($", num_tab_preco=@{nameof(Proposta.TracoPrecoNumeroTabela)}");
                sql.Append($", local_cobranca=@{nameof(localCobrancaSequencia)}");
                sql.Append($", local_fatur=@{nameof(localFaturamentoSequencia)}");
                sql.Append($", resp_solidario=@{nameof(localResponsavelSolidarioSequencia)}");
                sql.Append($", dt_carta_reajus=null");
                sql.Append($", faturamento_ac=@{nameof(Proposta.Contato)}");
                sql.Append($", obs=@{nameof(Proposta.Observacao)}");
                sql.Append($", id_cadast=@{nameof(idd)}");
                sql.Append($", id_atual=''");
                sql.Append($", no_obra=@{nameof(Proposta.CodigoObraPrefeitura)}");
                sql.Append($", fis_jur=@{nameof(Proposta.IntervenienteTipo)}");
                sql.Append($", fechado='N'");
                sql.Append($", id_aprov_vend=@{nameof(Proposta.IdCadastro)}");
                sql.Append($", id_aprov_dir='', id_aprov_cad='', id_aprov_prog=''");
                sql.Append($", no_ctr_ant=''");
                sql.Append($", vlr_concreto=@{nameof(Proposta.ValorConcreto)}");
                sql.Append($", vlr_bomba=@{nameof(Proposta.ValorBomba)}");
                sql.Append($", vlr_extras=@{nameof(Proposta.ValorExtras)}");
                sql.Append($", vlr_total_ctr=@{nameof(Proposta.ValorTotalContrato)}");
                sql.Append($", total_m3=@{nameof(Proposta.VolumeTotal)}");
                sql.Append($", usina_principal=@{nameof(Obra.UsinaEntregaCodigo)}");
                sql.Append($", pag_ant_analis='', id_analise='', cad_aprovado=''");
                sql.Append($", cheque_analis='', id_analise_chq='', id_aprov_dinh=''");
                sql.Append($", dt_pag_dinh=null, vlr_dinheiro=0");
                sql.Append($", aguard_aprov='{aguardandoAprovacaoCadastro}'");
                sql.Append($", descr_coincid='{mensagemCoincidencias}'");
                sql.Append($", aprov_coincid='', num_cartaocred=0");
                sql.Append($", email_enviado='N'");
                sql.Append($", aprov_eng='{aprovaEngenharia}'");
                sql.Append($", id_aprov_eng=''");
                sql.Append($", vlr_iss_ret=0");
                sql.Append($", status={statusContrato}");
                sql.Append($", vend_padrinho=@{nameof(Proposta.VendedorPadrinhoCodigo)}");
                sql.Append($", ccm_obra=''");
                sql.Append($", modelo_doc_remessa_concreto=@{nameof(Proposta.ModeloDocumentoRemessaConcreto)}");
                sql.Append($", modelo_doc_remessa_bomba=@{nameof(Proposta.ModeloDocumentoRemessaBomba)}");
                sql.Append($", modelo_danfe=@{nameof(Proposta.ModeloItensDanfeERomaneio)}");
                sql.Append($", segmentacao=@{nameof(Proposta.Segmentacao)}");
                sql.Append($", finalidade_ctr=@{nameof(Proposta.ContratoFinalidade)}");
                sql.Append($", inicio_vigencia=CURDATE()");

                sql.Append($", aprov_medicao=@{nameof(Proposta.AprovacaoMedicao)}");
                sql.Append($", tempo_aprov_medicao=@{nameof(Proposta.TempoAprovacaoMedicao)}");

                parametros = new
                {
                    propostaUsina,
                    contratoAno,
                    contratoNumero,
                    intervenienteCodigo,
                    proposta.RepresentanteCodigo,
                    proposta.VendedorCodigo,
                    proposta.TracoPrecoNumeroTabela,
                    localCobrancaSequencia,
                    localFaturamentoSequencia,
                    localResponsavelSolidarioSequencia,
                    proposta.Contato,
                    proposta.Observacao,
                    idd,
                    proposta.ObraCodigo,
                    proposta.IntervenienteTipo,
                    proposta.IdCadastro,
                    proposta.ValorConcreto,
                    proposta.ValorBomba,
                    proposta.ValorExtras,
                    proposta.ValorTotalContrato,
                    proposta.VolumeTotal,
                    obra.UsinaEntregaCodigo,
                    proposta.VendedorPadrinhoCodigo,
                    proposta.CodigoObraPrefeitura,
                    proposta.ModeloDocumentoRemessaConcreto,
                    proposta.ModeloDocumentoRemessaBomba,
                    proposta.ModeloItensDanfeERomaneio,
                    proposta.Segmentacao,
                    proposta.ContratoFinalidade,
                    proposta.AprovacaoMedicao,
                    proposta.TempoAprovacaoMedicao
                };
                cnn.Execute(sql.ToString(), parametros, transacao);
                cnn.GravarLogGeral(usuario, "con_contrato", sql.ToString(), parametros, transacao);

                //Update pra disparar a trigger.
                sql.Clear();
                sql.Append($"UPDATE con_contrato");
                sql.Append($" SET dt_contrato=curdate()");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND ano_contrato=@{nameof(contratoAno)}");
                sql.Append($" AND num_contrato=@{nameof(contratoNumero)}");
                cnn.Execute(sql.ToString(), new
                {
                    propostaUsina,
                    contratoAno,
                    contratoNumero
                }, transacao);


                sql.Clear();
                sql.Append($"UPDATE con_obras_trib_mun");
                sql.Append($" SET ano_contrato=@{nameof(contratoAno)}");
                sql.Append($", num_contrato=@{nameof(contratoNumero)}");
                sql.Append($" WHERE usina_contrato=@{nameof(propostaUsina)}");
                sql.Append($" AND no_obra=@{nameof(Proposta.ObraCodigo)}");

                parametros = new { contratoAno, contratoNumero, propostaUsina, proposta.ObraCodigo };
                cnn.Execute(sql.ToString(), parametros, transacao);
                cnn.GravarLogGeral(usuario, "con_obras_trib_mun", sql.ToString(), parametros, transacao);

                if (proposta.CodigoObraPrefeitura.Trim() != "")
                {
                    sql.Clear();
                    sql.Append($"INSERT IGNORE INTO con_obras_trib_mun");
                    sql.Append($" SET usina_contrato=@{nameof(propostaUsina)}");
                    sql.Append($", num_contrato=@{nameof(contratoNumero)}");
                    sql.Append($", ano_contrato=@{nameof(contratoAno)}");
                    sql.Append($", no_obra=@{nameof(Proposta.ObraCodigo)}");
                    sql.Append($", usina=0");
                    sql.Append($", no_obra_pref=@{nameof(Proposta.CodigoObraPrefeitura)}");
                    sql.Append($", ccm_obra=''");
                    sql.Append($", trib_iss=0");

                    parametros = new { propostaUsina, contratoNumero, contratoAno, proposta.ObraCodigo, proposta.CodigoObraPrefeitura };
                    cnn.Execute(sql.ToString(), parametros, transacao);
                    cnn.GravarLogGeral(usuario, "con_obras_trib_mun", sql.ToString(), parametros, transacao);
                }

                sql.Clear();
                sql.Append($"INSERT INTO con_contrato_pag");
                sql.Append($" (usina, ano_contrato, num_contrato, seq, cond_pagto");
                sql.Append($", tp_cobranca, forma, valor_fixo, valor, pct, necessita_aprov, id_cadast)");
                sql.Append($" SELECT ch.usina, @{nameof(contratoAno)}, @{nameof(contratoNumero)}, ch.seq, ch.cond_pgto");
                sql.Append($", ch.tp_cobranca, ch.forma, ch.valor_fixo, ch.valor, ch.pct, ch.necessita_aprov");
                sql.Append($", @{nameof(idd)}");
                sql.Append($" FROM con_chtel_pag ch");
                sql.Append($" WHERE ch.usina=@{nameof(propostaUsina)}");
                sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                parametros = new { contratoAno, contratoNumero, idd, propostaUsina, propostaAno, propostaNumero };
                cnn.Execute(sql.ToString(), parametros, transacao);
                cnn.GravarLogGeral(usuario, "con_contrato_pag", sql.ToString(), parametros, transacao);

                var dataHora = DateTime.Now;

                sql.Clear();
                sql.Append($"SELECT IFNULL(MAX(seq_log),0) + 1");
                sql.Append($" FROM con_obras_log");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND obra=@{nameof(Proposta.ObraCodigo)}");
                sql.Append($" AND dt_hora_evento=@{nameof(dataHora)}");
                sql.Append($" AND usuario=@{nameof(usuario)}");
                sql.Append($" AND evento='CONTRATO GERADO'");
                sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND no_chamada=@{nameof(propostaNumero)}");

                parametros = new { propostaUsina, proposta.ObraCodigo, dataHora, usuario, propostaAno, propostaNumero };
                var obraLogProximaSequencia = cnn.QueryFirstOrDefault<int>(sql.ToString(), parametros, transacao);

                var complementoLog = $"No.Contrato: {propostaUsina}/{contratoNumero}-{contratoAno}";

                sql.Clear();
                sql.Append($"INSERT INTO con_obras_log");
                sql.Append($" SET usina=@{nameof(propostaUsina)}");
                sql.Append($", obra=@{nameof(Proposta.ObraCodigo)}");
                sql.Append($", dt_hora_evento=NOW()");
                sql.Append($", usuario=@{nameof(usuario)}");
                sql.Append($", evento='CONTRATO GERADO'");
                sql.Append($", complemento=@{nameof(complementoLog)}");
                sql.Append($", obs='Gerado pelo TopConWeb'");
                sql.Append($", envia_email='N'");
                sql.Append($", email_enviado='N'");
                sql.Append($", dt_hora_email=''");
                sql.Append($", ano_Chamada=@{nameof(propostaAno)}");
                sql.Append($", no_chamada=@{nameof(propostaNumero)}");
                sql.Append($", seq_log=@{nameof(obraLogProximaSequencia)}");

                parametros = new
                {
                    propostaUsina,
                    proposta.ObraCodigo,
                    usuario,
                    complementoLog,
                    propostaAno,
                    propostaNumero,
                    obraLogProximaSequencia
                };
                cnn.Execute(sql.ToString(), parametros, transacao);
                cnn.GravarLogGeral(usuario, "con_obras_log", sql.ToString(), parametros, transacao);

                // VERIFICAÇÃO DE REGRAS DE ALTERAÇÃO DE TRAÇO
                foreach (var traco in tracos)
                {
                    VerificaRegrasAlteracaoTraco(traco.UsinaCodigo, traco.ObraCodigo, traco.Sequencia, traco.IdAlteracaoTracoPesado, intervenienteCodigo, traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo, traco.M3Quantidade);
                }

                var parametrosPropostaContrato = new { contratoAno, contratoNumero, propostaUsina, propostaAno, propostaNumero };

                // Atualiza con_programacao (caso exista)
                sql.Clear();
                sql.Append($"UPDATE con_programacao");
                sql.Append($" SET ano_contrato=@{nameof(contratoAno)}");
                sql.Append($", no_contrato=@{nameof(contratoNumero)}");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                cnn.Execute(sql.ToString(), parametrosPropostaContrato, transacao);
                cnn.GravarLogGeral(usuario, "con_programacao", sql.ToString(), parametrosPropostaContrato, transacao);

                // Atualiza tabelas de detalhamento de pagamento (caso exista)
                foreach (var tabelaDetalhamento in new[] { "con_contrato_ccredit", "con_contrato_dep", "con_contrato_dinheir", "con_contrato_cheque" })
                {
                    sql.Clear();
                    sql.Append($"UPDATE topsys.{tabelaDetalhamento}");
                    sql.Append($" SET ano_contrato=@{nameof(contratoAno)}");
                    sql.Append($", num_contrato=@{nameof(contratoNumero)}");
                    sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                    sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                    sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                    cnn.Execute(sql.ToString(), parametrosPropostaContrato, transacao);
                    cnn.GravarLogGeral(usuario, tabelaDetalhamento, sql.ToString(), parametrosPropostaContrato, transacao);
                }

                // Atualiza con_programacao_log (caso exista)
                sql.Clear();
                sql.Append($"UPDATE con_programacao_log");
                sql.Append($" SET ano_contrato=@{nameof(contratoAno)}");
                sql.Append($", no_contrato=@{nameof(contratoNumero)}");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND ano_chamada=@{nameof(propostaAno)}");
                sql.Append($" AND num_chamada=@{nameof(propostaNumero)}");

                cnn.Execute(sql.ToString(), parametrosPropostaContrato, transacao);
                cnn.GravarLogGeral(usuario, "con_programacao_log", sql.ToString(), parametrosPropostaContrato, transacao);

                transacao.Commit();
            }
            catch (Exception e)
            {
                mensagem = $"{e.Message}\n{e.StackTrace}";

                if (transacaoAberta)
                    transacao.Rollback();

                return false;
            }

            return true;
        }

        public void VerificaRegrasAlteracaoTraco(int propostaUsina, int numeroObra, int sequenciaProposta, string idAlteracaoTracoProposta, int codigoInterveniente, int uso, int pedra, int slump, int tipoResistencia, float fck, int consumo, float quantidadeM3, int pUso = 0, int pPedra = 0, int pSlump = 0, int pTipoResistencia = 0, int pConsumo = 0, float pFck = 0)
        {
            const int SEM_VINCULO = 0;
            const int VINCULO_MPA = 1;
            const int VINCULO_CONSUMO = 2;

            var cnn = _context.Connection;

            if (idAlteracaoTracoProposta == "")
            {
                var sql = new StringBuilder();
                float fckConsumo = fck + consumo;

                sql.Append($"SELECT a.uso_p, a.pedra_p, a.slump_p, a.tp_resist_p, a.fck_cons_p");
                sql.Append($", IF(a.tp_resist_p=0,tpr1.cod,a.tp_resist_p) tp_resist");
                sql.Append($", IF(a.tp_resist_p=0,tpr1.mpa_cons,tpr.mpa_cons) vinc_mpa_cons");
                sql.Append($" FROM con_alt_traco a");
                sql.Append($" LEFT JOIN con_tipo_resistencia tpr ON a.tp_resist_p=tpr.cod");
                sql.Append($" LEFT JOIN con_tipo_resistencia tpr1 ON tpr1.cod=@{nameof(tipoResistencia)}");
                sql.Append($" WHERE (tipo_cliente = 'T' OR tipo_cliente =(");
                sql.Append($" SELECT IFNULL(IF(tp_cliente='0','',tp_cliente),'') TpCliente");
                sql.Append($" FROM ger_interv WHERE cod=@{nameof(codigoInterveniente)} LIMIT 1))");
                sql.Append($" AND a.uso=@{nameof(uso)}");
                sql.Append($" AND a.pedra=@{nameof(pedra)}");
                sql.Append($" AND (a.slump=0 OR a.slump=@{nameof(slump)})");
                sql.Append($" AND (a.tp_resist=0 OR a.tp_resist=@{nameof(tipoResistencia)})");
                sql.Append($" AND (a.fck_cons=0 OR a.fck_cons= @{nameof(fckConsumo)})");
                sql.Append($" AND @{nameof(quantidadeM3)} BETWEEN a.volume_min AND a.volume_max");
                sql.Append($" ORDER BY a.slump_p DESC, a.fck_cons_p DESC, a.tp_resist_p");
                sql.Append($" LIMIT 1");

                var rs2 = cnn.QueryFirstOrDefault(sql.ToString(), new { tipoResistencia, codigoInterveniente, uso, pedra, slump, fckConsumo, quantidadeM3 });

                if (rs2 != null)
                {
                    pUso = rs2.uso_p;
                    pPedra = rs2.pedra_p;

                    if (rs2.slump_p > 0)
                        pSlump = rs2.slump_p;
                    else
                        pSlump = slump;

                    if (rs2.tp_resist_p > 0)
                        pTipoResistencia = rs2.tp_resist_p;
                    else
                        pTipoResistencia = tipoResistencia;

                    if (rs2.fck_cons_p > 0)
                    {
                        switch ((int)rs2.vinc_mpa_cons)
                        {
                            case VINCULO_MPA:
                                pConsumo = 0;
                                pFck = rs2.fck_cons_p;
                                break;
                            case VINCULO_CONSUMO:
                                pFck = 0;
                                pConsumo = rs2.fck_cons_p;
                                break;
                        }
                    }
                    else
                    {
                        pFck = fck;
                        pConsumo = consumo;
                    }
                }
                else
                {
                    pUso = 0;
                    pPedra = 0;
                    pSlump = 0;
                    pTipoResistencia = 0;
                    pFck = 0;
                    pConsumo = 0;
                }

                sql = new StringBuilder();

                sql.Append($"UPDATE con_proposta_item");
                sql.Append($" SET pTp_resist=@{nameof(pTipoResistencia)}");
                sql.Append($" ,pfck=@{nameof(pFck)}");
                sql.Append($" ,pconsumo=@{nameof(pConsumo)}");
                sql.Append($" ,puso=@{nameof(pUso)}");
                sql.Append($" ,ppedra=@{nameof(pPedra)}");
                sql.Append($" ,pslump=@{nameof(pSlump)}");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND no_obra=@{nameof(numeroObra)}");
                sql.Append($" AND seq=@{nameof(sequenciaProposta)}");

                cnn.Execute(sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta });
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_proposta_item", sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta });
                _obraRepository.AdicionarLogPropostaItem(new PropostaItemLog(propostaUsina, numeroObra, 0, 0, sequenciaProposta, 0, _identityHelperService.GetUserName(), "ComercialLegacyService.VerificaRegrasAlteracaoTraco", DapperHelper.SubstituirParametros(sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta })));
            }
        }

        public void VerificaRegrasAlteracaoTraco(int numVersao, int propostaUsina, int numeroObra, int sequenciaProposta, string idAlteracaoTracoProposta, int codigoInterveniente, int uso, int pedra, int slump, int tipoResistencia, float fck, int consumo, float quantidadeM3, int pUso = 0, int pPedra = 0, int pSlump = 0, int pTipoResistencia = 0, int pConsumo = 0, float pFck = 0)
        {
            const int SEM_VINCULO = 0;
            const int VINCULO_MPA = 1;
            const int VINCULO_CONSUMO = 2;

            var cnn = _context.Connection;

            if (idAlteracaoTracoProposta == "")
            {
                var sql = new StringBuilder();
                float fckConsumo = fck + consumo;

                sql.Append($"SELECT a.uso_p, a.pedra_p, a.slump_p, a.tp_resist_p, a.fck_cons_p");
                sql.Append($", IF(a.tp_resist_p=0,tpr1.cod,a.tp_resist_p) tp_resist");
                sql.Append($", IF(a.tp_resist_p=0,tpr1.mpa_cons,tpr.mpa_cons) vinc_mpa_cons");
                sql.Append($" FROM con_alt_traco a");
                sql.Append($" LEFT JOIN con_tipo_resistencia tpr ON a.tp_resist_p=tpr.cod");
                sql.Append($" LEFT JOIN con_tipo_resistencia tpr1 ON tpr1.cod=@{nameof(tipoResistencia)}");
                sql.Append($" WHERE (tipo_cliente = 'T' OR tipo_cliente =(");
                sql.Append($" SELECT IFNULL(IF(tp_cliente='0','',tp_cliente),'') TpCliente");
                sql.Append($" FROM ger_interv WHERE cod=@{nameof(codigoInterveniente)} LIMIT 1))");
                sql.Append($" AND a.uso=@{nameof(uso)}");
                sql.Append($" AND a.pedra=@{nameof(pedra)}");
                sql.Append($" AND (a.slump=0 OR a.slump=@{nameof(slump)})");
                sql.Append($" AND (a.tp_resist=0 OR a.tp_resist=@{nameof(tipoResistencia)})");
                sql.Append($" AND (a.fck_cons=0 OR a.fck_cons= @{nameof(fckConsumo)})");
                sql.Append($" AND @{nameof(quantidadeM3)} BETWEEN a.volume_min AND a.volume_max");
                sql.Append($" ORDER BY a.slump_p DESC, a.fck_cons_p DESC, a.tp_resist_p");
                sql.Append($" LIMIT 1");

                var rs2 = cnn.QueryFirstOrDefault(sql.ToString(), new { tipoResistencia, codigoInterveniente, uso, pedra, slump, fckConsumo, quantidadeM3 });

                if (rs2 != null)
                {
                    pUso = rs2.uso_p;
                    pPedra = rs2.pedra_p;

                    if (rs2.slump_p > 0)
                        pSlump = rs2.slump_p;
                    else
                        pSlump = slump;

                    if (rs2.tp_resist_p > 0)
                        pTipoResistencia = rs2.tp_resist_p;
                    else
                        pTipoResistencia = tipoResistencia;

                    if (rs2.fck_cons_p > 0)
                    {
                        switch ((int)rs2.vinc_mpa_cons)
                        {
                            case VINCULO_MPA:
                                pConsumo = 0;
                                pFck = rs2.fck_cons_p;
                                break;
                            case VINCULO_CONSUMO:
                                pFck = 0;
                                pConsumo = rs2.fck_cons_p;
                                break;
                        }
                    }
                    else
                    {
                        pFck = fck;
                        pConsumo = consumo;
                    }
                }
                else
                {
                    pUso = 0;
                    pPedra = 0;
                    pSlump = 0;
                    pTipoResistencia = 0;
                    pFck = 0;
                    pConsumo = 0;
                }

                sql = new StringBuilder();

                sql.Append($"UPDATE con_proposta_item_versao");
                sql.Append($" SET pTp_resist=@{nameof(pTipoResistencia)}");
                sql.Append($" ,pfck=@{nameof(pFck)}");
                sql.Append($" ,pconsumo=@{nameof(pConsumo)}");
                sql.Append($" ,puso=@{nameof(pUso)}");
                sql.Append($" ,ppedra=@{nameof(pPedra)}");
                sql.Append($" ,pslump=@{nameof(pSlump)}");
                sql.Append($" WHERE usina=@{nameof(propostaUsina)}");
                sql.Append($" AND no_obra=@{nameof(numeroObra)}");
                sql.Append($" AND seq=@{nameof(sequenciaProposta)}");
                sql.Append($" AND num_versao=@{nameof(numVersao)}");

                cnn.Execute(sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta });
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_proposta_item_versao", sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta });
                _obraRepository.AdicionarLogPropostaItem(new PropostaItemLog(propostaUsina, numeroObra, 0, 0, sequenciaProposta, numVersao, _identityHelperService.GetUserName(), "ComercialLegacyService.VerificaRegrasAlteracaoTraco(Versao)", DapperHelper.SubstituirParametros(sql.ToString(), new { pTipoResistencia, pFck, pConsumo, pUso, pPedra, pSlump, propostaUsina, numeroObra, sequenciaProposta, numVersao })));
            }
        }

        public bool VerificaRegrasAprovacaoEngenharia(ObraTraco traco, int codigoInterveniente, string tipoCliente = "")
        {
            float fck_Cons;

            var cnn = _context.Connection;

            var sql = new StringBuilder();

            sql.Append($"SELECT aprov_eng FROM ger_interv");
            sql.Append($" WHERE cod={codigoInterveniente}");
            sql.Append($" AND aprov_eng='S'");

            var rs2 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs2 != null)
                return true;

            sql = new StringBuilder();

            sql.Append($"SELECT a.slump_val, a.slump, a.fck_cons_val, a.fck_cons, a.volume_val, a.volume");
            sql.Append($" FROM con_aprov_eng a");
            sql.Append($" LEFT JOIN con_tipo_resistencia tpr ON a.tp_resist=tpr.cod");
            sql.Append($" WHERE (a.tipo_cliente='T'");
            if (tipoCliente == "")
            {
                sql.Append($"  OR a.tipo_cliente=(SELECT IFNULL(IF(tp_cliente='0','',tp_cliente),'') as TpCliente");
                sql.Append($"      FROM ger_interv WHERE cod={codigoInterveniente} LIMIT 1))");
            }
            else
            {
                sql.Append($"  OR a.tipo_cliente={tipoCliente})");
            }
            sql.Append($" AND (a.interv=0 OR a.interv={codigoInterveniente})");
            sql.Append($" AND (a.uso=0 OR a.uso={traco.UsoCodigo})");
            sql.Append($" AND (a.pedra=0 OR a.pedra={traco.PedraCodigo})");
            sql.Append($" AND (a.tp_resist=0 OR a.tp_resist={traco.ResistenciaTipoCodigo})");

            fck_Cons = traco.Fck + traco.Consumo;

            rs2 = cnn.Query(sql.ToString());

            if (rs2 != null)
            {
                foreach (var result in rs2)
                {
                    if (AvaliaExpressao(traco.SlumpCodigo, result.slump_val, result.slump) &&
                        AvaliaExpressao(traco.M3Quantidade, result.volume_val, result.volume) &&
                        AvaliaExpressao(fck_Cons, result.fck_cons_val, result.fck_cons)
                        )
                        return true;
                }
            }
            return false;
        }

        public bool VerificaRegrasAprovacaoEngenharia(ObraTracoVersao traco, int codigoInterveniente, string tipoCliente = "")
        {
            float fck_Cons;

            var cnn = _context.Connection;

            var sql = new StringBuilder();

            sql.Append($"SELECT aprov_eng FROM ger_interv");
            sql.Append($" WHERE cod={codigoInterveniente}");
            sql.Append($" AND aprov_eng='S'");

            var rs2 = cnn.QueryFirstOrDefault(sql.ToString());

            if (rs2 != null)
                return true;

            sql = new StringBuilder();

            sql.Append($"SELECT a.slump_val, a.slump, a.fck_cons_val, a.fck_cons, a.volume_val, a.volume");
            sql.Append($" FROM con_aprov_eng a");
            sql.Append($" LEFT JOIN con_tipo_resistencia tpr ON a.tp_resist=tpr.cod");
            sql.Append($" WHERE (a.tipo_cliente='T'");
            if (tipoCliente == "")
            {
                sql.Append($"  OR a.tipo_cliente=(SELECT IFNULL(IF(tp_cliente='0','',tp_cliente),'') as TpCliente");
                sql.Append($"      FROM ger_interv WHERE cod={codigoInterveniente} LIMIT 1))");
            }
            else
            {
                sql.Append($"  OR a.tipo_cliente={tipoCliente})");
            }
            sql.Append($" AND (a.interv=0 OR a.interv={codigoInterveniente})");
            sql.Append($" AND (a.uso=0 OR a.uso={traco.UsoCodigo})");
            sql.Append($" AND (a.pedra=0 OR a.pedra={traco.PedraCodigo})");
            sql.Append($" AND (a.tp_resist=0 OR a.tp_resist={traco.ResistenciaTipoCodigo})");

            fck_Cons = traco.Fck + traco.Consumo;

            rs2 = cnn.Query(sql.ToString());

            if (rs2 != null)
            {
                foreach (var result in rs2)
                {
                    if (AvaliaExpressao(traco.SlumpCodigo, result.slump_val, result.slump) &&
                        AvaliaExpressao(traco.M3Quantidade, result.volume_val, result.volume) &&
                        AvaliaExpressao(fck_Cons, result.fck_cons_val, result.fck_cons)
                        )
                        return true;
                }
            }
            return false;
        }

        private bool AvaliaExpressao(float valor1, string operador, float valor2)
        {
            switch (operador)
            {
                case "TODOS":
                    return true;
                case ">=":
                    return valor1 >= valor2;
                case "=<":
                    return valor1 <= valor2;
                default:
                    return false;
            }
        }

        public void DesaprovarCondicaoPagamento(int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = true)
        {

            var contratoPagamento = _contratoPagamentoRepository.ObterContratoPagamentoDetalhado(contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, true);
            var obra = _contratoPagamentoRepository.ListarFiltrados<Obra>(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.Numero == contratoPagamento.ObraCodigo, t => t.UsinaEntrega).FirstOrDefault();

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (contratoPagamento.TipoCobranca.TipoCobrancaBoleto())
            {

                foreach (var detalhe in contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheBoleto>())
                {
                    if (!detalhe.DesaprovacaoContratoPagamentoDetalheBoletoScopeIsValid())
                        return;
                }
            }

            if (!contratoPagamento.TipoCobranca.TipoCobrancaBoleto())
            {
                sql.Append($"SELECT gera_cred_cli");
                sql.Append($" FROM con_usina");
                sql.Append($" WHERE cod=@USINA");

                var rs = cnn.QueryFirstOrDefault(sql.ToString(), new { USINA = obra.UsinaEntregaCodigo });

                if (rs != null)
                {
                    if (rs.gera_cred_cli == "S")
                        if (ExcluirFinCar(contratoPagamento, verificaMovimentoDeBancoConciliado))
                            return;
                }
            }

            if (contratoPagamento.TipoCobranca.TipoCobrancaCheque())
            {
                sql.Clear();
                sql.Append($"UPDATE con_contrato_cheque");
                sql.Append($" SET dt_receb=NULL");
                sql.Append($" WHERE usina=@USINA");
                sql.Append($" AND ano_contrato=@ANOCONTRATO");
                sql.Append($" AND num_contrato=@NUMCONTRATO");
                sql.Append($" AND seq_pgto=@SEQUENCIAPAGAMENTO");

                var filtroCheque = new
                {
                    USINA = contratoPagamento.UsinaCodigo,
                    ANOCONTRATO = contratoPagamento.ContratoAno,
                    NUMCONTRATO = contratoPagamento.ContratoNumero,
                    SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia
                };

                cnn.Execute(sql.ToString(), filtroCheque);
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_cheque", sql.ToString(), filtroCheque);

                _contasAReceberRepository.DeletarContasAReceberTipoCheque((ContratoPagamentoDetalheCheque)contratoPagamento.Detalhes.FirstOrDefault());
            }

            sql.Clear();
            sql.Append($"UPDATE con_contrato_pag");
            sql.Append($" SET id_aprovacao=''");
            sql.Append($" WHERE usina=@USINA");
            sql.Append($" AND ano_contrato=@ANOCONTRATO");
            sql.Append($" AND num_contrato=@NUMCONTRATO");
            sql.Append($" AND seq=@SEQUENCIAPAGAMENTO");

            var filtro = new
            {
                USINA = contratoPagamento.UsinaCodigo,
                ANOCONTRATO = contratoPagamento.ContratoAno,
                NUMCONTRATO = contratoPagamento.ContratoNumero,
                SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia
            };

            cnn.Execute(sql.ToString(), filtro);
            cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_pag", sql.ToString(), filtro);

            _obraRepository.Adicionar(new ObraLog
            {
                UsinaCodigo = obra.UsinaCodigo,
                ObraCodigo = obra.Numero,
                AnoChamada = obra.AnoChamada ?? 0,
                NumChamada = obra.NumChamada ?? 0,
                DataHora = DateTime.Now,
                Usuario = usuario,
                Evento = "ACOMPANHAMENTO",
                Complemento = $"Desaprovação de pagamento",
                Observacao = $"PAGAMENTO SEQUENCIA: {contratoPagamento.Sequencia} / {contratoPagamento.CondicaoPagamento.Descricao} - {contratoPagamento.TipoCobranca.Descricao} / Valor: {contratoPagamento.Valor} / Realizado pelo TopConWeb",
                Sequencia = 1
            });

            _obraRepository.SaveChanges();


            sql.Clear();
            sql.Append($"SELECT ano_chamada,no_chamada FROM con_obras");
            sql.Append($" WHERE usina=@USINA");
            sql.Append($" AND ano_contrato=@ANOCONTRATO");
            sql.Append($" AND no_contrato=@NUMCONTRATO");


            var rs2 = cnn.QueryFirstOrDefault(sql.ToString(), new
            {
                USINA = contratoPagamento.UsinaCodigo,
                ANOCONTRATO = contratoPagamento.ContratoAno,
                NUMCONTRATO = contratoPagamento.ContratoNumero,
                SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia
            });

            if (rs2 != null)
            {
                if (rs2.ano_chamada != 0 && rs2.no_chamada != 0)
                {
                    sql.Clear();
                    sql.Append($"UPDATE con_chtel_pag");
                    sql.Append($" SET id_aprovacao=''");
                    sql.Append($" WHERE usina=@USINA");
                    sql.Append($" AND ano_chamada=@ANOCHAMADA");
                    sql.Append($" AND num_chamada=@NUMCHAMADA");
                    sql.Append($" AND seq=@SEQUENCIAPAGAMENTO");

                    cnn.Execute(sql.ToString(), new
                    {
                        USINA = contratoPagamento.UsinaCodigo,
                        ANOCHAMADA = rs2.ano_chamada,
                        NUMCHAMADA = rs2.no_chamada,
                        SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia
                    });

                    cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel_pag", sql.ToString(), new
                    {
                        USINA = contratoPagamento.UsinaCodigo,
                        ANOCHAMADA = rs2.ano_chamada,
                        NUMCHAMADA = rs2.no_chamada,
                        SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia
                    });
                }
            }
        }

        public void DesaprovarCondicaoPagamento(int numeroVersao, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = true)
        {

            var contratoPagamento = _contratoPagamentoRepository.ObterContratoPagamentoDetalhado(numeroVersao, contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, true);
            var obra = _contratoPagamentoRepository.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.Numero == contratoPagamento.ObraCodigo, t => t.UsinaEntrega).FirstOrDefault();

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (contratoPagamento.TipoCobranca.TipoCobrancaBoleto())
            {

                foreach (var detalhe in contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheBoleto>())
                {
                    if (!detalhe.DesaprovacaoContratoPagamentoDetalheBoletoScopeIsValid())
                        return;
                }
            }

            if (!contratoPagamento.TipoCobranca.TipoCobrancaBoleto())
            {
                sql.Append($"SELECT gera_cred_cli");
                sql.Append($" FROM con_usina");
                sql.Append($" WHERE cod=@USINA");

                var rs = cnn.QueryFirstOrDefault(sql.ToString(), new { USINA = obra.UsinaEntregaCodigo });

                if (rs != null)
                {
                    if (rs.gera_cred_cli == "S")
                        if (ExcluirFinCar(contratoPagamento, verificaMovimentoDeBancoConciliado))
                            return;
                }
            }

            if (contratoPagamento.TipoCobranca.TipoCobrancaCheque())
            {
                sql.Clear();
                sql.Append($"UPDATE con_contrato_cheque_versao");
                sql.Append($" SET dt_receb=NULL");
                sql.Append($" WHERE usina=@USINA");
                sql.Append($" AND ano_contrato=@ANOCONTRATO");
                sql.Append($" AND num_contrato=@NUMCONTRATO");
                sql.Append($" AND seq_pgto=@SEQUENCIAPAGAMENTO");
                sql.Append($" AND num_versao=@NUMVERSAO");

                var filtroCheque = new
                {
                    USINA = contratoPagamento.UsinaCodigo,
                    ANOCONTRATO = contratoPagamento.ContratoAno,
                    NUMCONTRATO = contratoPagamento.ContratoNumero,
                    SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia,
                    NUMVERSAO = contratoPagamento.NumeroVersao
                };

                cnn.Execute(sql.ToString(), filtroCheque);
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_cheque_versao", sql.ToString(), filtroCheque);

                _contasAReceberRepository.DeletarContasAReceberTipoCheque((ContratoPagamentoDetalheChequeVersao)contratoPagamento.Detalhes.FirstOrDefault());
            }

            sql.Clear();
            sql.Append($"UPDATE con_contrato_pag_versao");
            sql.Append($" SET id_aprovacao=''");
            sql.Append($" WHERE usina=@USINA");
            sql.Append($" AND ano_contrato=@ANOCONTRATO");
            sql.Append($" AND num_contrato=@NUMCONTRATO");
            sql.Append($" AND seq=@SEQUENCIAPAGAMENTO");
            sql.Append($" AND num_versao=@NUMVERSAO");

            var filtro = new
            {
                USINA = contratoPagamento.UsinaCodigo,
                ANOCONTRATO = contratoPagamento.ContratoAno,
                NUMCONTRATO = contratoPagamento.ContratoNumero,
                SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia,
                NUMVERSAO = contratoPagamento.NumeroVersao
            };

            cnn.Execute(sql.ToString(), filtro);
            cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_pag_versao", sql.ToString(), filtro);

            _obraRepository.Adicionar(new ObraLogVersao
            {
                NumeroVersao = numeroVersao,
                UsinaCodigo = obra.UsinaCodigo,
                ObraCodigo = obra.Numero,
                AnoChamada = obra.AnoChamada ?? 0,
                NumChamada = obra.NumChamada ?? 0,
                DataHora = DateTime.Now,
                Usuario = usuario,
                Evento = "ACOMPANHAMENTO",
                Complemento = $"Desaprovação de pagamento",
                Observacao = $"PAGAMENTO SEQUENCIA: {contratoPagamento.Sequencia} / {contratoPagamento.CondicaoPagamento.Descricao} - {contratoPagamento.TipoCobranca.Descricao} / Valor: {contratoPagamento.Valor} / Realizado pelo TopConWeb",
                Sequencia = 1
            });

            _obraRepository.SaveChanges();


            sql.Clear();
            sql.Append($"SELECT ano_chamada,no_chamada FROM con_obras_versao");
            sql.Append($" WHERE usina=@USINA");
            sql.Append($" AND ano_contrato=@ANOCONTRATO");
            sql.Append($" AND no_contrato=@NUMCONTRATO");
            sql.Append($" AND num_versao=@NUMVERSAO");


            var rs2 = cnn.QueryFirstOrDefault(sql.ToString(), new
            {
                USINA = contratoPagamento.UsinaCodigo,
                ANOCONTRATO = contratoPagamento.ContratoAno,
                NUMCONTRATO = contratoPagamento.ContratoNumero,
                SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia,
                NUMVERSAO = contratoPagamento.NumeroVersao
            });

            if (rs2 != null)
            {
                if (rs2.ano_chamada != 0 && rs2.no_chamada != 0)
                {
                    sql.Clear();
                    sql.Append($"UPDATE con_chtel_pag_versao");
                    sql.Append($" SET id_aprovacao=''");
                    sql.Append($" WHERE usina=@USINA");
                    sql.Append($" AND ano_chamada=@ANOCHAMADA");
                    sql.Append($" AND num_chamada=@NUMCHAMADA");
                    sql.Append($" AND seq=@SEQUENCIAPAGAMENTO");
                    sql.Append($" AND num_versao=@NUMVERSAO");

                    cnn.Execute(sql.ToString(), new
                    {
                        USINA = contratoPagamento.UsinaCodigo,
                        ANOCHAMADA = rs2.ano_chamada,
                        NUMCHAMADA = rs2.no_chamada,
                        SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia,
                        NUMVERSAO = contratoPagamento.NumeroVersao
                    });

                    cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel_pag_versao", sql.ToString(), new
                    {
                        USINA = contratoPagamento.UsinaCodigo,
                        ANOCHAMADA = rs2.ano_chamada,
                        NUMCHAMADA = rs2.no_chamada,
                        SEQUENCIAPAGAMENTO = contratoPagamento.Sequencia,
                        NUMVERSAO = contratoPagamento.NumeroVersao
                    });
                }
            }
        }

        public bool ExcluirFinCar(ContratoPagamento contratoPagamento, bool verificaMovimentoDeBancoConciliado = true)
        {
            var cnn = _context.Connection;
            var sql = new StringBuilder();

            var obra = _contratoPagamentoRepository.ListarFiltrados<Obra>(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.Numero == contratoPagamento.ObraCodigo, t => t.UsinaEntrega).FirstOrDefault();

            var tituloContasAReceber = _tituloContasAReceberRepository.ListarFiltrados(t => t.ContratoUsinaCodigo == contratoPagamento.UsinaCodigo &&
                t.ContratoNumero == contratoPagamento.ContratoNumero &&
                t.ContratoAno == contratoPagamento.ContratoAno &&
                t.DocumentoTipoCodigo == (int)EDocumentoTipo.Cheque &&
                t.Desdobramento == 0);

            if (tituloContasAReceber.Count() > 0)
                if (!tituloContasAReceber.ChequeJaLiquidadoScope())
                    return true;

            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {
                foreach (var detalhe in contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheCartao>())
                {
                    var contasAReceberCartaoList = _contasAReceberRepository.ListarContasAReceberDeCartaoVinculado(detalhe, obra.UsinaEntrega.EmpresaCodigo);
                    foreach (var contasAReceber in contasAReceberCartaoList)
                    {
                        if (!contasAReceber.CartaoDeCreditoJaVinculadoScope())
                            return true;
                    }
                }

                var contrato = _intervenienteRepository.ObterPorId<Contrato>(contratoPagamento.UsinaCodigo, contratoPagamento.ContratoAno, contratoPagamento.ContratoNumero);

                var contasAReceberPagamentoCompensado = _tituloContasAReceberRepository.ListarFiltrados(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.ContratoUsinaCodigo == contratoPagamento.UsinaCodigo &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    t.DocumentoSequencia.StartsWith(contratoPagamento.Sequencia.ToString()) &&
                    t.IntervenienteCodigo == contrato.IntervenienteCodigo
                    );


                foreach (var tituloContas in contasAReceberPagamentoCompensado)
                {
                    if (!tituloContas.PagamentoJaCompensadoScope())
                        return true;
                }
            }

            //Verificar
            var seq = string.Join(",", contratoPagamento.Detalhes.Select(t => $"'{contratoPagamento.Sequencia}{t.DetalheSequencia}'"));
            var sequenciaLista = seq.Replace("'", "").Split(',').ToList();

            IEnumerable<TituloContasAReceber> contasAReceberASeremDesaprovados;

            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {

                contasAReceberASeremDesaprovados = _contasAReceberRepository.ListarContasAReceberCartaoASeremDesaprovados(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, seq, contratoPagamento.ChaveContrato);
            }
            else
            {
                contasAReceberASeremDesaprovados = _tituloContasAReceberRepository.ListarFiltrados(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente &&
                    t.DocumentoSerie == contratoPagamento.UsinaCodigo.ToString() &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    sequenciaLista.Contains(t.DocumentoSequencia) &&
                    t.Desdobramento == 0
                    );
            }

            if (contasAReceberASeremDesaprovados.Count() == 0)
                return false;

            foreach (var contasAReceber in contasAReceberASeremDesaprovados)
            {

                sql.Clear();
                sql.Append($"select max(mes_ano_compet) as comp from fin_parametro");
                sql.Append($" WHERE emp=@EMPRESA");

                var rs = cnn.QueryFirstOrDefault(sql.ToString(), new
                {
                    EMPRESA = obra.UsinaEntrega.EmpresaCodigo,
                });
                string competenciaData = Convert.ToString(rs.comp);
                DateTime.TryParse(competenciaData, out var competencia);
                if (!contasAReceber.ValidaMesFechadoScope(competencia))
                    return true;

                if (!contasAReceber.VerificadoCartaoJaConciliadoScope())
                    return true;

                if (verificaMovimentoDeBancoConciliado)
                {
                    if (VerificaMovimentoBancoConciliado(contasAReceber))
                    {
                        AssertionConcern.Notify("MovimentoBancarioConciliado", "Para esse Recebimento Antecipado já existe movimento de banco, que esta conciliado." +
                               " Não será excluído o movimento de banco! Mesmo assim confirma o cancelamento deste Recebimento Antecipado?" +
                               "\n OBS.: Caso queira excluir o movimento de banco, será necessário cancelar a conciliação deste movimento para depois desaprovar o Recebimento Antecipado.");

                        return true;
                    }
                    else
                    {
                        verificaMovimentoDeBancoConciliado = false;
                    }

                }
            }

            foreach (var contasAReceber in contasAReceberASeremDesaprovados)
            {
                if (!contasAReceber.PagamentoJaCompensadoScope())
                    return true;
            }

            //verificar if mais externo
            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {
                var contasAReceberParaBuscarTransacao = _tituloContasAReceberRepository.ListarFiltradosTracking(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente &&
                    t.DocumentoSerie == contratoPagamento.UsinaCodigo.ToString() &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    sequenciaLista.Contains(t.DocumentoSequencia)
                    );

                foreach (var contasAReceber in contasAReceberParaBuscarTransacao)
                {
                    int.TryParse(contasAReceber.CartaoNumero, out int cartaoNumero);
                    var cartaoTransacao = _cartaoTransacaoRepository.ObterPorDataNumeroCartaoAutorizacao(
                        contasAReceber.DataEmissao ?? DateTime.MinValue, cartaoNumero, contasAReceber.CartaoAutorizacao);

                    if (cartaoTransacao != null)
                    {
                        //int.TryParse(contasAReceber.CartaoNumero, out cartaoNumero);
                        //var contasAReceberParaModificar = _contasAReceberRepository.ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(cartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year)
                        //        .Where(t => t.DocumentoTipo == ((int)EDocumentoTipo.ContasAReceberOperadora)).FirstOrDefault();

                        if (cartaoTransacao.Origem == "manual")
                        {
                            _contasAReceberRepository.DeletarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(contasAReceber.CartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year, EDocumentoTipo.ContasAReceberOperadora);
                            _cartaoTransacaoRepository.RemoverPorId(cartaoTransacao.Id);
                        }
                        else
                        {
                            //Dar Commit
                            _contasAReceberRepository.AtualizarAlocadoContasAReceberPorCartaoEAutorizacao(contasAReceber.CartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year, EContasAReceberStatusAlocado.Automatico);
                            _intervenienteRepository.SaveChanges();
                        }
                    }
                }
                _contasAReceberRepository.DeletarContasAReceberDeCartaoDoCliente(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, contratoPagamento.ChaveContrato, seq);
            }
            else
            {
                ExcluirMovimentoDeBancoVinculadoContrato(contratoPagamento.UsinaCodigo, contratoPagamento.ContratoNumero, contratoPagamento.ContratoAno, seq, verificaMovimentoDeBancoConciliado);
                _contasAReceberRepository.DeletarContasAReceberDoCliente(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, contratoPagamento.ChaveContrato, seq);
            }
            return false;
        }

        public bool ExcluirFinCar(ContratoPagamentoVersao contratoPagamento, bool verificaMovimentoDeBancoConciliado = true)
        {
            var cnn = _context.Connection;
            var sql = new StringBuilder();

            var obra = _contratoPagamentoRepository.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.Numero == contratoPagamento.ObraCodigo, t => t.UsinaEntrega).FirstOrDefault();

            var tituloContasAReceber = _tituloContasAReceberRepository.ListarFiltrados(t => t.ContratoUsinaCodigo == contratoPagamento.UsinaCodigo &&
                t.ContratoNumero == contratoPagamento.ContratoNumero &&
                t.ContratoAno == contratoPagamento.ContratoAno &&
                t.DocumentoTipoCodigo == (int)EDocumentoTipo.Cheque &&
                t.Desdobramento == 0);

            if (tituloContasAReceber.Count() > 0)
                if (!tituloContasAReceber.ChequeJaLiquidadoScope())
                    return true;

            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {
                foreach (var detalhe in contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheCartao>())
                {
                    var contasAReceberCartaoList = _contasAReceberRepository.ListarContasAReceberDeCartaoVinculado(detalhe, obra.UsinaEntrega.EmpresaCodigo);
                    foreach (var contasAReceber in contasAReceberCartaoList)
                    {
                        if (!contasAReceber.CartaoDeCreditoJaVinculadoScope())
                            return true;
                    }
                }

                var contrato = _intervenienteRepository.ObterPorId<ContratoVersao>(contratoPagamento.NumeroVersao, contratoPagamento.UsinaCodigo, contratoPagamento.ContratoAno, contratoPagamento.ContratoNumero);

                var contasAReceberPagamentoCompensado = _tituloContasAReceberRepository.ListarFiltrados(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.ContratoUsinaCodigo == contratoPagamento.UsinaCodigo &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    t.DocumentoSequencia.StartsWith(contratoPagamento.Sequencia.ToString()) &&
                    t.IntervenienteCodigo == contrato.IntervenienteCodigo
                    );


                foreach (var tituloContas in contasAReceberPagamentoCompensado)
                {
                    if (!tituloContas.PagamentoJaCompensadoScope())
                        return true;
                }
            }

            //Verificar
            var seq = string.Join(",", contratoPagamento.Detalhes.Select(t => $"'{contratoPagamento.Sequencia}{t.DetalheSequencia}'"));
            var sequenciaLista = seq.Replace("'", "").Split(',').ToList();

            IEnumerable<TituloContasAReceber> contasAReceberASeremDesaprovados;

            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {

                contasAReceberASeremDesaprovados = _contasAReceberRepository.ListarContasAReceberCartaoASeremDesaprovados(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, seq, contratoPagamento.ChaveContrato);
            }
            else
            {
                contasAReceberASeremDesaprovados = _tituloContasAReceberRepository.ListarFiltrados(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente &&
                    t.DocumentoSerie == contratoPagamento.UsinaCodigo.ToString() &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    sequenciaLista.Contains(t.DocumentoSequencia) &&
                    t.Desdobramento == 0
                    );
            }

            if (contasAReceberASeremDesaprovados.Count() == 0)
                return false;

            foreach (var contasAReceber in contasAReceberASeremDesaprovados)
            {

                sql.Clear();
                sql.Append($"select max(mes_ano_compet) as comp from fin_parametro");
                sql.Append($" WHERE emp=@EMPRESA");

                var rs = cnn.QueryFirstOrDefault(sql.ToString(), new
                {
                    EMPRESA = obra.UsinaEntrega.EmpresaCodigo,
                });
                string competenciaData = Convert.ToString(rs.comp);
                DateTime.TryParse(competenciaData, out var competencia);
                if (!contasAReceber.ValidaMesFechadoScope(competencia))
                    return true;

                if (!contasAReceber.VerificadoCartaoJaConciliadoScope())
                    return true;

                if (verificaMovimentoDeBancoConciliado)
                {
                    if (VerificaMovimentoBancoConciliado(contasAReceber))
                    {
                        AssertionConcern.Notify("MovimentoBancarioConciliado", "Para esse Recebimento Antecipado já existe movimento de banco, que esta conciliado." +
                               " Não será excluído o movimento de banco! Mesmo assim confirma o cancelamento deste Recebimento Antecipado?" +
                               "\n OBS.: Caso queira excluir o movimento de banco, será necessário cancelar a conciliação deste movimento para depois desaprovar o Recebimento Antecipado.");

                        return true;
                    }
                    else
                    {
                        verificaMovimentoDeBancoConciliado = false;
                    }

                }
            }

            foreach (var contasAReceber in contasAReceberASeremDesaprovados)
            {
                if (!contasAReceber.PagamentoJaCompensadoScope())
                    return true;
            }

            //verificar if mais externo
            if (contratoPagamento.TipoCobranca.TipoCobrancaCartao())
            {
                var contasAReceberParaBuscarTransacao = _tituloContasAReceberRepository.ListarFiltradosTracking(
                    t => t.EmpresaCodigo == obra.UsinaEntrega.EmpresaCodigo &&
                    t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente &&
                    t.DocumentoSerie == contratoPagamento.UsinaCodigo.ToString() &&
                    t.DocumentoNumero == contratoPagamento.ChaveContrato &&
                    sequenciaLista.Contains(t.DocumentoSequencia)
                    );

                foreach (var contasAReceber in contasAReceberParaBuscarTransacao)
                {
                    int.TryParse(contasAReceber.CartaoNumero, out int cartaoNumero);
                    var cartaoTransacao = _cartaoTransacaoRepository.ObterPorDataNumeroCartaoAutorizacao(
                        contasAReceber.DataEmissao ?? DateTime.MinValue, cartaoNumero, contasAReceber.CartaoAutorizacao);

                    if (cartaoTransacao != null)
                    {
                        //int.TryParse(contasAReceber.CartaoNumero, out cartaoNumero);
                        //var contasAReceberParaModificar = _contasAReceberRepository.ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(cartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year)
                        //        .Where(t => t.DocumentoTipo == ((int)EDocumentoTipo.ContasAReceberOperadora)).FirstOrDefault();

                        if (cartaoTransacao.Origem == "manual")
                        {
                            _contasAReceberRepository.DeletarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(contasAReceber.CartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year, EDocumentoTipo.ContasAReceberOperadora);
                            _cartaoTransacaoRepository.RemoverPorId(cartaoTransacao.Id);
                        }
                        else
                        {
                            //Dar Commit
                            _contasAReceberRepository.AtualizarAlocadoContasAReceberPorCartaoEAutorizacao(contasAReceber.CartaoNumero, contasAReceber.CartaoAutorizacao, contasAReceber.DataEmissao?.Year ?? DateTime.MinValue.Year, EContasAReceberStatusAlocado.Automatico);
                            _intervenienteRepository.SaveChanges();
                        }
                    }
                }
                _contasAReceberRepository.DeletarContasAReceberDeCartaoDoCliente(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, contratoPagamento.ChaveContrato, seq);
            }
            else
            {
                ExcluirMovimentoDeBancoVinculadoContrato(contratoPagamento.UsinaCodigo, contratoPagamento.ContratoNumero, contratoPagamento.ContratoAno, seq, verificaMovimentoDeBancoConciliado);
                _contasAReceberRepository.DeletarContasAReceberDoCliente(obra.UsinaEntrega.EmpresaCodigo, contratoPagamento.UsinaCodigo, contratoPagamento.ChaveContrato, seq);
            }
            return false;
        }

        public void ExcluirMovimentoDeBancoVinculadoContrato(int contratoUsinaCodigo, long contratoNumero, int contratoAno, string sequencia, bool verificaMovimentoDeBancoConciliado)
        {
            var sql = new StringBuilder();
            var rs = _context.Connection;
            var rs2 = _context.Connection;
            var sequenciaLista = sequencia.Replace("'", "").Split(',').ToList();

            var contasAReceberList = _tituloContasAReceberRepository.ListarFiltradosTracking(
                   t => t.ContratoUsinaCodigo == contratoUsinaCodigo &&
                   t.ContratoNumero == contratoNumero &&
                   t.ContratoAno == contratoAno &&
                   sequenciaLista.Contains(t.DocumentoSequencia)
                   );

            if (contasAReceberList.Count() > 0)
            {
                foreach (var contasAReceber in contasAReceberList)
                {
                    sql.Clear();
                    sql.Append($"select id_mov_bco,emp,tp_doc,ser_doc,num_doc,seq,cod_banco_band,num_agencia,num_conta,CAST(dv_conta AS UNSIGNED) dv_conta,desdo,valor");
                    sql.Append($" from fin_vinc_car_mov_bco");
                    sql.Append($" where emp=@{nameof(contasAReceber.EmpresaCodigo)}");
                    sql.Append($" and tp_doc=@{nameof(contasAReceber.DocumentoTipoCodigo)}");
                    sql.Append($" and ser_doc=@{nameof(contasAReceber.DocumentoSerie)}");
                    sql.Append($" and num_doc=@{nameof(contasAReceber.DocumentoNumero)}");
                    sql.Append($" and seq=@{nameof(contasAReceber.DocumentoSequencia)}");
                    sql.Append($" and cod_banco_band=@{nameof(contasAReceber.BancoCodigoOficial)}");
                    sql.Append($" and num_agencia=@{nameof(contasAReceber.BancoNumeroAgencia)}");
                    sql.Append($" and num_conta=@{nameof(contasAReceber.BancoNumeroConta)}");
                    sql.Append($" and dv_conta=@{nameof(contasAReceber.BancoDvConta)}");
                    sql.Append($" and desdo=@{nameof(contasAReceber.Desdobramento)}");

                    var result = rs.Query(sql.ToString(), contasAReceber);

                    foreach (var vinculoFinCarMovBanco in result)
                    {
                        //Se for false é porque o usuario já confirmou.
                        if (!verificaMovimentoDeBancoConciliado)
                        {
                            var controleString = Convert.ToString(vinculoFinCarMovBanco.id_mov_bco);
                            long.TryParse(controleString, out long controle);
                            MovimentoBanco movimentoBanco = _movimentoBancoRepository.ObterPorControle(controle);

                            var vinculoValorString = Convert.ToString(vinculoFinCarMovBanco.valor);
                            float.TryParse(vinculoValorString, out float vinculoValor);
                            if (movimentoBanco != null && movimentoBanco.Valor <= vinculoValor)
                                _movimentoBancoRepository.RemoverNaoConciliadoPorControle(movimentoBanco.Id);
                            else
                            {
                                movimentoBanco.Valor = movimentoBanco.Valor - vinculoValor;
                                _movimentoBancoRepository.AtualizarNaoConciliado(movimentoBanco);
                            }
                        }

                        sql.Clear();
                        sql.Append($"delete from fin_vinc_car_mov_bco");
                        sql.Append($" where id_mov_bco=@IDMOVBANCO");
                        sql.Append($" and emp=@EMPRESA");
                        sql.Append($" and tp_doc=@DOCUMENTOTIPO");
                        sql.Append($" and ser_doc=@DOCUMENTOSERIE");
                        sql.Append($" and num_doc=@DOCUMENTONUMERO");
                        sql.Append($" and seq=@SEQUENCIA");
                        sql.Append($" and cod_banco_band=@CODBANCO");
                        sql.Append($" and num_agencia=@AGENCIANUMERO");
                        sql.Append($" and num_conta=@CONTANUMERO");
                        sql.Append($" and dv_conta=@CONTADIGITO");
                        sql.Append($" and desdo=@DESDOBRAMENTO");
                        var filtro = new
                        {
                            IDMOVBANCO = vinculoFinCarMovBanco.id_mov_bco,
                            EMPRESA = vinculoFinCarMovBanco.emp,
                            DOCUMENTOTIPO = vinculoFinCarMovBanco.tp_doc,
                            DOCUMENTOSERIE = vinculoFinCarMovBanco.ser_doc,
                            DOCUMENTONUMERO = vinculoFinCarMovBanco.num_doc,
                            SEQUENCIA = vinculoFinCarMovBanco.seq,
                            CODBANCO = vinculoFinCarMovBanco.cod_banco_band,
                            AGENCIANUMERO = vinculoFinCarMovBanco.num_agencia,
                            CONTANUMERO = vinculoFinCarMovBanco.num_conta,
                            CONTADIGITO = vinculoFinCarMovBanco.dv_conta,
                            DESDOBRAMENTO = vinculoFinCarMovBanco.desdo
                        };
                        rs2.Execute(sql.ToString(), filtro);
                        rs2.GravarLogGeral(_identityHelperService.GetUserName(), "fin_vinc_car_mov_bco", sql.ToString(), filtro);

                        contasAReceber.IdMovimentoBanco = 0;

                        _tituloContasAReceberRepository.SaveChanges();
                    }
                }
            }
        }

        public bool VerificaMovimentoBancoConciliado(TituloContasAReceber contasAReceber)
        {
            var cnn = _context.Connection;
            var sql = new StringBuilder();

            sql.Append($"select v.id_mov_bco,c.emp,c.liq_bco,c.liq_dt,mb.dt_conc");
            sql.Append($" from fin_car as c");
            sql.Append($" left join fin_vinc_car_mov_bco as v");
            sql.Append($" on v.emp=c.emp");
            sql.Append($" and v.tp_doc=c.tp_doc");
            sql.Append($" and v.ser_doc=c.ser_doc");
            sql.Append($" and v.num_doc=c.num_doc");
            sql.Append($" and v.seq=c.seq");
            sql.Append($" and v.cod_banco_band=c.cod_banco_band");
            sql.Append($" and v.num_agencia=c.num_agencia");
            sql.Append($" and v.num_conta=c.num_conta");
            sql.Append($" and v.dv_conta=c.dv_conta");
            sql.Append($" and v.desdo=c.desdo");
            sql.Append($" left join fin_mov_banco as mb");
            sql.Append($" on v.id_mov_bco=mb.controle");
            sql.Append($" where c.Emp= @EMPRESACOD");
            sql.Append($" and c.tp_doc= @DOCUMENTOTIPO");
            sql.Append($" and c.ser_doc=@DOCUMENTOSERIE");
            sql.Append($" and c.num_doc=@DOCUMENTONUMERO");
            sql.Append($" and c.seq=@SEQUENCIA");
            sql.Append($" and c.cod_banco_band=@BANDEIRACODIGO");
            sql.Append($" and c.num_agencia= @AGENCIANUMERO");
            sql.Append($" and c.num_conta= @CONTANUMERO");
            sql.Append($" and c.dv_conta= @CONTADIGITO");
            sql.Append($" and c.desdo=@DESDOBRAMENTO");


            var rs = cnn.Query(sql.ToString(), new
            {
                EMPRESACOD = contasAReceber.EmpresaCodigo,
                DOCUMENTOTIPO = contasAReceber.DocumentoTipoCodigo,
                DOCUMENTOSERIE = contasAReceber.DocumentoSerie,
                DOCUMENTONUMERO = contasAReceber.DocumentoNumero,
                SEQUENCIA = contasAReceber.DocumentoSequencia,
                BANDEIRACODIGO = contasAReceber.BancoCodigoOficial,
                AGENCIANUMERO = contasAReceber.BancoNumeroAgencia,
                CONTANUMERO = contasAReceber.BancoNumeroConta,
                CONTADIGITO = contasAReceber.BancoDvConta,
                DESDOBRAMENTO = contasAReceber.Desdobramento
            });

            if (rs.Count() > 0)
            {
                foreach (var item in rs)
                {

                    string dataUltimaConciliacao = Convert.ToString(item.dt_conc);
                    if (DateTime.TryParse(dataUltimaConciliacao, out DateTime dateTemp))
                        return true;
                }
            }
            return false;
        }

        public void TotalizarValoresProgramacao(Programacao programacao)
        {
            var cnn = _context.Connection;

            var sql = new StringBuilder();

            var taxasExtras = _obraTaxaService.ListarByIdObra(programacao.UsinaEntregaCodigo, programacao.ObraNumero);

            var obraTraco = _intervenienteRepository.ObterPorId<ObraTraco>(programacao.UsinaCodigo, programacao.ObraNumero, programacao.ObraTracoSequencia);
            var obraBomba = _intervenienteRepository.ObterPorId<ObraBomba>(programacao.UsinaCodigo, programacao.ObraNumero, programacao.ObraBombaSequencia);

            var contrato = _intervenienteRepository.ObterPorId<Contrato>(programacao.UsinaCodigo, programacao.ContratoAno ?? 0, programacao.ContratoNumero ?? 0);
            var cliente = _intervenienteRepository.ObterPorId(contrato?.IntervenienteCodigo ?? 0);
            var obra = _intervenienteRepository.ObterPorId<Obra>(programacao.UsinaCodigo, programacao.ObraNumero);

            var valorEstimadoConcretoPorM3 = 0f;
            var valorEstimadoConcretoTotal = 0f;

            if (obraTraco != null)
            {
                if (obraTraco.DataUltimoReajuste == null)
                    valorEstimadoConcretoPorM3 = obraTraco.M3PrecoProposto - obraTraco.ValorRessarcido;
                else if (obraTraco.DataUltimoReajuste > DateTime.Today)
                    valorEstimadoConcretoPorM3 = obraTraco.PrecoReajustadoAnterior - obraTraco.ValorRessarcido;
                else
                    valorEstimadoConcretoPorM3 = obraTraco.PrecoReajustadoAtual - obraTraco.ValorRessarcido;

                valorEstimadoConcretoTotal = programacao.VolumeTotal * valorEstimadoConcretoPorM3;
            }

            var bombeadoVolume = 0f;
            var bombeadoValorTaxaMinima = 0f;
            var bombeadoValorM3 = 0f;
            var bombeadoVolumeFixo = 0f;

            if (obraBomba != null)
            {
                sql.Clear();
                sql.Append($"SELECT tx_min_recalc, vol_min_recalc, vlr_m3_recalc");
                sql.Append($" FROM con_reaj_bomba");
                sql.Append($" WHERE usina={programacao.UsinaCodigo}");
                sql.Append($" AND num_contrato={programacao.ContratoNumero}");
                sql.Append($" AND ano_contrato={programacao.ContratoAno}");
                sql.Append($" AND dt_vigencia<=@{nameof(Programacao.DataConcretagem)}");
                sql.Append($" AND seq={obraBomba.Sequencia}");
                sql.Append($" AND NOT ISNULL(data_carta)");
                sql.Append($" ORDER BY dt_vigencia DESC LIMIT 1");

                var reajusteBomba = cnn.QueryFirstOrDefault(sql.ToString(), programacao);

                if (reajusteBomba != null)
                {
                    bombeadoVolume = reajusteBomba.vol_min_recalc;
                    bombeadoValorTaxaMinima = reajusteBomba.tx_min_recalc;
                    bombeadoValorM3 = reajusteBomba.vlr_m3_recalc;
                }
                else
                {
                    bombeadoVolume = obraBomba.M3PropostoAte;
                    bombeadoValorTaxaMinima = obraBomba.TaxaMinimaPrecoProposto;
                    bombeadoValorM3 = obraBomba.M3PrecoProposto;
                }

                bombeadoVolumeFixo = bombeadoVolume;
            }

            var programacoes = _intervenienteRepository
                .ListarFiltrados<Programacao>(t => t.UsinaCodigo == programacao.UsinaCodigo && t.ObraNumero == programacao.ObraNumero);

            if (programacao.ContinuidadeDaSequencia != 0)
                programacoes = programacoes.Where(t => t.Sequencia == programacao.Sequencia || t.ContinuidadeDaSequencia == programacao.Sequencia
                || t.Sequencia == programacao.ContinuidadeDaSequencia || t.ContinuidadeDaSequencia == programacao.ContinuidadeDaSequencia)
                 .OrderBy(t => t.Sequencia);
            else
                programacoes = programacoes.Where(t => t.Sequencia == programacao.Sequencia || t.ContinuidadeDaSequencia == programacao.Sequencia)
                .OrderBy(t => t.Sequencia);


            foreach (var prog in programacoes)
            {
                var valorEstimadoBomba = 0f;

                if (Math.Round(prog.VolumeTotal, 1) > Math.Round(bombeadoVolume, 1))
                {
                    if (Math.Round(bombeadoVolume, 1) == Math.Round(bombeadoVolumeFixo, 1))
                    {
                        if (obraBomba?.TipoCalculo == EBombaM3CalculoTipo.TaxaMinimaOuExcedente)
                            valorEstimadoBomba = bombeadoValorM3 * prog.VolumeTotal;
                        else
                            valorEstimadoBomba = bombeadoValorTaxaMinima + (bombeadoValorM3 * (prog.VolumeTotal - bombeadoVolume));
                    }
                    else
                    {
                        valorEstimadoBomba = (prog.VolumeTotal - bombeadoVolume) * bombeadoValorM3;
                    }
                    bombeadoVolume = 0f;
                }
                else
                {
                    if (Math.Round(bombeadoVolume, 1) == Math.Round(bombeadoVolumeFixo, 1))
                        valorEstimadoBomba = bombeadoValorTaxaMinima;

                    bombeadoVolume -= prog.VolumeTotal;
                }

                valorEstimadoBomba += obraBomba?.HoraTaxaMinimaPrecoProposto ?? 0f;

                sql.Clear();
                sql.Append($"UPDATE con_programacao");
                sql.Append($" SET vlr_bomba=@{nameof(valorEstimadoBomba)}");
                sql.Append($" WHERE usina={prog.UsinaCodigo}");
                sql.Append($" AND ano_contrato={prog.ContratoAno ?? 0}");
                sql.Append($" AND no_contrato={prog.ContratoNumero ?? 0}");
                sql.Append($" AND seq_prog={prog.Sequencia}");
                sql.Append($" AND ano_chamada={prog.PropostaAno ?? 0}");
                sql.Append($" AND num_chamada={prog.PropostaNumero ?? 0}");
                sql.Append($" AND no_obra={prog.ObraNumero}");

                cnn.Execute(sql.ToString(), new { valorEstimadoBomba });
                cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_programacao", sql.ToString(), new { valorEstimadoBomba });
            }

            sql.Clear();
            sql.Append($"SELECT f.mun FROM con_usina u");
            sql.Append($" INNER JOIN fis_filial f ON u.emp_filial=f.emp_filial");
            sql.Append($" WHERE u.cod={programacao.UsinaEntregaCodigo}");

            var municipioUsinaEntrega = cnn.QueryFirstOrDefault<int>(sql.ToString());

            var valorAdicionalDomingoEFeriados = 0f;

            if (EhDomingoOuFeriado(programacao.DataConcretagem, municipioUsinaEntrega))
            {
                valorAdicionalDomingoEFeriados = _obraTaxaService.ObterValorAdicionalDomingosEFeriados(valorEstimadoConcretoTotal, taxasExtras);
            }

            var intervenienteTipos = new string[] { cliente?.IntervenienteTipo ?? "", "T" };

            var horarios = _intervenienteRepository.ListarFiltrados<ProgramacaoHora>(t =>
                t.UsinaCodigo == programacao.UsinaCodigo
                && t.ContratoAno == programacao.ContratoAno
                && t.ContratoNumero == programacao.ContratoNumero);

            var valorAdicionalNoturno = horarios
                .Select(t => _obraTaxaService.ObterValorAdicionalNoturno(t.Horario, programacao.DataConcretagem, t.VolumeProgramado, valorEstimadoConcretoPorM3, intervenienteTipos, taxasExtras))
                .Sum(t => t);

            var valorAdicionalM3Faltante = _obraTaxaService.ObterValorM3Faltante(obraBomba != null, programacao.VolumeTotal, programacao.VolumePorCarga, taxasExtras);

            var valorAdicionalKmRodado = _obraTaxaService.ObterValorAdicionalPorKmRodado(obra.DistanciaUsina, programacao.VolumeTotal, programacao.VolumePorCarga, taxasExtras, obraBomba != null);

            var obraDemaisServicos = _intervenienteRepository
                .ListarFiltrados<ObraDemaisServicos>(t => t.UsinaCodigo == programacao.UsinaCodigo && t.ObraNumero == programacao.ObraNumero);

            var valorDemaisServicos = 0f;

            if (obraDemaisServicos.Count() > 0)
            {
                var programacaoDemaisServicos = _intervenienteRepository
                    .ListarFiltradosTracking<ProgramacaoDemaisServicos>(t => t.UsinaCodigo == programacao.UsinaCodigo && t.ObraNumero == programacao.ObraNumero && t.ProgramacaoSequencia == programacao.Sequencia);

                foreach (var item in programacaoDemaisServicos)
                {
                    var servico = obraDemaisServicos.FirstOrDefault(t => t.Sequencia == item.Sequencia);

                    if (servico == null)
                        continue;

                    var valorTotal = 0f;
                    var valorCobrado = 0f;

                    switch (servico.FrequenciaDeCobranca)
                    {
                        case EFrequenciaDeCobranca.Contrato:
                        case EFrequenciaDeCobranca.Programacao:
                            valorTotal = item.Quantidade * servico.PrecoProposto;
                            valorCobrado = valorTotal;
                            break;
                        case EFrequenciaDeCobranca.Remessa:
                            valorTotal = item.Quantidade * servico.PrecoProposto;
                            var volumePorCarga = (programacao.VolumePorCarga > 0 ? programacao.VolumePorCarga : 8);
                            var quantidadeViagens = programacao.VolumeTotal / volumePorCarga;
                            var volumeUltimaCarga = programacao.VolumeTotal % volumePorCarga;
                            if (volumeUltimaCarga > 0) quantidadeViagens++;
                            valorCobrado = valorTotal * quantidadeViagens;
                            break;
                        case EFrequenciaDeCobranca.M3:
                            valorTotal = item.Quantidade * servico.PrecoProposto;
                            valorCobrado = valorTotal * programacao.VolumeTotal;
                            break;
                        case EFrequenciaDeCobranca.M3Bombeado:
                            valorTotal = item.Quantidade * servico.PrecoProposto;
                            if (obraBomba != null)
                                valorCobrado = valorTotal * programacao.VolumeTotal;
                            break;
                        case EFrequenciaDeCobranca.Bombeamento:
                            valorTotal = item.Quantidade * servico.PrecoProposto;
                            if (obraBomba != null)
                                valorCobrado = valorTotal;
                            break;
                    }

                    item.ValorTotal = valorTotal;
                    item.ValorCobrado = valorCobrado;
                    _intervenienteRepository.SaveChanges();
                }

                valorDemaisServicos = programacaoDemaisServicos.Select(t => t.ValorCobrado).Sum();
            }

            var valorTotalVibrador = programacao.VibradorQuantidade * programacao.VibradorValorUnitario;
            var valorTotalExtras = valorAdicionalDomingoEFeriados + valorAdicionalNoturno + valorAdicionalM3Faltante + valorAdicionalKmRodado;

            sql.Clear();
            sql.Append("UPDATE con_programacao");
            sql.Append($" SET vlr_concreto=@{nameof(valorEstimadoConcretoTotal)}");
            sql.Append($", vlr_extras=@{nameof(valorTotalExtras)}");
            sql.Append($", vlr_total_vib=@{nameof(valorTotalVibrador)}");
            sql.Append($", vlr_total_prog=(vlr_concreto + vlr_bomba + vlr_extras + vlr_total_vib + @{nameof(valorDemaisServicos)})");
            sql.Append($", vlr_demais_servicos=@{nameof(valorDemaisServicos)}");
            sql.Append($" WHERE usina=@{nameof(Programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(Programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(Programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(Programacao.Sequencia)}");
            sql.Append($" AND ano_chamada=@{nameof(Programacao.PropostaAno)}");
            sql.Append($" AND num_chamada=@{nameof(Programacao.PropostaNumero)}");
            sql.Append($" AND numero_obra=@{nameof(Programacao.ObraNumero)}");

            var filtro = new
            {
                valorEstimadoConcretoTotal,
                valorTotalExtras,
                valorTotalVibrador,
                valorDemaisServicos,
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.PropostaAno,
                programacao.PropostaNumero,
                programacao.ObraNumero
            };
            cnn.Execute(sql.ToString(), filtro);
            cnn.GravarLogGeral(_identityHelperService.GetUserName(), "con_programacao", sql.ToString(), filtro);
        }

        public bool EhDomingoOuFeriado(DateTime data, int municipioCodigo)
        {
            var cnn = _context.Connection;

            var sql = new StringBuilder();

            sql.Append("SELECT *");
            sql.Append(" FROM topsys.ger_feriado");
            sql.Append($" WHERE municipio IN({municipioCodigo},0)");
            sql.Append($" AND data=@{nameof(data)}");

            var result = cnn.QueryFirstOrDefault(sql.ToString(), new { data });

            if (result != null)
                return true;

            return (data.DayOfWeek == DayOfWeek.Sunday);
        }

        public bool GeraProgramacao(int idUsina, int obraNumero, int sequencia, bool atualizaComplexidadeBombeado, bool gravaContinuidadeProgramacao, string usuario)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            var programacao = _programacaoRepository.ObterDetalhadaPorId(idUsina, obraNumero, sequencia);

            var codUsina = (int)programacao.UsinaCodigo;
            var anoContrato = (int)programacao.ContratoAno;
            var numeroContrato = (int)programacao.ContratoNumero;

            var contrato = _contratoRepository.ObterPorId(codUsina, anoContrato, numeroContrato);

            if (contrato.ContratoIsEncerrado())
                return false;

            if (programacao.Status == EProgramacaoStatus.Cancelada)
            {
                AssertionConcern.Notify("Programacao.Status", "A programação esta cancelada.");
                return false;
            }

            var pendencia = "801";
            var dataAtraso = DateTime.Now.AddDays(ToleranciaAtraso());
            var tipoBomba = "0";

            sql.Clear();
            sql.Append("select cr.tp_doc,cr.Num_Doc,cr.Sal,cr.Dt_Oper,cr.dt_util_vencto Dt_Vcto,t.Descr from fin_car as cr");
            sql.Append(" left join ger_tp_doc as t on");
            sql.Append(" cr.tp_doc=t.cod");
            sql.Append($" where cr.Cli=@{nameof(pendencia)}");
            sql.Append($" and cr.dt_util_vencto<@{nameof(dataAtraso)}");
            sql.Append(" order by cr.dt_util_vencto");

            var duplicatas = cnn.QueryFirstOrDefault(sql.ToString(), new { pendencia, dataAtraso });

            if (duplicatas != null)
            {
                AssertionConcern.Notify("Duplicatas em Aberto", "Interveniente possui duplicatas em aberto.");
                return false;

            }

            if (!programacao.HorarioBombaScopeIsValid())
                return false;

            var ObrigaInformarBomba = _parametroRepository.ObterParametroN("TopCon", "ObrigaEquipProg");

            if (ObrigaInformarBomba.Equals("1") && programacao.EquipamentoBombaCodigo.Equals("0") && !programacao.EquipamentoBombaCodigo.Equals(""))
            {
                AssertionConcern.Notify("EquipamentoBomba", "É Obrigatório Informar o Equipamento Que Irá Fazer o Bombeado!");
                return false;
            }

            if (TracoNaoExisteNoContrato(programacao))
            {
                AssertionConcern.Notify("TracoContrato", "Traço não cadastrado no contrato!");
                return false;
            }

            if (BombaNaoExisteNoContrato(programacao))
            {
                AssertionConcern.Notify("EquipamentoBomba", "Bomba não cadastrada no contrato!");
                return false;
            }


            if (ViagensUltrapassamMeiaNoite(programacao))
            {
                AssertionConcern.Notify("Horario", "A Quantidade de Viagens Para o Intervalo e Volume Especificados Ultrapassa 23:59h! Deverá Ser Dividido Em Duas Programações!");
                return false;
            }

            if (ValidaStatusEquipamento(programacao))
                return false;

            if (ValidaEquipamentoBomba(programacao, out tipoBomba))
            {
                AssertionConcern.Notify("EquipamentoBomba", "Tipo de bomba do equipamento selecionado não suporta a pedra a ser pesada! Favor alterar o equipamento!");
                return false;
            }

            if (RetValorBombaTubulacao(programacao, tipoBomba))
                return false;

            if (ValidacaoObra(programacao))
            {
                AssertionConcern.Notify("Obra", "Obra Não Localizada!");
                return false;
            }

            if (!ApenasBombeamento(programacao))
            {
                if (ValidaTracoHomologadoN(programacao))
                    return false;
            }

            if (ValidarEmpresaUsinaObraContratoComUsinaEntregaProgramacao(programacao, contrato))
            {
                AssertionConcern.Notify("ValidarEmpresaUsinaObraContratoComUsinaEntregaProgramacao", "A Usina de Entrega da programação deve pertencer à mesma empresa da Usina Principal da obra vinculada ao contrato.");
                return false;
            }

            if (!programacao.TemNotaFicalEmitida)
                AtualizaHorarioProgramacao(contrato, programacao);

            if (ValidaIntervenienteBloqueado(contrato, obraNumero, usuario))
                return false;

            var AnalisaLimiteCreditoProducao = _parametroRepository.ObterParametroN("TopCon", "AnalizaLimCredProd");

            if (AnalisaLimiteCreditoProducao.Equals("1") && LimiteDeCreditoVencido(contrato.Interveniente))
            {
                AssertionConcern.Notify("LimiteCredito", "Limite de crédito vencido.");
                return false;
            }

            DeletaDemaisServicos(programacao);

            InsereDemaisServicos(programacao);

            TotalizarValoresProgramacao(programacao);

            AtualizaValoresContrato(contrato, programacao);

            if (atualizaComplexidadeBombeado) AtualizaComplexidadeBombeado(programacao);

            if (gravaContinuidadeProgramacao) AtualizaContinuidadeBombeamento(programacao);

            sql.Clear();
            sql.Append($"UPDATE con_programacao p");
            sql.Append($" SET status = 9162");
            sql.Append($" WHERE p.usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND p.ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND p.no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND p.seq_prog=@{nameof(programacao.Sequencia)}");
            if (programacao.PropostaAno > 0) sql.Append($" AND p.ano_chamada=@{nameof(programacao.PropostaAno)}");
            if (programacao.PropostaNumero > 0) sql.Append($" AND p.num_chamada=@{nameof(programacao.PropostaNumero)}");
            if (programacao.ObraNumero > 0) sql.Append($" AND p.no_obra=@{nameof(programacao.ObraNumero)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.PropostaAno,
                programacao.PropostaNumero,
                programacao.ObraNumero
            };

            cnn.Execute(sql.ToString(), parametroQuery);

            return true;
        }

        public int RetIntervaloCargas(int codUsina)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append("select ifnull(intervalo_carga,0) as intervaloCarga");
            sql.Append(" from con_usina");
            sql.Append($" where cod=@{nameof(codUsina)}");

            var result = cnn.QueryFirstOrDefault<int>(sql.ToString(), new { codUsina });

            return result;

        }

        public int ToleranciaAtraso()
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            var dataDia = DateTime.Now;

            sql.Clear();
            sql.Append($"select dias_atraso from fin_parametro where inicio_validade<=@{nameof(dataDia)}");
            sql.Append(" order by inicio_validade desc limit 1");

            var result = cnn.QueryFirstOrDefault<int>(sql.ToString(), new { dataDia });

            return result;

        }

        public bool TracoNaoExisteNoContrato(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Clear();
            sql.Append($"SELECT no_obra FROM con_proposta_item WHERE usina={programacao.UsinaCodigo}");
            sql.Append($" AND no_obra={programacao.ObraNumero}");
            sql.Append($" AND seq={programacao.ObraTracoSequencia}");
            sql.Append($" AND tp_resist={programacao.ResistenciaTipoCodigo}");
            sql.Append($" AND fck={programacao.Mpa}");
            sql.Append($" AND consumo={programacao.Consumo}");
            sql.Append($" AND uso={programacao.UsoCodigo}");
            sql.Append($" AND pedra={programacao.PedraCodigo}");
            sql.Append($" AND slump={programacao.SlumpCodigo}");

            var traco = cnn.QueryFirstOrDefault(sql.ToString());

            return (traco == null);

        }

        public bool BombaNaoExisteNoContrato(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (programacao.EquipamentoBombaCodigo.Equals("") || programacao.EquipamentoBombaCodigo.Equals("0"))
                return false;

            sql.Clear();
            sql.Append($"SELECT no_obra FROM con_prop_bomba WHERE usina={programacao.UsinaCodigo}");
            sql.Append($" AND no_obra={programacao.ObraNumero}");
            sql.Append($" AND seq={programacao.ObraBombaSequencia}");

            var bomba = cnn.QueryFirstOrDefault(sql.ToString());

            return (bomba == null);

        }

        public bool ViagensUltrapassamMeiaNoite(Programacao programacao)
        {

            var UltimoHorario = DateTime.Parse("23:59");
            var Horario = DateTime.Parse($"{programacao.Horario.Substring(0, 2)}:{programacao.Horario.Substring(2, 2)}");

            var volumeBloqueado = programacao.VolumeTotal;
            var volumeLiberado = programacao.VolumeTotal;
            var volumeBt = programacao.VolumePorCarga;

            if (programacao.VolumeLiberado > 0)
            {
                volumeLiberado = programacao.VolumeLiberado;
                volumeBloqueado -= programacao.VolumeLiberado;
            }
            else
            {
                volumeBloqueado -= programacao.VolumeTotal;
            }

            if (programacao.VolumePorCarga == 0) volumeBt = 8;

            var QuantidadeConcretagensLiberadas = ((10 * volumeLiberado) / (10 * volumeBt)) - PossuiResto((10 * volumeLiberado), (10 * volumeBt));
            var QuantidadeConcretagensBloqueadas = ((10 * volumeBloqueado) / (10 * volumeBt)) - PossuiResto((10 * volumeBloqueado), (10 * volumeBt));
            var QuantidadeConcretagensTotal = QuantidadeConcretagensLiberadas + QuantidadeConcretagensBloqueadas;

            var HorarioUltimaConcretagem = Horario.AddMinutes(QuantidadeConcretagensTotal * programacao.IntervaloEmMinutosEntreCargas);

            return (HorarioUltimaConcretagem > UltimoHorario);

        }

        public int PossuiResto(float valor1, float valor2)
        {
            if (valor1 % valor2 > 0)
            {
                return 1;
            }

            return 0;

        }

        public bool ValidaEquipamentoBomba(Programacao programacao, out string tipoBomba)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;
            tipoBomba = "0";

            if (programacao.EquipamentoBombaCodigo.Equals("") || programacao.EquipamentoBombaCodigo.Equals("0"))
                return false;

            sql.Append($"select e.cod,e.tipo,ifnull(tb.pedra,0) as pedra");
            sql.Append($" from con_equipamento as e");
            sql.Append($" left join con_tipo_bomba_pedra as tb");
            sql.Append($" on e.tipo=tb.tipo_bomba");
            sql.Append($" where e.cod='{programacao.EquipamentoBombaCodigo}'");
            sql.Append($" and (ifnull(tb.pedra,0)=0 or ifnull(tb.pedra,0)={programacao.TracoPesadoPedraCodigo}");
            sql.Append($" ) limit 1");

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString());

            if (result == null)
                return true;

            tipoBomba = result.tipo.ToString();

            return false;

        }

        public bool ValidaStatusEquipamento(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (programacao.EquipamentoBombaCodigo.Equals("") || programacao.EquipamentoBombaCodigo.Equals("0"))
                return false;

            sql.Append($"SELECT e.status,g.descr");
            sql.Append($" FROM con_equipamento e");
            sql.Append($" LEFT JOIN ger_geral g ON e.status=g.cod");
            sql.Append($" WHERE e.cod='{programacao.EquipamentoBombaCodigo}'");

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString());

            if (result == null)
                return false;

            if (result.status != 0 && result.status != 7201)
            {
                AssertionConcern.Notify("StatusEquipamento", $"Equipamento com status '{result.status} - {result.descr}', não pode ser utilizado!");
                return true;
            }

            return false;

        }

        public bool RetValorBombaTubulacao(Programacao programacao, string tipoBomba)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (programacao.DistanciaTubulacao <= 0)
                return false;

            sql.Append($"select dist_tub_de,permitido,vlr_adic_tub from con_preco_bomba_tub");
            sql.Append($" where usina={programacao.UsinaCodigo}");
            sql.Append($" and tipo_bomba={tipoBomba}");
            sql.Append($" and (dist_tub_de<={programacao.DistanciaTubulacao}");
            sql.Append($" and dist_tub_ate>={programacao.DistanciaTubulacao})");

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString());

            if (result == null)
                return false;

            if (result.permitido.Equals("N"))
            {
                AssertionConcern.Notify("RetValorBombaTubulacao", "Não permitido informar esta distância de tubulação para o tipo de equipamento selecionado!");
                return true;
            }

            var QuantidadeMetrosTubulacao = programacao.DistanciaTubulacao - result.dist_tub_de + 1;
            programacao.ValorAdicionalTubulacao = (result.vlr_adic_tub * QuantidadeMetrosTubulacao);

            return false;

        }

        public bool ValidacaoObra(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append("select numero,no_chamada,ano_chamada from con_obras");
            sql.Append($" where usina={programacao.UsinaCodigo}");
            sql.Append($" and ano_contrato={programacao.ContratoAno}");
            sql.Append($" and no_contrato={programacao.ContratoNumero}");

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString());

            return result == null;

        }

        public bool ApenasBombeamento(Programacao programacao)
        {

            if (programacao.ObraTracoSequencia == 0)
                return true;

            return false;

        }

        public bool ValidaTracoHomologadoN(Programacao programacao)
        {

            var statusTracoHomologado = 7101;

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"select max(dt_inic_valid) as dt_inic_valid from con_formula_concreto");
            sql.Append($" where dt_inic_valid<='{DateTime.Now.ToString("yyyy-MM-dd")}'");
            sql.Append($" and usina={programacao.UsinaEntregaCodigo}");
            sql.Append($" and uso={programacao.UsoCodigo}");
            sql.Append($" and pedra={programacao.PedraCodigo}");
            sql.Append($" and slump={programacao.SlumpCodigo}");
            sql.Append($" and tp_resist={programacao.ResistenciaTipoCodigo}");
            sql.Append($" and fck={programacao.Mpa}");
            sql.Append($" and consumo={programacao.Consumo}");

            var dateInicioValidade = cnn.QueryFirstOrDefault<DateTime>(sql.ToString());

            if (dateInicioValidade == null)
            {
                AssertionConcern.Notify("ValidaTracoHomologado", "Traço a ser pesado não localizado! Favor entrar em contato com a Engenharia!");
                return true;
            }

            sql.Clear();
            sql.Append($"select d.status from con_formula_concreto as f");
            sql.Append($" left join con_def_familiar as d");
            sql.Append($" on f.especificacao=d.especificacao");
            sql.Append($" where f.dt_inic_valid='{dateInicioValidade.ToString("yyyy-MM-dd")}'");
            sql.Append($" and f.usina={programacao.UsinaEntregaCodigo}");
            sql.Append($" and f.uso={programacao.UsoCodigo}");
            sql.Append($" and f.pedra={programacao.PedraCodigo}");
            sql.Append($" and f.slump={programacao.SlumpCodigo}");
            sql.Append($" and f.tp_resist={programacao.ResistenciaTipoCodigo}");
            sql.Append($" and f.fck={programacao.Mpa}");
            sql.Append($" and f.consumo={programacao.Consumo}");
            sql.Append($" and d.status={statusTracoHomologado}");
            sql.Append($" group by f.usina,f.uso,f.pedra,f.slump,f.fck,f.consumo");

            var statusTraco = cnn.QueryFirstOrDefault<int>(sql.ToString());

            if (statusTraco != statusTracoHomologado)
            {
                AssertionConcern.Notify("ValidaTracoHomologado", "Traço a ser pesado não está homologado! Favor entrar em contato com a Engenharia!");
                return true;
            }

            return false;

        }

        public bool ValidarEmpresaUsinaObraContratoComUsinaEntregaProgramacao(Programacao programacao, Contrato contrato)
        {
            var EmpresaUsinaObraContratoDeveSerIgualAEmpresaUsinaEntregaProgramacao = _parametroRepository.ObterParametroN("TopCon", "EmpresaUsinaObraContratoDeveSerIgualAEmpresaUsinaEntregaProgramacao");

            if (!EmpresaUsinaObraContratoDeveSerIgualAEmpresaUsinaEntregaProgramacao.Equals("1"))
                return false;

            var usinaEntrega = programacao.UsinaEntregaCodigo;
            var usinaObraContrato = contrato.UsinaEntrega;

            return !(usinaEntrega == usinaObraContrato);

        }

        public void AtualizaHorarioProgramacao(Contrato contrato, Programacao programacao)
        {

            var ProgramacaoHoraStatusBloqueado = "B";
            var ProgramacaoHoraStatusLiberado = "L";

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            var filtroUsina = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia
            };

            sql.Append($"DELETE FROM con_programacao_hora");
            sql.Append($" WHERE usina<>0");
            sql.Append($" AND usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");

            cnn.Execute(sql.ToString(), filtroUsina);

            sql.Clear();
            sql.Append($"DELETE FROM con_programacao_log");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");

            cnn.Execute(sql.ToString(), filtroUsina);

            var Horario = DateTime.Parse($"{programacao.Horario.Substring(0, 2)}:{programacao.Horario.Substring(2, 2)}");

            var volumeBloqueado = programacao.VolumeTotal;
            var volumeLiberado = programacao.VolumeTotal;
            var volumeBt = programacao.VolumePorCarga;

            if (programacao.VolumeLiberado > 0)
            {
                volumeLiberado = programacao.VolumeLiberado;
                volumeBloqueado -= programacao.VolumeLiberado;
            }
            else
            {
                volumeBloqueado -= programacao.VolumeTotal;
            }

            if (programacao.VolumePorCarga == 0) volumeBt = 8;

            var QuantidadeConcretagensLiberadas = ((10 * volumeLiberado) / (10 * volumeBt)) - PossuiResto((10 * volumeLiberado), (10 * volumeBt));
            var QuantidadeConcretagensBloqueadas = ((10 * volumeBloqueado) / (10 * volumeBt)) - PossuiResto((10 * volumeBloqueado), (10 * volumeBt));
            var QuantidadeConcretagensTotal = QuantidadeConcretagensLiberadas + QuantidadeConcretagensBloqueadas;

            var horarioUltimaConcretagem = Horario.AddMinutes((QuantidadeConcretagensTotal - 1) * programacao.IntervaloEmMinutosEntreCargas);

            var horarioConcretagem = Horario;

            var statusAuxiliar = programacao.NecessitaConfirmacao.Equals("S") ? ProgramacaoHoraStatusBloqueado : ProgramacaoHoraStatusLiberado;

            float volumeAuxiliarTotal = 0;
            float volumeAuxiliarLiberadoTotal = 0;
            var somaIntervalos = 0;

            while (horarioConcretagem <= horarioUltimaConcretagem)
            {

                var volumeAuxiliar = volumeBt;

                volumeAuxiliarLiberadoTotal = volumeAuxiliarLiberadoTotal > programacao.VolumeLiberado ? programacao.VolumeLiberado : (volumeAuxiliarTotal + volumeBt);

                if (horarioConcretagem == horarioUltimaConcretagem)
                    volumeAuxiliar = programacao.VolumeTotal - volumeAuxiliarTotal;

                volumeAuxiliarTotal += volumeAuxiliar;

                if (programacao.VolumeLiberado > 0)
                    statusAuxiliar = volumeAuxiliarTotal > programacao.VolumeLiberado ? ProgramacaoHoraStatusBloqueado : ProgramacaoHoraStatusLiberado;

                sql.Clear();


                sql.Append($"INSERT INTO con_programacao_hora");
                sql.Append($" SET usina=@{nameof(programacao.UsinaCodigo)}");
                sql.Append($" ,ano_contrato=@{nameof(programacao.ContratoAno)}");
                sql.Append($" ,no_contrato=@{nameof(programacao.ContratoNumero)}");
                sql.Append($" ,seq_prog=@{nameof(programacao.Sequencia)}");
                sql.Append($" ,horario='{horarioConcretagem.ToString("HHmm")}'");
                sql.Append($" ,volume_prog={volumeAuxiliar.ToString().Replace(",", ".")}");
                sql.Append($" ,status='{statusAuxiliar}'");
                sql.Append($" ,filial=0");
                sql.Append($" ,interv=0");
                sql.Append($" ,tp_doc=0");
                sql.Append($" ,num_nf=0");
                sql.Append($" ,serie=''");
                sql.Append($" ,seq_nf=0");
                sql.Append($" ,volume_entregue=0");

                var corpoDeProvaQuantidade = programacao.CorpoDeProvaQuantidade;

                if (programacao.CorpoDeProvaIntervalo > 0)
                    corpoDeProvaQuantidade = (((somaIntervalos / programacao.IntervaloEmMinutosEntreCargas) % programacao.CorpoDeProvaIntervalo) == 0 || horarioConcretagem == Horario ? programacao.CorpoDeProvaQuantidade : 0);

                sql.Append($" ,qtde_cp={corpoDeProvaQuantidade}");

                var filtro = new
                {
                    programacao.UsinaCodigo,
                    programacao.ContratoAno,
                    programacao.ContratoNumero,
                    programacao.Sequencia,
                };

                cnn.Execute(sql.ToString(), filtro);

                somaIntervalos += programacao.IntervaloEmMinutosEntreCargas;

                horarioConcretagem = horarioConcretagem.AddMinutes(programacao.IntervaloEmMinutosEntreCargas);


            }


        }

        public bool ValidaIntervenienteBloqueado(Contrato contrato, int obraNumero, string usuario)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            var limiteCreditoPorGrupoEconomico = _parametroRepository.ObterParametroN("web", "LimiteCreditoPorGrupoEconomico") == "1";

            var interveniente = contrato.Interveniente;
            var idAtual = $"{usuario.ToUpper()} {DateTime.Now.ToString("dd/MM/yy")}";

            if (interveniente.GrupoEconomico != null && limiteCreditoPorGrupoEconomico)
            {
                if (interveniente.GrupoEconomico.BloqueioMotivoCodigo == null || interveniente.GrupoEconomico.BloqueioMotivoCodigo == 0)
                    return false;
            }
            else
            {
                if (interveniente.BloqueioMotivoCodigo == null || interveniente.BloqueioMotivoCodigo == 0)
                    return false;
            }

            var contratosInterveniente = _contratoRepository.ListarFiltrados(x => x.IntervenienteCodigo == interveniente.Codigo && x.Status != EContratoStatus.RevalidacaoCadastro);

            foreach (var contratoLocalizado in contratosInterveniente)
            {

                var novostatus = contratoLocalizado.Status;

                if (contratoLocalizado.Status == EContratoStatus.Aprovado && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                {

                    novostatus = EContratoStatus.RevalidacaoCadastro;

                    var complemento = $"DE {Enum.GetName(typeof(EContratoStatus), contratoLocalizado.Status).ToUpper()} PARA RE-VALIDAÇÃO DE CADASTRO";

                    InserirObraLog(contrato.Usina, obraNumero, usuario, ObterEventoConObra(EContratoStatus.AguardandoDadosPagamento), complemento, "CLIENTE BLOQUEADO");

                }
                else if (contratoLocalizado.Status == EContratoStatus.PreAnalise || contratoLocalizado.Status == EContratoStatus.EmAnalise)
                {

                    InserirObraLog(contrato.Usina, obraNumero, usuario, "ACOMPANHAMENTO", "PROGRAMAÇÃO", "CLIENTE BLOQUEADO");

                }
                else
                {

                    novostatus = EContratoStatus.EmAnalise;

                    var complemento = $"DE {Enum.GetName(typeof(EContratoStatus), contratoLocalizado.Status).ToUpper()} PARA EM ANALISE";

                    InserirObraLog(contrato.Usina, obraNumero, usuario, ObterEventoConObra(EContratoStatus.AguardandoDadosPagamento), complemento, "CLIENTE BLOQUEADO");

                }


                if (novostatus != contratoLocalizado.Status)
                {

                    sql.Clear();

                    sql.Append($"UPDATE con_contrato AS ctr");
                    sql.Append($" INNER JOIN con_programacao AS prog");
                    sql.Append($" ON ctr.usina=prog.usina");
                    sql.Append($" AND ctr.ano_contrato=prog.ano_contrato");
                    sql.Append($" AND ctr.num_contrato=prog.no_contrato");
                    sql.Append($" AND prog.dt_concretagem >= curdate()");
                    sql.Append($" SET ctr.status=@{nameof(novostatus)}");
                    sql.Append($" , ctr.id_atual=@{nameof(idAtual)}");

                    if (novostatus == EContratoStatus.RevalidacaoCadastro)
                    {
                        sql.Append($" , ctr.cad_aprovado=''");
                        sql.Append($" , ctr.id_aprov_cad=''");
                    }

                    sql.Append($", ctr.id_aprov_dir=''");
                    sql.Append($", ctr.fechado=''");
                    sql.Append($" WHERE ctr.usina=@{nameof(contratoLocalizado.Usina)}");
                    sql.Append($" AND ctr.ano_contrato=@{nameof(contratoLocalizado.Ano)}");
                    sql.Append($" AND ctr.num_contrato=@{nameof(contratoLocalizado.Numero)}");

                    var parametros = new
                    {
                        novostatus,
                        idAtual,
                        contratoLocalizado.Usina,
                        contratoLocalizado.Ano,
                        contratoLocalizado.Numero
                    };

                    cnn.Execute(sql.ToString(), parametros);


                }


            }

            if (interveniente.GrupoEconomico != null && limiteCreditoPorGrupoEconomico)
                AssertionConcern.Notify("Interveniente", $"Interveniente em um Grupo Econômico Bloqueado!{Environment.NewLine}Motivo: {interveniente.GrupoEconomico.BloqueioMotivoCodigo} - {interveniente.GrupoEconomico.BloqueioMotivo}{Environment.NewLine}Observação: {interveniente.GrupoEconomico.BloqueioObservacao}");
            else
                AssertionConcern.Notify("Interveniente", $"Interveniente Bloqueado!{Environment.NewLine}Motivo: {interveniente.BloqueioMotivoCodigo} - {interveniente.BloqueioMotivo}{Environment.NewLine}Observação: {interveniente.BloqueioObservacao}");
            
            return true;
        }

        public void InserirObraLog(int usina, int obraNumero, string usuario, string evento, string complementoLog, string observacao)
        {
            var dataHora = DateTime.Now;

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (cnn.State == ConnectionState.Closed)
                cnn.Open();

            var obra = _obraRepository.ObterObraParaAnaliseCadastro(usina, obraNumero);

            sql.Clear();
            sql.Append($"SELECT IFNULL(MAX(seq_log),0) + 1");
            sql.Append($" FROM con_obras_log");
            sql.Append($" WHERE usina=@{nameof(usina)}");
            sql.Append($" AND obra=@{nameof(obraNumero)}");
            sql.Append($" AND dt_hora_evento=@{nameof(dataHora)}");
            sql.Append($" AND usuario=@{nameof(usuario)}");
            sql.Append($" AND evento=@{nameof(evento)}");
            sql.Append($" AND ano_chamada=@{nameof(obra.AnoChamada)}");
            sql.Append($" AND no_chamada=@{nameof(obra.NumChamada)}");

            var parametros = new { usina, obraNumero, dataHora, usuario, evento, obra.AnoChamada, obra.NumChamada };
            var obraLogProximaSequencia = cnn.QueryFirstOrDefault<int>(sql.ToString(), parametros);


            sql.Clear();
            sql.Append($"INSERT INTO con_obras_log");
            sql.Append($" SET usina=@{nameof(usina)}");
            sql.Append($", obra=@{nameof(obraNumero)}");
            sql.Append($", dt_hora_evento=@{nameof(dataHora)}");
            sql.Append($", usuario=@{nameof(usuario)}");
            sql.Append($", evento=@{nameof(evento)}");
            sql.Append($", complemento=@{nameof(complementoLog)}");
            sql.Append($", obs=@{nameof(observacao)}");
            sql.Append($", envia_email='N'");
            sql.Append($", email_enviado='N'");
            sql.Append($", dt_hora_email=''");
            sql.Append($", ano_Chamada=@{nameof(obra.AnoChamada)}");
            sql.Append($", no_chamada=@{nameof(obra.NumChamada)}");
            sql.Append($", seq_log=@{nameof(obraLogProximaSequencia)}");

            var parametro = new
            {
                usina,
                obraNumero,
                dataHora,
                usuario,
                evento,
                complementoLog,
                observacao,
                obra.AnoChamada,
                obra.NumChamada,
                obraLogProximaSequencia
            };

            cnn.Execute(sql.ToString(), parametro);
            cnn.GravarLogGeral(usuario, "con_obras_log", sql.ToString(), parametros);

        }

        public void InserirObraLog(int numVersao, int usina, int obraNumero, string usuario, string evento, string complementoLog, string observacao)
        {
            var dataHora = DateTime.Now;

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            if (cnn.State == ConnectionState.Closed)
                cnn.Open();

            var obra = _obraRepository.ObterObraParaAnaliseCadastro(numVersao, usina, obraNumero);

            sql.Clear();
            sql.Append($"SELECT IFNULL(MAX(seq_log),0) + 1");
            sql.Append($" FROM con_obras_log_versao");
            sql.Append($" WHERE usina=@{nameof(usina)}");
            sql.Append($" AND obra=@{nameof(obraNumero)}");
            sql.Append($" AND dt_hora_evento=@{nameof(dataHora)}");
            sql.Append($" AND usuario=@{nameof(usuario)}");
            sql.Append($" AND evento=@{nameof(evento)}");
            sql.Append($" AND ano_chamada=@{nameof(obra.AnoChamada)}");
            sql.Append($" AND no_chamada=@{nameof(obra.NumChamada)}");
            sql.Append($" AND num_versao=@{nameof(obra.NumeroVersao)}");

            var parametros = new { usina, obraNumero, dataHora, usuario, evento, obra.AnoChamada, obra.NumChamada, obra.NumeroVersao };
            var obraLogProximaSequencia = cnn.QueryFirstOrDefault<int>(sql.ToString(), parametros);


            sql.Clear();
            sql.Append($"INSERT INTO con_obras_log_versao");
            sql.Append($" SET usina=@{nameof(usina)}");
            sql.Append($", obra=@{nameof(obraNumero)}");
            sql.Append($", dt_hora_evento=@{nameof(dataHora)}");
            sql.Append($", usuario=@{nameof(usuario)}");
            sql.Append($", evento=@{nameof(evento)}");
            sql.Append($", complemento=@{nameof(complementoLog)}");
            sql.Append($", obs=@{nameof(observacao)}");
            sql.Append($", envia_email='N'");
            sql.Append($", email_enviado='N'");
            sql.Append($", dt_hora_email=''");
            sql.Append($", ano_Chamada=@{nameof(obra.AnoChamada)}");
            sql.Append($", no_chamada=@{nameof(obra.NumChamada)}");
            sql.Append($", seq_log=@{nameof(obraLogProximaSequencia)}");
            sql.Append($", num_versao=@{nameof(obra.NumeroVersao)}");

            var parametro = new
            {
                usina,
                obraNumero,
                dataHora,
                usuario,
                evento,
                complementoLog,
                observacao,
                obra.AnoChamada,
                obra.NumChamada,
                obraLogProximaSequencia,
                obra.NumeroVersao
            };

            cnn.Execute(sql.ToString(), parametro);
            cnn.GravarLogGeral(usuario, "con_obras_log_versao", sql.ToString(), parametros);

        }

        private string ObterEventoConObra(EContratoStatus status)
        {

            if (status == EContratoStatus.PreAnalise)
                return "CONTRATO GERADO";

            if (status == EContratoStatus.Reprovado)
                return "CONTRATO REPROVADO";

            if (status == EContratoStatus.Aprovado)
                return "CONTRATO APROVADO";

            if (status == EContratoStatus.Cancelado)
                return "CONTRATO CANCELADO";

            if (status == EContratoStatus.EmAnalise)
                return "EM ANÁLISE";

            if (status == EContratoStatus.AguardandoDadosPagamento)
                return "ALTERAÇÃO STATUS";

            if (status == EContratoStatus.AguardandoAprovacaoComercial)
                return "AG. APROVAÇÃO COMERCIAL";

            return "";
        }

        private string ObterEventoConObra(EPropostaStatus status)
        {

            if (status == EPropostaStatus.AguardandoAprovacaoComercial)
                return "ANÁLISE COMERCIAL";

            if (status == EPropostaStatus.ReprovadaComercialmente)
                return "REPROVADA CML";

            return "";
        }

        private string ObterEventoConObra(int status)
        {

            if (status == 9142)
                return "ALTERAÇÃO ANALISTA";

            if (status == 9143)
                return "ACOMPANHAMENTO";

            return "";
        }

        public bool TemComplexidadeBombeado(Programacao programacao)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"select if(b.txa_min_pr_prop>b.taxa_minima_tab,'S','N') as temComplexBomb");
            sql.Append($" from con_programacao as p");
            sql.Append($" inner join con_prop_bomba as  b");
            sql.Append($" on b.usina=p.usina");
            sql.Append($" and b.no_obra=p.no_obra");
            sql.Append($" and b.seq=p.tipo_bomba");
            sql.Append($" where p.usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" and p.ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" and p.no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" and p.seq_prog=@{nameof(programacao.Sequencia)}");
            sql.Append($" and p.no_obra=@{nameof(programacao.ObraNumero)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.ObraNumero
            };

            var result = cnn.QueryFirstOrDefault<String>(sql.ToString(), parametroQuery);

            if (result == null)
                return false;

            return (result.Equals("S"));

        }

        private bool AtualizaComplexidadeBombeado(Programacao programacao)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"UPDATE con_prop_bomba AS b,");
            sql.Append($" (SELECT b.usina,b.no_obra,b.seq,b.ano_chamada,b.no_chamada FROM con_programacao AS p");
            sql.Append($" INNER JOIN con_prop_bomba AS b");
            sql.Append($" ON b.usina=p.usina");
            sql.Append($" AND b.no_obra=p.no_obra");
            sql.Append($" AND b.seq=p.tipo_bomba");
            sql.Append($" WHERE p.usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND p.ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND p.no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND p.seq_prog=@{nameof(programacao.Sequencia)}");
            if (programacao.PropostaAno > 0) sql.Append($" AND p.ano_chamada=@{nameof(programacao.PropostaAno)}");
            if (programacao.PropostaNumero > 0) sql.Append($" AND p.num_chamada=@{nameof(programacao.PropostaNumero)}");
            if (programacao.ObraNumero > 0) sql.Append($" AND p.no_obra=@{nameof(programacao.ObraNumero)}");
            sql.Append($") AS p set b.complex_bomb = 'S'");
            sql.Append($" WHERE b.usina=p.usina");
            sql.Append($" AND b.no_obra=p.no_obra");
            sql.Append($" AND b.seq=p.seq");
            sql.Append($" AND b.ano_chamada=p.ano_chamada");
            sql.Append($" AND b.no_chamada=p.no_chamada");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.PropostaAno,
                programacao.PropostaNumero,
                programacao.ObraNumero
            };

            cnn.Execute(sql.ToString(), parametroQuery);

            return true;
        }

        public string VerificaContinuidade(Programacao programacao)
        {
            var continuaSeq = ContinuidadeDaSequencia(programacao);
            var continuidadeAnterior = ContinuidadeVinculada(programacao);

            if (continuidadeAnterior == 0 && continuaSeq > 0)
            {
                return GravaContinuidadeProgramacao(programacao, continuaSeq);
            }

            return null;
        }

        public string GravaContinuidadeProgramacao(Programacao programacao, int continuaSeq)
        {
            var sql = new StringBuilder();
            var mensagem = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"SELECT seq_prog, dt_concretagem, tipo_bomba, cod_equip_bomba, horario");
            sql.Append($" FROM con_programacao");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(continuaSeq)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                continuaSeq
            };

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), parametroQuery);

            if (result == null || result == "")
                return null;

            mensagem.Append($"\nContrato: {programacao.UsinaCodigo}-{programacao.ContratoNumero}/{programacao.ContratoAno}");
            mensagem.Append($" / Seq.Prog.: {result.seq_prog}");
            mensagem.Append($"\nData Concretagem: {result.dt_concretagem}");
            mensagem.Append($"\nHorário: {result.horario.ToString("00:00")}");
            mensagem.Append($"\nBomba selecionada: {result.tipo_bomba}ª / Equipamento: {result.cod_equip_bomba}\n");

            return mensagem.ToString();
        }

        private bool AtualizaContinuidadeBombeamento(Programacao programacao)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;
            var continuaSeq = ContinuidadeDaSequencia(programacao);

            sql.Append($"SELECT seq_prog, dt_concretagem, tipo_bomba, cod_equip_bomba, horario");
            sql.Append($" FROM con_programacao");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(continuaSeq)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                continuaSeq
            };

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), parametroQuery);

            sql.Clear();

            sql.Append($"UPDATE con_programacao");
            sql.Append($" SET continua_seq={continuaSeq}");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");

            cnn.Execute(sql.ToString(), parametroQuery);

            return true;
        }

        public int ContinuidadeDaSequencia(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            var horarioProgramacao = DateTime.Parse($"{programacao.Horario.Substring(0, 2)}:{programacao.Horario.Substring(2, 2)}");

            sql.Append($"SELECT hr_saida_abaixo FROM con_usina WHERE cod=@{nameof(programacao.UsinaEntregaCodigo)}");

            var parametroQuery = new
            {
                programacao.UsinaEntregaCodigo,
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.Status,
                programacao.EquipamentoBombaCodigo
            };

            var minutosHorarioAbaixo = cnn.QueryFirstOrDefault<int>(sql.ToString());
            var horarioSaidaAbaixo = DateTime.Parse("00:00").AddMinutes(minutosHorarioAbaixo);

            sql.Clear();

            sql.Append($"SELECT seq_prog FROM con_programacao");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog<@{nameof(programacao.Sequencia)}"); ;

            if (horarioProgramacao < horarioSaidaAbaixo)
            {
                sql.Append($" AND (dt_concretagem='{programacao.DataConcretagem.ToString("yyyy-MM-dd")}'");
                sql.Append($" OR dt_concretagem='{programacao.DataConcretagem.ToString("yyyy-MM-dd")}')");
            }
            else
            {
                sql.Append($" AND dt_concretagem='{programacao.DataConcretagem.ToString("yyyy-MM-dd")}'");
            }

            sql.Append($" AND status=@{nameof(programacao.Status)}");
            sql.Append($" AND cod_equip_bomba=@{nameof(programacao.EquipamentoBombaCodigo)}");
            sql.Append($" ORDER BY seq_prog");

            var result = cnn.QueryFirstOrDefault<int>(sql.ToString());

            return result;

        }

        public int ContinuidadeVinculada(Programacao programacao)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"SELECT seq_prog FROM con_programacao");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($" AND seq_prog<@{nameof(programacao.Sequencia)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia
            };

            var result = cnn.QueryFirstOrDefault<int>(sql.ToString(), parametroQuery);

            return result;
        }

        private void DeletaDemaisServicos(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"DELETE FROM con_programacao_dem_serv");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND obra=@{nameof(programacao.ObraNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ObraNumero,
                programacao.Sequencia
            };

            cnn.Execute(sql.ToString(), parametroQuery);
        }

        private void InsereDemaisServicos(Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"SELECT ds.seq SequenciaServico, ds.merc Mercadoria, ds.quantidade Quantidade, ds.Frequencia_Cobranca FrequenciaCobranca, ds.Casas_decimais CasasDecimais, m.descr Descricao");
            sql.Append($" FROM con_obras_dem_serv ds");
            sql.Append($" LEFT JOIN fis_mercadoria m");
            sql.Append($" ON ds.merc = m.cod");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND obra=@{nameof(programacao.ObraNumero)}");
            sql.Append($" AND usina_entrega=@{nameof(programacao.UsinaEntregaCodigo)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ObraNumero,
                programacao.UsinaEntregaCodigo
            };

            var results = cnn.Query<dynamic>(sql.ToString(), parametroQuery);

            if (results == null)
                return;

            foreach (var demaisServico in results)
            {

                sql.Clear();

                sql.Append($"INSERT INTO con_programacao_dem_serv");
                sql.Append($" SET usina=@{nameof(programacao.Usina)}");
                sql.Append($" ,obra=@{nameof(programacao.ObraNumero)}");
                sql.Append($" ,seq_prog=@{nameof(programacao.Sequencia)}");
                sql.Append($" ,seq=@{nameof(demaisServico.SequenciaServico)}");
                sql.Append($" ,Quantidade=@{nameof(demaisServico.Quantidade)}");

                var parametroUpdate = new
                {
                    programacao.Usina,
                    programacao.ObraNumero,
                    programacao.Sequencia,
                    demaisServico.SequenciaServico,
                    demaisServico.Quantidade
                };

                cnn.Execute(sql.ToString(), parametroUpdate);

            }

        }

        private void AtualizaValoresContrato(Contrato contrato, Programacao programacao)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"SELECT c.usina,c.ano_contrato,c.num_contrato,c.total_m3 m3_ctr,c.vlr_extras vlr_extras_ctr,c.vlr_total_ctr");
            sql.Append($" ,IFNULL(SUM(p.qtde_m3),0) m3_prog,IFNULL((SUM(p.vlr_total_vib)+SUM(p.vlr_extras)),0) vlr_extras_prog");
            sql.Append($" FROM con_contrato c");
            sql.Append($" LEFT JOIN con_programacao p ON c.usina=p.usina");
            sql.Append($" AND c.ano_contrato=p.ano_contrato");
            sql.Append($" AND c.num_contrato=p.no_contrato");
            sql.Append($" AND p.cancelada<>'S'");
            sql.Append($" WHERE c.usina=@{nameof(contrato.Usina)}");
            sql.Append($" AND c.ano_contrato=@{nameof(contrato.Ano)}");
            sql.Append($" AND c.num_contrato=@{nameof(contrato.Numero)}");
            sql.Append($" GROUP BY c.usina,c.ano_contrato,c.num_contrato");


            var parametroQuery = new
            {
                contrato.Usina,
                contrato.Ano,
                contrato.Numero
            };

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), parametroQuery);

            if (result == null)
                return;

            double diferenca = result.vlr_extras_prog - result.vlr_extras_ctr;

            if (diferenca == 0)
                return;

            sql.Clear();

            sql.Append($"UPDATE con_contrato");
            sql.Append($" SET vlr_extras=vlr_extras+(@{nameof(diferenca)})");
            sql.Append($" ,vlr_total_ctr=vlr_total_ctr+(@{nameof(diferenca)})");
            sql.Append($" WHERE usina=@{nameof(contrato.Usina)}");
            sql.Append($" AND ano_contrato=@{nameof(contrato.Ano)}");
            sql.Append($" AND num_contrato=@{nameof(contrato.Numero)}");

            var parametroUpdate = new
            {
                diferenca,
                contrato.Usina,
                contrato.Ano,
                contrato.Numero,
            };

            cnn.Execute(sql.ToString(), parametroUpdate);

            sql.Clear();

            sql.Append($"SELECT a.usina, a.ano_contrato, a.num_contrato, a.seq FROM (");
            sql.Append($" SELECT usina, ano_contrato, num_contrato, seq, valor_fixo FROM con_contrato_pag WHERE usina=@{nameof(contrato.Usina)}");
            sql.Append($" AND ano_contrato=@{nameof(contrato.Ano)}");
            sql.Append($" AND num_contrato=@{nameof(contrato.Numero)} ");
            sql.Append($" AND id_aprovacao=''");
            sql.Append($" AND forma<>'BE'");
            sql.Append($" AND ativo='S'");
            sql.Append($" UNION");
            sql.Append($" SELECT pag.usina, pag.ano_contrato, pag.num_contrato, pag.seq, pag.valor_fixo FROM con_contrato_pag AS pag");
            sql.Append($" LEFT JOIN con_contrato_boleto AS boleto");
            sql.Append($" ON boleto.usina=pag.usina AND boleto.ano_contrato=pag.ano_contrato AND boleto.num_contrato=pag.num_contrato");
            sql.Append($" AND boleto.seq_pgto=pag.seq");
            sql.Append($" WHERE pag.usina=@{nameof(contrato.Usina)}");
            sql.Append($" AND pag.ano_contrato=@{nameof(contrato.Ano)}");
            sql.Append($" AND pag.num_contrato=@{nameof(contrato.Numero)}");
            sql.Append($" AND ISNULL(boleto.dt_hr_imp)");
            sql.Append($" AND pag.id_aprovacao=''");
            sql.Append($" AND pag.ativo='S') AS a");
            sql.Append($" ORDER BY a.valor_fixo, a.seq");

            result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), parametroQuery);

            if (result == null)
                return;

            sql.Clear();

            sql.Append($"UPDATE con_contrato_pag SET valor=valor+(@{nameof(diferenca)})");
            sql.Append($" WHERE usina=@{nameof(result.usina)}");
            sql.Append($" AND ano_contrato=@{nameof(result.ano_contrato)}");
            sql.Append($" AND num_contrato=@{nameof(result.num_contrato)}");
            sql.Append($" AND seq=@{nameof(result.seq)}");

            var parametroUpdateContrato = new
            {
                diferenca,
                result.usina,
                result.ano_contrato,
                result.num_contrato,
                result.seq
            };

            cnn.Execute(sql.ToString(), parametroUpdateContrato);

        }

        public bool LimiteDeCreditoVencido(Interveniente interveniente)
        {

            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"select limite_cred,lim_cred_val from ger_interv where Cod=@{nameof(interveniente.Codigo)}");

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), new { interveniente.Codigo });

            if (result == null)
                return false;

            if (result.limite_cred == 0)
                return false;

            return (result.lim_cred_val < DateTime.Now);

        }

        public bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, string observacao, string usuario)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            var programacao = _programacaoRepository.ObterDetalhadaPorId(idUsina, obraNumero, sequencia);

            var codUsina = (int)programacao.UsinaCodigo;
            var anoContrato = (int)programacao.ContratoAno;
            var numeroContrato = (int)programacao.ContratoNumero;

            var contrato = _contratoRepository.ObterPorId(codUsina, anoContrato, numeroContrato);

            if (contrato == null)
            {
                AssertionConcern.Notify("Programacao.Status", "Contrato não existente!");
                return false;
            }

            if (ValidaSolicitacaoCancelamento(programacao))
            {
                AssertionConcern.Notify("Programacao.Status", "Vendedor Solicitou Cancelamento. Só Será Permitido Cancelar a Programação!");
                return false;
            }

            if (contrato.ContratoIsEncerrado())
            {
                AssertionConcern.Notify("Programacao.Status", "O Contrato esta encerrado.");
                return false;
            }

            if (programacao.Status == EProgramacaoStatus.Cancelada)
            {
                AssertionConcern.Notify("Programacao.Status", "A programação esta cancelada.");
                return false;
            }

            sql.Append($"UPDATE con_programacao p");
            sql.Append($" LEFT JOIN con_obras o ON p.usina=o.usina AND p.ano_chamada=o.ano_chamada");
            sql.Append($" AND p.num_chamada = o.no_chamada AND p.no_obra = o.numero");
            sql.Append($" SET p.ano_contrato=@{nameof(programacao.ContratoAno)}");
            sql.Append($",p.no_contrato=@{nameof(programacao.ContratoNumero)}");
            sql.Append($",p.status= 9163");
            sql.Append($",p.obra_cep=o.obra_cep");
            sql.Append($",p.obra_end=o.obra_end");
            sql.Append($",p.obra_no=o.obra_num");
            sql.Append($",p.obra_compl=o.obra_compl");
            sql.Append($",p.obra_bairro=o.obra_bairro");
            sql.Append($",p.obra_mun=o.obra_mun");
            sql.Append($" WHERE p.usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND p.ano_chamada=@{nameof(programacao.PropostaAno)}");
            sql.Append($" AND p.num_chamada=@{nameof(programacao.PropostaNumero)}");
            sql.Append($" AND p.seq_prog=@{nameof(programacao.Sequencia)}");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.ContratoAno,
                programacao.ContratoNumero,
                programacao.Sequencia,
                programacao.PropostaAno,
                programacao.PropostaNumero
            };

            cnn.Execute(sql.ToString(), parametroQuery);

            return true;
        }

        private bool ValidaSolicitacaoCancelamento(Programacao programacao)
        {
            var sql = new StringBuilder();
            var cnn = _context.Connection;

            sql.Append($"SELECT evento,complemento FROM con_programacao_log");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_chamada=@{nameof(programacao.PropostaAno)}");
            sql.Append($" AND num_chamada=@{nameof(programacao.PropostaNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");
            sql.Append($" AND horario=''");
            sql.Append($" ORDER BY data_hora DESC LIMIT 1");

            var parametroQuery = new
            {
                programacao.UsinaCodigo,
                programacao.Sequencia,
                programacao.PropostaAno,
                programacao.PropostaNumero
            };

            var result = cnn.QueryFirstOrDefault<dynamic>(sql.ToString(), parametroQuery);

            if (result == null)
                return false;

            sql.Clear();

            sql.Append($"UPDATE con_programacao_log");
            sql.Append($" SET evento='ALTERAÇÃO STATUS'");
            sql.Append($",complemento= 'CANCELADA'");
            sql.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sql.Append($" AND ano_chamada=@{nameof(programacao.PropostaAno)}");
            sql.Append($" AND num_chamada=@{nameof(programacao.PropostaNumero)}");
            sql.Append($" AND seq_prog=@{nameof(programacao.Sequencia)}");
            sql.Append($" AND horario=''");
            sql.Append($" ORDER BY data_hora DESC LIMIT 1");

            cnn.Execute(sql.ToString(), parametroQuery);

            return true;
        }
        
        //private void AtualizaLogLimiteDeCredito(Contrato contrato, Programacao programacao)
        //{
        //    var sql = new StringBuilder();
        //    var cnn = _context.Connection;
        //}
    }

    

    internal class DadosLogAprovacaoProposta
    {
        public bool TemLogAprovacao { get; set; }
        public string LogAprovacaoProposta { get; set; }
        public string Complemento { get; set; }
        public string ObservacaoProposta { get; set; }
    }

    internal class DadosAprovacaoProposta
    {
        public int NumVersao { get; set; }
        public int Usina { get; set; }
        public int ObraNumero { get; set; }
        public int AnoChamada { get; set; }
        public int NumChamada { get; set; }
        public string AprovacaoProposta { get; set; }

        public List<DadosLogAprovacaoProposta> LogsAprovacaoProposta;
    }
}
