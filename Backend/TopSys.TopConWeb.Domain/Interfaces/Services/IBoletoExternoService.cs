using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IBoletoExternoService : IServiceBase<BoletoExterno>
    {
        ICollection<BoletoExterno> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato);
        byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia);
        BoletoExterno ObterPorChaveNomeArquivo(string chave, string nomeArquivo);
        void AdicionarBoletoExterno(BoletoExterno boletoExterno);
    }
}
