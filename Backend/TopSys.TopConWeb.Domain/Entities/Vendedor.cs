using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Vendedor
    {
        public int  Codigo { get; set; }

        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        public string Ativo { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int Celular { get; set; }
        public string Complemento { get; set; }
        public int Usina { get; set; }
        public int Municipio { get; set; }
        public int DDDCelular { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
        public int VendedorPadrinho { get; set; }
        public int Interveniente { get; set; }
        public int EnderecoNumero { get; set; }
        public int CondicaoPagamento { get; set; }
        public int Re { get; set; }
        public string Funcao { get; set; }
        //public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Cep { get; set; }
        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }
        public string CpfCnpj { get; set; }
    }
}
