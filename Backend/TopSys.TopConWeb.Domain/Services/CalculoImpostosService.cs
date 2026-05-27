using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CalculoImpostosService : ICalculoImpostosService
    {
        private readonly IUsinaService _usinaService;

        public CalculoImpostosService(IUsinaService usinaService)
        {
            _usinaService = usinaService;
        }

        public float CalcularIss(Obra obra, float valorM3, float valorMaterial, bool calculoBomba = false, bool calcularISSParaPrecoSugerido = false)
        {
            var usina = _usinaService.ObterPorId(obra.UsinaEntregaCodigo) ??
                _usinaService.ObterPorId(obra.UsinaEntrega.Codigo);

            var filial = _usinaService.ObterPorId<Filial>(usina.FilialCodigo);

            if (filial?.ValorDanfe == EValorDanfe.Venda)
                return 0;

            var empresa = _usinaService.ObterPorId<Empresa>(usina.EmpresaCodigo);

            var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

            if (municipio == null)
                return 0;

            var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : municipio.AliquotaIss;

            if (calcularISSParaPrecoSugerido)
            {
                return CalcularISSParaPrecoSugerido(municipio, valorM3, valorMaterial, aliquota);
            }

            switch (municipio.BaseCalculo)
            {
                case EBaseCalculoIss.Servico:
                    return CalcularIssServico(valorM3, aliquota, valorMaterial);
                case EBaseCalculoIss.TotalFatura:
                    return CalcularImpostoSomenteSobreAliquota(valorM3, aliquota);
                case EBaseCalculoIss.PorcentagemFixaSobreValorFatura:
                    if (calculoBomba)
                        return CalcularImpostoSomenteSobreAliquota(valorM3, aliquota);
                    else
                        return CalcularIssPorcentagemFixaSobreValorFatura(valorM3, aliquota, municipio.PorcentagemServico);
                case EBaseCalculoIss.PorcentagemDeducaoSobreMaterial:
                    return CalcularIssDeducaoSobreMaterial(valorM3, aliquota, municipio.PorcentagemServico, valorMaterial);
                case EBaseCalculoIss.PorcentagemDeducaoMaterialLimitado:
                    return CalcularIssDeducaoSobreMaterialLimitado(valorM3, aliquota, municipio.PorcentagemDeducaoMaterial, valorMaterial);
                default:
                    return 0;
            }
        }

        public float CalcularIss(ObraVersao obra, float valorM3, float valorMaterial, bool calculoBomba = false, bool calcularISSParaPrecoSugerido = false)
        {
            var usina = _usinaService.ObterPorId(obra.UsinaEntregaCodigo) ??
                _usinaService.ObterPorId(obra.UsinaEntrega.Codigo);

            var filial = _usinaService.ObterPorId<Filial>(usina.FilialCodigo);

            if (filial?.ValorDanfe == EValorDanfe.Venda)
                return 0;

            var empresa = _usinaService.ObterPorId<Empresa>(usina.EmpresaCodigo);

            var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

            if (municipio == null)
                return 0;

            var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : municipio.AliquotaIss;

            if (calcularISSParaPrecoSugerido)
            {
                return CalcularISSParaPrecoSugerido(municipio, valorM3, valorMaterial, aliquota);
            }

            switch (municipio.BaseCalculo)
            {
                case EBaseCalculoIss.Servico:
                    return CalcularIssServico(valorM3, aliquota, valorMaterial);
                case EBaseCalculoIss.TotalFatura:
                    return CalcularImpostoSomenteSobreAliquota(valorM3, aliquota);
                case EBaseCalculoIss.PorcentagemFixaSobreValorFatura:
                    if (calculoBomba)
                        return CalcularImpostoSomenteSobreAliquota(valorM3, aliquota);
                    else
                        return CalcularIssPorcentagemFixaSobreValorFatura(valorM3, aliquota, municipio.PorcentagemServico);
                case EBaseCalculoIss.PorcentagemDeducaoSobreMaterial:
                    return CalcularIssDeducaoSobreMaterial(valorM3, aliquota, municipio.PorcentagemServico, valorMaterial);
                case EBaseCalculoIss.PorcentagemDeducaoMaterialLimitado:
                    return CalcularIssDeducaoSobreMaterialLimitado(valorM3, aliquota, municipio.PorcentagemDeducaoMaterial, valorMaterial);
                default:
                    return 0;
            }
        }

        public float CalcularIssServico(float valorM3, float aliquota, float valorMaterial)
        {
            return (valorM3 - valorMaterial) * (aliquota / 100);
        }

        public float CalcularImpostoSomenteSobreAliquota(float valorM3, float aliquota)
        {
            return (valorM3) * (aliquota / 100);
        }

        public float CalcularIssPorcentagemFixaSobreValorFatura(float valorM3, float aliquota, float deducao)
        {
            return (valorM3 * (deducao / 100)) * (aliquota / 100);
        }

        public float CalcularIssDeducaoSobreMaterial(float valorM3, float aliquota, float deducao, float valorMaterial)
        {
            return ((valorM3 - (valorMaterial * (deducao / 100)))) * (aliquota / 100);
        }

        public float CalcularIssDeducaoSobreMaterialLimitado(float valorM3, float aliquota, float deducao, float valorMaterial)
        {
            float deducaoBase = CalcularDeducaobase(valorM3, valorMaterial, deducao);
            return ((valorM3 - deducaoBase) * (aliquota / 100));
        }

        private float CalcularISSParaPrecoSugerido(Municipio municipio, float valorM3, float valorMaterial, float aliquota)
        {
            float deducaoBase = CalcularDeducaobase(valorM3, valorMaterial, municipio.PorcentagemDeducaoMaterial);
            var valorServico = valorM3 - valorMaterial;

            switch (municipio.BaseCalculo)
            {
                case EBaseCalculoIss.Servico:
                    return valorServico / (1 - aliquota / 100) - valorServico;
                case EBaseCalculoIss.TotalFatura:
                    return (valorServico / (1 - aliquota / 100) - valorServico) + (valorMaterial / (1 - aliquota / 100) - valorMaterial);
                case EBaseCalculoIss.PorcentagemFixaSobreValorFatura:
                    return ((valorMaterial * municipio.PorcentagemServico / 100) / (1 - aliquota / 100) - (valorMaterial * municipio.PorcentagemServico / 100)) + ((valorServico * municipio.PorcentagemServico / 100) / (1 - aliquota / 100) - (valorServico * municipio.PorcentagemServico / 100));
                case EBaseCalculoIss.PorcentagemDeducaoSobreMaterial:
                    return (valorM3 - (valorMaterial * municipio.PorcentagemServico / 100)) / (1 - aliquota / 100) - (valorM3 - (valorMaterial * municipio.PorcentagemServico / 100));
                case EBaseCalculoIss.PorcentagemDeducaoMaterialLimitado:
                    return (valorM3 - deducaoBase) / (1 - aliquota / 100) - (valorM3 - deducaoBase); ;
                default:
                    return 0;
            }
            
            //var baseCalculoImpostoMunicipal = SelecionarBaseCalculoPorRegraMunicipal(municipio, valorM3, valorMaterial);
            //return baseCalculoImpostoMunicipal / (1 - aliquota / 100) - baseCalculoImpostoMunicipal;
        }

        private float SelecionarBaseCalculoPorRegraMunicipal(Municipio municipio, float valorM3, float valorMaterial)
        {
            float deducaoBase = CalcularDeducaobase(valorM3, valorMaterial, municipio.PorcentagemDeducaoMaterial);

            switch (municipio.BaseCalculo)
            {
                case EBaseCalculoIss.Servico:
                    return valorM3 - valorMaterial;
                case EBaseCalculoIss.TotalFatura:
                    return valorM3;
                case EBaseCalculoIss.PorcentagemFixaSobreValorFatura:
                    return valorM3 * municipio.PorcentagemServico / 100;
                case EBaseCalculoIss.PorcentagemDeducaoSobreMaterial:
                    return valorM3 - (valorMaterial * municipio.PorcentagemServico / 100);
                case EBaseCalculoIss.PorcentagemDeducaoMaterialLimitado:
                    return valorM3 - deducaoBase;
                default:
                    return 0;
            }
        }

        private float CalcularDeducaobase(float valorM3, float valorMaterial, float deducao)
        {
            if ((valorMaterial / valorM3) < (deducao / 100))
                return valorMaterial;
            else
                return valorMaterial * (deducao / 100);
        }
    }
}
