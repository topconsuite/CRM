using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class AprovacaoComercialPendenteRepository : RepositoryBase<AprovacaoComercialPendente>, IAprovacaoComercialPendenteRepository
    {

        private readonly LogGeralRepository _logGeralRepository;

        public AprovacaoComercialPendenteRepository(AppDataContext context, LogGeralRepository logGeralRepository) : base(context)
        {
            _context = context;
            _logGeralRepository = logGeralRepository;
        }

        public void RemoverVestigiosAprovacoesAnteriores(int obraUsina, int obraNumero, int obraVersao, int excluirAcimaNivelAutoridade = 0)
        {

            var whereComplement = $"nivel_hierarquia > {excluirAcimaNivelAutoridade}";
            var pendentes = ListarTodas(obraUsina, obraNumero, obraVersao, whereComplement);

            foreach(var pendente in pendentes)
            {
                RemoverAprovacaoPendente(pendente);
            }

        }

        public void ForcarAprovacaoRegistrosAlcada(int obraUsina, int obraNumero, int obraVersao) 
        {

            var usuarioAprovacao = "ALCADA OFF";
            var aprovacaoData = DateTime.Now;
            var aprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
            var sql = new StringBuilder();

            sql.AppendLine($"UPDATE topsys.con_aprovacao_comercial_pendente_@TABELA SET");
            sql.AppendLine($"   aprovacao_usuario = @{nameof(usuarioAprovacao)},");
            sql.AppendLine($"   aprovacao_data = @{nameof(aprovacaoData)},");
            sql.AppendLine($"   aprovacao_status = @{nameof(aprovacaoStatus)}");
            sql.AppendLine($"WHERE obra_usina = @{nameof(obraUsina)}");
            sql.AppendLine($"   AND obra_numero = @{nameof(obraNumero)}");
            sql.AppendLine($"   AND obra_versao = @{nameof(obraVersao)}");

            var parametros = new
            {
                usuarioAprovacao,
                aprovacaoData,
                aprovacaoStatus,
                obraUsina,
                obraNumero,
                obraVersao
            };

            _context.Database.Connection.Execute(sql.ToString().Replace("@TABELA", "bomba"), parametros);
            _context.Database.Connection.Execute(sql.ToString().Replace("@TABELA", "traco"), parametros);
            _context.Database.Connection.Execute(sql.ToString().Replace("@TABELA", "cond_pagto"), parametros);
            _context.Database.Connection.Execute(sql.ToString().Replace("@TABELA", "volume"), parametros);

            sql.Clear();

            sql.AppendLine($"UPDATE topsys.con_aprovacao_comercial_pendente SET");
            sql.AppendLine($"   aprovacao_data = @{nameof(aprovacaoData)},");
            sql.AppendLine($"   aprovacao_status = @{nameof(aprovacaoStatus)}");
            sql.AppendLine($"WHERE obra_usina = @{nameof(obraUsina)}");
            sql.AppendLine($"   AND obra_numero = @{nameof(obraNumero)}");
            sql.AppendLine($"   AND obra_versao = @{nameof(obraVersao)}");

            _context.Database.Connection.Execute(sql.ToString(), parametros);

        }

        public void RemoverAprovacaoPendente(AprovacaoComercialPendente pendente)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"DELETE FROM topsys.con_aprovacao_comercial_pendente_traco WHERE id_aprovacao = @{nameof(pendente.Id)};");
            sql.AppendLine($"DELETE FROM topsys.con_aprovacao_comercial_pendente_bomba WHERE id_aprovacao = @{nameof(pendente.Id)};");
            sql.AppendLine($"DELETE FROM topsys.con_aprovacao_comercial_pendente_volume WHERE id_aprovacao = @{nameof(pendente.Id)};");
            sql.AppendLine($"DELETE FROM topsys.con_aprovacao_comercial_pendente_cond_pagto WHERE id_aprovacao = @{nameof(pendente.Id)};");
            sql.AppendLine($"DELETE FROM topsys.con_aprovacao_comercial_pendente WHERE id = @{nameof(pendente.Id)};");

            _context.Database.Connection.Execute(sql.ToString(), new { pendente.Id });

        }
        public void RemoverAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente) => RemoverAprovacaoPendente(pendente.Id, "traco");
        public void RemoverAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente) => RemoverAprovacaoPendente(pendente.Id, "bomba");
        public void RemoverAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente) => RemoverAprovacaoPendente(pendente.Id, "volume");
        public void RemoverAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente) => RemoverAprovacaoPendente(pendente.Id, "cond_pagto");

        private void RemoverAprovacaoPendente(Guid id, string tabela = "")
        {

            var sql = new StringBuilder();
            var tabelaNome = $"con_aprovacao_comercial_pendente{(string.IsNullOrEmpty(tabela) ? "" : "_")}{tabela}";

            sql.AppendLine($"DELETE FROM topsys.{tabelaNome}");
            sql.AppendLine($"WHERE id = @{nameof(id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { id });
            _context.Database.Connection.GravarLogGeral("CRM", tabelaNome, sql.ToString(), new { id });

        }

        public void AdicionarAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente)
        {

            var sql = new StringBuilder();

            sql.Clear();
            sql.AppendLine($"REPLACE INTO topsys.con_aprovacao_comercial_pendente_traco SET");
            sql.AppendLine($"id = @{nameof(AprovacaoComercialPendenteTraco.Id)}");
            sql.AppendLine($", id_aprovacao = @{nameof(AprovacaoComercialPendenteTraco.IdAprovacao)}");
            sql.AppendLine($", obra_versao = @{nameof(AprovacaoComercialPendenteTraco.ObraVersao)}");
            sql.AppendLine($", obra_usina = @{nameof(AprovacaoComercialPendenteTraco.ObraUsina)}");
            sql.AppendLine($", obra_numero = @{nameof(AprovacaoComercialPendenteTraco.ObraNumero)}");
            sql.AppendLine($", obra_seq = @{nameof(AprovacaoComercialPendenteTraco.ObraSeq)}");
            sql.AppendLine($", nivel_hierarquia = @{nameof(AprovacaoComercialPendenteTraco.NivelHierarquia)}");
            sql.AppendLine($", aprovacao_status = @{nameof(AprovacaoComercialPendenteTraco.AprovacaoStatus)}");
            sql.AppendLine($", aprovacao_data = @{nameof(AprovacaoComercialPendenteTraco.AprovacaoData)}");
            sql.AppendLine($", aprovacao_usuario = @{nameof(AprovacaoComercialPendenteTraco.AprovacaoUsuario)}");
            sql.AppendLine($", aprovacao_sequencia = @{nameof(AprovacaoComercialPendenteTraco.AprovacaoSequencia)}");

            _context.Database.Connection.Execute(sql.ToString(), pendente);
            _context.Database.Connection.GravarLogGeral("CRM", "con_aprovacao_comercial_pendente_traco", sql.ToString(), pendente);

        }

        public void AdicionarAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente)
        {

            var sql = new StringBuilder();

            sql.Clear();
            sql.AppendLine($"REPLACE INTO topsys.con_aprovacao_comercial_pendente_bomba SET");
            sql.AppendLine($"id = @{nameof(AprovacaoComercialPendenteBomba.Id)}");
            sql.AppendLine($", id_aprovacao = @{nameof(AprovacaoComercialPendenteBomba.IdAprovacao)}");
            sql.AppendLine($", obra_versao = @{nameof(AprovacaoComercialPendenteBomba.ObraVersao)}");
            sql.AppendLine($", obra_usina = @{nameof(AprovacaoComercialPendenteBomba.ObraUsina)}");
            sql.AppendLine($", obra_numero = @{nameof(AprovacaoComercialPendenteBomba.ObraNumero)}");
            sql.AppendLine($", obra_seq = @{nameof(AprovacaoComercialPendenteBomba.ObraSeq)}");
            sql.AppendLine($", nivel_hierarquia = @{nameof(AprovacaoComercialPendenteBomba.NivelHierarquia)}");
            sql.AppendLine($", aprovacao_status = @{nameof(AprovacaoComercialPendenteBomba.AprovacaoStatus)}");
            sql.AppendLine($", aprovacao_data = @{nameof(AprovacaoComercialPendenteBomba.AprovacaoData)}");
            sql.AppendLine($", aprovacao_usuario = @{nameof(AprovacaoComercialPendenteBomba.AprovacaoUsuario)}");
            sql.AppendLine($", aprovacao_sequencia = @{nameof(AprovacaoComercialPendenteBomba.AprovacaoSequencia)}");

            _context.Database.Connection.Execute(sql.ToString(), pendente);
            _context.Database.Connection.GravarLogGeral("CRM", "con_aprovacao_comercial_pendente_bomba", sql.ToString(), pendente);

        }

        public void AdicionarAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente)
        {

            var sql = new StringBuilder();

            sql.Clear();
            sql.AppendLine($"REPLACE INTO topsys.con_aprovacao_comercial_pendente_volume SET");
            sql.AppendLine($"id = @{nameof(AprovacaoComercialPendenteVolume.Id)}");
            sql.AppendLine($", id_aprovacao = @{nameof(AprovacaoComercialPendenteVolume.IdAprovacao)}");
            sql.AppendLine($", obra_versao = @{nameof(AprovacaoComercialPendenteVolume.ObraVersao)}");
            sql.AppendLine($", obra_usina = @{nameof(AprovacaoComercialPendenteVolume.ObraUsina)}");
            sql.AppendLine($", obra_numero = @{nameof(AprovacaoComercialPendenteVolume.ObraNumero)}");
            sql.AppendLine($", nivel_hierarquia = @{nameof(AprovacaoComercialPendenteVolume.NivelHierarquia)}");
            sql.AppendLine($", aprovacao_status = @{nameof(AprovacaoComercialPendenteVolume.AprovacaoStatus)}");
            sql.AppendLine($", aprovacao_data = @{nameof(AprovacaoComercialPendenteVolume.AprovacaoData)}");
            sql.AppendLine($", aprovacao_usuario = @{nameof(AprovacaoComercialPendenteVolume.AprovacaoUsuario)}");
            sql.AppendLine($", aprovacao_sequencia = @{nameof(AprovacaoComercialPendenteVolume.AprovacaoSequencia)}");

            _context.Database.Connection.Execute(sql.ToString(), pendente);
            _context.Database.Connection.GravarLogGeral("CRM", "con_aprovacao_comercial_pendente_volume", sql.ToString(), pendente);

        }

        public void AdicionarAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente)
        {

            var sql = new StringBuilder();

            sql.Clear();
            sql.AppendLine($"REPLACE INTO topsys.con_aprovacao_comercial_pendente_cond_pagto SET");
            sql.AppendLine($"id = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.Id)}");
            sql.AppendLine($", id_aprovacao = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.IdAprovacao)}");
            sql.AppendLine($", obra_versao = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.ObraVersao)}");
            sql.AppendLine($", obra_usina = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.ObraUsina)}");
            sql.AppendLine($", obra_numero = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.ObraNumero)}");
            sql.AppendLine($", nivel_hierarquia = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.NivelHierarquia)}");
            sql.AppendLine($", aprovacao_status = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.AprovacaoStatus)}");
            sql.AppendLine($", aprovacao_data = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.AprovacaoData)}");
            sql.AppendLine($", aprovacao_usuario = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.AprovacaoUsuario)}");
            sql.AppendLine($", aprovacao_sequencia = @{nameof(AprovacaoComercialPendenteCondicaoPagamento.AprovacaoSequencia)}");

            _context.Database.Connection.Execute(sql.ToString(), pendente);
            _context.Database.Connection.GravarLogGeral("CRM", "con_aprovacao_comercial_pendente_cond_pagto", sql.ToString(), pendente);

        }

        public void AdicionarAprovacoesPendentes(List<AprovacaoComercialPendente> pendentes)
        {

            foreach(var pendente in pendentes)
            {

                RemoverAprovacaoPendente(pendente);

                var sql = new StringBuilder();

                sql.AppendLine($"REPLACE INTO topsys.con_aprovacao_comercial_pendente SET");
                sql.AppendLine($"id = @{nameof(AprovacaoComercialPendente.Id)}");
                sql.AppendLine($", data_criacao = @{nameof(AprovacaoComercialPendente.DataCriacao)}");
                sql.AppendLine($", obra_versao = @{nameof(AprovacaoComercialPendente.ObraVersao)}");
                sql.AppendLine($", obra_usina = @{nameof(AprovacaoComercialPendente.ObraUsina)}");
                sql.AppendLine($", obra_numero = @{nameof(AprovacaoComercialPendente.ObraNumero)}");
                sql.AppendLine($", nivel_hierarquia = @{nameof(AprovacaoComercialPendente.NivelHierarquia)}");
                sql.AppendLine($", aprovacao_status = @{nameof(AprovacaoComercialPendente.AprovacaoStatus)}");
                sql.AppendLine($", aprovacao_data = @{nameof(AprovacaoComercialPendente.AprovacaoData)}");

                _context.Database.Connection.Execute(sql.ToString(), pendente);
                _context.Database.Connection.GravarLogGeral("CRM", "con_aprovacao_comercial_pendente", sql.ToString(), pendente);

                foreach (var traco in pendente.Tracos)
                {
                    AdicionarAprovacaoPendenteTraco(traco);
                }

                foreach (var bomba in pendente.Bombas)
                {
                    AdicionarAprovacaoPendenteBomba(bomba);
                }

                foreach (var volume in pendente.Volumes)
                {
                    AdicionarAprovacaoPendenteVolume(volume);
                }

                foreach (var condicaoPagamento in pendente.CondicoesPagamento)
                {
                    AdicionarAprovacaoPendenteCondicaoPagamento(condicaoPagamento);
                }

            }


        }

        public AprovacaoComercialPendente ObterAprovacaoPendentePorObraVersaoNivelHierarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia)
        {
            var whereComplement = new StringBuilder();
            whereComplement.AppendLine($"aprovacao_status IN ({(int)Domain.Enums.EAprovacaoComercialPendenteStatus.AguardandoAprovacao}, {(int)Domain.Enums.EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior})");
            whereComplement.AppendLine($"AND nivel_hierarquia = {nivelHierarquia}");

            return ListarTodas(obraUsina, obraNumero, obraVersao, whereComplement.ToString()).FirstOrDefault();
        }

        public AprovacaoComercialPendente ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia)
        {
            var whereComplement = new StringBuilder();
            whereComplement.AppendLine($"aprovacao_status IN ({(int)Domain.Enums.EAprovacaoComercialPendenteStatus.Reprovado})");
            whereComplement.AppendLine($"AND nivel_hierarquia = {nivelHierarquia}");

            return ListarTodas(obraUsina, obraNumero, obraVersao, whereComplement.ToString()).FirstOrDefault();
        }

        private bool UsuarioJaRealizouAprovacaoPendente(string tabela, int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, string usuarioId, int obraSequencia = 0)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELEC COUNT(*) Contagem");
            sql.AppendLine($"FROM con_aprovacao_comercial_pendente_{tabela} WHERE");
            sql.AppendLine($"    obra_usina = @{nameof(obraUsina)}");
            sql.AppendLine($"AND obra_numero = @{nameof(obraNumero)}");
            sql.AppendLine($"AND obra_versao = @{nameof(obraVersao)}");

            if(obraSequencia > 0)
                sql.AppendLine($"AND obra_seq = @{nameof(obraSequencia)}");

            sql.AppendLine($"AND nivel_hierarquia = @{nameof(nivelHierarquia)}");
            sql.AppendLine($"AND aprovacao_usuario = @{nameof(usuarioId)}");

            var parametros = new
            {
                obraUsina,
                obraNumero,
                obraVersao,
                obraSequencia,
                nivelHierarquia,
                usuarioId
            };

            var result = _context.Database.Connection.QueryFirst<int>(sql.ToString(), parametros);

            return result > 0;

        }

        public bool UsuarioJaRealizouAprovacaoPendenteTracoDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaTraco, string usuarioId)
        {
            return UsuarioJaRealizouAprovacaoPendente("traco", obraUsina, obraNumero, obraVersao, nivelHierarquia, usuarioId, sequenciaTraco);
        }

        public bool UsuarioJaRealizouAprovacaoPendenteBombaDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaBomba, string usuarioId)
        {
            return UsuarioJaRealizouAprovacaoPendente("bomba", obraUsina, obraNumero, obraVersao, nivelHierarquia, usuarioId, sequenciaBomba);
        }

        public IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentePorObraVersao(int obraUsina, int obraNumero, int obraVersao)
        {
            var whereComplement = $"aprovacao_status IN ({(int)Domain.Enums.EAprovacaoComercialPendenteStatus.AguardandoAprovacao}, {(int)Domain.Enums.EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior})";
            return ListarTodas(obraUsina, obraNumero, obraVersao, whereComplement);
        }

        public IEnumerable<AprovacaoComercialPendente> ListarTodasAprovacoesPorObraVersao(int obraUsina, int obraNumero, int obraVersao)
        {
            return ListarTodas(obraUsina, obraNumero, obraVersao);
        }

        private IEnumerable<AprovacaoComercialPendente> ListarTodas(int obraUsina, int obraNumero, int obraVersao, string whereComplement = "")
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"   DISTINCT(nivel_hierarquia) {nameof(AprovacaoComercialPendente.NivelHierarquia)}");
            sql.AppendLine($",  id {nameof(AprovacaoComercialPendente.Id)}");
            sql.AppendLine($",  data_criacao {nameof(AprovacaoComercialPendente.DataCriacao)}");
            sql.AppendLine($",  obra_versao {nameof(AprovacaoComercialPendente.ObraVersao)}");
            sql.AppendLine($",  obra_usina {nameof(AprovacaoComercialPendente.ObraUsina)}");
            sql.AppendLine($",  obra_numero {nameof(AprovacaoComercialPendente.ObraNumero)}");
            sql.AppendLine($",  CAST(aprovacao_status AS SIGNED) {nameof(AprovacaoComercialPendente.AprovacaoStatus)}");
            sql.AppendLine($",  aprovacao_data {nameof(AprovacaoComercialPendente.AprovacaoData)}");
            sql.AppendLine($"FROM topsys.con_aprovacao_comercial_pendente");
            sql.AppendLine($"WHERE obra_numero = @{nameof(obraNumero)}");
            sql.AppendLine($"  AND obra_usina = @{nameof(obraUsina)}");
            sql.AppendLine($"  AND obra_versao = @{nameof(obraVersao)}");
            
            if(!string.IsNullOrEmpty(whereComplement))
                sql.AppendLine($"{(whereComplement.Trim().StartsWith("AND", StringComparison.OrdinalIgnoreCase) ? "" : " AND ")} {whereComplement}");

            sql.AppendLine($"ORDER BY data_criacao DESC");

            //var pendentes = ConvertFromDynamic<AprovacaoComercialPendente>(_context.Database.Connection.Query(sql.ToString(), new { obraUsina, obraNumero, obraVersao }));
            var pendentesQuery = _context.Database.Connection.Query<AprovacaoComercialPendente>(sql.ToString(), new { obraUsina, obraNumero, obraVersao });
            var niveis = pendentesQuery.Select(x => x.NivelHierarquia).Distinct().ToList();
            var pendentes = new List<AprovacaoComercialPendente>();

            foreach (var nivel in niveis)
            {
                var pendenteNivel = pendentesQuery.Where(x => x.NivelHierarquia == nivel).OrderByDescending(x => x.DataCriacao);
                pendentes.Add(pendenteNivel.FirstOrDefault());
            }
            
            var pendentesId = pendentes.Select(x => x.Id).ToList();

            if (pendentes.Count() == 0)
                return pendentes;

            sql.Clear();
            sql.AppendLine($"SELECT");
            sql.AppendLine($"   id {nameof(AprovacaoComercialPendenteTraco.Id)}");
            sql.AppendLine($",  id_aprovacao {nameof(AprovacaoComercialPendenteTraco.IdAprovacao)}");
            sql.AppendLine($",  obra_versao {nameof(AprovacaoComercialPendenteTraco.ObraVersao)}");
            sql.AppendLine($",  obra_usina {nameof(AprovacaoComercialPendenteTraco.ObraUsina)}");
            sql.AppendLine($",  obra_numero {nameof(AprovacaoComercialPendenteTraco.ObraNumero)}");
            sql.AppendLine($",  nivel_hierarquia {nameof(AprovacaoComercialPendenteTraco.NivelHierarquia)}");
            sql.AppendLine($",  CAST(aprovacao_status AS SIGNED) {nameof(AprovacaoComercialPendenteTraco.AprovacaoStatus)}");
            sql.AppendLine($",  aprovacao_data {nameof(AprovacaoComercialPendenteTraco.AprovacaoData)}");
            sql.AppendLine($",  aprovacao_usuario {nameof(AprovacaoComercialPendenteTraco.AprovacaoUsuario)}");
            sql.AppendLine($",  aprovacao_sequencia {nameof(AprovacaoComercialPendenteTraco.AprovacaoSequencia)}");
            sql.AppendLine($"@EXTRA");
            sql.AppendLine($"FROM topsys.con_aprovacao_comercial_pendente_@TABELA");
            sql.AppendLine($"WHERE id_aprovacao IN @{nameof(pendentesId)}");

            var sqlTraco = sql.ToString().Replace("@TABELA", "traco").Replace("@EXTRA", $",  obra_seq {nameof(AprovacaoComercialPendenteTraco.ObraSeq)}");
            var sqlBomba = sql.ToString().Replace("@TABELA", "bomba").Replace("@EXTRA", $",  obra_seq {nameof(AprovacaoComercialPendenteBomba.ObraSeq)}");
            var sqlVolume = sql.ToString().Replace("@TABELA", "volume").Replace("@EXTRA", $"");
            var sqlCondPagamento = sql.ToString().Replace("@TABELA", "cond_pagto").Replace("@EXTRA", $"");

            var parametro = new { pendentesId };
            var pendentesTraco = _context.Database.Connection.Query<AprovacaoComercialPendenteTraco>(sqlTraco, parametro);
            var pendentesBomba = _context.Database.Connection.Query<AprovacaoComercialPendenteBomba>(sqlBomba, parametro);
            var pendentesVolume = _context.Database.Connection.Query<AprovacaoComercialPendenteVolume>(sqlVolume, parametro);
            var pendentesCondPagamento = _context.Database.Connection.Query<AprovacaoComercialPendenteCondicaoPagamento>(sqlCondPagamento, parametro);

            foreach (var pendente in pendentes)
            {
                pendente.Tracos = pendentesTraco.Where(x => x.IdAprovacao == pendente.Id).ToList();
                pendente.Bombas = pendentesBomba.Where(x => x.IdAprovacao == pendente.Id).ToList();
                pendente.Volumes = pendentesVolume.Where(x => x.IdAprovacao == pendente.Id).ToList();
                pendente.CondicoesPagamento = pendentesCondPagamento.Where(x => x.IdAprovacao == pendente.Id).ToList();
            }

            return pendentes;

        }

    }
}
