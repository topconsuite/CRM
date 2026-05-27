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
    public class FuncionarioService : ServiceBase<Funcionario>, IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository) : base(funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        public IEnumerable<Funcionario> ListarAnalistas()
        {
            return _funcionarioRepository.ListarAnalistas();
        }

        public int ObterProximoCodigo()
        {
            return _funcionarioRepository.ObterProximoCodigo();
        }

        public bool ReEmUso(int re)
        {
            return _funcionarioRepository.ReEmUso(re);
        }
    }
}
