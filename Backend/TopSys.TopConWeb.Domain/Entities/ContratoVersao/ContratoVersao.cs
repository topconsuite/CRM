using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoVersao : ContratoBase<ContratoTracoReajusteVersao, ContratoBombaReajusteVersao>
    {
        public int NumeroVersao { get; set; }
        public DateTime? DataVersaoCriada { get; set; }
    }
}
