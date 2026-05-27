
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IArquivoRepository
    {
        IEnumerable<ArquivoBanco> ListarArquivosPorChave(int numProg, string chave);
        void SalvarArquivo(int numProg, string chave, byte[] reportPDF, int sequencia, string idCadastro);
        byte[] ObterArquivo(int numProg, string chave, int sequencia);
    }
}
