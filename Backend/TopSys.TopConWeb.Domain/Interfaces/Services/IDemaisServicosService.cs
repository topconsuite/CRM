using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IDemaisServicosService: IServiceBase<DemaisServicos>
    {
        PagedList<DemaisServicos> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<DemaisServicos, bool>> filter);

        void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra);

        void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra);

        void AdicionarContrato(int codUsina, int numVersao, int numObra);

        void ExcluirContrato(int codUsina, int numObra);
    }
}
