using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IBoletoExternoRepository : IRepositoryBase<BoletoExterno>
    {
        ICollection<BoletoExterno> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato);
        byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia);
        BoletoExterno ObterPorChaveNomeArquivo(string chave, string nomeArquivo);
        void AdicionarBoletoExterno(BoletoExterno externalBankSlip, Guid idFile, Guid idHistory);
    }
}