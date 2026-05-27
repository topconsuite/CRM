using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Filial
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        public string PermiteDocumentoDiferentePadraoRemessa { get; set; }

        public string PermiteDocumentoDiferentePadraoBomba { get; set; }

        public EValorDanfe ValorDanfe { get; set; }

        //For Public Integration
        public string EnderecoCep { get; set; }

        public string EnderecoLogradouro { get; set; }

        public int EnderecoNumero { get; set; }

        public string EnderecoComplemento { get; set; }

        public string EnderecoBairro { get; set; }

        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        public string Cnpj { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoMunicipal { get; set; }

        public int CentroCusto { get; set; }
    }
}
