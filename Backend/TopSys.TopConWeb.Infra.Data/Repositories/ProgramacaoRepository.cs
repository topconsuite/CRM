using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ProgramacaoRepository : RepositoryBase<Programacao>, IProgramacaoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public ProgramacaoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(Programacao programacao)
        {
            var sqlComando = programacao.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_programacao", sqlComando.ToString());

            programacao.Sequencia = _context.Database.Connection.Query<int>("SELECT @SEQUENCIA_PROGRAMACAO_INSERIDA").FirstOrDefault();
        }

        public IEnumerable<Programacao> ListarComPropostaContrato()
        {
            return _context
                .Programacoes
                .Include(c => c.Proposta)
                .Include(c => c.Contrato)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.Usina)
                .AsNoTracking()
                .ToList();
        }

        public PagedList<Programacao> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            var pagedList = _context.Programacoes
                .OrderByDescending(t => new { t.DataConcretagem, t.Horario })
                .Include(c => c.Proposta)
                .Include(c => c.Contrato)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.Usina)
                .Include(c => c.EnderecoMunicipio)
                .Where(filter)
                .PagedList(pagina, porPagina, t => t.Usina.Nome != null && t.UsinaEntrega.Nome != null);

            return pagedList;
        }

        public PagedList<Programacao> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            var pagedList = _context.Programacoes
                .OrderBy(t => new { t.DataConcretagem, t.Horario })
                .Include(c => c.Proposta)
                .Include(c => c.Proposta.Segmento)
                .Include(c => c.Proposta.Obra)
                .Include(c => c.Proposta.Obra.CondicaoPagamento)
                .Include(c => c.Contrato)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.ResistenciaTipo)
                .Include(c => c.Uso)
                .Include(c => c.Pedra)
                .Include(c => c.Slump)
                .Include(c => c.Usina)
                .Include(c => c.EnderecoMunicipio)
                .Where(filter)
                .PagedList(pagina, porPagina, t => t.Usina.Nome != null && t.UsinaEntrega.Nome != null);

            StringBuilder sqlComando = new StringBuilder();

            foreach (var programacao in pagedList.Records)
            {
                sqlComando.Clear();
                sqlComando.Append($"SELECT SUM(c.vlr_total_cobranca) FROM con_nf n");
                sqlComando.Append($" INNER JOIN con_nf_complemento c");
                sqlComando.Append($" ON n.filial=c.filial AND n.interv=c.interv AND n.tp_doc=c.tp_doc");
                sqlComando.Append($" AND n.num_nf=c.num_nf AND n.serie=c.serie AND n.seq_nf=c.seq_nf");
                sqlComando.Append($" WHERE n.usina_contrato={programacao.UsinaCodigo}");
                sqlComando.Append($" AND n.num_contrato={programacao.ContratoNumero}");
                sqlComando.Append($" AND n.ano_contrato={programacao.ContratoAno}");
                sqlComando.Append($" AND n.seq_prog={programacao.Sequencia}");

                var valorTotalRemessasEmitidas =  _context.Database.Connection.Query<decimal?>(sqlComando.ToString()).FirstOrDefault();

                programacao.setValorTotalRemessasEmitidas(valorTotalRemessasEmitidas ?? decimal.Zero);
            }

            return pagedList;
        }

        public IEnumerable<ProgramacaoLog> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia)
        {
            return _context.ProgramacaoLogs
                .Where(t => t.UsinaCodigo == idUsina && t.ObraCodigo == obraNumero && t.ProgramacaoSequencia == sequencia && t.Horario == "")
                .AsNoTracking()
                .OrderByDescending(t => new { t.DataHora, t.Sequencia })
                .ToList();
        }

        public Programacao ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia, bool tracking = false)
        {
            var result = _context.Programacoes
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.Pedra)
                .Include(t => t.ResistenciaTipo)
                .Include(t => t.Slump)
                .Include(t => t.TracoPesadoPedra)
                .Include(t => t.TracoPesadoResistenciaTipo)
                .Include(t => t.TracoPesadoSlump)
                .Include(t => t.TracoPesadoUso)
                .Include(t => t.Usina)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Uso)
                .Include(t => t.VibradorVendedor)
                .Include(t => t.Contrato)
                .Where(t => t.UsinaCodigo == idUsina && t.ObraNumero == obraNumero && t.Sequencia == sequencia)
                .Tracking(tracking)
                .FirstOrDefault();

            result.DemaisServicos = _context.ProgramacaoDemaisServicos
                .Where(t => t.UsinaCodigo == idUsina && t.ObraNumero == obraNumero && t.ProgramacaoSequencia == sequencia)
                .AsNoTracking()
                .ToList();

            return result;
        }

        public float ObterVolumeTotalProgramado(int idUsina, int obraNumero)
        {
            return _context.Programacoes
                .Where(t => t.UsinaCodigo == idUsina && t.ObraNumero == obraNumero
                    && t.Status != EProgramacaoStatus.Cancelada)
                .AsNoTracking()
                .ToList()
                .Sum(t => t.VolumeTotal);
        }

        public IEnumerable<ProgramacaoHora> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            var horarios = _context.ProgramacaoHoras
                .Include(t => t.Nf)
                .Where(t => t.UsinaCodigo == idUsina
                    && t.ContratoAno == contratoAno
                    && t.ContratoNumero == contratoNumero
                    && t.Sequencia == sequencia)
                .AsNoTracking()
                .ToList();

            // TODO: VERIFICAR POSSIBILIDADE DE REFATORAÇÃO PARA UTILIZAR APENAS UMA CONSULTA NO BANCO
            foreach (var nf in horarios.Select(t => t.Nf))
            {
                if (nf != null)
                {
                    nf.Complemento = _context.NotasFiscaisFisicasComplemento
                        .Where(t => t.FilialCodigo == nf.FilialCodigo
                            && t.IntervenienteCodigo == nf.IntervenienteCodigo
                            && t.TipoDocumentoCodigo == nf.TipoDocumentoCodigo
                            && t.Numero == nf.Numero
                            && t.Serie == nf.Serie
                            && t.Sequencia == nf.Sequencia)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }

            return horarios;
        }

        public int ObterQuantidadeDeProgramacoesHora(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(c.no_contrato) FROM con_programacao_hora c");
            sqlComando.Append($" WHERE c.usina = {idUsina}");
            sqlComando.Append($" AND c.no_contrato = {contratoNumero}");
            sqlComando.Append($" AND c.ano_contrato={contratoAno}");
            sqlComando.Append($" AND c.seq_prog={sequencia}");

            return _context.Database.Connection.Query<int>(sqlComando.ToString()).FirstOrDefault();
        }

        public void AdicionarValorAvulsoTaxaCancelamentoProgramacao(Programacao programacao, Mercadoria mercadoriaTaxa, int seqVlrAvulso, decimal valorTaxa, string idUsuario)
        {
            var codTaxaCancelamento = "987";
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"INSERT INTO con_vlr_avulso SET");
            sqlComando.Append($" filial=@{nameof(programacao.UsinaEntrega.FilialCodigo)}, ");
            sqlComando.Append($" seq_vlr_avulso=@{nameof(seqVlrAvulso)},");
            sqlComando.Append($" dt_oper='{DateTime.Now.ToString("yy/MM/dd")}',");
            sqlComando.Append($" num_contrato=@{nameof(programacao.ContratoNumero)},");
            sqlComando.Append($" ano_contrato=@{nameof(programacao.ContratoAno)},");
            sqlComando.Append($" taxa_cancelamento_prog='S',");
            sqlComando.Append($" seq_prog=@{nameof(programacao.Sequencia)},");
            sqlComando.Append($" interv=@{nameof(programacao.Contrato.IntervenienteCodigo)},");
            sqlComando.Append($" cod_descr=@{nameof(codTaxaCancelamento)},");
            sqlComando.Append($" vlr=@{nameof(valorTaxa)},");
            sqlComando.Append($" id_cadast='{idUsuario} {DateTime.Now.ToString("dd/MM/yy")}',");
            sqlComando.Append($" usina_remessa=@{nameof(programacao.UsinaEntregaCodigo)},");
            sqlComando.Append($" usina_ctr=@{nameof(programacao.UsinaCodigo)},");
            sqlComando.Append($" obs='TAXA DE CANCELAMENTO DE PROGRAMACAO'");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                programacao.UsinaEntrega.FilialCodigo,
                seqVlrAvulso,
                programacao.ContratoNumero,
                programacao.ContratoAno,
                programacao.Sequencia,
                programacao.Contrato.IntervenienteCodigo,
                codTaxaCancelamento,
                valorTaxa,
                programacao.UsinaEntregaCodigo,
                programacao.UsinaCodigo
            });

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_vlr_avulso", sqlComando.ToString(), new
            {
                programacao.UsinaEntrega.FilialCodigo,
                seqVlrAvulso,
                programacao.ContratoNumero,
                programacao.ContratoAno,
                programacao.Sequencia,
                programacao.Contrato.IntervenienteCodigo,
                codTaxaCancelamento,
                valorTaxa,
                programacao.UsinaEntregaCodigo,
                programacao.UsinaCodigo
            });
        }

        public void AlterarStatusLiberacaoProgramacao(Programacao programacao, string idUsuario)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_programacao SET");
            sqlComando.Append($" pend_sts_aprov='A', ");
            sqlComando.Append($" pend_id_aprov='{StringHelper.GetIDD(idUsuario)}',"); 
            sqlComando.Append($" libera_entrega='S',");
            sqlComando.Append($" at = at + 1");
            sqlComando.Append($" WHERE usina=@{nameof(programacao.UsinaCodigo)}");
            sqlComando.Append($" AND no_contrato=@{nameof(programacao.ContratoNumero)}");
            sqlComando.Append($" AND ano_contrato=@{nameof(programacao.ContratoAno)}");
            sqlComando.Append($" AND seq_prog=@{nameof(programacao.Sequencia)};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                programacao.UsinaCodigo,
                programacao.ContratoNumero,
                programacao.ContratoAno,
                programacao.Sequencia
            });
        }              
        }
    }
