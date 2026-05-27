using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IArquivoService
    {
        void SalvarArquivo(int numProg, string chave, Stream reportPDF, int sequencia = 0, string idCadastro = "AUTO");
        Stream ObterArquivo(int numProg, string chave, int sequencia = 0);
        IEnumerable<ArquivoBanco> ListarArquivosPorChave(int numProg, string chave);
    }
}
