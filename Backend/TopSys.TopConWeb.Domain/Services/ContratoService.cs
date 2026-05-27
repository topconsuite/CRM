using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ContratoService : ServiceBase<Contrato>, IContratoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IComercialLegacyService _comercialLegacyService;

        public ContratoService(IContratoRepository contratoRepository, IComercialLegacyService comercialLegacyService) 
            : base(contratoRepository)
        {
            _contratoRepository = contratoRepository;
            _comercialLegacyService = comercialLegacyService;
        }

        public ICollection<ContratoPagamento> ObterContratoPagamentos(int usina, int numeroContrato, int anoContrato)
        {
            return _contratoRepository.ObterContratoPagamentos(usina, numeroContrato, anoContrato);
        }
        public ICollection<ContratoPagamentoVersao> ObterContratoPagamentosVersao(int numVersao, int usina, int numeroContrato, int anoContrato)
        {
            return _contratoRepository.ObterContratoPagamentosVersao(numVersao, usina, numeroContrato, anoContrato);
        }

        public void AprovarContratoRevalidacaoContrato(string usuario, Contrato contrato, string observacaoLog, ref string mensagem)
        {
            mensagem = _comercialLegacyService.FinalizarRevalidacaoCadastro(usuario, contrato, observacaoLog);
        }

        public bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out Contrato contrato, out string mensagem)
        {
            var retornoGerarContrato = _comercialLegacyService.GerarContrato(usuario, propostaUsina, propostaAno, propostaNumero, out contrato, out mensagem);

            if (contrato != null)
            {
                var contratoNovo = _contratoRepository.ObterPorId(propostaUsina, contrato.Ano, contrato.Numero);
                if (contratoNovo != null)
                {
                    if (contratoNovo.Status == EContratoStatus.Aprovado)
                    {
                        contratoNovo.FechadoSimNao = "S";

                        _contratoRepository.Atualizar(contratoNovo);
                    }
                }
            }

            return retornoGerarContrato;
        }

        public ICollection<Contrato> ListarContratosRevalidacaoCadastro()
        {
            return _contratoRepository.ListarContratosRevalidacaoCadastro();
        }

        public int GetUltimaVersaoContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetUltimaVersaoContrato(codUsina, anoContrato, numeroContrato);
        }

        public int GetUltimaVersaoContratoAberta(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetUltimaVersaoContratoAberta(codUsina, anoContrato, numeroContrato);
        }

        public int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetUltimaVersaoContratoAprovado(codUsina, anoContrato, numeroContrato);
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _contratoRepository.AdicionarVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _contratoRepository.ExcluirVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }
        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _contratoRepository.AdicionarContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            _contratoRepository.ExcluirContrato(codUsina, anoContrato, numeroContrato);
        }

        public ContratoVersao ContratoVersaoObterPorId(int numeroVersao, int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.ContratoVersaoObterPorId(numeroVersao, codUsina, anoContrato, numeroContrato);
        }

        public string GetSegmentacaoContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetSegmentacaoContrato(codUsina, anoContrato, numeroContrato);
        }

        public DateTime? GetDataCriacaoVersaoContrato(int numVersao, int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoRepository.GetDataCriacaoVersaoContrato(numVersao, codUsina, anoContrato, numeroContrato);
        }

        public ICollection<ContratoFinalidade> ListarFinalidades()
        {
            return _contratoRepository.ListarFinalidades();
        }

        public void AtualizarDataEncerramentoEStatusContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            DateTime? dataEncerramento = null;
            int status = 0;

            _contratoRepository.ObterDataEncerramentoEStatusVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao, out dataEncerramento, out status);

            _contratoRepository.AtualizarDataEncerramentoEStatusContrato(codUsina, anoContrato, numeroContrato, dataEncerramento, status);
        }

        public void CriarTabelaTemporariaTaxaExtraVersao(int numVersao, int codUsina, int anoContrato, int numeroContrato)
        {
            _contratoRepository.CriarTabelaTemporariaTaxaExtraVersao(numVersao, codUsina, anoContrato, numeroContrato);
        }

        public int ObterUsinaClausula(int usinaEntrega)
        {
            return _contratoRepository.ObterUsinaClausula(usinaEntrega);
        }

        public string ObterContratoUsina(int usina, string segmento)
        {
            return _contratoRepository.ObterContratoUsina(usina, segmento);
        }
    }
}
