using Dapper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class OportunidadeRepository : RepositoryBase<Oportunidade>, IOportunidadeRepository
    {

        private readonly IdentityHelperService _identityHelperService;
        private readonly ILeadRepository _leadRepository;

        public OportunidadeRepository(ILeadRepository leadRepository, AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _identityHelperService = identityHelperService;
            _leadRepository = leadRepository;
        }

        new public void Adicionar(Oportunidade oportunidade)
        {
            var sqlComando = oportunidade.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_oportunidade", sqlComando.ToString());

            oportunidade.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_OPORTUNIDADE_INSERIDA").FirstOrDefault();

            if (oportunidade.ContatoPrincipal != null && !string.IsNullOrWhiteSpace(oportunidade.ContatoPrincipal.Nome))
            {
                var visitaContato = oportunidade.ContatoPrincipal;

                visitaContato.AnoOportunidade = oportunidade.Ano;
                visitaContato.NumeroOportunidade = oportunidade.Numero;
                visitaContato.Usina = oportunidade.UsinaCodigo;
                visitaContato.Sequencia = 1;

                sqlComando = visitaContato.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_oportunidade_contato", sqlComando.ToString());

            }

            if (oportunidade.ContatoSecundario != null && !string.IsNullOrWhiteSpace(oportunidade.ContatoSecundario.Nome))
            {

                var visitaContato = oportunidade.ContatoSecundario;

                visitaContato.AnoOportunidade = oportunidade.Ano;
                visitaContato.NumeroOportunidade = oportunidade.Numero;
                visitaContato.Usina = oportunidade.UsinaCodigo;
                visitaContato.Sequencia = 2;

                sqlComando = visitaContato.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_oportunidade_contato", sqlComando.ToString());

            }

            var faseOportunidade = _context.OportunidadeFases.Where(x => x.Codigo == oportunidade.FaseCodigo).FirstOrDefault();
            var classificacao = oportunidade.Classificacao.ToString();

            var logComplemento = new StringBuilder();
            logComplemento.Append($"Inserido a oportunidade {oportunidade.Numero}-{oportunidade.Ano} ");
            logComplemento.Append($"para o cliente {(oportunidade.IntervenienteCodigo == 0 ? "" : $"{oportunidade.IntervenienteCodigo}-")}{oportunidade.Cliente} ");
            logComplemento.Append($"na fase {(faseOportunidade != null ? faseOportunidade.Descricao : oportunidade.FaseCodigo.ToString())} ");
            logComplemento.Append($"com classificação {classificacao}.");


            var log = new OportunidadeLog()
            {

                Usina = oportunidade.UsinaCodigo,
                Ano = oportunidade.Ano,
                Numero = oportunidade.Numero,

                DataHoraEvento = DateTime.Now,
                Usuario = _identityHelperService.GetUserName(),
                Evento = "INSERÇÃO OPORTUNIDADE",
                Complemento = logComplemento.ToString(),
                Tipo = "VISITAS"

            };

            sqlComando = log.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_oportunidade_log", sqlComando.ToString());

            if (oportunidade.NumeroLead != 0)
            {
                int sequenciaLeadLog = _leadRepository.ListarFiltrados<LeadLog>(t => t.Usina == oportunidade.UsinaCodigo && t.AnoLead == oportunidade.AnoLead && t.NumeroLead == oportunidade.NumeroLead).Max(t => t.Sequencia) + 1;

                var logLead = new LeadLog()
                {
                    Usina = oportunidade.UsinaCodigo,
                    AnoLead = oportunidade.AnoLead,
                    NumeroLead = oportunidade.NumeroLead,
                    Sequencia = sequenciaLeadLog,
                    DataHoraEvento = DateTime.Now,
                    Usuario = _identityHelperService.GetUserName(),
                    Evento = "ALTERAÇÃO LEAD",
                    Complemento = $"Oportunidade gerada {oportunidade.Numero}-{oportunidade.Ano}.",
                    Tipo = ""
                };

                sqlComando = logLead.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead_log", sqlComando.ToString());

                var lead = _leadRepository.ObterPorId(oportunidade.UsinaCodigo, oportunidade.AnoLead, oportunidade.NumeroLead);
                lead.OportunidadeNumero = oportunidade.Numero;
                lead.OportunidadeAno = oportunidade.Ano;

                sqlComando = lead.MontarSqlUpdate(_context);
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead", sqlComando.ToString());
            }
        }

        public PagedList<Oportunidade> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Oportunidade, bool>> filter)
        {
            var pagedList = _context.Oportunidades
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .Include(t => t.Usina)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Vendedor)
                .Include(t => t.Segmentacao)
                .Include(t => t.MotivoPerda)
                .Include(t => t.OportunidadeTipo)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.ViaCaptacao)
                .Include(t => t.Fase)
                .Include(t => t.Concorrente)
                .Include(t => t.PorteObra)
                .Include(t => t.Logs)
                .Include(t => t.Propostas)
                .Where(filter)
                .AsNoTracking()
                .PagedList(pagina, porPagina, t => t.Cliente != null);

            foreach (var oportunidade in pagedList.Records)
            {

                var contatos = _context.OportunidadeContatos.Where(x => x.Usina == oportunidade.UsinaCodigo
                    && x.AnoOportunidade == oportunidade.Ano
                    && x.NumeroOportunidade == oportunidade.Numero).ToList();

                oportunidade.ContatoPrincipal = contatos.FirstOrDefault(x => x.Sequencia == 1);
                oportunidade.ContatoSecundario = contatos.FirstOrDefault(x => x.Sequencia == 2);

            }

            return pagedList;

        }

        public Oportunidade ObterPorId(int usina, int ano, int numero, bool tracking = false)
        {

            var oportunidade = _context.Oportunidades
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .Include(t => t.Usina)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Vendedor)
                .Include(t => t.Segmentacao)
                .Include(t => t.MotivoPerda)
                .Include(t => t.OportunidadeTipo)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.ViaCaptacao)
                .Include(t => t.Fase)
                .Include(t => t.Concorrente)
                .Include(t => t.PorteObra)
                .Include(t => t.Logs)
                .Where(x => x.UsinaCodigo == usina && x.Ano == ano && x.Numero == numero)
                .Tracking(tracking)
                .FirstOrDefault();

            if (oportunidade == null)
                return null;

            var contatos = _context.OportunidadeContatos.Where(x => x.Usina == oportunidade.UsinaCodigo
                    && x.AnoOportunidade == oportunidade.Ano
                    && x.NumeroOportunidade == oportunidade.Numero).Tracking(tracking).ToList();

            oportunidade.ContatoPrincipal = contatos.FirstOrDefault(x => x.Sequencia == 1);
            oportunidade.ContatoSecundario = contatos.FirstOrDefault(x => x.Sequencia == 2);

            return oportunidade;

        }

        public PagedList<OportunidadeInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<OportunidadeInteracao, bool>> filter)
        {
            return _context.OportunidadeInteracoes
                    .OrderByDescending(t => new { t.Data, t.Hora })
                    .Where(filter)
                    .AsNoTracking()
                    .PagedList(pagina, porPagina, t => t.Usina != 0);
        }

        public void AdicionarInteracao(string usuario, OportunidadeInteracao oportunidadeInteracao)
        {
            var sqlComando = oportunidadeInteracao.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_oportunidade_hist", sqlComando.ToString());
        }



    }
}
