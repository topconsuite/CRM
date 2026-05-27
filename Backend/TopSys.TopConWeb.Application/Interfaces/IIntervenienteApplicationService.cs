using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.AlterarLimiteCreditoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteAnexo;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteHistorico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Response.IntervenienteAnexo;
using TopSys.TopConWeb.Application.DTOS.Response.IntervenienteHistorico;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IIntervenienteApplicationService : IApplicationServiceBase<Interveniente>
    {
        IntervenienteResponse ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual);

        bool InscricaoEstadualEhValida(string inscricaoEstadual, string uf = "");

        void AprovarIss(string usuario, int codInterveniente);

        void AlterarLimiteDeCredito(AlterarLimiteCreditoRequest alterarLimiteCreditoRequest);

        PagedList<IntervenienteHistoricoResponse> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter);

        IntervenienteResponse ObterPorCodigo(int intervenienteCodigo);

        void AdicionarHistorico(string usuario, IntervenienteHistoricoRequest historico);

        IntervenienteResponse Adicionar(IntervenienteInclusaoRequest interveniente, string usuario);

        void Atualizar(IntervenienteAlteracaoRequest interveniente, string usuario);

        void AdicionarAnexo(string usuario, IntervenienteAnexoAdicionarRequest anexo, out string mensagem);

        ICollection<IntervenienteAnexoResponse> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada);
        ICollection<IntervenienteAnexoResponse> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade);

        byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        string ObterAnexoConvertidoBase64(byte[] anexo, string nome);

        void AtualizarDescricaoAnexo(IntervenienteAnexoAtualizarRequest anexo);

        void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        //Public Integration

        ResultDTO<IntervenienteAdicionarResponse> IntervenienteAdicionar(IntervenienteAdicionarRequest[] request);
        ResultDTO<PublicoIntervenienteResponse> AtualizarPorId(int id, IntervenienteAtualizarRequest request);
        ResultDTO<PublicoIntervenienteResponse> AtualizarPorCnpjCpf(string cnpjCpf, IntervenienteAtualizarRequest request);
        ResultDTO<PublicoIntervenienteResponse> AtualizarPorExternalId(string externalId, IntervenienteAtualizarRequest request);
        ResultDTO<PagedList<PublicoIntervenienteResponse>> Listar(int page = 0, int limit = 0);
        ResultDTO<PublicoIntervenienteResponse> ObterPorId(int id);
        ResultDTO<PublicoIntervenienteResponse> ObterPorExternalId(string externalId);
        ResultDTO<PublicoIntervenienteResponse> ObterPorCnpjCpf(string CnpjCpf);
        ResultDTO<PagedList<PublicoIntervenienteResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
    }
}
