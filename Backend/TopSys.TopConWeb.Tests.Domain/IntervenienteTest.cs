
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.CrossCuting;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Infra.Legacy.Services;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TopSys.TopConWeb.Tests.Domain
{
    [TestClass]
    public class IntervenienteTest : TestBase
    {
       

        [TestMethod]
        [TestProperty("Order", "1")]
        public async Task VericaSeOcorreraDuplicadeAtravesDeExecucoesSimuntaneas()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _parametroRepository.AtualizarParametroN(
                 _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                 _parametroIntervenienteSequence.FaixaInicialIntervenienteParametroN,
                 "1"
               );

            _parametroRepository.AtualizarParametroN(
                        _parametroIntervenienteSequence.GrupoFaixaIntervenienteParametroN,
                        _parametroIntervenienteSequence.FaixaFinalIntervenienteParametroN,
                        "99999999");

            DropaTabelaSeExitir();
            _intervenienteSequenceRepository.CriaOuIgnoraTabela();

            await RemoverIntervenientes();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(20))
            {
                if (_exceptionCapture is null) GerarIntervenientes();
                else {
                    await RemoverIntervenientes();
                    throw _exceptionCapture;
                }           
            }     
        }

    }
    
}
