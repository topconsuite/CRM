using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ParametroIntervenienteSequence
    {
        private readonly IParametroRepository _parametroRepository;
        public ParametroIntervenienteSequence(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        private int faixaInicialIntervenienteValor = 0;
        private int faixaFinalIntervenienteParametro = 0;
        public int FaixaInicialIntervenienteValor
        {
            get
            {
                if(!FaixasEstaoCarregadasEPadronizadas) CarregaFaixasEVerificaPadronizacao();
                return faixaInicialIntervenienteValor;
            }
 
            private set => faixaInicialIntervenienteValor = value;
        }
        public int FaixaFinalIntervenienteValor
        {
            get
            {
                if (!FaixasEstaoCarregadasEPadronizadas) CarregaFaixasEVerificaPadronizacao();
                return faixaFinalIntervenienteParametro;
            }
            private set => faixaFinalIntervenienteParametro = value;
        }
        private string FaixaInicialIntervenienteParametro { get; set; } = string.Empty;
        private string FaixaFinalIntervenienteParametro { get; set; } = string.Empty;
        public string FaixaInicialIntervenienteParametroN { get => "FaixaInicialInterv"; }
        public string FaixaFinalIntervenienteParametroN { get => "CodFinalInterv"; }
        public string GrupoFaixaIntervenienteParametroN { get => "TopCon"; }
        private bool FaixasEstaoCarregadasEPadronizadas { get; set; } = false;

        private void CarregaFaixasEVerificaPadronizacao()
        {
            var resultFaixaInicialIntervenienteValor = 0;
            var resultFaixaFinalIntervenienteValor = 0;

            FaixaInicialIntervenienteParametro = _parametroRepository.ObterParametroN(
                   GrupoFaixaIntervenienteParametroN,
                   FaixaInicialIntervenienteParametroN
             );

            FaixaFinalIntervenienteParametro = _parametroRepository.ObterParametroN(
                   GrupoFaixaIntervenienteParametroN,
                   FaixaFinalIntervenienteParametroN
               );

            if (!(int.TryParse(FaixaInicialIntervenienteParametro, out resultFaixaInicialIntervenienteValor)))
                throw new Exception($@"Não foi possível carregar a faixa inicial parametrizada para os intervenientes! 
                    Por favor, forneça ou verifique se o valor da faixa inicial está correto em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave '{FaixaInicialIntervenienteParametroN}' .",
                    new Exception("CarregarFaixaNumericaParametrizada"));


            if (!(int.TryParse(FaixaFinalIntervenienteParametro, out resultFaixaFinalIntervenienteValor)))
                throw new Exception($@"Não foi possível carregar a faixa final parametrizada para os intervenientes! 
                    Por favor, forneça ou verifique se o valor da faixa final está correto em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave '{FaixaInicialIntervenienteParametroN}' .",
                    new Exception("CarregarFaixaNumericaParametrizada"));

            if (resultFaixaInicialIntervenienteValor == 0)
                throw new Exception($@"Faixa inicial para controle de faixa não pode ser definida como zero (0) para os intervenientes! 
                    Por favor, forneça valor diferente de zero para a faixa inicial em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave '{FaixaInicialIntervenienteParametroN}'.",
                    new Exception("ValidarFaixaNumericaParametrizadaCarregada - Faixa Inicial Zerada"));

            if (resultFaixaFinalIntervenienteValor == 0)
                throw new Exception($@"Faixa final para controle de faixa inicial não pode ser definida como zero (0) para os intervenientes! 
                    Por favor, forneça valor diferente de zero para a faixa em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave '{FaixaInicialIntervenienteParametroN}'.",
                    new Exception("ValidarFaixaNumericaParametrizadaCarregada - Faixa Final Zerada"));

            if (resultFaixaFinalIntervenienteValor < resultFaixaInicialIntervenienteValor)
                throw new Exception($@"Faixa final para controle de faixa não pode ser inferior que a faixa inicial para os intervenientes!
                    Por favor, forneça valor da faixa final superior a faixa inicial em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave Inicial '{FaixaInicialIntervenienteParametroN}' e Chave Final'{FaixaFinalIntervenienteParametroN}' .",
                    new Exception("ValidarFaixaNumericaParametrizadaCarregada - Faixa Final Menor Que Faixa Inicial"));

            if ((resultFaixaFinalIntervenienteValor - resultFaixaInicialIntervenienteValor) < 1)
                throw new Exception($@"Faixa cadastrada não está respeitando a faixa mínima para os intervenientes!
                    Por favor, forneça uma faixa maior que 1 para a faixa em 
                    ParametroN -> Grupo '{GrupoFaixaIntervenienteParametroN}' -> Chave Inicial'{FaixaInicialIntervenienteParametroN}' e Chave Final'{FaixaFinalIntervenienteParametroN}' .",
                    new Exception("ValidarFaixaNumericaParametrizadaCarregada - Faixa Menor que 1"));

            FaixaInicialIntervenienteValor = resultFaixaInicialIntervenienteValor;
            FaixaFinalIntervenienteValor = resultFaixaFinalIntervenienteValor;

            FaixasEstaoCarregadasEPadronizadas = true;
        }
       
 

    }
}
