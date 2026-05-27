using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContratoVersaoService
    {
        ICollection<ContratoVersao> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato);
        string[] ObterAditivoReport(int versao, int codUsina, int anoProposta, int numeroProposta, out string tracosAlterados);
        int GetUltimaVersaoContratoAprovado(int codUsina, int anoContrato, int numeroContrato);
        void SalvarPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato, Stream Contrato);
        Stream ObterPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato);
        bool ExisteVersaoEmAberto(int codUsina, int anoContrato, int numeroContrato);
    }
}
