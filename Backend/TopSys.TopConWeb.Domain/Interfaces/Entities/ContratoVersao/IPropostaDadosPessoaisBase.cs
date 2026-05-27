using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IPropostaDadosPessoaisBase<TProposta> : IDadosPessoais
    {
        int UsinaCodigo { get; set; }
        int PropostaAno { get; set; }
        int PropostaNumero { get; set; }
        TProposta Proposta { get; set; }
    }
}
