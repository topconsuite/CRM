using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.SharedKernel.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Interveniente : IHasEndereco, IQueryResult
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }
        
        public string IntervenienteTipo { get; set; }
        public string CpfCnpj { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        public string Profissao { get; set; }
        public string EmpresaTrabalho { get; set; }
        public int TelefoneComercialDdd { get; set; }
        public int TelefoneComercialNumero { get; set; }
        public string NomeMae { get; set; }
        public string NomeConjuge { get; set; }
        public string Contato { get; set; }
        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }
        public int Ramal { get; set; }
        public int CelularDdd { get; set; }
        public int CelularNumero { get; set; }
        public string Email { get; set; }
        public string EmailCobranca { get; set; }

        public int? VendedorCodigo { get; set; } = 0;
        public virtual Vendedor Vendedor { get; set; }

        public float LimiteValor { get; set; }

        public DateTime? LimiteData { get; set; } 

        public int? BloqueioMotivoCodigo { get; set; } = 0;

        public string BloqueioObservacao { get; set; } = "";

        public string Observacao { get; set; }

        public string bombista { get; set; } = "N";

        public virtual CadastroGeral BloqueioMotivo { get; set; }

        public virtual Municipio EnderecoMunicipio { get; set; }

        public virtual ICollection<Contrato> Contratos { get; set; }

        public string IdAtualizacao { get; set; }

        public string RetemIss { get; set; } = "X";

        public string IdAprovacaoRetencaoIss { get; set; } = "";

        public string Cliente { get; set; }

        public int? GrupoEconomicoCodigo { get; set; }

        public virtual GrupoEconomico GrupoEconomico { get; set; } = null;

        //For Public Integration
        public string Fornecedor { get; set; }
        public string Transportador { get; set; }
        public string PrestadorServico { get; set; }
        public string OrgaoPublico { get; set; }
        public string Outros { get; set; } 
        public string Cei { get; set; }
        public int Atividade { get; set; } = 0;
        public int TipoCobranca { get; set; } = 0;
        public float PorcentagemDesconto { get; set; } = 0;
        public string In86 { get; set; } = "N";
        public string ContaContabil { get; set; } = "";
        public string FornecedorMp { get; set; } = "";
        public int Regiao { get; set; } = 0;
        public int Rota { get; set; } = 0;
        public int RotaSequencia { get; set; } = 0;
        public int Transp { get; set; } = 0;
        public string LocalEntrega { get; set; } = "";
        public string Especificacao { get; set; } = "";
        public int? PortadorCobranca { get; set; }
        public string Funcionario { get; set; } = "N";
        public string Site { get; set; } = "";
        public string AprovacaoEngenharia { get; set; } = "N";
        public string SimplesNacional { get; set; } = "N";
        public string RetemInss { get; set; } = "X";
        public int ContribuiIcms { get; set; } = 0;
        public string Inativo { get; set; } = "N";
        public string IdExterno { get; set; } = "";
        public string RetemIrrf { get; set; } = "N";
        public string RetemCofins { get; set; } = "N";
        public string RetemPis { get; set; } = "N";
        public string RetemCsll { get; set; } = "N";
        public DateTime DataAtualizacao { get; set; }
        
        public void AtualizaLimiteCredito(DateTime? limiteData, float limiteValor, int bloqueioMotivoCodigo)
        {
            this.LimiteData = limiteData;
            this.LimiteValor = limiteValor;
            this.BloqueioMotivoCodigo = bloqueioMotivoCodigo;
        }

        public void AtualizaApenasLimiteCredito(DateTime? limiteData, float limiteValor)
        {
            this.LimiteData = limiteData;
            this.LimiteValor = limiteValor;
        }
        public void AtualizaInformacoesBloqueio(int bloqueioMotivoCodigo, string observacaoBloqueio)
        {
            this.BloqueioMotivoCodigo = bloqueioMotivoCodigo;
            this.BloqueioObservacao = observacaoBloqueio;
        }

        public void AprovaIss(string usuario)
        {
            IdAprovacaoRetencaoIss = StringHelper.GetIDD(usuario);
        }

    }
}
