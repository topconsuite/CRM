using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Obra;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecao;
using TopSys.TopConWeb.Domain.Entities;
using System;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecao;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IObraProjecaoApplicationService : IApplicationServiceBase<ObraProjecao>
    {

        IEnumerable<ObraProjecaoResponse> ListarPorObra(int obraUsina, int obraNumero);

        void Adicionar(string usuario, ObraProjecaoRequest obraProjecaoRequest, string userRequest);
        void Atualizar(string usuario, ObraProjecaoRequest obraProjecaoRequest, string userRequest);
        float? ObterSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada);
        float? ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada);
        DateTime? GetProximoPeriodoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada);


    }
}
