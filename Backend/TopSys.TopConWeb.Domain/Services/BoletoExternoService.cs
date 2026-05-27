using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class BoletoExternoService : ServiceBase<BoletoExterno>, IBoletoExternoService
    {
        private readonly IBoletoExternoRepository _boletoExternoRepository;

        public BoletoExternoService(IBoletoExternoRepository boletoExternoRepository) : base(boletoExternoRepository)
        {
            _boletoExternoRepository = boletoExternoRepository;
        }

        public ICollection<BoletoExterno> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato)
        {
            return _boletoExternoRepository.ListarBoletosExternos(codUsina, anoContrato, numeroContrato);
        }

        public byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia)
        {
            return _boletoExternoRepository.ObterArquivo(idArquivo, chave, sequencia);
        }

        public BoletoExterno ObterPorChaveNomeArquivo(string chave, string nomeArquivo)
        {
            return _boletoExternoRepository.ObterPorChaveNomeArquivo(chave, nomeArquivo);
        }

        public void AdicionarBoletoExterno(BoletoExterno externalBankSlip)
        {
            var idFile = Guid.NewGuid();
            var idHistory = Guid.NewGuid();

            _boletoExternoRepository.AdicionarBoletoExterno(externalBankSlip, idFile, idHistory);
        }
    }
}
