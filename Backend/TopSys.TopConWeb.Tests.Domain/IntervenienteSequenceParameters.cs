
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Repositories;

namespace TopSys.TopConWeb.Tests.Domain
{
    public class IntervenienteSequenceParameters
    {
        public string MessagemEsperada { get; set; }
        public string FaixaInicial { get; set; }
        public string FaixaFinal { get; set; }
        public IntervenienteSequence IntervenienteControleFaixa { get; set; }
    }
}
