using System;
using System.Collections.Generic;
using System.Linq;


using System.Data.Entity;

using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using System.Text;
using Dapper;
using System.Transactions;
using System.Linq.Expressions;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ContratoRepository : RepositoryBase<Contrato>, IContratoRepository
    {
        private IDatabaseRepository _databaseRepository;
        private readonly IObraRepository _obraRepository;

        public ContratoRepository(AppDataContext context, IDatabaseRepository databaseRepository, IObraRepository obraRepository) : base(context)
        {
            _context = context;
            _databaseRepository = databaseRepository;
            _obraRepository = obraRepository;
        }

        public ICollection<ContratoPagamento> ObterContratoPagamentos(int usina, int numeroContrato, int anoContrato)
        {

            var pagamentos = _context.ContratoPagamentos
                .Include(x => x.TipoCobranca)
                .Include(x => x.CondicaoPagamento)
                .Where(x => x.UsinaCodigo == usina
                && x.ContratoNumero == numeroContrato
                && x.ContratoAno == anoContrato)
                .AsNoTracking()
                .ToList(); 

            return pagamentos;

        }

        public ICollection<ContratoPagamentoVersao> ObterContratoPagamentosVersao(int numVersao, int usina, int numeroContrato, int anoContrato)
        {

            var pagamentos = _context.ContratoPagamentosVersoes
                .Include(x => x.TipoCobranca)
                .Include(x => x.CondicaoPagamento)
                .Where(x => x.UsinaCodigo == usina
                && x.ContratoNumero == numeroContrato
                && x.ContratoAno == anoContrato
                && x.NumeroVersao == numVersao)
                .AsNoTracking()
                .ToList(); 

            return pagamentos;

        }

        public ICollection<Contrato> ListarContratosRevalidacaoCadastro()
        {
            return _context
                .Contratos
                .Include(c => c.Interveniente)
                .Include(c => c.Interveniente.BloqueioMotivo)
                .Where(t => t.Status == EContratoStatus.RevalidacaoCadastro).ToList();
        }

        public Contrato ObterPorId(int codUsina, int anoContrato, int numeroContrato)
        {

            return _context
                .Contratos
                .Include(c => c.Interveniente)
                .Include(c => c.Interveniente.BloqueioMotivo)
                .Where(c => c.Ano == anoContrato && c.Numero == numeroContrato && c.Usina == codUsina)
                .FirstOrDefault();
        }

        public int GetUltimaVersaoContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" coalesce(max(num_versao),0) as num_versao");
            sqlComando.Append(" FROM topsys.con_contrato_versao");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina=@COD_USINA");
            sqlComando.Append(" AND ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND num_contrato=@NUMERO_CONTRATO");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { 
                COD_USINA = codUsina, ANO_CONTRATO = anoContrato, NUMERO_CONTRATO = numeroContrato })
                .Select(row => new { row.num_versao })
                .FirstOrDefault();

            return (int)result.num_versao;
        }

        public int GetUltimaVersaoContratoAberta(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            // Seleciona a última versão (MAX sem filtro de status) e só retorna se ela estiver aberta.
            // Isso evita retornar uma versão anterior pendente quando a mais recente já foi aprovada (9136) ou encerrada (9133).
            sqlComando.Append("SELECT coalesce(cv.num_versao, 0) as num_versao");
            sqlComando.Append(" FROM topsys.con_contrato_versao cv");
            sqlComando.Append(" WHERE cv.usina=@COD_USINA");
            sqlComando.Append(" AND cv.ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND cv.num_contrato=@NUMERO_CONTRATO");
            sqlComando.Append(" AND cv.num_versao = (SELECT MAX(cv2.num_versao) FROM topsys.con_contrato_versao cv2 WHERE cv2.usina=@COD_USINA AND cv2.ano_contrato=@ANO_CONTRATO AND cv2.num_contrato=@NUMERO_CONTRATO)");
            sqlComando.Append(" AND cv.status not in (9136, 9133)");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new
            {
                COD_USINA = codUsina,
                ANO_CONTRATO = anoContrato,
                NUMERO_CONTRATO = numeroContrato
            })
                .Select(row => new { row.num_versao })
                .FirstOrDefault();

            // Retorna 0 quando a última versão já está aprovada/encerrada (sem versão aberta)
            return result != null ? (int)result.num_versao : 0;
        }

        public int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" coalesce(max(num_versao),0) as num_versao");
            sqlComando.Append(" FROM topsys.con_contrato_versao");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina=@COD_USINA");
            sqlComando.Append(" AND ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND num_contrato=@NUMERO_CONTRATO");
            sqlComando.Append(" AND status=@STATUS");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new
            {
                COD_USINA = codUsina,
                ANO_CONTRATO = anoContrato,
                NUMERO_CONTRATO = numeroContrato,
                STATUS = 9136
            })
            .Select(row => new { row.num_versao })
            .FirstOrDefault();

            return (int)result.num_versao;
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_versao");
            sqlComando.Append($" SELECT {numVersao}, CURRENT_DATE(), c.* from topsys.con_contrato c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");

            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");

            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_versao", "con_contrato");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_contrato");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");

            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }


        public ICollection<ContratoVersao> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato, bool parametroGeraAditivoContratoSemAprovCadastro = false)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT cont.num_versao {nameof(ContratoVersao.NumeroVersao)}");
            sqlComando.Append($", cont.usina {nameof(ContratoVersao.Usina)}");
            sqlComando.Append($", cont.num_contrato {nameof(ContratoVersao.Numero)}");
            sqlComando.Append($", cont.ano_contrato {nameof(ContratoVersao.Ano)}");
            sqlComando.Append($", cont.status {nameof(ContratoVersao.Status)}");
            sqlComando.Append($" FROM con_contrato_versao cont");
            sqlComando.Append($" INNER JOIN con_obras_versao obra");
            sqlComando.Append($" ON obra.usina=cont.usina");
            sqlComando.Append($" AND obra.no_contrato=cont.num_contrato");
            sqlComando.Append($" AND obra.ano_contrato=cont.ano_contrato");
            sqlComando.Append($" AND obra.num_versao=cont.num_versao");
            sqlComando.Append($" WHERE cont.usina={codUsina}");
            sqlComando.Append($" AND cont.num_contrato={numeroContrato}");
            sqlComando.Append($" AND cont.ano_contrato={anoContrato}");
            sqlComando.Append($" AND (cont.status={(int)EContratoStatus.Aprovado}");

            if (parametroGeraAditivoContratoSemAprovCadastro)
            {
                sqlComando.Append($" OR obra.status_comercial IN({(int)EObraStatusComercial.NaoNecessita}, {(int)EObraStatusComercial.Aprovado})");
                sqlComando.Append($" AND obra.status_financeiro IN({(int)EObraStatusFinanceiro.NaoNecessita}, {(int)EObraStatusFinanceiro.Aprovado})");
                sqlComando.Append($" AND obra.status_engenharia IN({(int)EObraStatusEngenharia.NaoNecessita}, {(int)EObraStatusEngenharia.Aprovado})");
            }

            sqlComando.Append($")");

            return _context.Database.Connection.Query<ContratoVersao>(sqlComando.ToString()).ToList();
        }

        public ContratoVersao ContratoVersaoObterPorId(int numeroVersao, int codUsina, int anoContrato, int numeroContrato)
        {

            return _context
                .ContratosVersoes
                .Include(c => c.Interveniente)
                .Include(c => c.Interveniente.BloqueioMotivo)
                .Where(c => c.NumeroVersao == numeroVersao && c.Ano == anoContrato && c.Numero == numeroContrato && c.Usina == codUsina)
                .FirstOrDefault();
        }

        public int ObterProximaSequenciaValorAvulso(int idUsinaEntrega)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT IFNULL(MAX(seq_vlr_avulso), 0) FROM con_vlr_avulso");
            sqlComando.Append($" WHERE usina_remessa = {idUsinaEntrega}");

            return _context.Database.Connection.Query<int>(sqlComando.ToString()).FirstOrDefault();
        }

        public string GetSegmentacaoContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" s.nome_abreviado");
            sqlComando.Append(" FROM topsys.con_contrato c");
            sqlComando.Append(" LEFT JOIN con_segmentacao s ON c.segmentacao = s.id");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina=@COD_USINA");
            sqlComando.Append(" AND ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND num_contrato=@NUMERO_CONTRATO");

            return _context.Database.Connection.QueryFirstOrDefault<string>(sqlComando.ToString(), new
            {
                COD_USINA = codUsina,
                ANO_CONTRATO = anoContrato,
                NUMERO_CONTRATO = numeroContrato
            });
        }

        public DateTime? GetDataCriacaoVersaoContrato(int numVersao, int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" dt_versao_criada");
            sqlComando.Append(" FROM topsys.con_contrato_versao");
            sqlComando.Append(" WHERE num_versao=@NUM_VERSAO");
            sqlComando.Append(" AND usina=@COD_USINA");
            sqlComando.Append(" AND ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND num_contrato=@NUMERO_CONTRATO");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new
            {
                NUM_VERSAO = numVersao,
                COD_USINA = codUsina,
                ANO_CONTRATO = anoContrato,
                NUMERO_CONTRATO = numeroContrato
            })
                .FirstOrDefault();

            return result != null ? (DateTime?)result.dt_versao_criada : null;
        }

        public bool ExisteVersaoEmAberto(int codUsina, int anoContrato, int numeroContrato)
        {

            var ultimaVersaoEmAberto = GetUltimaVersaoContratoAberta(codUsina, anoContrato, numeroContrato);

            //var contratoVersao = _context.ContratosVersoes
            //    .Where(t => t.Ano == anoContrato && t.Numero == numeroContrato && t.Usina == codUsina)
            //    .Where(t => t.Status != EContratoStatus.Aprovado && t.Status != EContratoStatus.Reprovado)
            //    .Where(t => t.NumeroVersao == ultimaVersao)
            //    .ToList();

            //if (contratoVersao.Count > 0)
            //    return true;

            return ultimaVersaoEmAberto > 0;
        }

        public ICollection<ContratoFinalidade> ListarFinalidades()
        {
            var finalidades = _context.ContratoFinalidades.ToList();
            return finalidades;
        }

        public void ObterDataEncerramentoEStatusVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, out DateTime? dataEncerramento, out int status)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" dt_encer_cont");
            sqlComando.Append(" , status");
            sqlComando.Append(" FROM topsys.con_contrato_versao");
            sqlComando.Append(" WHERE num_versao=@NUM_VERSAO");
            sqlComando.Append(" AND usina=@COD_USINA");
            sqlComando.Append(" AND ano_contrato=@ANO_CONTRATO");
            sqlComando.Append(" AND num_contrato=@NUMERO_CONTRATO");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new
            {
                NUM_VERSAO = numVersao,
                COD_USINA = codUsina,
                ANO_CONTRATO = anoContrato,
                NUMERO_CONTRATO = numeroContrato
            })
                .FirstOrDefault();

            dataEncerramento = result != null ? (DateTime?)result.dt_encer_cont : null;
            status = result != null ? (int)result.status : 0;
        }

        public void AtualizarDataEncerramentoEStatusContrato(int codUsina, int anoContrato, int numeroContrato, DateTime? dataEncerramento, int status)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE topsys.con_contrato");
            sqlComando.Append($" SET");
            if (dataEncerramento != null) 
                sqlComando.Append($" dt_encer_cont='{dataEncerramento?.ToString("yyyy-MM-dd")}',");
            else
                sqlComando.Append($" dt_encer_cont=NULL,");
            sqlComando.Append($" status={status}");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");

            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void CriarTabelaTemporariaTaxaExtraVersao(int numVersao, int codUsina, int anoContrato, int numeroContrato)
        {
            var sqlComando = new StringBuilder();

            var obra = _obraRepository.ObterObraPorContrato(codUsina, anoContrato, numeroContrato);

            sqlComando.Append($"DROP TABLE IF EXISTS tmp_con_taxa_extra_versao");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"CREATE TABLE IF NOT EXISTS tmp_con_taxa_extra_versao LIKE con_taxa_extra_versao");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"INSERT INTO tmp_con_taxa_extra_versao");
            sqlComando.Append($" SELECT {numVersao}, taxa.* FROM con_taxa_extra taxa");
            sqlComando.Append($" WHERE NOT (taxa.taxa_adicional='ACRECIMO PARA ALTERAÇÃO DE PEDRAS'");
            sqlComando.Append($" OR taxa.taxa_adicional='ACRECIMO PARA ALTERAÇÃO DE SLUMP')");
            sqlComando.Append($" AND taxa.usina={obra.UsinaEntregaCodigo}");
            sqlComando.Append($" AND taxa.obra=0");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"INSERT INTO tmp_con_taxa_extra_versao");
            sqlComando.Append($" SELECT * FROM con_taxa_extra_versao");
            sqlComando.Append($" WHERE NOT (taxa_adicional='ACRECIMO PARA ALTERAÇÃO DE PEDRAS'");
            sqlComando.Append($" OR taxa_adicional='ACRECIMO PARA ALTERAÇÃO DE SLUMP')");
            sqlComando.Append($" AND usina={obra.UsinaEntregaCodigo}");
            sqlComando.Append($" AND obra={obra.Numero}");
            sqlComando.Append($" AND num_versao={numVersao}");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
    	}

        public int ObterUsinaClausula(int usinaEntrega)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT usina FROM con_contrato_clausu WHERE usina={usinaEntrega}");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }

        public string ObterContratoUsina(int usina, string segmento)
        {
            StringBuilder sqlComando = new StringBuilder();
            int segmentoCodigo = 0;

            switch (segmento)
            {
                case "CON":
                    segmentoCodigo = 1;
                    break;
                case "EXP":
                    segmentoCodigo = 2;
                    break;
                case "ARG":
                    segmentoCodigo = 3;
                    break;
            }


            sqlComando.Append($"SELECT valor FROM con_modelo_contrato_usina WHERE usina={usina} AND segmento={segmentoCodigo}");

            return _context.Database.Connection.QueryFirstOrDefault<string>(sqlComando.ToString());
        }
    }
}
