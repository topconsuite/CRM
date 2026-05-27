using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaImportacaoSimplesResponse;
using TopSys.TopConWeb.Domain.Entities;
using Topsys.TopConWeb.SharedKernel.Common;
using System.Linq.Expressions;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaInseridaResponse;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaVersaoResponse;
using System.IO;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IPropostaApplicationService : IApplicationServiceBase<Proposta>
    {
        PropostaInseridaResponse Adicionar(string usuario, PropostaInclusaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        void Atualizar(string usuario, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        void AtualizarContrato(string usuario, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        void AtualizarContratoVersao(string usuario, int numVersao, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        PagedList<PropostaSimplesResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false);

        PropostaDetalhadaResponse ObterPorUsinaAnoNumero(int idUsina, int ano, int numero);

        PagedList<PropostaImportacaoSimplesResponse> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina);

        float ObterVolumeTotalProposto(int idUsina, int ano, int numero);

        ICollection<PropostaReportPDFResponse> ListarPropostaReportPDFSequencias(int codUsina, int anoChamada, int numeroChamada);

        int CriarNovaPropostaReportPDF(int codUsina, int anoChamada, int numeroChamada, string usuario, Guid? propagandaId);

        Stream ObterPropostaReportPDF(int codUsina, int anoChamada, int numeroChamada, int sequenciaVersao);

        Proposta MapObraPagamentosDetalhesVersaoParaObraPagamentos(PropostaVersao propostaVersao, Proposta proposta);
    }
}
