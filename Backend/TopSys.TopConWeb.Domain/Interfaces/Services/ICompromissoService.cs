using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICompromissoService : IServiceBase<Compromisso>
    {
        PagedList<Compromisso> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter);
        Dictionary<string, string> ListarGrupoUsuario();
    }
}
