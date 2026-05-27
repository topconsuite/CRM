using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class CadastroGeralViaCaptacaoDTO
    {

        public int Codigo { get; set; }
        public string Ativo { get; set; }
        public ECadastroGeralViaCaptacaoTipoIndicador TipoIndicacao { get; set; } 

    }
}
