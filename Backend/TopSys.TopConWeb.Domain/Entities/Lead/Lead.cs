using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities.Lead
{
    public class Lead : IHasEndereco
    {
        public virtual Usina Usina { get; set; }
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }
        //public Visita Visita { get; set; }
        public int VisitaNumero { get; set; }
        public int VisitaAno { get; set; }
        public int OportunidadeNumero { get; set; }
        public int OportunidadeAno { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; } = "";
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; } = "";
        public Vendedor Vendedor { get; set; }
        public int? VendedorCodigo { get; set; }
        public CadastroGeral ViaCaptacao { get; set; }
        public int? ViaCaptacaoCodigo { get; set; }
        public LeadFase Fase { get; set; }
        public int? FaseCodigo { get; set; }
        public EClassificacaoTemperatura Classificacao { get; set; }
        public string ProximaEtapa { get; set; } = "";
        public string ObraNome { get; set; } = "";
        public string EnderecoCep { get; set; } = "";
        public string EnderecoLogradouro { get; set; } = "";
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; } = "";
        public string EnderecoBairro { get; set; } = "";
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public Municipio EnderecoMunicipio { get; set; }
        public MotivoPerda MotivoPerda { get; set; }
        public int? MotivoPerdaCodigo { get; set; }
        public string ObservacaoInterna { get; set; } = "";
        public LeadContato ContatoPrincipal { get; set; }
        public LeadContato ContatoSecundario { get; set; }
        public string IdCadastro { get; set; } = "";
        public string IdAtualizacao { get; set; } = "";
        public ICollection<LeadLog> Logs { get; set; }

        public string GetClassificacaoDescricao
        {
            get
            {
                if (Classificacao == EClassificacaoTemperatura.Quente)
                    return "QUENTE";
                else if (Classificacao == EClassificacaoTemperatura.Morno)
                    return "MORNO";

                return "FRIO";
            }
        }
    }
}
