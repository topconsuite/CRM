using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using Topsys.TopConWeb.SharedKernel.Common;
using System.Linq.Expressions;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using System.IO;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IPropostaService : IServiceBase<Proposta>
    {
        void Adicionar(string usuario, Proposta proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        void Adicionar(string usuario, PropostaVersao proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores);

        bool ValidarProposta(string usuario, Proposta proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores,
            string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0);

        bool ValidarProposta(string usuario, PropostaVersao proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores,
            string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0);

        void ValidarDemaisAprovacoes(string usuario, Proposta proposta, string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0);

        void ValidarDemaisAprovacoes(string usuario, PropostaVersao proposta, string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0);

        PagedList<Proposta> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false);

        Proposta ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false);

        PropostaVersao ObterPorUsinaAnoNumero(int numVersao, int idUsina, int ano, int numero, bool tracking = false);

        PagedList<Proposta> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina);

        float ObterVolumeTotalProposto(int idUsina, int ano, int numero);

        void AtualizarStatusProposta(Proposta proposta, string usuario);

        void AtualizarStatusProposta(PropostaVersao proposta, string usuario);

        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao); 
        
        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);

        PropostaVersao ObterPorId(int codUsina, int anoProposta, int numeroProposta, int numVersao);

        PropostaVersao ObterVersaoPorIdForList(int codUsina, int anoProposta, int numeroProposta, int numVersao);

        int GetUltimaVersaoProposta(int codUsina, int anoProposta, int numeroProposta);


        void SalvarPDFPropostaVersao(int versao, int codUsina, int anoChamada, int numeroChamada, Stream contrato);

        Stream ObterPDFPropostaVersao(int versao, int codUsina, int anoChamada, int numeroChamada);

        void ValidarNumeracaoProdutoCorretaObraTraco(ObraTraco obraTraco, int obraUsina);

        void ValidarNumeracaoProdutoCorretaObraTracoVersao(ObraTracoVersao obraTraco, int obraUsina);

        void VerificaTracoJaIncluso(ObraTraco obraTraco);

        void VerificaTracoJaInclusoVersao(ObraTracoVersao obraTraco, int numVersao);

    }
}