using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class IntervenienteSequence
    {
        public int UltimoID { get; set; }
        public int FaixaInicial { get; set; }
        public int FaixaFinal { get; set; }
    }
}
