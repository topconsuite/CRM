using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ObraFrenteService : ServiceBase<ObraFrente>, IObraFrenteService
    {

        private readonly IObraFrenteRepository _obraFrenteRepository;

        public ObraFrenteService(IObraFrenteRepository obraFrenteRepository) : base(obraFrenteRepository)
        {
            _obraFrenteRepository = obraFrenteRepository;
        }


        public IEnumerable<ObraFrente> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false)
        {
            return _obraFrenteRepository.ListarPorObra(obraUsina, obraNumero, tracking);
        }

        public ObraFrente ObterPorObra(int obraUsina, int obraNumero, int sequencia, bool tracking = false)
        {
            return _obraFrenteRepository.ObterPorObra(obraUsina, obraNumero, sequencia, tracking);
        }

        public int ProximaSequenciaNaObra(int obraUsina, int obraNumero)
        {
            return _obraFrenteRepository.ProximaSequenciaNaObra(obraUsina, obraNumero);
        }

        public bool VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia)
        {
            return _obraFrenteRepository.VerificarEnderecoPossuiProgramacao(obraUsina, obraNumero, obraSequencia);
        }
    }
}
