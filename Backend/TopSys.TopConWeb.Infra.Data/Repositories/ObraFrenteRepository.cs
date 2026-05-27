using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ObraFrenteRepository : RepositoryBase<ObraFrente>, IObraFrenteRepository
    {

        public ObraFrenteRepository(AppDataContext context) : base(context)
        {

        }

        public IEnumerable<ObraFrente> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false)
        {
            return _context
                .ObraFrente
                .Where(x => x.UsinaCodigo == obraUsina && x.ObraCodigo == obraNumero)
                .Tracking(tracking)
                .OrderBy(x => x.ObraSequencia);

        }

        public ObraFrente ObterPorObra(int obraUsina, int obraNumero, int sequencia, bool tracking = false)
        {
            return _context
                .ObraFrente
                .Where(x => x.UsinaCodigo == obraUsina && x.ObraCodigo == obraNumero && x.ObraSequencia == sequencia)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public int ProximaSequenciaNaObra(int obraUsina, int obraNumero)
        {

            var frentesObra = _context
                .ObraFrente
                .Where(x => x.UsinaCodigo == obraUsina && x.ObraCodigo == obraNumero);

            if (frentesObra.Count() == 0)
                return 1;

            var max = frentesObra.Max(x => x.ObraSequencia);

            if (max == 0)
                return 1;

            return max + 1;

        }

        public bool VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia)
        {

            var obra = _context
                .Obras
                .Where(x => x.UsinaCodigo == obraUsina && x.Numero == obraNumero)
                .FirstOrDefault();

            if (obra == null)
                return false;

            var existeProgramacoesComEnderecoSelecionado = _context
                .Programacoes
                .Where(x => x.UsinaCodigo == obra.UsinaCodigo
                            && x.PropostaAno == obra.AnoChamada
                            && x.PropostaNumero == obra.NumChamada
                            && x.ObraFrenteSequencia == obraSequencia)
                .Count() > 0;

            return existeProgramacoesComEnderecoSelecionado;

        }
    }
}
