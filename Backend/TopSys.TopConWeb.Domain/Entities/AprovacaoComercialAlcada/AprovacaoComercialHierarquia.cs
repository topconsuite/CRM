using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada
{
    public class AprovacaoComercialHierarquia
    {
        public Guid Id { get; set; }

        public Guid AprovacaoComercialUsinaId { get; set; }
        public AprovacaoComercialUsina AprovacaoComercialUsina { get; set; }

        public string Titulo { get; set; }
        public int NivelAutoridade { get; set; }
        public int QuantidadeAprovacoesNecessarias { get; set; }

        public bool AprovacaoObrigatoria { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AprovacaoComercialHierarquiaCondicao> Condicoes { get; set; }

    }

    public class AprovacaoComercialHierarquiaCondicao
    { 
        public Guid Id { get; set; }

        public double ValorDe { get; set; }
        public double ValorAte { get; set; }

        public double PercentualDe { get; set; }
        public double PercentualAte { get; set; }

        public Guid TipoPessoaId { get; set; }
        public AprovacaoComercialTipoPessoa TipoPessoa { get; set; }


        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public AprovacaoComercialHierarquia AprovacaoComercialHierarquia { get; set; }


        public string Valor { get; set; }
        public EAprovacaoComercialHierarquiaValor TipoValor // IGNORADO NO BANCO
        { 
            get 
            {
                if (this.Valor.Equals("VendaTracos"))
                    return EAprovacaoComercialHierarquiaValor.ValorVendaTracos;
                else if (this.Valor.Equals("VendaBomba"))
                    return EAprovacaoComercialHierarquiaValor.ValorVendaBomba;
                else if (this.Valor.Equals("MargemMCC"))
                    return EAprovacaoComercialHierarquiaValor.MargemMCC;
                else if (this.Valor.Equals("MargemTransporte"))
                    return EAprovacaoComercialHierarquiaValor.MargemTransporte;
                else if (this.Valor.Equals("Ebtida"))
                    return EAprovacaoComercialHierarquiaValor.Ebtida;
                else if (this.Valor.Equals("Volume"))
                    return EAprovacaoComercialHierarquiaValor.Volume;
                else
                    throw new Exception($"AprovacaoComercialHierarquiaCondicao : Valor não confere com 'VendaTracos', 'VendaBomba', 'MargemMCC', 'MargemTransporte' ou 'Ebtida' e 'Volume'(Valor atual {this.Valor})");

            } 
        }

    }

    public class AprovacaoComercialHierarquiaCondicaoPagamento
    {

        public Guid Id { get; set; }

        public Guid TipoPessoaId { get; set; }
        public AprovacaoComercialTipoPessoa TipoPessoa { get; set; }

        public int SegmentacaoId { get; set; }
        public Segmentacao Segmentacao { get; set; }

        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public AprovacaoComercialHierarquia AprovacaoComercialHierarquia { get; set; }

        public float MediaDiasDe { get; set; }
        public float MediaDiasAte { get; set; }

    }

    public class AprovacaoComercialHierarquiaUsuario
    {
        public Guid Id { get; set; }

        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public AprovacaoComercialHierarquia AprovacaoComercialHierarquia { get; set; }

        public string UsuarioId { get; set; }
    }

}
