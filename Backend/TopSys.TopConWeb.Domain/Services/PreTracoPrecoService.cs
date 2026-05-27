using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class PreTracoPrecoService : ServiceBase<PreTracoPreco>, IPreTracoPrecoService
    {

        private readonly IPreTracoPrecoRepository _preTracoPrecoRepository;
        private readonly ITracoPrecoService _tracoPrecoService;

        public PreTracoPrecoService(IPreTracoPrecoRepository preTracoPrecoRepository, ITracoPrecoService tracoPrecoService) : base(preTracoPrecoRepository)
        {
            _preTracoPrecoRepository = preTracoPrecoRepository;
            _tracoPrecoService = tracoPrecoService;
        }

        public PagedList<PreTracoPreco> ListarAguardandoCienciaPorPagina(int pagina, int porPagina, int segmentacao, Expression<Func<PreTracoPreco, bool>> filter)
        {
            var pagedList = _preTracoPrecoRepository.ListarAguardandoCienciaPorPagina(pagina, porPagina, segmentacao, filter);

            

            return pagedList;
        }

        public PreTracoPreco ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistencia, float mpa, int consumo, bool tracking = false)
        {
            return _preTracoPrecoRepository.ObterUltimoAguardandoCienciaPorTraco(usina, uso, pedra, slump, resistencia, mpa, consumo, tracking);
        }

        public PreTracoPreco ObterPorId(Guid id)
        {
            return _preTracoPrecoRepository.ObterPorId(id);
        }

        public IEnumerable<PreTracoPreco> ListarAguardandoCienciaPorTracoUsina(int usina, int uso, int pedra, int slump, int resistenciaTipo, float mpa, int consumo)
        {
            return _preTracoPrecoRepository.ListarAguardandoCienciaPorTracoUsina(usina, uso, pedra, slump, resistenciaTipo, mpa, consumo);
        }

    }
}
