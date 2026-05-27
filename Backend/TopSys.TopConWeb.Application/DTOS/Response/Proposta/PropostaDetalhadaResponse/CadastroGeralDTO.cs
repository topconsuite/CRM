using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class CadastroGeralDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public CadastroGeralViaCaptacaoDTO ViaCaptacao { get; set; }
    }
}
