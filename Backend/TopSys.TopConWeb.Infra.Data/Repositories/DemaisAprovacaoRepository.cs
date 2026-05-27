using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class DemaisAprovacaoRepository : RepositoryBase<DemaisAprovacao>, IDemaisAprovacaoRepository
    {
        public DemaisAprovacaoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int usinaCodigo, int obraCodigo)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT");
            sqlCommand.Append(" aprov.chave");
            sqlCommand.Append(", aprov.tipo_aprov AprovacaoTipoCodigo");
            sqlCommand.Append(", aprov.usuario_req UsuarioRequisitante");
            sqlCommand.Append(", aprov.usuario_aprov UsuarioAprovacao");
            sqlCommand.Append(", aprov.dt_hora_solic DataHoraSolicitacao");
            sqlCommand.Append(", aprov.dt_hora_exec DataHoraExecucao");
            sqlCommand.Append(", aprov.complemento Complemento");
            sqlCommand.Append(", aprov.observacao Observacao");
            sqlCommand.Append(" FROM topsys.con_obras obra");
            sqlCommand.Append(" INNER JOIN topsys.con_aprov aprov");
            sqlCommand.Append(" ON SUBSTRING_INDEX(aprov.complemento , '/', 1 )= obra.usina");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX(aprov.complemento, '/', -1), '-', 1)= obra.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX(aprov.complemento, '/', 2), '/', -1)= obra.no_chamada");
            sqlCommand.Append(" WHERE obra.usina=@UsinaCodigo");
            sqlCommand.Append(" AND obra.numero=@ObraCodigo");


            ICollection<DemaisAprovacao> aprovacoesComerciais = _context.DemaisAprovacoes.SqlQuery(sqlCommand.ToString(),
                new MySqlParameter("@UsinaCodigo", usinaCodigo),
                new MySqlParameter("@ObraCodigo", obraCodigo)
            ).ToList();

            //Carregando o objetos AprovacaoTipo
            foreach (var aprovacao in aprovacoesComerciais)
                _context.Entry(aprovacao).Reference(c => c.AprovacaoTipo).Load();

            return aprovacoesComerciais;
        }

        public ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int numVersao, int usinaCodigo, int obraCodigo)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT");
            sqlCommand.Append(" aprov.chave");
            sqlCommand.Append(", aprov.tipo_aprov AprovacaoTipoCodigo");
            sqlCommand.Append(", aprov.usuario_req UsuarioRequisitante");
            sqlCommand.Append(", aprov.usuario_aprov UsuarioAprovacao");
            sqlCommand.Append(", aprov.dt_hora_solic DataHoraSolicitacao");
            sqlCommand.Append(", aprov.dt_hora_exec DataHoraExecucao");
            sqlCommand.Append(", aprov.complemento Complemento");
            sqlCommand.Append(", aprov.observacao Observacao");
            sqlCommand.Append(" FROM topsys.con_obras_versao obra");
            sqlCommand.Append(" INNER JOIN topsys.con_aprov_versao aprov");
            sqlCommand.Append(" ON SUBSTRING_INDEX(aprov.complemento , '/', 1 )= obra.usina");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX(aprov.complemento, '/', -1), '-', 1)= obra.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX(aprov.complemento, '/', 2), '/', -1)= obra.no_chamada");
            sqlCommand.Append(" AND aprov.num_versao= obra.num_versao");
            sqlCommand.Append(" WHERE obra.usina=@UsinaCodigo");
            sqlCommand.Append(" AND obra.numero=@ObraCodigo");
            sqlCommand.Append(" AND obra.num_versao=@NumVersao");


            ICollection<DemaisAprovacao> aprovacoesComerciais = _context.DemaisAprovacoes.SqlQuery(sqlCommand.ToString(),
                new MySqlParameter("@UsinaCodigo", usinaCodigo),
                new MySqlParameter("@ObraCodigo", obraCodigo),
                new MySqlParameter("@NumVersao", numVersao)
            ).ToList();

            //Carregando o objetos AprovacaoTipo
            foreach (var aprovacao in aprovacoesComerciais)
                _context.Entry(aprovacao).Reference(c => c.AprovacaoTipo).Load();

            return aprovacoesComerciais;
        }

        public void RemoverAprovacoes(string id)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM con_aprov WHERE chave='{id}'");

            _context.Connection.Execute(sqlComando.ToString());
        }
    }
}
