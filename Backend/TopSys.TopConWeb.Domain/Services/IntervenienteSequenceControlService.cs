using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;


namespace TopSys.TopConWeb.Domain.Services
{
    public class IntervenienteSequenceControlService : IIntervenienteSequenceControlService
    {
        private readonly IIntervenienteRepository _intervenienteRepository;

        private readonly IIntervenienteSequenceRepository _intervenienteControleFaixaRepository;

        private readonly ParametroIntervenienteSequence _intervenienteControleFaixaParametro;

        public IntervenienteSequenceControlService(
            IIntervenienteRepository intervenienteRepository,
            IIntervenienteSequenceRepository intervenienteControleFaixaRepository,
            ParametroIntervenienteSequence intervenienteControleFaixaParametro
            )
        {
            _intervenienteRepository = intervenienteRepository;
            _intervenienteControleFaixaParametro = intervenienteControleFaixaParametro;
            _intervenienteControleFaixaRepository = intervenienteControleFaixaRepository;
        }

        public int GerarProximaSequencia()
        {
            ValidaIntegridadeDaTabela();

            var proximaSequencia = _intervenienteControleFaixaRepository.AtualizarEObterProximaSequencia(
                _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor,
                _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor
            );

            return proximaSequencia;
        }

        public void ValidaIntegridadeDaTabela()
        {
            _intervenienteControleFaixaRepository.CriaOuIgnoraTabela();

            if (_intervenienteControleFaixaRepository.ObterFaixa(
                _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor, 
                _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor) == 0
                )
            {
                if (_intervenienteControleFaixaRepository.ValidaProbabilidadeDeDuplicidadeFaixaUtilizada(
                    _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor, 
                    _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor))
                    throw new Exception($@"A faixa informada está dentro do intervalo de uma faixa já cadastrada para os intervenientes!
                        Por favor, forneça uma nova faixa em
                        ParametroN -> Grupo '{_intervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN}' -> Chave '{_intervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN}' e Chave '{_intervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN}' .",
                        new Exception("ValidaProbabilidadeDeDuplicidadeFaixaUtilizada")
                    );

                if (_intervenienteRepository.ObterCodigoMaximoCadastrado(
                    _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor,
                    _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor) >= _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor)
                        throw new Exception($@"A Faixa final não pode ser usada pois o último código encontrado é maior ou igual a ela para os intervenientes!
                            Por favor, forneça uma nova faixa em
                            ParametroN -> Grupo '{_intervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN}' -> Chave '{_intervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN}' .",
                            new Exception("ValidaCodigoMaiorFaixaFinal")
                    );
            
            }

            _intervenienteControleFaixaRepository.SincronizarFaixa(
                new IntervenienteSequence
                {
                    FaixaInicial = _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor,
                    FaixaFinal = _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor
                }

            );

            if (_intervenienteControleFaixaRepository.ValidaFaixaTotalmenteUtilizada(
                _intervenienteControleFaixaParametro.FaixaInicialIntervenienteValor, 
                _intervenienteControleFaixaParametro.FaixaFinalIntervenienteValor))
            {
                throw new Exception($@"A faixa informada já foi totalmente utilizada para intervenientes!
                    Por favor, forneça uma nova faixa em
                    ParametroN -> Grupo '{_intervenienteControleFaixaParametro.GrupoFaixaIntervenienteParametroN}' -> Chave '{_intervenienteControleFaixaParametro.FaixaInicialIntervenienteParametroN}' e Chave '{_intervenienteControleFaixaParametro.FaixaFinalIntervenienteParametroN}' .",
                    new Exception("ValidaFaixaTotalmenteUtilizada")
                );
            }
            

            if (_intervenienteControleFaixaRepository.ValidaDuplicacoesDeFaixasIniciais())
            {
                throw new Exception($@"Foram encontradas duplicidades nas faixas iniciais para os intervenientes!
                    Por favor, entre em contato com o suporte para validar a situação.",
                    new Exception("ValidaDuplicidadeFaixaInicial")
                 );

            }

            if (_intervenienteControleFaixaRepository.ValidaDuplicacoesDeFaixasFinais())
            {
                throw new Exception($@"Foram encontradas duplicidades nas faixas finais para os intervenientes!
                    Por favor, entre em contato com o suporte para validar a situação.",
                    new Exception("ValidaDuplicidadeFaixaFinal")
                 );
            }

            if (_intervenienteControleFaixaRepository.ValidaCodigosForaDeFaixa())
            {
                throw new Exception($@"Foram encontrados códigos fora de suas respectivas faixas para os intervenientes!
                    Por favor, entre em contato com o suporte para validar a situação.",
                    new Exception("ValidaCodigosForaDeFaixa")
                 );
            }
        }
    }
}
