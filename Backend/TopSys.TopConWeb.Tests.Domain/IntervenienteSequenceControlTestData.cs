
using System;
using System.Collections.Generic;
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
    public static class IntervenienteSequenceControlTestData
    {
        public static IntervenienteSequenceControlTestParameters[] EstouraErroCasoNaoEstiverParametrizadoCorretamente()
        {
            var mensagemErroEsperado = "CarregarFaixaNumericaParametrizada";

            return new IntervenienteSequenceControlTestParameters[]
            {
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "", FaixaFinal = "" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "0" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "A" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = (string)null },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "0", FaixaFinal = (string)null },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "", FaixaFinal = (string)null },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = (string)null, FaixaFinal = (string)null },
            };
        }

        public static IntervenienteSequenceControlTestParameters[] EstouraErroCasoFaixaCarregadaNaoSeguirOPadraoCorreto()
        {
            var mensagemErroEsperado = "ValidarFaixaNumericaParametrizadaCarregada - ";

            return new IntervenienteSequenceControlTestParameters[]
            {
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Final Menor Que Faixa Inicial", FaixaInicial = "100", FaixaFinal = "10" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Inicial Zerada", FaixaInicial = "0", FaixaFinal = "10" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Final Zerada", FaixaInicial = "10", FaixaFinal = "0" },
                new IntervenienteSequenceControlTestParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Menor que 1", FaixaInicial = "10", FaixaFinal = "10" },
            };
        }

        public static IntervenienteSequenceControlTestParameters[] EstouraErroCasoVerifiqueProbabilidadeDeDuplicidadeDeFaixa()
        {
            var mensagemErroEsperado = "ValidaProbabilidadeDeDuplicidadeFaixaUtilizada";

            var intervenienteControleFaixa = new IntervenienteSequence() { UltimoID = 15, FaixaInicial = 10, FaixaFinal = 20 };

            return new IntervenienteSequenceControlTestParameters[]
            {
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "10",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "20",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "30",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "20",
                    FaixaFinal = "30",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "15",
                    FaixaFinal = "40",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "9",
                    FaixaFinal = "21",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "9",
                    FaixaFinal = "11",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "19",
                    FaixaFinal = "21",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceControlTestParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "18",
                    FaixaFinal = "19",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
            };
        }
    }
}
