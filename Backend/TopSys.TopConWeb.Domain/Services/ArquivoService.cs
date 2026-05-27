using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ArquivoService : IArquivoService
    {
        private readonly IArquivoRepository _arquivoRepository;

        public ArquivoService(IArquivoRepository arquivoRepository)
        {
            _arquivoRepository = arquivoRepository;
        }

        public IEnumerable<ArquivoBanco> ListarArquivosPorChave(int numProg, string chave)
        {
            return _arquivoRepository.ListarArquivosPorChave(numProg, chave);
        }

        public void SalvarArquivo(int numProg, string chave, Stream reportPDF, int sequencia = 0, string idCadastro = "AUTO")
        {
            byte[] reportPDFBytes;

            using (var memoryStream = new MemoryStream())
            {
                if (reportPDF.CanSeek)
                    reportPDF.Seek(0, System.IO.SeekOrigin.Begin);

                reportPDF.CopyTo(memoryStream);
                reportPDFBytes = memoryStream.ToArray();
            }

            _arquivoRepository.SalvarArquivo(numProg, chave, reportPDFBytes, sequencia, idCadastro);
        }
        public Stream ObterArquivo(int numProg, string chave, int sequencia = 0)
        {
            var arquivoBytes = _arquivoRepository.ObterArquivo(numProg, chave, sequencia);

            if (arquivoBytes == null)
                return null;

            var arquivoStream = new MemoryStream(arquivoBytes);
            return arquivoStream;
        }

    }
}
