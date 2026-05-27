using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IPreTracoPrecoRepository : IRepositoryBase<PreTracoPreco>
    {

        PagedList<PreTracoPreco> ListarAguardandoCienciaPorPagina(int pagina, int porPagina, int segmentacao, Expression<Func<PreTracoPreco, bool>> filter);

        PreTracoPreco ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistencia, float mpa, int consumo, bool tracking = false);

        IEnumerable<PreTracoPreco> ListarAguardandoCienciaPorTracoUsina(int usina, int uso, int pedra, int slump, int resistenciaTipo, float mpa, int consumo);

        PreTracoPreco ObterPorId(Guid id);

    }
}
