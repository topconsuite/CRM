using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.AprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ReprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoGeradoResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato;
using System.IO;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoIntegracao;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IContratoApplicationService
    {
        IEnumerable<ContratoRevalidacaoCadastroResponse> ListarContratosRevalidacaoCadastro();
        void AprovarContratoRevalidacaoDeCadastro(string usuario, ContratoRevalidacaoCadastroRequest contratoRevalidacaoCadastro, ref string mensagem);
        bool GerarEValidarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out ContratoGeradoResponse contrato, out string mensagem);
        bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out ContratoGeradoResponse contrato, out string mensagem);
        void AprovarCoincidenciasCadastrais(string usuario, AprovarCoincidenciasCadastraisRequest aprovaCoincidenciasCadastraisRequest);
        void ReprovarCoincidenciasCadastrais(string usuario, ReprovarCoincidenciasCadastraisRequest reprovaCoincidenciasCadastraisRequest);
        ICollection<ContratoVersaoResponse> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato);
        Stream ObterAditivoReport(int versao, int codUsina, int anoProposta, int numeroProposta, int anoContrato, int numeroContrato);
        void SalvarPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato);
        Stream ObterPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato);
        ContratoVersaoParametrosResponse ObterParametrosContratoVersao();
        ICollection<ContratoFinalidadeResponse> ListarFinalidades();

        //API
        ResultDTO<ContratoSimplificadoResponse> ObterPorId(int idUsina, int contratoAno, int contratoNumero);
        ResultDTO<ContratoAprovacaoFinanceiraResponse> AprovacaoFinanceira(PagamentosRequest pagamentosRequest);
    }
}
