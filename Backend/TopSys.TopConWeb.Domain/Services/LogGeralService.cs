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
    public class LogGeralService : ServiceBase<LogGeral>, ILogGeralService
    {
        private ILogGeralRepository _logGeralRepository;

        public LogGeralService(ILogGeralRepository logGeralRepository) : base(logGeralRepository)
        {
            _logGeralRepository = logGeralRepository;
        }
    }
}
