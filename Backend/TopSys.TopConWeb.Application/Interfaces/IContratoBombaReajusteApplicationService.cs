using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.ContratoReajuste;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoTracoReajuste.ContratoTracoReajusteVigenciasResponse;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IContratoBombaReajusteApplicationService
    {
        IEnumerable<ContratoReajusteVigenciasResponse> ObterVigencias();
        PagedList<ContratoReajusteResponse> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter);
        void AprovarReajuste(ContratoReajusteAlteracaoRequest contratoReajusteAlteracao, string usuario);
        void AprovarTodos(IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajustes, string usuario);
        void ReprovarReajuste(ContratoReajusteAlteracaoRequest contratoReajusteAlteracao, string usuario);
        void ReprovarTodos(IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajustes, string usuario);
        IEnumerable<ContratoReajusteLogResponse> ListaReajusteLogs(int usina, int contratoAno, int contratoNumero, DateTime dataVigencia);
    }
}
