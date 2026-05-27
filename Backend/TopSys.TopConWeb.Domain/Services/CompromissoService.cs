using System;
using System.Collections.Generic;
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
    public class CompromissoService : ServiceBase<Compromisso>, ICompromissoService
    {
        private readonly ICompromissoRepository _compromissoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        private const string APLICATIVO_AGENDA_GRUPO = "WEB7009";
        private const string APLICATIVO_AGENDA_GERAL = "WEB7010";

        public CompromissoService(ICompromissoRepository repository, IUsuarioRepository usuarioRepository) : base(repository)
        {
            _compromissoRepository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public PagedList<Compromisso> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter)
        {
            return _compromissoRepository.ListarEmOrdemDecrescentePorHorario(pagina, porPagina, filter);
        }

        public Dictionary<string, string> ListarGrupoUsuario()
        {
            return _usuarioRepository.ListarUsuariosComGrupos(); ;
        }

    }
}
