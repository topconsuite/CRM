using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ObraTaxaRepository : RepositoryBase<ObraTaxa>, IObraTaxaRepository
    {
        private IDatabaseRepository _databaseRepository;
        private readonly IdentityHelperService _identityHelperService;
        public ObraTaxaRepository(AppDataContext context, IDatabaseRepository databaseRepository, IdentityHelperService identityHelperService) : base(context)
        {
            _identityHelperService = identityHelperService;
            _context = context;
            _databaseRepository = databaseRepository;
        }

        public void AtualizarObraTaxa(ObraTaxa obraTaxa)
        {
                _context.ObraTaxas.Attach(obraTaxa);
                
        }

        public void AtualizarObraTaxa(ObraTaxaVersao obraTaxa)
        {
            _context.ObraTaxasVersoes.Attach(obraTaxa);
	    }

        public bool AdicionalNoturnoVerificarDia(ObraTaxa taxa, DateTime data)
        {

            if (taxa.QuandoDe.Equals("0") || taxa.QuandoAte.Equals("0"))
                return false;

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT cod FROM ger_diverso");
            sql.AppendLine($"WHERE");
            sql.AppendLine($"   descr = @parametro");
            sql.AppendLine($"   AND prog = 6022");
            sql.AppendLine($"   AND campo = @campo");

            var quandoDe = _context.Connection.QueryFirst<int>(sql.ToString(), new { parametro = taxa.QuandoDe, campo = "quando_de" });
            var quandoAte = _context.Connection.QueryFirst<int>(sql.ToString(), new { parametro = taxa.QuandoDe, campo = "quando_ate" });

            var diaDaSemana = ((int)data.DayOfWeek) + 1; // No C# começa no 0 enquanto na ger_diverso começa no 1;

            if (taxa.QuandoOperacao.Equals("E/OU"))
                return diaDaSemana == quandoDe || diaDaSemana == quandoAte;
            else // ATÉ
                return diaDaSemana >= quandoDe && diaDaSemana <= quandoAte;
            

        }

        public ICollection<ObraTaxa> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int segmentacao)
        {

            var listaTaxas = (
                from obraTx in _context.ObraTaxas
                join txExtraPersonalizada in _context.TaxasExtras
                on
                new { obraTx.UsinaCodigo, obraTx.ObraCodigo, obraTx.Sequencia }
                equals
                new { txExtraPersonalizada.UsinaCodigo, txExtraPersonalizada.ObraCodigo, txExtraPersonalizada.Sequencia }

                into jnTxExtraPersonalizada

                from txExtraPersonalizada in jnTxExtraPersonalizada.DefaultIfEmpty()

                join taxaExtraPadrao in _context.TaxasExtras
                on
                new { obraTx.UsinaCodigo, obraTx.Sequencia }
                equals
                new { taxaExtraPadrao.UsinaCodigo, taxaExtraPadrao.Sequencia }

                where taxaExtraPadrao.ObraCodigo == 0 && obraTx.UsinaCodigo == usinaEntregaCodigo && obraTx.ObraCodigo == obraCodigo

                select new {
                    taxa = (txExtraPersonalizada ?? taxaExtraPadrao),
                    obraCodigo,
                    obraTx.AprovacaoSolicitante,
                    obraTx.Aprovada,
                    obraTx.Selecionada,
                    isPersonalizada = !txExtraPersonalizada.Equals(null)
                }
            ).ToList();

            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxa> taxas = new List<ObraTaxa>();

            var taxasDefault = ListarTaxaPadraoByIdUsinaAndSegmento(usinaEntregaCodigo, segmentacao);
            var taxasNovas = taxasDefault.Where(x => !listaTaxas.Any(y => y.taxa.Sequencia == x.Sequencia)).ToList();

            //Varrendo as taxas extras as adicionando na coleção
            listaTaxas.ForEach(t => taxas.Add(new ObraTaxa(t.taxa, t.obraCodigo, t.AprovacaoSolicitante, t.Aprovada, t.Selecionada, t.isPersonalizada)));

            taxasNovas.ForEach(taxa => {
                taxa.Selecionada = "N";
                taxa.Aprovada = "";
                taxa.ObraCodigo = obraCodigo;
                taxa.Nova = true;
                taxas.Add(taxa);
            });

            return taxas;
        }

        public ICollection<ObraTaxaVersao> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int numVersao, int segmentacao)
        {
            var listaTaxas = (
                from obraTx in _context.ObraTaxasVersoes
                join txExtraPersonalizada in _context.TaxasExtrasVersoes
                on
                new {obraTx.NumeroVersao, obraTx.UsinaCodigo, obraTx.ObraCodigo, obraTx.Sequencia }
                equals
                new {txExtraPersonalizada.NumeroVersao, txExtraPersonalizada.UsinaCodigo, txExtraPersonalizada.ObraCodigo, txExtraPersonalizada.Sequencia }

                into jnTxExtraPersonalizada

                from txExtraPersonalizada in jnTxExtraPersonalizada.DefaultIfEmpty()

                join taxaExtraPadrao in _context.TaxasExtras
                on
                new { obraTx.UsinaCodigo, obraTx.Sequencia }
                equals
                new { taxaExtraPadrao.UsinaCodigo, taxaExtraPadrao.Sequencia }

                where taxaExtraPadrao.ObraCodigo == 0 && obraTx.UsinaCodigo == usinaEntregaCodigo && obraTx.ObraCodigo == obraCodigo && obraTx.NumeroVersao == numVersao

                select new 
                {
                    taxaPersonalizada = txExtraPersonalizada,
                    taxaPadrao = taxaExtraPadrao,
                    obraCodigo,
                    obraTx.AprovacaoSolicitante,
                    obraTx.Aprovada,
                    obraTx.Selecionada,
                    obraTx.NumeroVersao,
                    isPersonalizada = !txExtraPersonalizada.Equals(null)
                }
            ).ToList();



            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxaVersao> taxas = new List<ObraTaxaVersao>();

            var taxasDefault = ListarTaxaPadraoByIdUsinaAndSegmentoVersao(numVersao, usinaEntregaCodigo, segmentacao) ;
            var taxasNovas = taxasDefault.Where(x => !listaTaxas.Any(y => (y.taxaPersonalizada?.Sequencia ?? y.taxaPadrao.Sequencia) == x.Sequencia)).ToList();

            //Varrendo as taxas extras as adicionando na coleção
            listaTaxas.ForEach(t => taxas.Add(new ObraTaxaVersao((t.taxaPersonalizada ?? (TaxaExtraVersao)t.taxaPadrao), t.obraCodigo, t.AprovacaoSolicitante, t.Aprovada, t.Selecionada, t.isPersonalizada, t.NumeroVersao)));

            taxasNovas.ForEach(taxa => {
                taxa.Selecionada = "N";
                taxa.Aprovada = "";
                taxa.ObraCodigo = obraCodigo;
                taxa.Nova = true;
                taxa.NumeroVersao = numVersao;
                taxas.Add(taxa);
            });

            return taxas;
        }

        public ICollection<ObraTaxa> ListarTaxaPadraoByIdUsinaAndSegmento(int usinaEntregaCodigo, int idSegmentacao)
        {
            var currentDate = _context.Database.SqlQuery<DateTime>("SELECT now()").FirstOrDefault();

            var lastDate = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo 
                            && t.ObraCodigo == 0 && t.DataInicioVigencia <= currentDate
                            && t.IdSegmentacao == idSegmentacao)
                .AsNoTracking()
                .Select(t => t.DataInicioVigencia).ToList().DefaultIfEmpty().Max();

            var taxasPadrao = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia == lastDate && t.IdSegmentacao == idSegmentacao)
                .AsNoTracking()
                .ToList();

            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxa> taxas = new List<ObraTaxa>();

            //Varrendo as taxas extras as adicionando na coleção
            taxasPadrao.ForEach(t => taxas.Add(new ObraTaxa(t, 0, "", "", "S", false)));

            return taxas;
        }

        public ICollection<ObraTaxa> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo)
        {
            var currentDate = _context.Database.SqlQuery<DateTime>("SELECT now()").FirstOrDefault();

            var lastDate = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia <= currentDate)
                .AsNoTracking()
                .Select(t => t.DataInicioVigencia).ToList().DefaultIfEmpty().Max();

            var taxasPadrao = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia == lastDate)
                .AsNoTracking()
                .ToList();

            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxa> taxas = new List<ObraTaxa>();

            //Varrendo as taxas extras as adicionando na coleção
            taxasPadrao.ForEach(t => taxas.Add(new ObraTaxa(t, 0, "", "", "S", false)));
            
            return taxas;
        }

        public ICollection<ObraTaxaVersao> ListarTaxaPadraoByIdUsinaAndSegmentoVersao(int numVersao, int usinaEntregaCodigo, int idSegmentacao)
        {
            var currentDate = _context.Database.SqlQuery<DateTime>("SELECT now()").FirstOrDefault();

            var lastDate = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia <= currentDate && t.IdSegmentacao == idSegmentacao)
                .AsNoTracking()
                .Select(t => t.DataInicioVigencia).ToList().DefaultIfEmpty().Max();

            var taxasPadrao = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia == lastDate && t.IdSegmentacao == idSegmentacao)
                .AsNoTracking()
                .ToList();

            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxaVersao> taxas = new List<ObraTaxaVersao>();

            //Varrendo as taxas extras as adicionando na coleção
            taxasPadrao.ForEach(t => taxas.Add(new ObraTaxaVersao((TaxaExtraVersao)t, 0, "", "", "S", false, 0)));

            return taxas;
        }

        public ICollection<ObraTaxaVersao> ListarTaxaPadraoByIdUsinaVersao(int numVersao, int usinaEntregaCodigo)
        {
            var currentDate = _context.Database.SqlQuery<DateTime>("SELECT now()").FirstOrDefault();

            var lastDate = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia <= currentDate)
                .AsNoTracking()
                .Select(t => t.DataInicioVigencia).ToList().DefaultIfEmpty().Max();

            var taxasPadrao = _context.TaxasExtras
                .Where(t => t.UsinaCodigo == usinaEntregaCodigo && t.ObraCodigo == 0 && t.DataInicioVigencia == lastDate)
                .AsNoTracking()
                .ToList();

            //Criando a coleção que irá armazenar as taxas extras
            ICollection<ObraTaxaVersao> taxas = new List<ObraTaxaVersao>();

            //Varrendo as taxas extras as adicionando na coleção
            taxasPadrao.ForEach(t => taxas.Add(new ObraTaxaVersao((TaxaExtraVersao)t, 0, "", "", "S", false,0)));

            return taxas;
        }

        public void SalvarPersonalizada(ObraTaxa obraTaxa)
        {
            var taxaPersonalizada = new TaxaExtra(obraTaxa);
            var ignoreProperties = new List<string>();

            if (obraTaxa.DataInicioVigencia.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.DataInicioVigencia));
            if (obraTaxa.PeriodoDe.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.PeriodoDe));
            if (obraTaxa.PeriodoAte.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.PeriodoAte));

            var sqlComando = taxaPersonalizada.MontarSqlInsert(_context, ignoreProperties.ToArray()).Replace("INSERT INTO", "REPLACE INTO");
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_taxa_extra", sqlComando.ToString());
        }

        public void SalvarPersonalizada(ObraTaxaVersao obraTaxa)
        {
            var taxaPersonalizada = new TaxaExtraVersao(obraTaxa);
            var ignoreProperties = new List<string>();

            if (obraTaxa.DataInicioVigencia.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.DataInicioVigencia));
            if (obraTaxa.PeriodoDe.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.PeriodoDe));
            if (obraTaxa.PeriodoAte.Equals(new DateTime()))
                ignoreProperties.Add(nameof(TaxaExtra.PeriodoAte));

            var sqlComando = taxaPersonalizada.MontarSqlInsert(_context, ignoreProperties.ToArray()).Replace("INSERT INTO", "REPLACE INTO");
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_taxa_extra_versao", sqlComando.ToString());
        }

        public void DeletarPersonalizada(ObraTaxa obraTaxa)
        {
            var taxaPersonalizada = _context.TaxasExtras.Find(obraTaxa.UsinaCodigo, obraTaxa.Sequencia, obraTaxa.ObraCodigo);

            if (taxaPersonalizada != null) Remover(taxaPersonalizada);
        }

        public void DeletarPersonalizada(ObraTaxaVersao obraTaxa)
        {
            var taxaPersonalizada = _context.TaxasExtrasVersoes.Find(obraTaxa.NumeroVersao, obraTaxa.UsinaCodigo, obraTaxa.Sequencia, obraTaxa.ObraCodigo);

            if (taxaPersonalizada != null) Remover(taxaPersonalizada);
        }

        public void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_tx_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras_tx c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_taxa_extra_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_taxa_extra c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();
        }

        public void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_obras_tx_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_taxa_extra_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();
        }

        public void AdicionarContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_tx_versao", "con_obras_tx");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_tx");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_tx_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_taxa_extra_versao", "con_taxa_extra");

            sqlComando.Append($"REPLACE INTO topsys.con_taxa_extra");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_taxa_extra_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();
        }

        public void ExcluirContrato(int codUsina, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_obras_tx");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_taxa_extra");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());

            sqlComando.Clear();
        }

        public ObraTaxa ObterTaxaCancelamentoProgramacao(int codUsina, int numObra, string tipoTaxa, string tipoAntecedencia, int valor)
        {
            var taxaCancelamento = (
                from obraTx in _context.ObraTaxas
                join txExtraPersonalizada in _context.TaxasExtras
                on
                new { obraTx.UsinaCodigo, obraTx.Sequencia }
                equals
                new { txExtraPersonalizada.UsinaCodigo, txExtraPersonalizada.Sequencia }

                into jnTxExtraPersonalizada

                from txExtraPersonalizada in jnTxExtraPersonalizada.DefaultIfEmpty()

                join taxaExtraPadrao in _context.TaxasExtras
                on
                new { obraTx.UsinaCodigo, obraTx.Sequencia }
                equals
                new { taxaExtraPadrao.UsinaCodigo, taxaExtraPadrao.Sequencia }

                where obraTx.UsinaCodigo == codUsina && obraTx.ObraCodigo == numObra && txExtraPersonalizada.Tipo == tipoTaxa && txExtraPersonalizada.Antecedencia == tipoAntecedencia && valor >= 0 && valor <= txExtraPersonalizada.Quantidade

                select new
                {
                    taxa = (txExtraPersonalizada ?? taxaExtraPadrao),
                    numObra,
                    obraTx.AprovacaoSolicitante,
                    obraTx.Aprovada,
                    obraTx.Selecionada,
                    isPersonalizada = !txExtraPersonalizada.Equals(null)
                }
            ).FirstOrDefault();

            if (taxaCancelamento != null && taxaCancelamento.Selecionada == "S")
            {
                return new ObraTaxa(
                    taxaCancelamento.taxa,
                    taxaCancelamento.numObra,
                    taxaCancelamento.AprovacaoSolicitante,
                    taxaCancelamento.Aprovada,
                    taxaCancelamento.Selecionada,
                    taxaCancelamento.isPersonalizada
                );
            }

            // Se não encontrou nenhuma taxa correspondente, retorne null ou lance uma exceção adequada
            return null; // 
        }

        public int ObterCodMercadoriaTaxaCancelamentoProgramacao()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT Cod FROM topsys.fis_mercadoria");
            sqlComando.Append($" WHERE COD = '987'");

            return _context.Database.Connection.Query<int>(sqlComando.ToString()).FirstOrDefault();
        }
    }
}
