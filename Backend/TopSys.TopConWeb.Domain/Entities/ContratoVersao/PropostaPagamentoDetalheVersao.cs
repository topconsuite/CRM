using System;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class PropostaPagamentoDetalheVersao : PropostaPagamentoDetalheBase
    {
        public int NumeroVersao { get; set; }
    }
}
