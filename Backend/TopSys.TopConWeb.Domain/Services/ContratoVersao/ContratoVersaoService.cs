using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ContratoVersaoService : ServiceBase<ContratoVersao>, IContratoVersaoService
    {
        private readonly IContratoVersaoRepository _contratoVersaoRepository;

        public ContratoVersaoService(IContratoVersaoRepository contratoVersaoRepository)
    :   base(contratoVersaoRepository)
        {
            _contratoVersaoRepository = contratoVersaoRepository;
        }

        public ICollection<ContratoVersao> ListarContratoVersoes(int codUsina, int anoContrato, int numeroContrato)
        {
            return _contratoVersaoRepository.ListarContratoVersoes(codUsina, anoContrato, numeroContrato);
        }
    }
}
