using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Response.CustoServico;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICustoServicoApplicationService: IApplicationServiceBase<CustoServico>
    {
        CustoServicoResponse ObterCustoServicoVigentePorUsina(int idUsina);
        void Deletar(int idUsina, DateTime dataInicioVigencia);
        PagedList<CustoServicoResponse> Listar(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter);
    }
}
