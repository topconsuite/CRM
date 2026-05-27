using Dapper;
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

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CadastroGeralRepository : RepositoryBase<CadastroGeral>, ICadastroGeralRepository
    {
        public CadastroGeralRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<CadastroGeral> ListarFuncoes()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6900 && t.Codigo <= 7099)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public ICollection<CadastroGeral> ListarMotivosBloqueioInterveniente()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 800 && t.Codigo <= 899)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public ICollection<CadastroGeral> ListarViasCaptacao()
        {
            var result = _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6400 && t.Codigo <= 6499)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();

            if (result.Count == 0)
                return result;

            var query = new StringBuilder();
            query.AppendLine($"SELECT");
            query.AppendLine($"   cod {nameof(CadastroGeralViaCaptacao.Codigo)}");
            query.AppendLine($"  ,ativo {nameof(CadastroGeralViaCaptacao.Ativo)} ");
            query.AppendLine($"  ,captacao_tipo_indicador {nameof(CadastroGeralViaCaptacao.TipoIndicacao)}");
            query.AppendLine($"FROM ger_geral_captacao");
            query.AppendLine($"WHERE cod IN ({string.Join(",", result.Select(x => x.Codigo.ToString()))})");

            var viaCaptacoes = _context.Connection.Query<CadastroGeralViaCaptacao>(query.ToString());

            result.ForEach((geral) =>
            {
                var captacao = viaCaptacoes.Where(x => x.Codigo == geral.Codigo).FirstOrDefault();

                if (captacao == null)
                    captacao = new CadastroGeralViaCaptacao() { Codigo = geral.Codigo };

                geral.ViaCaptacao = captacao;

            });

            result = result.Where(x => string.Equals(x.ViaCaptacao.Ativo, "S")).ToList();

            return result;
        }

        public ICollection<CadastroGeral> ListarTipoObra()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 5300 && t.Codigo <= 5399)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public ICollection<CadastroGeral> ListarPorteObra()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 5100 && t.Codigo <= 5199)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public ICollection<CadastroGeral> ListarTemposAprovacaoMedicao()
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT cod {nameof(CadastroGeral.Codigo)}");
            sqlComando.Append($", descr {nameof(CadastroGeral.Descricao)}");
            sqlComando.Append($" FROM ger_geral");
            sqlComando.Append($" WHERE cod>=1500 AND cod<=1599");
            sqlComando.Append($" ORDER BY CAST(descr AS UNSIGNED)");

            return _context.Connection.Query<CadastroGeral>(sqlComando.ToString()).ToList();
        }

        #region Funcionário Função
        public ICollection<CadastroGeral> ListarFuncionarioFuncao()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6900 && t.Codigo <= 7099)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public CadastroGeral ObterPorIdFuncionarioFuncao(int id, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6900 && t.Codigo <= 7099 && t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public CadastroGeral ObterPorExternalIdFuncionarioFuncao(string externalId, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6900 && t.Codigo <= 7099 && t.ExternalId == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoFuncionarioFuncao(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(funcao) Contagem FROM con_funcionario WHERE funcao = {id}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        #endregion

        #region Funcionário Departamento
        public ICollection<CadastroGeral> ListarFuncionarioDepartamento()
        {
            return _context
                .CadastrosGerais
                .Where(t => (t.Codigo >= 8000 && t.Codigo <= 8099) || (t.Codigo >= 9200 && t.Codigo <= 9299))
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public CadastroGeral ObterPorIdFuncionarioDepartamento(int id, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => ((t.Codigo >= 8000 && t.Codigo <= 8099) || (t.Codigo >= 9200 && t.Codigo <= 9299)) && t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public CadastroGeral ObterPorExternalIdFuncionarioDepartamento(string externalId, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => ((t.Codigo >= 8000 && t.Codigo <= 8099) || (t.Codigo >= 9200 && t.Codigo <= 9299)) && t.ExternalId == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }


        public bool EstaEmUsoFuncionarioDepartamento(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(departamento) Contagem FROM con_funcionario WHERE departamento = {id}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        #endregion

        #region Funcionário Status
        public ICollection<CadastroGeral> ListarFuncionarioStatus()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 7300 && t.Codigo <= 7399)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public CadastroGeral ObterPorIdFuncionarioStatus(int id, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 7300 && t.Codigo <= 7399 && t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public CadastroGeral ObterPorExternalIdFuncionarioStatus(string externalId, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 7300 && t.Codigo <= 7399 && t.ExternalId == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoFuncionarioStatus(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(status) Contagem FROM con_funcionario WHERE status = {id}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        #endregion

        #region Equipamento Tipo
        public ICollection<CadastroGeral> ListarEquipamentoTipo()
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6100 && t.Codigo <= 6199)
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public CadastroGeral ObterPorIdEquipamentoTipo(int id, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6100 && t.Codigo <= 6199 && t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public CadastroGeral ObterPorExternalIdEquipamentoTipo(string externalId, bool tracking = false)
        {
            return _context
                .CadastrosGerais
                .Where(t => t.Codigo >= 6100 && t.Codigo <= 6199 && t.ExternalId == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoEquipamentoTipo(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(tipo) Contagem FROM con_equipamento WHERE tipo = {id}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        #endregion

        public void AtualizarId(int idAtual, int idNovo)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE ger_geral SET cod = {idNovo} WHERE cod = {idAtual}");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public string ObterDescricaoEquipamentoBombaPorObraBomba(int obraUsina, int obraNumero, int obraSequencia, int obraVersao = 0)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine($"SELECT");
            sqlComando.AppendLine($"	gg.descr");
            sqlComando.AppendLine($"FROM con_prop_bomba{(obraVersao == 0 ? "" : "_versao")} cpb");
            sqlComando.AppendLine($"INNER JOIN ger_geral gg ON gg.cod = cpb.tipo_bomba");
            sqlComando.AppendLine($"WHERE");
            sqlComando.AppendLine($"    cpb.usina = @{nameof(obraUsina)}");
            sqlComando.AppendLine($"AND cpb.no_obra = @{nameof(obraNumero)}");
            sqlComando.AppendLine($"AND cpb.seq = @{nameof(obraSequencia)}");
            sqlComando.AppendLine($"{(obraVersao == 0 ? "" : $"AND   cpb.num_versao = @{nameof(obraVersao)}")}");

            var descricao = _context.Database.Connection.QueryFirstOrDefault<string>(sqlComando.ToString(), new { obraUsina, obraNumero, obraSequencia, obraVersao });

            return string.IsNullOrEmpty(descricao) ? "TIPO NÃO INFORMADO" : descricao;

        }

        public int ObterProximoCodigo(ECadastroGeralTipo type)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("SELECT IFNULL(MAX(cod) + 1");

            var codigo = 0;
            if (type == ECadastroGeralTipo.FuncionarioFuncao)
            {
                sqlComando.Append(", 6900) FROM ger_geral WHERE cod>=6900 AND cod<=7099");

                codigo = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            }   
            else if (type == ECadastroGeralTipo.FuncionarioDepartamento)
            {
                sqlComando.Append(", 8000) FROM ger_geral WHERE (cod>=8000 AND cod<=8099)");

                if (codigo > 8099)
                {
                    sqlComando.Clear();
                    sqlComando.Append("SELECT IFNULL(MAX(cod) + 1, 9200) FROM ger_geral WHERE (cod>=9200 AND cod<=9299)");

                    return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
                }
            }
            else if (type == ECadastroGeralTipo.FuncionarioStatus)
            {
                sqlComando.Append(", 7300) FROM ger_geral WHERE (cod>=7300 AND cod<=7399)");

                codigo = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            }
            else if (type == ECadastroGeralTipo.EquipamentoTipo)
            {
                sqlComando.Append(", 6100) FROM ger_geral WHERE (cod>=6100 AND cod<=6199)");

                codigo = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            }

            return codigo;
        }
    }
}
