using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.CrossCuting;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Infra.Legacy.Services;

public class TestBase
{
    private UnityContainer _container = new UnityContainer();

    internal readonly IIntervenienteRepository _intervenienteRepository;
    internal readonly IParametroRepository _parametroRepository;
    internal readonly IIntervenienteSequenceRepository _intervenienteSequenceRepository;
    internal readonly IComercialLegacyService _comercialLegacyService;
    internal readonly IIntervenienteSequenceControlService _intervenienteSequenceControlService;
    internal readonly ParametroIntervenienteSequence _parametroIntervenienteSequence;
    internal readonly AppDataContext _appContext;

    internal Exception _exceptionCapture { get; set; } = null;
    internal const int quantidadeDeExecucoesInfinitas = -1;

    public TestBase()
    {
        DependencyResolver.Resolve(_container);
        _intervenienteRepository = _container.Resolve<IntervenienteRepository>();
        _parametroRepository = _container.Resolve<ParametroRepository>();
        _intervenienteSequenceRepository = _container.Resolve<IntervenienteSequenceRepository>();
        _comercialLegacyService = _container.Resolve<ComercialLegacyService>();
        _intervenienteSequenceControlService = _container.Resolve<IntervenienteSequenceControlService>();
        _parametroIntervenienteSequence = _container.Resolve<ParametroIntervenienteSequence>();
        _appContext = _container.Resolve<AppDataContext>();
    }


    public async Task GerarIntervenientes()
    {
        var sqlCommand = new StringBuilder();

        try
        {
            var proximoCodigo = _intervenienteSequenceControlService.GerarProximaSequencia();

            sqlCommand.Clear();
            sqlCommand.Append($"INSERT INTO ger_interv");
            sqlCommand.Append($" SET");
            sqlCommand.Append($" nome='TESTADOR'");
            sqlCommand.Append($", cod={proximoCodigo}");
            sqlCommand.Append($", razao='TEST LTDA'");


            await _appContext.Database.ExecuteSqlCommandAsync(sqlCommand.ToString());
        }
        catch (Exception e)
        {
            _exceptionCapture = e;
        }

    }

    public async Task RemoverIntervenientes()
    {
        var sqlCommand = new StringBuilder();


        sqlCommand.Clear();
        sqlCommand.Append($"DELETE FROM ger_interv");
        sqlCommand.Append($" WHERE");
        sqlCommand.Append($" nome like '%TESTADOR%';"); ;

        await _appContext.Database.ExecuteSqlCommandAsync(sqlCommand.ToString());
    }


    public void DropaTabelaSeExitir()
    {
        var sqlCommand = new StringBuilder();

        sqlCommand.Append("DROP TABLE IF EXISTS sequence_ger_interv");

        _appContext.Database.ExecuteSqlCommand(sqlCommand.ToString());
    }


    public int GerarProximaSequencia(
        string messagemEsperada,
        string faixaInicialParametrizada = null,
        string faixaFinalParametrizada = null,
        IntervenienteSequence[] intervenienteControleFaixaCadastrados = null,
        int quantidadeDeExecucoes = 1,
        bool validaTesteInternamente = true
        )
    {
        int sequencia = 0;

        var faixaInicialIntervenienteParametroNOriginal = _parametroRepository.ObterParametroN(
              _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
              _parametroIntervenienteSequence.FaixaInicialIntervenienteParametroN
        );

        var faixaFinalIntervenienteParametroNOriginal = _parametroRepository.ObterParametroN(
               _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
               _parametroIntervenienteSequence.FaixaFinalIntervenienteParametroN
           );

        try
        {
            if (faixaInicialParametrizada is null)
                _parametroRepository.ApagarParametroN(
                    _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                    _parametroIntervenienteSequence.FaixaInicialIntervenienteParametroN
                 );
            else
                _parametroRepository.AtualizarParametroN(
                    _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                    _parametroIntervenienteSequence.FaixaInicialIntervenienteParametroN,
                    faixaInicialParametrizada
                  );

            if (faixaFinalParametrizada is null)
                _parametroRepository.ApagarParametroN(
                    _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                    _parametroIntervenienteSequence.FaixaFinalIntervenienteParametroN);
            else
                _parametroRepository.AtualizarParametroN(
                    _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                    _parametroIntervenienteSequence.FaixaFinalIntervenienteParametroN,
                    faixaFinalParametrizada);

            DropaTabelaSeExitir();
            _intervenienteSequenceRepository.CriaOuIgnoraTabela();

            if (!(intervenienteControleFaixaCadastrados is null))
            {
                foreach (var intervenienteControleFaixaCadastrado in intervenienteControleFaixaCadastrados)
                {
                    _intervenienteSequenceRepository.SincronizarFaixa(
                        new IntervenienteSequence()
                        {
                            FaixaInicial = intervenienteControleFaixaCadastrado.FaixaInicial,
                            FaixaFinal = intervenienteControleFaixaCadastrado.FaixaFinal,
                            UltimoID = intervenienteControleFaixaCadastrado.UltimoID
                        }
                    );
                }
            }

            if (quantidadeDeExecucoes == quantidadeDeExecucoesInfinitas)
            {
                while (true)
                {
                    sequencia = _intervenienteSequenceControlService.GerarProximaSequencia();
                }
            }
            else
            {
                for (int i = 0; i < quantidadeDeExecucoes; i++)
                {
                    sequencia = _intervenienteSequenceControlService.GerarProximaSequencia();
                }
            }


            if (validaTesteInternamente)
                throw new Exception("", new Exception("Não passou no teste"));
        }
        catch (Exception ex)
        {
            _parametroRepository.AtualizarParametroN(
                _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                _parametroIntervenienteSequence.FaixaInicialIntervenienteParametroN,
                faixaInicialIntervenienteParametroNOriginal);
            _parametroRepository.AtualizarParametroN(
                _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                _parametroIntervenienteSequence.FaixaFinalIntervenienteParametroN,
                faixaFinalIntervenienteParametroNOriginal);

            if (validaTesteInternamente)
                Assert.AreEqual(messagemEsperada, ex.InnerException.Message);
        }

        return sequencia;
    }
}
