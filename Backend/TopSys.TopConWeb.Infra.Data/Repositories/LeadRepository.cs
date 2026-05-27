using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class LeadRepository : RepositoryBase<Lead>, ILeadRepository
    {
        private readonly ILeadContatoRepository _leadContatoRepository;
        private readonly IdentityHelperService _identityHelperService;

        public LeadRepository(AppDataContext context, ILeadContatoRepository leadContatoRepository, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _leadContatoRepository = leadContatoRepository;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(Lead lead)
        {
            var sqlComando = lead.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead", sqlComando.ToString());

            lead.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_LEAD_INSERIDA").FirstOrDefault();

            var log = new LeadLog()
            {
                Usina = lead.UsinaCodigo,
                AnoLead = lead.Ano,
                NumeroLead = lead.Numero,
                Sequencia = 1,
                DataHoraEvento = DateTime.Now,
                Usuario = _identityHelperService.GetUserName(),
                Evento = "INSERÇÃO LEAD",
                Complemento = $"Inserido no sistema o lead {lead.Numero}-{lead.Ano} dia {DateTime.Now.ToString("dd/MM/yy")} às {DateTime.Now.ToString("HH:mm")}." +
                              $" Fase: { lead.Fase.Descricao }." +
                              $" Classificação: { lead.GetClassificacaoDescricao }.",
                Tipo = ""
            };

            sqlComando = log.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead_log", sqlComando.ToString());
        }

        public void AdicionarAnexo(string usuario, Guid id, int usina, int anoLead, int numeroLead, string anexo, string nome)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT IGNORE INTO topsys.con_lead_anexo SET ");
            sql.Append($"descricao=@{nameof(nome)}, ");
            sql.Append($"nome=@{nameof(nome)}, ");
            sql.Append($"usuario=@{nameof(usuario)}, ");
            sql.Append($"data=CURRENT_DATE(), ");
            sql.Append($"data_hora=NOW(), ");
            sql.Append($"arquivo=@{nameof(anexo)}, ");
            sql.Append($"id=@{nameof(id)}, ");
            sql.Append($"usina=@{nameof(usina)}, ");
            sql.Append($"ano_lead=@{nameof(anoLead)}, ");
            sql.Append($"numero_lead=@{nameof(numeroLead)} ");

            _context.Database.Connection.Execute(sql.ToString(), new { nome, usuario, anexo, id, usina, anoLead, numeroLead });
        }

        public ICollection<LeadAnexo> ListarAnexos(int usina, int anoLead, int numeroLead)
        {
            return _context.LeadAnexos
                    .Where(t => t.Usina == usina && t.AnoLead == anoLead && t.NumeroLead == numeroLead)
                    .OrderByDescending(t => t.DataHora)
                    .AsNoTracking()
                    .ToList();
        }

        public byte[] ObterAnexo(Guid id)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT arquivo");
            sql.Append($" FROM topsys.con_lead_anexo");
            sql.Append($" WHERE id=@{nameof(id)}");

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { id });
        }

        public LeadAnexo ObterLeadAnexoPorId(Guid id)
        {
            return _context.LeadAnexos.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AtualizarDescricaoAnexo(LeadAnexo anexo)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.con_lead_anexo SET");
            sql.Append($"   descricao=@{nameof(anexo.Descricao)}");
            sql.Append($"   WHERE id=@{nameof(anexo.Id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { anexo.Descricao, anexo.Id });
        }

        public void RemoverAnexo(Guid id)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.con_lead_anexo WHERE ");
            sql.Append($"id=@{nameof(id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { id });
        }

        public PagedList<Lead> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<System.Func<Lead, bool>> filter)
        {
            var pagedList = _context.Leads
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.ViaCaptacao)
                .Include(t => t.Fase)
                .Include(t => t.MotivoPerda)
                .Where(filter)
                .AsNoTracking()
                .PagedList(pagina, porPagina, t => t.Usina.Nome != null);

            foreach (var record in pagedList.Records)
            {
                record.ContatoPrincipal = _leadContatoRepository.ObterPorId(record.UsinaCodigo, record.Ano, record.Numero, 1);
                record.ContatoSecundario = _leadContatoRepository.ObterPorId(record.UsinaCodigo, record.Ano, record.Numero, 2);
            }

            return pagedList;
        }

        public Lead ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false)
        {
            var leadResult = _context.Leads
                .Where(t => t.UsinaCodigo == idUsina && t.Ano == ano && t.Numero == numero)
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.ViaCaptacao)
                .Include(t => t.Fase)
                .Include(t => t.MotivoPerda)
                .Tracking(tracking)
                .FirstOrDefault();

            leadResult.ContatoPrincipal = _context.LeadContatos
                .Where(t => t.Usina == idUsina && t.AnoLead == ano && t.NumeroLead == numero && t.Sequencia == 1)
                .Include(t => t.Funcao)
                .Tracking(tracking)
                .FirstOrDefault();

            leadResult.ContatoSecundario = _context.LeadContatos
                .Where(t => t.Usina == idUsina && t.AnoLead == ano && t.NumeroLead == numero && t.Sequencia == 2)
                .Include(t => t.Funcao)
                .Tracking(tracking)
                .FirstOrDefault();

            if (leadResult.ContatoSecundario == null) leadResult.ContatoSecundario = new LeadContato();

            return leadResult;
        }

        public PagedList<LeadInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<LeadInteracao, bool>> filter)
        {
            return _context.LeadInteracoes
                    .OrderByDescending(t => new { t.Data, t.Hora })
                    .Where(filter)
                    .AsNoTracking()
                    .PagedList(pagina, porPagina, t => t.Usina != 0);
        }

        public void AdicionarInteracao(string usuario, LeadInteracao leadInteracao)
        {
            var sqlComando = leadInteracao.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead_hist", sqlComando.ToString());
        }
    }
}
