using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IAprovacaoComercialApplicationService : IApplicationServiceBase<AprovacaoComercialUsina>
    {
        AprovacaoComercialUsina Adicionar(AprovacaoComercialUsinaInsercaoRequest insercaoRequest);
        void Atualizar(AprovacaoComercialUsinaAlteracaoRequest alteracaoRequest);
        void AprovarObraPendente(string usuario, ObraPendenteAprovacaoRequest obra);
        PagedList<AprovacaoComercialUsinaResponse> ListarAprovacaoComercialUsina(int pagina, int porPagina, Expression<Func<AprovacaoComercialUsina, bool>> filter);
        void AdicionarLog(AprovacaoComercialLog log);


    }
}
