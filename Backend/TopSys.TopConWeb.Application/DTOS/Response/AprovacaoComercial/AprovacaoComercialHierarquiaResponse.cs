using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaResponse
    {
        public Guid Id { get; set; }

        public Guid AprovacaoComercialUsinaId { get; set; }
        public AprovacaoComercialUsinaResponse AprovacaoComercialUsina { get; set; }

        public string Titulo { get; set; }
        public int NivelAutoridade { get; set; }
        public int QuantidadeAprovacoesNecessarias { get; set; }

        public bool AprovacaoObrigatoria { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<AprovacaoComercialHierarquiaCondicaoResponse> Condicoes { get; set; }

    }

    public class AprovacaoComercialHierarquiaCondicaoResponse
    {
        public Guid Id { get; set; }

        public double ValorDe { get; set; }
        public double ValorAte { get; set; }

        public double PercentualDe { get; set; }
        public double PercentualAte { get; set; }

        public Guid TipoPessoaId { get; set; }
        public AprovacaoComercialTipoPessoaResponse TipoPessoa { get; set; }


        public Guid AprovacaoComercialHierarquiaId { get; set; }


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
                    throw new Exception($"AprovacaoComercialHierarquiaCondicao : Valor não confere com 'VendaTracos', 'VendaBomba', 'MargemMCC', 'MargemTransporte', 'Ebtida' ou 'Volume'(Valor atual {this.Valor})");

            }
        }

    }

    public class AprovacaoComercialHierarquiaUsuarioResponse
    {
        public Guid Id { get; set; }

        public Guid AprovacaoComercialHierarquiaId { get; set; }

        public string UsuarioId { get; set; }
    }
}
