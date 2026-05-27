using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ContratoPagamentoRepository : RepositoryBase<ContratoPagamento>, IContratoPagamentoRepository
    {
        private IDatabaseRepository _databaseRepository;
        private readonly IdentityHelperService _identityHelperService;

        public ContratoPagamentoRepository(AppDataContext context, IDatabaseRepository databaseRepository, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
            _databaseRepository = databaseRepository;
        }

        public void AtualizarIdAprovacao(ContratoPagamento contratoPagamento)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.con_contrato_pag ");
            sql.Append($"SET id_aprovacao = @{nameof(contratoPagamento.IdAprovacao)} ");
            sql.Append("WHERE ");
            sql.Append($"usina=@{nameof(contratoPagamento.UsinaCodigo)} ");
            sql.Append($"AND ano_contrato=@{nameof(contratoPagamento.ContratoAno)} ");
            sql.Append($"AND num_contrato=@{nameof(contratoPagamento.ContratoNumero)} ");
            sql.Append($"AND seq=@{nameof(contratoPagamento.Sequencia)} ");


            _context.Database.Connection.Execute(sql.ToString(), contratoPagamento);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_pag", sql.ToString(), contratoPagamento);
        }

        public void AtualizarIdAprovacao(ContratoPagamentoVersao contratoPagamento)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.con_contrato_pag_versao ");
            sql.Append($"SET id_aprovacao = @{nameof(contratoPagamento.IdAprovacao)} ");
            sql.Append("WHERE ");
            sql.Append($"usina=@{nameof(contratoPagamento.UsinaCodigo)} ");
            sql.Append($"AND ano_contrato=@{nameof(contratoPagamento.ContratoAno)} ");
            sql.Append($"AND num_contrato=@{nameof(contratoPagamento.ContratoNumero)} ");
            sql.Append($"AND seq=@{nameof(contratoPagamento.Sequencia)} ");
            sql.Append($"AND num_versao=@{nameof(contratoPagamento.NumeroVersao)} ");


            _context.Database.Connection.Execute(sql.ToString(), contratoPagamento);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_contrato_pag_versao", sql.ToString(), contratoPagamento);
        }

        public void CarregarDetalhes(ContratoPagamento contratoPagamento, bool tracking = false)
        {
            switch (contratoPagamento.TipoCobranca?.Forma?.ToUpper())
            {
                case "CC":
                case "CD":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesCartao
                        .Include(t => t.Bandeira.Portador.Conta)
                        .Include(t => t.Bandeira.Interveniente)
                        .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalhe>().ToList();
                    break;
                case "CH":
                case "CP":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesCheque
                        .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalhe>()
                        .ToList();
                    break;
                case "DP":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesDeposito
                        .Include(t => t.Portador.Conta)
                        .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalhe>()
                        .ToList();
                    break;
                case "DN":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesDinheiro
                        .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalhe>()
                        .ToList();
                    break;
                case "BE":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesBoleto
                        .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalhe>()
                        .ToList();
                    break;
                default:
                    break;
            }
        }

        public void CarregarDetalhes(ContratoPagamentoVersao contratoPagamento, bool tracking = false)
        {
            switch (contratoPagamento.TipoCobranca?.Forma?.ToUpper())
            {
                case "CC":
                case "CD":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesCartaoVersoes
                        .Include(t => t.Bandeira.Portador.Conta)
                        .Include(t => t.Bandeira.Interveniente)
                        .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalheVersao>().ToList();
                    break;
                case "CH":
                case "CP":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesChequeVersoes
                        .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalheVersao>()
                        .ToList();
                    break;
                case "DP":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesDepositoVersoes
                        .Include(t => t.Portador.Conta)
                        .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalheVersao>()
                        .ToList();
                    break;
                case "DN":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesDinheiroVersoes
                        .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalheVersao>()
                        .ToList();
                    break;
                case "BE":
                    contratoPagamento.Detalhes = _context.ContratoPagamentoDetalhesBoletoVersoes
                        .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia)
                        .Tracking(tracking)
                        .AsEnumerable<ContratoPagamentoDetalheVersao>()
                        .ToList();
                    break;
                default:
                    break;
            }
        }

        public TDetalhe BuscarDetalhes<TDetalhe>(string forma, int contratoUsina, int ano, int numero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            var tipo = typeof(TDetalhe);
            if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] {"CC", "CD"}).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesCartao
                    .Where(t => t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesCartao
                    .Where(t => t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesCheque
                    .Where(t => t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesCheque
                    .Where(t => t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "DP" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesDeposito
                    .Where(t => t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "DP" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesDeposito
                    .Where(t => t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "DN" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesDinheiro
                    .Where(t => t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "DN" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesDinheiro
                    .Where(t => t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "BE" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesBoleto
                   .Where(t => t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                   .Tracking(tracking)
                   .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "BE" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesBoleto
                   .Where(t => t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                   .Tracking(tracking)
                   .FirstOrDefault() as TDetalhe;
            }
            else
                return null;
        }

        public TDetalhe BuscarDetalhesVersao<TDetalhe>(string forma, int contratoUsina, int ano, int numero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, int numVersao = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            var tipo = typeof(TDetalhe);
            if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesCartaoVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesCartaoVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesChequeVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesChequeVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "DP" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesDepositoVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "DP" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesDepositoVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "DN" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesDinheiroVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "DN" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesDinheiroVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "BE" }).Contains(forma))
            {
                return _context.ContratoPagamentoDetalhesBoletoVersoes
                   .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.ContratoAno == ano && t.ContratoNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                   .Tracking(tracking)
                   .FirstOrDefault() as TDetalhe;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "BE" }).Contains(forma))
            {
                return _context.PropostaPagamentoDetalhesBoletoVersoes
                   .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina && t.PropostaAno == ano && t.PropostaNumero == numero && t.PagamentoSequencia == pagamentoSequencia && t.DetalheSequencia == detalheSequencia)
                   .Tracking(tracking)
                   .FirstOrDefault() as TDetalhe;
            }
            else
                return null;
        }

        public IEnumerable<TDetalhe> ListarDetalhes<TDetalhe>(string forma, Expression<Func<TDetalhe, bool>> filter, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            var tipo = typeof(TDetalhe);
            if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheCartao, bool>(filter);
                return _context.ContratoPagamentoDetalhesCartao
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheCartao, bool>(filter);
                return _context.PropostaPagamentoDetalhesCartao
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheCartaoVersao, bool>(filter);
                return _context.ContratoPagamentoDetalhesCartaoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "CC", "CD" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheCartaoVersao, bool>(filter);
                return _context.PropostaPagamentoDetalhesCartaoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheCheque, bool>(filter);
                return _context.ContratoPagamentoDetalhesCheque
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheCheque, bool>(filter);
                return _context.PropostaPagamentoDetalhesCheque
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheChequeVersao, bool>(filter);
                return _context.ContratoPagamentoDetalhesChequeVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "CH", "CP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheChequeVersao, bool>(filter);
                return _context.PropostaPagamentoDetalhesChequeVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "DP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheDeposito, bool>(filter);
                return _context.ContratoPagamentoDetalhesDeposito
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "DP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheDeposito, bool>(filter);
                return _context.PropostaPagamentoDetalhesDeposito
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "DP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheDepositoVersao, bool>(filter);
                return _context.ContratoPagamentoDetalhesDepositoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "DP" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheDepositoVersao, bool>(filter);
                return _context.PropostaPagamentoDetalhesDepositoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "DN" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheDinheiro, bool>(filter);
                return _context.ContratoPagamentoDetalhesDinheiro
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "DN" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheDinheiro, bool>(filter);
                return _context.PropostaPagamentoDetalhesDinheiro
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "DN" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheDinheiroVersao, bool>(filter);
                return _context.ContratoPagamentoDetalhesDinheiroVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "DN" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheDinheiroVersao, bool>(filter);
                return _context.PropostaPagamentoDetalhesDinheiroVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalhe)) && (new[] { "BE" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheBoleto, bool>(filter);
                return _context.ContratoPagamentoDetalhesBoleto
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalhe)) && (new[] { "BE" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheBoleto, bool>(filter);
                return _context.PropostaPagamentoDetalhesBoleto
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(ContratoPagamentoDetalheVersao)) && (new[] { "BE" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, ContratoPagamentoDetalheBoletoVersao, bool>(filter);
                return _context.ContratoPagamentoDetalhesBoletoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else if (tipo.Equals(typeof(PropostaPagamentoDetalheVersao)) && (new[] { "BE" }).Contains(forma))
            {
                var convertedFilter = ConvertHelper.ConvertExpression<TDetalhe, PropostaPagamentoDetalheBoletoVersao, bool>(filter);
                return _context.PropostaPagamentoDetalhesBoletoVersoes
                    .Where(convertedFilter)
                    .Tracking(tracking)
                    .ToList() as IEnumerable<TDetalhe>;
            }
            else
                return new List<TDetalhe>();
        }

        public ContratoPagamentoDetalheCheque ObterDetalheCheque(ContratoPagamento contratoPagamento, int detalheSequencia, bool tracking = false)
        {
            return _context.ContratoPagamentoDetalhesCheque
                    .Where(t => t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault();
        }

        public ContratoPagamentoDetalheChequeVersao ObterDetalheCheque(ContratoPagamentoVersao contratoPagamento, int detalheSequencia, bool tracking = false)
        {
            return _context.ContratoPagamentoDetalhesChequeVersoes
                    .Where(t => t.NumeroVersao == contratoPagamento.NumeroVersao && t.UsinaCodigo == contratoPagamento.UsinaCodigo && t.ContratoAno == contratoPagamento.ContratoAno && t.ContratoNumero == contratoPagamento.ContratoNumero && t.PagamentoSequencia == contratoPagamento.Sequencia && t.DetalheSequencia == detalheSequencia)
                    .Tracking(tracking)
                    .FirstOrDefault();
        }

        public IEnumerable<ContratoPagamento> ListarContratoPagamentosDetalhados(int contratoUsina, int contratoAno, int contratoNumero, bool tracking = false)
        {
            var result = _context.ContratoPagamentos
                .Where(t => t.UsinaCodigo == contratoUsina
                    && t.ContratoAno == contratoAno
                    && t.ContratoNumero == contratoNumero)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador)
                .Tracking(tracking)
                .ToList();

            result.ToList().ForEach(p => CarregarDetalhes(p, tracking));

            return result;
        }

        public IEnumerable<ContratoPagamentoVersao> ListarContratoPagamentosDetalhados(int numVersao, int contratoUsina, int contratoAno, int contratoNumero, bool tracking = false)
        {
            var result = _context.ContratoPagamentosVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == contratoUsina
                    && t.ContratoAno == contratoAno
                    && t.ContratoNumero == contratoNumero)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador)
                .Tracking(tracking)
                .ToList();

            result.ToList().ForEach(p => CarregarDetalhes(p, tracking));

            return result;
        }

        public ContratoPagamento ObterContratoPagamentoDetalhado(int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, bool tracking = false)
        {

            var contratoPagamento = _context.ContratoPagamentos
                .Where(t => t.UsinaCodigo == contratoUsina
                && t.ContratoAno == contratoAno
                && t.ContratoNumero == contratoNumero
                && t.Sequencia == pagamentoSequencia)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.Usina)
                .Include(t => t.TipoCobranca.Portador)
                .Tracking(tracking)
                .FirstOrDefault();

            CarregarDetalhes(contratoPagamento, tracking);

            return contratoPagamento;
        }

        public ContratoPagamentoVersao ObterContratoPagamentoDetalhado(int numeroVersao, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, bool tracking = false)
        {

            var contratoPagamento = _context.ContratoPagamentosVersoes
                .Where(t => t.UsinaCodigo == contratoUsina
                && t.ContratoAno == contratoAno
                && t.ContratoNumero == contratoNumero
                && t.Sequencia == pagamentoSequencia
                && t.NumeroVersao == numeroVersao)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.Usina)
                .Include(t => t.TipoCobranca.Portador)
                .Tracking(tracking)
                .FirstOrDefault();

            CarregarDetalhes(contratoPagamento, tracking);

            return contratoPagamento;
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_pag_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_pag c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_ccredit_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_ccredit c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_dep_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_dep c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_dinheir_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_dinheir c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_cheque_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_cheque c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_boleto_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_contrato_boleto c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_pag_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_ccredit_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_dep_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_dinheir_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_cheque_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_boleto_versao");
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

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_pag_versao", "con_contrato_pag");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_pag");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_pag_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_ccredit_versao", "con_contrato_ccredit");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_ccredit");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_ccredit_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_dep_versao", "con_contrato_dep");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_dep");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_dep_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_dinheir_versao", "con_contrato_dinheir");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_dinheir");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_dinheir_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_cheque_versao", "con_contrato_cheque");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_cheque");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_cheque_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_contrato_boleto_versao", "con_contrato_boleto");

            sqlComando.Append($"REPLACE INTO topsys.con_contrato_boleto");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_contrato_boleto_versao");
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

            sqlComando.Append($"DELETE FROM topsys.con_contrato_pag");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_ccredit");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_dep");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_dinheir");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_cheque");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_boleto");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }
    }
}
