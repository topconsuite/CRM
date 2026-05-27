
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;

namespace TopSys.TopConWeb.Tests.Domain
{
    public static class IntervenienteSequenceData
    {
        public static IntervenienteSequenceParameters[] EstouraErroCasoNaoEstiverParametrizadoCorretamente()
        {
            var mensagemErroEsperado = "CarregarFaixaNumericaParametrizada";

            return new IntervenienteSequenceParameters[]
            {
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "", FaixaFinal = "" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "0" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "A" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = "" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "A", FaixaFinal = (string)null },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "0", FaixaFinal = (string)null },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = "", FaixaFinal = (string)null },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado, FaixaInicial = (string)null, FaixaFinal = (string)null },
            };
        }

        public static IntervenienteSequenceParameters[] EstouraErroCasoFaixaCarregadaNaoSeguirOPadraoCorreto()
        {
            var mensagemErroEsperado = "ValidarFaixaNumericaParametrizadaCarregada - ";

            return new IntervenienteSequenceParameters[]
            {
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Final Menor Que Faixa Inicial", FaixaInicial = "100", FaixaFinal = "10" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Inicial Zerada", FaixaInicial = "0", FaixaFinal = "10" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Final Zerada", FaixaInicial = "10", FaixaFinal = "0" },
                new IntervenienteSequenceParameters(){MessagemEsperada = mensagemErroEsperado + "Faixa Menor que 1", FaixaInicial = "10", FaixaFinal = "10" },
            };
        }

        public static IntervenienteSequenceParameters[] EstouraErroCasoVerifiqueProbabilidadeDeDuplicidadeDeFaixa()
        {
            var mensagemErroEsperado = "ValidaProbabilidadeDeDuplicidadeFaixaUtilizada";

            var intervenienteControleFaixa = new IntervenienteSequence() { UltimoID = 15, FaixaInicial = 10, FaixaFinal = 20 };

            return new IntervenienteSequenceParameters[]
            {
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "10",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "20",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "1",
                    FaixaFinal = "30",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "20",
                    FaixaFinal = "30",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "15",
                    FaixaFinal = "40",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "9",
                    FaixaFinal = "21",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "9",
                    FaixaFinal = "11",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "19",
                    FaixaFinal = "21",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
                new IntervenienteSequenceParameters(){
                    MessagemEsperada = mensagemErroEsperado,
                    FaixaInicial = "18",
                    FaixaFinal = "19",
                    IntervenienteControleFaixa = intervenienteControleFaixa
                },
            };
        }
    }
}
