using CrystalDecisions.CrystalReports.Engine;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;


namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class IntervenienteSequenceRepository : RepositoryBase<IntervenienteSequence>, IIntervenienteSequenceRepository
    {
        private readonly IdentityHelperService _identityHelperService;

        public IntervenienteSequenceRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public void CriaOuIgnoraTabela()
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append("CREATE TABLE IF NOT EXISTS sequence_ger_interv");
            sqlCommand.Append("(");
            sqlCommand.Append("     ultimo_id_ger_interv BIGINT(11) AUTO_INCREMENT PRIMARY KEY,");
            sqlCommand.Append("     faixa_inicial BIGINT(11) NOT NULL,");
            sqlCommand.Append("     faixa_final BIGINT(11) NOT NULL,");
            sqlCommand.Append("     UNIQUE KEY(FAIXA_INICIAL, FAIXA_FINAL)");
            sqlCommand.Append(")");
            sqlCommand.Append("ENGINE=MyISAM DEFAULT CHARSET=latin1;");

            _context.Database.Connection.Execute(sqlCommand.ToString());
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "sequence_ger_interv", sqlCommand.ToString());
        }

        public int AtualizarEObterProximaSequencia(int faixaInicial, int faixaFinal)
        {
            var sqlCommand = new StringBuilder();

            var sqlCodigoIntervinenteSequenceCommandQuery = GerarStringCodigoAtualFaixaParaInterveniente();

            sqlCommand.Append($" SET @{nameof(IntervenienteSequence.UltimoID)}=({sqlCodigoIntervinenteSequenceCommandQuery}) + 1;");

            sqlCommand.Append(" REPLACE INTO sequence_ger_interv");
            sqlCommand.Append(" (faixa_inicial, faixa_final, ultimo_id_ger_interv)");
            sqlCommand.Append($" VALUES(");
            sqlCommand.Append($" @{nameof(IntervenienteSequence.FaixaInicial)},");
            sqlCommand.Append($" @{nameof(IntervenienteSequence.FaixaFinal)},");
            sqlCommand.Append($" @{nameof(IntervenienteSequence.UltimoID)}");
            sqlCommand.Append($" );");

            sqlCommand.Append($" SELECT @{nameof(IntervenienteSequence.UltimoID)};");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(
               sqlCommand.ToString(),
               new
               {
                   FaixaInicial = faixaInicial,
                   FaixaFinal = faixaFinal,
               });

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "sequence_ger_interv", sqlCommand.ToString());

            return result;
        }

        public void SincronizarFaixa(IntervenienteSequence intervenienteSequence)
        {
            var sqlCommand = new StringBuilder();

            var sqlMaximoCodigoIntervinenteCommandQuery = GerarStringMaximoCodigoCadastradoDaFaixaDoInterveniente();
            var sqlCodigoIntervinenteSequenceCommandQuery = GerarStringCodigoAtualFaixaParaInterveniente();

            if (intervenienteSequence.UltimoID == 0)
            {
                sqlCommand.Append($" SET @{nameof(Interveniente.Codigo)}=({sqlMaximoCodigoIntervinenteCommandQuery});");
                sqlCommand.Append($" SET @{nameof(IntervenienteSequence.UltimoID)}=({sqlCodigoIntervinenteSequenceCommandQuery});");

                sqlCommand.Append($" SET @ProximoID=");
                sqlCommand.Append($"(IF(@{nameof(Interveniente.Codigo)} > @{nameof(IntervenienteSequence.UltimoID)},");
                sqlCommand.Append($" @{nameof(Interveniente.Codigo)},");
                sqlCommand.Append($" @{nameof(IntervenienteSequence.UltimoID)}));");
            }
            
            sqlCommand.Append(" REPLACE INTO sequence_ger_interv");
            sqlCommand.Append(" (faixa_inicial, faixa_final, ultimo_id_ger_interv)");
            sqlCommand.Append($" VALUES(");
            sqlCommand.Append($" @{nameof(IntervenienteSequence.FaixaInicial)},");
            sqlCommand.Append($" @{nameof(IntervenienteSequence.FaixaFinal)},");

            if (intervenienteSequence.UltimoID == 0)
                sqlCommand.Append($" @ProximoID");            
            else
                sqlCommand.Append($" @{nameof(IntervenienteSequence.UltimoID)}");
                         
            sqlCommand.Append($");");

            if (intervenienteSequence.UltimoID == 0)
                _context.Database.Connection.Execute(
                   sqlCommand.ToString(),
                   new
                   {
                       intervenienteSequence.FaixaInicial,
                       intervenienteSequence.FaixaFinal,
                   }
                );
            else
                 _context.Database.Connection.Execute(
                    sqlCommand.ToString(),
                    new
                    {
                        intervenienteSequence.FaixaInicial,
                        intervenienteSequence.FaixaFinal,
                        intervenienteSequence.UltimoID,
                    }
                );

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "sequence_ger_interv", sqlCommand.ToString());
        }

     

        public int ObterFaixa(int faixaInicial, int faixaFinal)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" WHERE");
            sqlQuery.Append($" faixa_inicial=@{nameof(IntervenienteSequence.FaixaInicial)}");
            sqlQuery.Append($" AND faixa_final=@{nameof(IntervenienteSequence.FaixaFinal)}");

            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
             sqlQuery.ToString(),
             new
             {
                 FaixaInicial = faixaInicial,
                 FaixaFinal = faixaFinal,
             });

            return result?.UltimoID ?? 0;
        }

        public bool ValidaFaixaTotalmenteUtilizada(int faixaInicial, int faixaFinal)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" WHERE");
            sqlQuery.Append($" faixa_inicial=@{nameof(IntervenienteSequence.FaixaInicial)}");
            sqlQuery.Append($" AND faixa_final=@{nameof(IntervenienteSequence.FaixaFinal)}");
            sqlQuery.Append($" AND faixa_final=ultimo_id_ger_interv");

            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
             sqlQuery.ToString(),
             new
             {
                 FaixaInicial = faixaInicial,
                 FaixaFinal = faixaFinal,
             });

            return result != null;
        }


        public bool ValidaProbabilidadeDeDuplicidadeFaixaUtilizada(int faixaInicial, int faixaFinal)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" WHERE");
            sqlQuery.Append($" (@{nameof(IntervenienteSequence.FaixaInicial)} BETWEEN faixa_inicial AND faixa_final)");
            sqlQuery.Append($" OR (@{nameof(IntervenienteSequence.FaixaFinal)}  BETWEEN faixa_inicial AND faixa_final)");
            sqlQuery.Append($" OR (@{nameof(IntervenienteSequence.FaixaInicial)} <= faixa_inicial AND @{nameof(IntervenienteSequence.FaixaFinal)} >= faixa_final)");

            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
             sqlQuery.ToString(),
             new
             {
                 FaixaInicial = faixaInicial,
                 FaixaFinal = faixaFinal,
             });

            return result != null ;
        }

        public bool ValidaCodigosForaDeFaixa()
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" WHERE");
            sqlQuery.Append($"( ultimo_id_ger_interv < faixa_inicial) ");
            sqlQuery.Append($" OR (ultimo_id_ger_interv > faixa_final) ");
          
            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
                sqlQuery.ToString()
            );

            return result != null;
        }

        public bool ValidaDuplicacoesDeFaixasIniciais()
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" GROUP BY");
            sqlQuery.Append($" faixa_inicial");
            sqlQuery.Append($" HAVING(COUNT(faixa_inicial) > 1)");

            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
                sqlQuery.ToString()
            );

            return result != null;
        }

        public bool ValidaDuplicacoesDeFaixasFinais()
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.Append(GerarStringConsultaIntervenienteSequence());
            sqlQuery.Append($" GROUP BY");
            sqlQuery.Append($" faixa_final");
            sqlQuery.Append($" HAVING(COUNT(faixa_final) > 1)");

            var result = _context.Database.Connection.QueryFirstOrDefault<IntervenienteSequence>(
                sqlQuery.ToString()
            );

            return result != null;
        }

        private StringBuilder GerarStringCodigoAtualFaixaParaInterveniente()
        {
            var result = new StringBuilder();

            result.Append(" COALESCE((");
            result.Append(" SELECT");
            result.Append($" ultimo_id_ger_interv AS {nameof(IntervenienteSequence.UltimoID)}");
            result.Append(" FROM");
            result.Append(" sequence_ger_interv");
            result.Append(" WHERE");
            result.Append($" faixa_inicial=@{nameof(IntervenienteSequence.FaixaInicial)}");
            result.Append(" AND");
            result.Append($" faixa_final=@{nameof(IntervenienteSequence.FaixaFinal)}");
            result.Append(" ),0)");

            return result;
        }

        private StringBuilder GerarStringMaximoCodigoCadastradoDaFaixaDoInterveniente()
        {
            var result = new StringBuilder();

            result.Append(" SELECT IF(");
            result.Append($" COALESCE(MAX(cod)>@{nameof(IntervenienteSequence.FaixaInicial)},TRUE),");
            result.Append($" COALESCE(MAX(cod),@{nameof(IntervenienteSequence.FaixaInicial)}),");
            result.Append($" @{nameof(IntervenienteSequence.FaixaFinal)}");
            result.Append($" ) AS {nameof(Interveniente.Codigo)}");
            result.Append(" FROM");
            result.Append(" ger_interv");
            result.Append(" WHERE");
            result.Append($" cod>=@{nameof(IntervenienteSequence.FaixaInicial)}");
            result.Append($" AND cod<=@{nameof(IntervenienteSequence.FaixaFinal)}");

            return result;
        }

        public StringBuilder GerarStringConsultaIntervenienteSequence()
        {
            var result = new StringBuilder();

            result.Append("SELECT");
            result.Append($" ultimo_id_ger_interv {nameof(IntervenienteSequence.UltimoID)}");
            result.Append($" ,faixa_inicial {nameof(IntervenienteSequence.FaixaInicial)}");
            result.Append($" ,faixa_final {nameof(IntervenienteSequence.FaixaFinal)}");
            result.Append($" FROM");
            result.Append($" sequence_ger_interv");

            return result;
        }
    }
}
