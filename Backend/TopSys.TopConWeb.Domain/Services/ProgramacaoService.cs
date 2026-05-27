using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ProgramacaoService : ServiceBase<Programacao>, IProgramacaoService
    {
        private readonly IProgramacaoRepository _programacaoRepository;

        private readonly INotaFiscalFisicaService _notaFiscalFisicaService;

        private readonly IComercialLegacyService _comercialLegacyService;

        private readonly IObraTaxaRepository _obraTaxaRepository;

        private readonly IContratoRepository _contratoRepository;

        private readonly IMercadoriaRepository _mercadoriaRepository;

        private readonly IParametroRepository _parametroRepository;

        public ProgramacaoService(IProgramacaoRepository programacaoRepository, INotaFiscalFisicaService notaFiscalFisicaService, IComercialLegacyService comercialLegacyService, IObraTaxaRepository obraTaxaRepository, IContratoRepository contratoRepository, IMercadoriaRepository mercadoriaRepository, IParametroRepository parametroRepository)
            : base(programacaoRepository)
        {
            _programacaoRepository = programacaoRepository;
            _notaFiscalFisicaService = notaFiscalFisicaService;
            _comercialLegacyService = comercialLegacyService;
            _obraTaxaRepository = obraTaxaRepository;
            _contratoRepository = contratoRepository;
            _mercadoriaRepository = mercadoriaRepository;
            _parametroRepository = parametroRepository;
        }

        public IEnumerable<Programacao> ListarComPropostaContrato()
        {
            return _programacaoRepository.ListarComPropostaContrato();
        }

        public PagedList<Programacao> ListarComPropostaContratoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            return _programacaoRepository.ListarEmOrdemDecrescente(pagina, porPagina, filter);
        }

        public PagedList<Programacao> ListarComPropostaContratoEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            return _programacaoRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);
        }

        public IEnumerable<ProgramacaoLog> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia)
        {
            return _programacaoRepository.ListarProgramacaoLogsPorId(idUsina, obraNumero, sequencia);
        }

        public Programacao ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia, bool tracking = false)
        {
            var programacao = _programacaoRepository.ObterDetalhadaPorId(idUsina, obraNumero, sequencia, tracking);

            programacao.TemNotaFicalEmitida = _notaFiscalFisicaService.Emitida(programacao);

            return programacao;
        }

        public bool TemNotaFiscalEmitida(int idUsina, int obraNumero, int sequencia)
        {
            var programacao = _programacaoRepository.ObterPorId(idUsina, obraNumero, sequencia);

            if (programacao == null) return false;

            return _notaFiscalFisicaService.Emitida(programacao);
        }

        public bool Validar(Programacao programacao)
        {
            programacao.TemNotaFicalEmitida = _notaFiscalFisicaService.Emitida(programacao);

            return programacao.TemNotaEmitidaScopeIsValid();
        }

        public float ObterVolumeTotalProgramado(int idUsina, int obraNumero)
        {
            return _programacaoRepository.ObterVolumeTotalProgramado(idUsina, obraNumero);
        }

        public IEnumerable<ProgramacaoHora> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            return _programacaoRepository.ListarHorarios(idUsina, contratoAno, contratoNumero, sequencia);
        }

        public bool GeraProgramacao(int idUsina, int obraNumero, int sequencia, bool atualizaComplexidadeBombeado, bool gravaContinuidadeProgramacao, string usuario)
        {
            return _comercialLegacyService.GeraProgramacao(idUsina, obraNumero, sequencia, atualizaComplexidadeBombeado, gravaContinuidadeProgramacao, usuario);
        }

        public bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia,  string observacao, string usuario)
        {
            return _comercialLegacyService.RejeitaProgramacao(idUsina, obraNumero, sequencia, observacao, usuario);
        }

        public void GeraValorAvulsoCancelamento(Programacao programacao, string idUsuario)
        {
            ObraTaxa taxa = null;
            var tipoTaxa = (programacao.EquipamentoBombaCodigo != "" && programacao.EquipamentoBombaCodigo != "0") ? "CANCELAMENTO DE PROGRAMAÇÃO BOMBEADO" : "CANCELAMENTO DE PROGRAMAÇÃO";
            Mercadoria mercadoriaTaxaDeCancelamento = _mercadoriaRepository.ListarFiltrados(t => t.Descricao.Equals("TAXA DE CANCELAMENTO DE PROGRAMACAO")).FirstOrDefault();
            decimal valor = 0;

            TimeSpan diferencaDias = programacao.DataConcretagem - DateTime.Today;

            DateTime horarioAtual = DateTime.Now;
            int horas = int.Parse(programacao.Horario.Substring(0, 2));
            int minutos = int.Parse(programacao.Horario.Substring(2, 2));
            DateTime horarioFornecido = new DateTime(horarioAtual.Year, horarioAtual.Month, horarioAtual.Day, horas, minutos, 0);
            TimeSpan diferencaHoras = horarioFornecido - horarioAtual;

            if ((int)diferencaDias.TotalDays == 0) taxa = _obraTaxaRepository.ObterTaxaCancelamentoProgramacao(programacao.UsinaEntregaCodigo, programacao.ObraNumero, tipoTaxa, "Horas", diferencaHoras.Hours);
            if (taxa == null) taxa = _obraTaxaRepository.ObterTaxaCancelamentoProgramacao(programacao.UsinaEntregaCodigo, programacao.ObraNumero, tipoTaxa, "Dias", (int)diferencaDias.TotalDays);

            if (taxa == null) return;

            var quantidadeProgramacoesHora = _programacaoRepository.ObterQuantidadeDeProgramacoesHora(programacao.UsinaCodigo, (int)programacao.ContratoAno, (int)programacao.ContratoNumero, programacao.Sequencia);
            if (quantidadeProgramacoesHora == 0) quantidadeProgramacoesHora = 1;

            if (taxa.ValorPor == "VIAGEM") {
                if (taxa.ValorTipo == "%") valor = programacao.ValorTotal + (programacao.ValorTotal * ((decimal)taxa.Valor / 100)) * quantidadeProgramacoesHora;
                else if (taxa.ValorTipo == "R$") valor = (decimal)taxa.Valor * quantidadeProgramacoesHora;
            } else if (taxa.ValorPor == "PROGRAMAÇÃO") {
                if (taxa.ValorTipo == "%") valor = programacao.ValorTotal + (programacao.ValorTotal * ((decimal)taxa.Valor / 100));
                else if (taxa.ValorTipo == "R$") valor = (decimal)taxa.Valor;
            }
            var proximaSequenciaValorAvulso = _contratoRepository.ObterProximaSequenciaValorAvulso(programacao.UsinaEntregaCodigo);
            if (proximaSequenciaValorAvulso > 0) proximaSequenciaValorAvulso += 1;
            else proximaSequenciaValorAvulso = 1;

            _programacaoRepository.AdicionarValorAvulsoTaxaCancelamentoProgramacao(programacao, mercadoriaTaxaDeCancelamento, proximaSequenciaValorAvulso, valor, idUsuario);
        }

        public void AprovaFinanceiro(int idUsina, int obraNumero, int sequencia, string idUsuario)
        {
            var programacao = _programacaoRepository.ObterPorId(idUsina, obraNumero, sequencia);

            if (programacao == null) return;

            if(programacao.Status == EProgramacaoStatus.AguardandoAnaliseLimiteCredito || 
                programacao.Status == EProgramacaoStatus.LimiteCreditoInsuficiente || 
                programacao.Status == EProgramacaoStatus.AprovacaoInadimplente)
            {
                programacao.Status = EProgramacaoStatus.Programado;

                _programacaoRepository.AlterarStatusLiberacaoProgramacao(programacao, idUsuario);

                _programacaoRepository.SaveChanges();
            }
        }

        public bool ValidaQuantidadeProgramada()
        {
            return _parametroRepository.ObterParametroN("Topcon", "AnalizaLimCredProd").Equals("1");
        }
    }
}
