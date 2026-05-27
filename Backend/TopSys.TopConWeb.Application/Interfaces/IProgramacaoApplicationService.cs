using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoDetalhadaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoHoraResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoIntegracao;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoLogResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IProgramacaoApplicationService : IApplicationServiceBase<Programacao>
    {
        PagedList<ProgramacaoSimplesResponse> ListarComPropostaContratoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        PagedList<ProgramacaoSimplesResponse> ListarComPropostaContratoEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter);

        ProgramacaoDetalhadaResponse ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia);

        bool TemNotaFiscalEmitida(int idUsina, int obraNumero, int sequencia);

        bool TemComplexidadeBombeado(int idUsina, int obraNumero, int sequencia);

        string VerificaContinuidade(int idUsina, int obraNumero, int sequencia);

        void Adicionar(string usuario, ProgramacaoInclusaoRequest programacao);

        void Atualizar(string usuario, ProgramacaoAlteracaoRequest programacao);

        void CancelarPorId(string usuario, int idUsina, int obraNumero, int sequencia, string observacao);

        IEnumerable<ProgramacaoHoraResponse> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia);

        IEnumerable<ProgramacaoLogResponse> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia);

        float ObterVolumeTotalProgramado(int idUsina, int obraNumero);

        bool GeraProgramacao(int idUsina, int obraNumero, int sequencia, bool atualizaComplexidadeBombeado, bool gravaContinuidadeProgramacao, string usuario);

        bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, string observacao, string usuario);

        void AprovaFinanceiro(int idUsina, int obraNumero, int sequencia, string usuario);
        ObraBomba ObterBombaDaProgramacao(int idUsina, int obraNumero, int sequencia);

        /// integracao
        ResultDTO<ProgramacaoAdicionarResponse> Adicionar(ProgramacaoAdicionarRequest[] request);
        ResultDTO<ProgramacaoResponse> AtualizarPorId(int idUsina, int contratoAno, int contratoNumero, int sequencia, ProgramacaoAtualizarRequest request);
        ResultDTO<ProgramacaoResponse> AtualizarPorExternalId(string externalID, ProgramacaoAtualizarRequest request);
        ResultDTO<List<ProgramacaoResponse>> ObterPorUsinaEPeriodo(int usina, DateTime dataInicio, DateTime? dataFim);
        ResultDTO<ProgramacaoResponse> ObterPorId(int idUsina, int contratoAno, int contratoNumero, int sequencia);
        ResultDTO<ProgramacaoResponse> ObterPorExternalId(string externalId);
    }
}
