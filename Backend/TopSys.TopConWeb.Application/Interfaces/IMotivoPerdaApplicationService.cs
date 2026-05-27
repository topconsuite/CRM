using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.MotivoPerda;
using TopSys.TopConWeb.Application.DTOS.Response.MotivoPerda;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IMotivoPerdaApplicationService : IApplicationServiceBase<MotivoPerda>
    {
        void Adicionar(MotivoPerdaInclusaoRequest motivoPerdaRequest, string userRequest);
        void Atualizar(MotivoPerdaAlteracaoRequest motivoPerdaRequest, string userRequest);
        void Deletar(int codigo, string userRequest);
        MotivoPerdaResponse ObterPorCodigo(int codigo);
        PagedList<MotivoPerdaResponse> Listar(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter);
    }
}
