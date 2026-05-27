using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using Topsys.TopConWeb.SharedKernel.Common;
using System.Linq.Expressions;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IPropostaRepository : IRepositoryBase<Proposta>
    {
        new void Adicionar(Proposta proposta);

        PagedList<Proposta> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false);

        Proposta ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false);

        PropostaVersao ObterPorUsinaAnoNumero(int numVersao, int idUsina, int ano, int numero, bool tracking = false);

        PagedList<Proposta> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina);

        float ObterVolumeTotalProposto(int idUsina, int ano, int numero);

        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);

        PropostaVersao ObterPorId(int codUsina, int anoProposta, int numeroProposta, int numVersao);

        PropostaVersao ObterVersaoPorIdForList(int codUsina, int anoProposta, int numeroProposta, int numVersao);

        ICollection<PropostaVersao> ListarPropostaVersoes(int versao, int codUsina, int anoProposta, int numeroProposta);

        int GetUltimaVersaoProposta(int codUsina, int anoProposta, int numeroProposta);

    }
}
