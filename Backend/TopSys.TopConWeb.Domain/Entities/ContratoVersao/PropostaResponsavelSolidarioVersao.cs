using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaResponsavelSolidarioVersao : PropostaResponsavelSolidarioBase<PropostaVersao> , IPropostaDadosPessoaisVersao
    {
        public int NumeroVersao { get; set; }
    }
}
