using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContratoService : IServiceBase<Contrato>
    {
        ICollection<ContratoPagamento> ObterContratoPagamentos(int usina, int numeroContrato, int anoContrato);
        ICollection<ContratoPagamentoVersao> ObterContratoPagamentosVersao(int numVersao, int usina, int numeroContrato, int anoContrato);
        void AprovarContratoRevalidacaoContrato(string usuario, Contrato contrato, string observacaoLog, ref string mensagem);
        ICollection<Contrato> ListarContratosRevalidacaoCadastro();
        bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out Contrato contrato, out string mensagem);
        int GetUltimaVersaoContrato(int codUsina, int anoContrato, int numeroContrato);
        int GetUltimaVersaoContratoAberta(int codUsina, int anoContrato, int numeroContrato);
        int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato); 
        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);
        ContratoVersao ContratoVersaoObterPorId(int numeroVersao, int codUsina, int anoContrato, int numeroContrato);
        string GetSegmentacaoContrato(int codUsina, int anoContrato, int numeroContrato);
        DateTime? GetDataCriacaoVersaoContrato(int numVersao, int codUsina, int anoContrato, int numeroContrato);
        ICollection<ContratoFinalidade> ListarFinalidades();
        void AtualizarDataEncerramentoEStatusContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void CriarTabelaTemporariaTaxaExtraVersao(int numVersao, int codUsina, int anoContrato, int numeroContrato);

        int ObterUsinaClausula(int usinaEntrega);
        string ObterContratoUsina(int usina, string segmento);
    }
}
