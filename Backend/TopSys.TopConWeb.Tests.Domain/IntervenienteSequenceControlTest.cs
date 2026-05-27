
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.SequenceControl;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Infra.Data.SequenceControl;

namespace TopSys.TopConWeb.Tests.Domain
{
    [TestClass]
    public class IntervenienteSequenceControlTest
    {
        private readonly IIntervenienteRepository _intervenienteRepository ;
        private readonly IParametroRepository _parametroRepository;
        private readonly IIntervenienteSequenceRepository _intervenienteControleFaixaRepository;
        private readonly IIntervenienteSequenceControl _intervenienteSequenceControl;

        private int quantidadeDeExecucoesInfinitas = -1;

        public IntervenienteSequenceControlTest()
        {
            var context = new AppDataContext();
            _intervenienteRepository = new IntervenienteRepository(context);
            _intervenienteControleFaixaRepository = new IntervenienteSequenceRepository(context);
            _parametroRepository = new ParametroRepository(context);
            _intervenienteSequenceControl = new IntervenienteSequenceControl(
                _intervenienteRepository,
                _intervenienteControleFaixaRepository,
                new IntervenienteSequenceParametro(_parametroRepository)
            );
        }

        [TestMethod]
        [TestProperty("Order", "1")]
        public void EstouraErroCasoNaoEstiverParametrizadoCorretamente()
        {
            var parameters = IntervenienteSequenceControlTestData.EstouraErroCasoNaoEstiverParametrizadoCorretamente();

            foreach ( var parameter in parameters)
            {
                GerarProximaSequencia(parameter.MessagemEsperada, parameter.FaixaInicial, parameter.FaixaFinal);
            }

        }


        [TestMethod]
        [TestProperty("Order", "2")]
        public void EstouraErroCasoFaixaCarregadaNaoSeguirOPadraoCorreto()
        {
            var parameters = IntervenienteSequenceControlTestData.EstouraErroCasoFaixaCarregadaNaoSeguirOPadraoCorreto();

            foreach (var paramter in parameters)
            {
                GerarProximaSequencia(paramter.MessagemEsperada, paramter.FaixaInicial, paramter.FaixaFinal);
            }
        }

        [TestMethod]
        [TestProperty("Order", "3")]
        public void EstouraErroCasoVerifiqueProbabilidadeDeDuplicidadeDeFaixa()
        {
            var parameters = IntervenienteSequenceControlTestData.EstouraErroCasoVerifiqueProbabilidadeDeDuplicidadeDeFaixa();

            foreach (var parameter in parameters)
            {
                GerarProximaSequencia(
                    parameter.MessagemEsperada,
                    parameter.FaixaInicial,
                    parameter.FaixaFinal,
                    new IntervenienteSequence[] { parameter.IntervenienteControleFaixa }
               );
            }
        }

        [TestMethod]
        [TestProperty("Order", "4")]
        public void EstouraErroCasoAFaixaFinalForMenorQueMaiorCodigoCadastrado()
        {
            var mensagemErro = "ValidaCodigoMaiorFaixaFinal";

            var faixaInicial = 100;
            var faixaFinal = _intervenienteRepository.ObterCodigoMaximoCadastrado(faixaInicial, 99999);
            var digitosAbaixoDoMaiorCodigoEncontrado =  - 10;
      
            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                (faixaFinal + digitosAbaixoDoMaiorCodigoEncontrado).ToString()
             );
            
        }

        [TestMethod]
        [TestProperty("Order", "5")]
        public void EstouraErroCasoAFaixaFoiTotalmenteUtilizada()
        {
            var mensagemErro = "ValidaFaixaTotalmenteUtilizada";

            var faixaInicial = 1;
            var faixaFinal = _intervenienteRepository.ObterCodigoMaximoCadastrado(faixaInicial, 99999);
            var quantidadesAbaixoDoCodigoMaximoEncontrado = 10;
 

            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                (faixaFinal + quantidadesAbaixoDoCodigoMaximoEncontrado).ToString(),
                quantidadeDeExecucoes: quantidadeDeExecucoesInfinitas
             );

        }

        [TestMethod]
        [TestProperty("Order", "6")]
        public void EstouraErroCasoAlgumCodigoEstejaForamDeSuaFaixa()
        {
            var mensagemErro = "ValidaCodigosForaDeFaixa";

            var faixaInicial = 50;
            var faixaFinal = 99999;

            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                new IntervenienteSequence[]
                {
                    new IntervenienteSequence()
                    {
                        UltimoID = 9,
                        FaixaInicial = 10,
                        FaixaFinal = 30,
                    }
                }
            );
        }


        [TestMethod]
        [TestProperty("Order", "7")]
        public void EstouraErroAoUsarTodasAsFaixasDisponiveis()
        {
            var mensagemErro = "ValidaFaixaTotalmenteUtilizada";

            var faixaInicial = 6000;
            var faixaFinal = 7000;

            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                quantidadeDeExecucoes : quantidadeDeExecucoesInfinitas
            );
        }

        [TestMethod]
        [TestProperty("Order", "8")]
        public void VerificaSeUltimaFaixaEIgualAFaixaFinalSeUtilizadasTodasAsFaixas()
        {
            var mensagemErro = "ValidaFaixaTotalmenteUtilizada";

            var faixaInicial = 6000;
            var faixaFinal = 7000;
            var faixaTotal = faixaFinal - faixaInicial;

            var ultimaFaixa = GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                quantidadeDeExecucoes: faixaTotal,
                validaTesteInternamente: false         
            );

            Assert.AreEqual(faixaFinal, ultimaFaixa);
        }

        [TestMethod]
        [TestProperty("Order", "9")]
        public void VerificaSeUltimaFaixaBateComAQuantidadeDeSolicitacoesPassadaFaixa()
        {
            var mensagemErro = "ValidaFaixaTotalmenteUtilizada";
       
            var faixaInicial = 6000;
            var faixaFinal = 7000;
            var faixa = 100;

            var ultimaFaixa = GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                quantidadeDeExecucoes: faixa,
                validaTesteInternamente: false
            );

            Assert.AreEqual(faixaInicial+faixa, ultimaFaixa);
        }

        [TestMethod]
        [TestProperty("Order", "10")]
        public void EstouraErroCasoAFaixasIniciaisEstejamDuplicada()
        {
            var mensagemErro = "ValidaDuplicidadeFaixaInicial";

            var faixaInicial = 50;
            var faixaFinal = 99999;

            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                new IntervenienteSequence[]
                {
                    new IntervenienteSequence()
                    {
                        UltimoID = 10,
                        FaixaInicial = 9,
                        FaixaFinal = 30,
                    },
                    new IntervenienteSequence()
                    {
                        UltimoID = 11,
                        FaixaInicial = 9,
                        FaixaFinal = 40,
                    }
                }
            );
        }

        [TestMethod]
        [TestProperty("Order", "11")]
        public void EstouraErroCasoAFaixasFinaisEstejamDuplicada()
        {
            var mensagemErro = "ValidaDuplicidadeFaixaFinal";

            var faixaInicial = 50;
            var faixaFinal = 99999;

            GerarProximaSequencia(
                mensagemErro,
                faixaInicial.ToString(),
                faixaFinal.ToString(),
                new IntervenienteSequence[]
                {
                    new IntervenienteSequence()
                    {
                        UltimoID = 11,
                        FaixaInicial = 10,
                        FaixaFinal = 30,
                    },
                    new IntervenienteSequence()
                    {
                        UltimoID = 10,
                        FaixaInicial = 9,
                        FaixaFinal = 30,
                    }
                }
            );
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
                  _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                  _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN
            );

            var faixaFinalIntervenienteParametroNOriginal = _parametroRepository.ObterParametroN(
                   _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                   _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN
               );

            try
            {
                if (faixaInicialParametrizada is null)
                    _parametroRepository.ApagarParametroN(
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN
                     );
                else
                    _parametroRepository.AtualizarParametroN(
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN,
                        faixaInicialParametrizada
                      );

                if (faixaFinalParametrizada is null)
                    _parametroRepository.ApagarParametroN(
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN);
                else
                    _parametroRepository.AtualizarParametroN(
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                        _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN,
                        faixaFinalParametrizada);

                _intervenienteControleFaixaRepository.DropaTabelaSeExitir();
                _intervenienteControleFaixaRepository.CriaOuIgnoraTabela();

                if (!(intervenienteControleFaixaCadastrados is null))
                {
                    foreach (var intervenienteControleFaixaCadastrado in intervenienteControleFaixaCadastrados)
                    {
                        _intervenienteControleFaixaRepository.SobreescreveNovaFaixa(
                            intervenienteControleFaixaCadastrado.FaixaInicial,
                            intervenienteControleFaixaCadastrado.FaixaFinal,
                            intervenienteControleFaixaCadastrado.UltimoID
                            );
                    }
                }

                if (quantidadeDeExecucoes == quantidadeDeExecucoesInfinitas)
                {
                    while (true)
                    {
                        sequencia = _intervenienteSequenceControl.GerarProximaSequencia();
                    }              
                }
                else
                {
                    for (int i = 0; i < quantidadeDeExecucoes; i++)
                    {
                        sequencia = _intervenienteSequenceControl.GerarProximaSequencia();
                    }
                }


                if (validaTesteInternamente) 
                    throw new Exception("", new Exception("Não passou no teste"));
            }
            catch (Exception ex)
            {
                _parametroRepository.AtualizarParametroN(
                    _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN,
                    _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN,
                    faixaInicialIntervenienteParametroNOriginal);
                _parametroRepository.AtualizarParametroN(
                    _intervenienteSequenceControl.IntervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN, 
                    _intervenienteSequenceControl.IntervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN,
                    faixaFinalIntervenienteParametroNOriginal);

                if (validaTesteInternamente) 
                    Assert.AreEqual(messagemEsperada, ex.InnerException.Message);           
            }

            return sequencia;
        }

    }

}
