using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities.Integracao;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class IntegracaoRepository : IIntegracaoRepository
    {
        protected AppDataContext _context;
        private readonly IObraRepository _obraRepository;

        public IntegracaoRepository(AppDataContext context, IObraRepository obraRepository)
        {
            _context = context;
            _obraRepository = obraRepository;
        }

        public IEnumerable<TotaisContrato> ObterTotaisRemessaContratosPorCliente(int codigoCliente)
        {

            var sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT");
            sqlCommand.AppendLine("    con.usina UsinaContrato,");
            sqlCommand.AppendLine("    con.ano_contrato AnoContrato,");
            sqlCommand.AppendLine("    con.num_contrato NumeroContrato,");
            sqlCommand.AppendLine("    con.total_m3 VolumeM3Contratado,");
            sqlCommand.AppendLine("    SUM(prog.volume_entregue) VolumeM3Entregue,");
            sqlCommand.AppendLine("    SUM(nf.vlr_venda_total) FaturamentoContrato");
            sqlCommand.AppendLine("FROM con_contrato con");
            sqlCommand.AppendLine("LEFT JOIN con_programacao_hora prog");
            sqlCommand.AppendLine("    ON prog.usina = con.usina");
            sqlCommand.AppendLine("    AND prog.ano_contrato = con.ano_contrato");
            sqlCommand.AppendLine("    AND prog.no_contrato = con.num_contrato");
            sqlCommand.AppendLine("    AND prog.interv = con.interv");
            sqlCommand.AppendLine("LEFT JOIN con_nf nf");
            sqlCommand.AppendLine("    ON nf.filial = prog.filial");
            sqlCommand.AppendLine("    AND nf.interv = prog.interv");
            sqlCommand.AppendLine("    AND nf.tp_doc = prog.tp_doc");
            sqlCommand.AppendLine("    AND nf.num_nf = prog.num_nf");
            sqlCommand.AppendLine("    AND nf.serie = prog.serie");
            sqlCommand.AppendLine("    AND nf.seq_nf = prog.seq_nf"); 
            sqlCommand.AppendLine("    AND nf.motivo_cancel = ''");
            sqlCommand.AppendLine("INNER JOIN ger_interv cli");
            sqlCommand.AppendLine("    ON cli.Cod = con.interv");
            sqlCommand.AppendLine("WHERE");
            sqlCommand.AppendLine($"    con.interv = {codigoCliente}");
            sqlCommand.AppendLine("GROUP BY con.usina, con.num_contrato, con.ano_contrato");
            sqlCommand.AppendLine("ORDER BY con.ano_contrato DESC, con.num_contrato DESC");

            var result = _context.Database.Connection.Query<TotaisContrato>(sqlCommand.ToString());

            return result;

        }

        public ResumoCredito obterInformacoesCreditoPorCliente(int codCliente)
        {

            var resCredito = new ResumoCredito();

            var _tipoDocCheque = 8;

            var sql = new StringBuilder();
            sql.Append($"SELECT limite_cred FROM ger_interv where Cod = {codCliente}");

            resCredito.LimiteCredito = _context.Database.Connection.QueryFirstOrDefault<double>(sql.ToString());

            sql.Clear();
            sql.Append("SELECT IFNULL(sum(sal), 0) as vlrSaldoCar, IFNULL(sum(soma_recbtos), 0) as vlrRecebimento");
            sql.AppendLine($" FROM fin_car where cli={codCliente}");
            sql.AppendLine($" and tp_doc<>{_tipoDocCheque}");

            resCredito.ValorEmAbertoTotal = _context.Database.Connection.QueryFirstOrDefault<double>(sql.ToString());

            sql.Clear();
            sql.Append("select IFNULL(sum(if(ifnull(datediff(current_date(),liq_dt),dias_lib_cred)>dias_lib_cred,sal,if(desdo=0,if(isnull(liq_dt),sal,liq_vl_rec),liq_vl_rec))), 0) as vlrSaldoCar");
            sql.AppendLine(" from fin_car as car");
            sql.AppendLine(" ,(select dias_lib_cred,inicio_validade from fin_parametro");
            sql.AppendLine($" where inicio_validade<=current_date");
            sql.AppendLine(" order by inicio_validade desc limit 1) as pm");
            sql.AppendLine($" where cli={codCliente}");
            sql.AppendLine($" and tp_doc={_tipoDocCheque}");

            resCredito.ValorEmAbertoTotal += _context.Database.Connection.QueryFirstOrDefault<double>(sql.ToString());

            sql.Clear();
            sql.Append("SELECT IFNULL(SUM(comp.vlr_total_cobranca), 0) vlr_total_cobranca FROM con_nf nf");
            sql.AppendLine(" LEFT JOIN con_nf_complemento comp");
            sql.AppendLine(" ON comp.filial=nf.filial AND comp.interv=nf.interv");
            sql.AppendLine(" AND comp.tp_doc=nf.tp_doc AND comp.num_nf=nf.num_nf");
            sql.AppendLine(" AND comp.serie=nf.serie AND comp.seq_nf=nf.seq_nf");
            sql.AppendLine(" WHERE ISNULL(nf.dt_fatura)");
            sql.AppendLine(" AND nf.motivo_cancel=0");
            sql.AppendLine($" AND nf.interv={codCliente}");
            sql.AppendLine($" AND nf.data_remessa<='{DateTime.Now.ToString("yyyy-MM-dd")}'");

            resCredito.ValorNotasFiscaisNaoFaturadas = _context.Database.Connection.QueryFirstOrDefault<double>(sql.ToString());

            resCredito.SaldoCredito = resCredito.LimiteCredito - (resCredito.ValorEmAbertoTotal + resCredito.ValorNotasFiscaisNaoFaturadas);

            return resCredito;

        }


        public void SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno)
        {

            var sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE con_nf");
            sqlCommand.Append(" SET ");
            sqlCommand.Append($" hr_saida_usina_efet= @{nameof(retorno.HoraSaidaUsina)},");
            sqlCommand.Append($" hr_cheg_obra= @{nameof(retorno.HoraChegadaObra)},");
            sqlCommand.Append($" hr_desc_inic= @{nameof(retorno.HoraInicioDescarga)},");
            sqlCommand.Append($" hr_desc_final= @{nameof(retorno.HoraFimDescarga)},");
            sqlCommand.Append($" hr_saida_obra= @{nameof(retorno.HoraSaidaObra)},");
            sqlCommand.Append($" hr_cheg_usina= @{nameof(retorno.HoraChegadaUsina)},");

            if(retorno.AdicaoAgua > 0)
                sqlCommand.Append($" adicao_agua= @{nameof(retorno.AdicaoAgua)},");

            sqlCommand.Append($" velocimento= @{nameof(retorno.VelocimetroInicial)},");
            sqlCommand.Append($" velocimento_final= @{nameof(retorno.VelocimetroFinal)}");

            sqlCommand.Append($" WHERE ");
            sqlCommand.Append($" filial=@{nameof(retorno.Filial)}");
            sqlCommand.Append($" AND interv=@{nameof(retorno.Interveniente)}");
            sqlCommand.Append($" AND tp_doc=@{nameof(retorno.TipoDocumento)}");
            sqlCommand.Append($" AND num_nf=@{nameof(retorno.NumeroNota)}");
            sqlCommand.Append($" AND serie=@{nameof(retorno.Serie)}");
            sqlCommand.Append($" AND seq_nf=@{nameof(retorno.Sequencia)}");

            _context.Database.Connection.Execute(sqlCommand.ToString(), retorno);
            


        }
    }
}
