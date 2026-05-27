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
    public class SlumpService : ServiceBase<Slump>, ISlumpService
    {
        private readonly ISlumpRepository _slumpRepository;

        public SlumpService(ISlumpRepository slumpRepository) : base(slumpRepository)
        {
            _slumpRepository = slumpRepository;
        }

        public IEnumerable<Slump> ListarPorSlumpReal(int slumpReal)
        {
            return _slumpRepository.ListarPorSlumpReal(slumpReal);
        }
    }
}
