using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Oportunidade
{
    public class OportunidadeContatoDTO
    {

        public int Usina { get; set; }
        public int NumeroOportunidade { get; set; }
        public int AnoOportunidade { get; set; }
        public int Sequencia { get; set; }
        public string Nome { get; set; }
        public CadastroGeralDTO Funcao { get; set; }
        public int FuncaoCodigo { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }

    }
}
