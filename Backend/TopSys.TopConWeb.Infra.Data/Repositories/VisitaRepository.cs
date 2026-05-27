using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class VisitaRepository : RepositoryBase<Visita>, IVisitaRepository
    {

        private readonly IdentityHelperService _identityHelperService;

        public VisitaRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(Visita visita)
        {
            var sqlComando = visita.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_visita_cliente", sqlComando.ToString());

            visita.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_VISITA_INSERIDA").FirstOrDefault();

            if(visita.ContatoPrincipal != null && !string.IsNullOrWhiteSpace(visita.ContatoPrincipal.Nome))
            {
                var visitaContato = visita.ContatoPrincipal;

                visitaContato.AnoVisita = visita.Ano;
                visitaContato.NumeroVisita = visita.Numero;
                visitaContato.Usina = visita.UsinaCodigo;
                visitaContato.Sequencia = 1;

                sqlComando = visitaContato.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_visita_contato", sqlComando.ToString());

            }

            if (visita.ContatoSecundario != null && !string.IsNullOrWhiteSpace(visita.ContatoSecundario.Nome))
            {

                var visitaContato = visita.ContatoSecundario;

                visitaContato.AnoVisita = visita.Ano;
                visitaContato.NumeroVisita = visita.Numero;
                visitaContato.Usina = visita.UsinaCodigo;
                visitaContato.Sequencia = 2;

                sqlComando = visitaContato.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_visita_contato", sqlComando.ToString());

            }

            var tipoVisita = _context.TiposVisitas.Where(x => x.Codigo == visita.VisitaTipoCodigo).FirstOrDefault();

            var log = new VisitaLog()
            {

                Usina = visita.UsinaCodigo,
                Ano = visita.Ano,
                Numero = visita.Numero,

                DataHoraEvento = DateTime.Now,
                Usuario = _identityHelperService.GetUserName(),
                Evento = "INSERÇÃO VISITA",
                Complemento = $"Inserido a visita {visita.Numero}-{visita.Ano} com tipo de visita {(tipoVisita != null ? tipoVisita.Descricao : "")} as {DateTime.Now.ToString("dd/MM/yy HH:mm")}.",
                Tipo = "VISITAS"

            };

            sqlComando = log.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_visita_cliente_log", sqlComando.ToString());

        }

        public PagedList<Visita> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Visita, bool>> filter)
        {
            var pagedList = _context.Visitas
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.TipoVisita)
                .Include(t => t.Logs)
                .Where(filter)
                .AsNoTracking()
                .PagedList(pagina, porPagina, t => t.Cliente != null); // cláusula para forçar o inner join com Obra no Count()

            foreach(var visita in pagedList.Records)
            {

                var contatos = _context.VisitaContatos.Where(x => x.Usina == visita.UsinaCodigo
                    && x.AnoVisita == visita.Ano
                    && x.NumeroVisita == visita.Numero).ToList();

                visita.ContatoPrincipal = contatos.FirstOrDefault(x => x.Sequencia == 1);
                visita.ContatoSecundario = contatos.FirstOrDefault(x => x.Sequencia == 2);

            }

            return pagedList;

        }

        public PagedList<VisitaHistorico> ListarHistoricoEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<VisitaHistorico, bool>> filter)
        {


            var pagedList = _context.VisitaHistoricos
                .OrderByDescending(t => new { t.Data, t.Hora })
                .Where(filter)
                .AsNoTracking()
                .PagedList(pagina, porPagina, t => t.Id != null); // cláusula para forçar o inner join com Obra no Count()

            return pagedList;

        }

        public Visita ObterPorId(int usina, int ano, int numero, bool tracking = false)
        {

            var visita = _context.Visitas
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.TipoVisita)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.Logs)
                .Where(x => x.UsinaCodigo == usina && x.Ano == ano && x.Numero == numero)
                .Tracking(tracking)
                .FirstOrDefault();

            if (visita == null)
                return null;

            var contatos = _context.VisitaContatos
                    .Include(t => t.Funcao)
                    .Where(x => x.Usina == visita.UsinaCodigo
                    && x.AnoVisita == visita.Ano
                    && x.NumeroVisita == visita.Numero).Tracking(tracking).ToList();

            visita.ContatoPrincipal = contatos.FirstOrDefault(x => x.Sequencia == 1);
            visita.ContatoSecundario = contatos.FirstOrDefault(x => x.Sequencia == 2);

            return visita;

        }

        public void AdicionarAnexo(string usuario, Guid id, int usina, int anoVisita, int numeroVisita, string anexo, string nome)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT IGNORE INTO topsys.con_visita_anexo SET ");
            sql.Append($"descricao=@{nameof(nome)}, ");
            sql.Append($"nome=@{nameof(nome)}, ");
            sql.Append($"usuario=@{nameof(usuario)}, ");
            sql.Append($"data=CURRENT_DATE(), ");
            sql.Append($"data_hora=NOW(), ");
            sql.Append($"arquivo=@{nameof(anexo)}, ");
            sql.Append($"id=@{nameof(id)}, ");
            sql.Append($"usina=@{nameof(usina)}, ");
            sql.Append($"ano_visita=@{nameof(anoVisita)}, ");
            sql.Append($"num_visita=@{nameof(numeroVisita)} ");

            _context.Database.Connection.Execute(sql.ToString(), new { nome, usuario, anexo, id, usina, anoVisita, numeroVisita });
        }

        public ICollection<VisitaAnexo> ListarAnexos(int usina, int anoVisita, int numeroVisita)
        {
            return _context.VisitaAnexos
                    .Where(t => t.Usina == usina && t.Ano == anoVisita && t.Numero == numeroVisita)
                    .OrderByDescending(t => t.DataHora)
                    .AsNoTracking()
                    .ToList();
        }

        public byte[] ObterAnexo(Guid id)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT arquivo");
            sql.Append($" FROM topsys.con_visita_anexo");
            sql.Append($" WHERE id=@{nameof(id)}");

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { id });
        }

        public void AtualizarDescricaoAnexo(VisitaAnexo anexo)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.con_visita_anexo SET");
            sql.Append($"   descricao=@{nameof(anexo.Descricao)}");
            sql.Append($"   WHERE id=@{nameof(anexo.Id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { anexo.Descricao, anexo.Id });
        }

        public void RemoverAnexo(Guid id)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.con_visita_anexo WHERE ");
            sql.Append($"id=@{nameof(id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { id });
        }

        public VisitaAnexo ObterVisitaAnexoPorId(Guid id)
        {
            return _context.VisitaAnexos.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AdicionarHistorico(VisitaHistorico visitaHistorico)
        {

            var sqlComando = visitaHistorico.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_visita_cliente_hist", sqlComando.ToString());

        }

    }
}
