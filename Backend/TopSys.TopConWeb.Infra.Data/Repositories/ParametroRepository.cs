using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ParametroRepository : IParametroRepository
    {
        protected AppDataContext _context;

        public ParametroRepository(AppDataContext context)
        {
            this._context = context;
        }

        public TEntity ObterPorDataBase<TEntity>(DateTime dataBase) where TEntity : Parametro
        {
            StringBuilder sqlComando = new StringBuilder();

            var tableName = "topsys.con_parametro";
            var tableDescribe = _context.DescribeTable(tableName);

            sqlComando.Append("SELECT");

            sqlComando.Append($" data_vigencia {nameof(Parametro.DataInicioVigencia)}");

            if (typeof(TEntity).IsEquivalentTo(typeof(ParametroProposta)))
            {
                AppendIfExists(ref sqlComando, "obriga_profissa", nameof(ParametroProposta.ObrigaProfissao), tableDescribe);
                AppendIfExists(ref sqlComando, "prev_ent_mat", nameof(ParametroProposta.ObrigaVolumeEstimadoEPrevisaoInicioTermino), tableDescribe);
                AppendIfExists(ref sqlComando, "obriga_email_pf", nameof(ParametroProposta.ObrigaEmailPessoaFisica), tableDescribe);
                AppendIfExists(ref sqlComando, "blq_imp_p_aprov", nameof(ParametroProposta.BloqueiaImpressaoPropostaContratoPendenteAprovacao), tableDescribe);
                AppendIfExists(ref sqlComando, "ap_dist_usina", nameof(ParametroProposta.ObrigaAprovacaoDistanciaUsinaEntrega), tableDescribe);
                AppendIfExists(ref sqlComando, "pct_desc_limite", nameof(ParametroProposta.PercentualDescontoLimite), tableDescribe);
                AppendIfExists(ref sqlComando, "info_usina_prop", nameof(ParametroProposta.DadosFilialNaImpressaoProposta), tableDescribe);
                AppendIfExists(ref sqlComando, "ocultar_taxa_proposta", nameof(ParametroProposta.OcultarTaxaProposta), tableDescribe);
                AppendIfExists(ref sqlComando, "obriga_tipo_obra", nameof(ParametroProposta.ObrigaTipoObra), tableDescribe);
                AppendIfExists(ref sqlComando, "obriga_porte_obra", nameof(ParametroProposta.ObrigaPorteObra), tableDescribe);
                AppendIfExists(ref sqlComando, "men_reajuste_padrao", nameof(ParametroProposta.MensagemReajustePadrao), tableDescribe);
                AppendIfExists(ref sqlComando, "validade_proposta", nameof(ParametroProposta.ValidadeProposta), tableDescribe);
                AppendIfExists(ref sqlComando, "informar_bomba_terc", nameof(ParametroProposta.InformarBombaTerceiros), tableDescribe);
                AppendIfExists(ref sqlComando, "obriga_fim_vigencia", nameof(ParametroProposta.ObrigaFimVigencia), tableDescribe);
                AppendIfExists(ref sqlComando, "obriga_num_ctr_anterior", nameof(ParametroProposta.ObrigaNumeroContratoAnterior), tableDescribe);
            }
            else if (typeof(TEntity).IsEquivalentTo(typeof(ParametroTaxaExtra)))
            {
                AppendIfExists(ref sqlComando, "mens_alt_pedra", nameof(ParametroTaxaExtra.MensagemAlteracaoPedra), tableDescribe);
                AppendIfExists(ref sqlComando, "mens_alt_slump", nameof(ParametroTaxaExtra.MensagemAlteracaoSlump), tableDescribe);
            }

            sqlComando.Append($" FROM {tableName}");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" data_vigencia<=@DATA_BASE");
            sqlComando.Append(" ORDER BY data_vigencia DESC");
            sqlComando.Append(" LIMIT 1");

            var result = _context.Database.Connection.Query<TEntity>(sqlComando.ToString(), new { DATA_BASE = dataBase })
                .FirstOrDefault();

            return result;
        }

        private void AppendIfExists(ref StringBuilder sqlComando, string fieldName, string propertyName, IEnumerable<TableFieldDescribe> tableDescribe)
        {
            if (_context.FieldExistsInTable(tableDescribe, fieldName))
                sqlComando.Append($",{fieldName} {propertyName}");
        }

        public string ObterParametroN(string grupo, string chave)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT valor");
            sqlComando.Append(" FROM topsys.ger_parametro");
            sqlComando.Append(" WHERE grupo=@GRUPO");
            sqlComando.Append(" AND chave=@CHAVE");
            sqlComando.Append(" LIMIT 1");

            try
            {
                var result = _context.Database.Connection.Query<string>(sqlComando.ToString(), new { GRUPO = grupo, CHAVE = chave })
                .FirstOrDefault();

                return result ?? "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string ObterParametroIntegracoes(string integracaoTipo, string parametroNome)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT parametro_valor");
            sqlComando.Append(" FROM topsys.con_integracoes");
            sqlComando.Append(" WHERE integracao_tipo=@INTEGRACAOTIPO");
            sqlComando.Append(" AND parametro_nome=@PARAMETRONOME");
            sqlComando.Append(" LIMIT 1");

            try
            {
                var result = _context.Database.Connection.Query<string>(sqlComando.ToString(), new { INTEGRACAOTIPO = integracaoTipo, PARAMETRONOME = parametroNome })
                .FirstOrDefault();

                return result ?? "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public void AtualizarParametroN(string grupo, string chave, string valor)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.ger_parametro (grupo, chave, valor)");
            sqlComando.Append($" values ('{grupo}', '{chave}', '{valor}');");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public void ApagarParametroN(string grupo, string chave)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.ger_parametro");
            sqlComando.Append($" WHERE");
            sqlComando.Append($" grupo='{grupo}'");
            sqlComando.Append($" AND");
            sqlComando.Append($" chave='{chave}'");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }
    }
}
