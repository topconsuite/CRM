using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IContratoRepository : IRepositoryBase<Contrato>
    {
        ICollection<ContratoPagamento> ObterContratoPagamentos(int usina, int numeroContrato, int anoContrato);
        ICollection<ContratoPagamentoVersao> ObterContratoPagamentosVersao(int numVersao, int usina, int numeroContrato, int anoContrato);

        ICollection<Contrato> ListarContratosRevalidacaoCadastro();

        Contrato ObterPorId(int codUsina, int anoContrato, int numeroContrato);
        
        int GetUltimaVersaoContrato(int codUsina, int anoContrato, int numeroContrato);

        int GetUltimaVersaoContratoAberta(int codUsina, int anoContrato, int numeroContrato);

        int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato);

        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);

        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);

        ICollection<ContratoVersao> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato, bool parametroGeraAditivoContratoSemAprovCadastro = false);

        ContratoVersao ContratoVersaoObterPorId(int numeroVersao, int codUsina, int anoContrato, int numeroContrato);

        int ObterProximaSequenciaValorAvulso(int idUsinaEntrega);

        string GetSegmentacaoContrato(int codUsina, int anoContrato, int numeroContrato);

        DateTime? GetDataCriacaoVersaoContrato(int numVersao, int codUsina, int anoContrato, int numeroContrato);
        bool ExisteVersaoEmAberto(int codUsina, int anoContrato, int numeroContrato);

        ICollection<ContratoFinalidade> ListarFinalidades();

        void ObterDataEncerramentoEStatusVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, out DateTime? dataEncerramento, out int status);

        void AtualizarDataEncerramentoEStatusContrato(int codUsina, int anoContrato, int numeroContrato, DateTime? dataEncerramento, int status);
        void CriarTabelaTemporariaTaxaExtraVersao(int numVersao, int codUsina, int anoContrato, int numeroContrato);

        int ObterUsinaClausula(int usinaEntrega);
        string ObterContratoUsina(int usina, string segmento);
    }
}
