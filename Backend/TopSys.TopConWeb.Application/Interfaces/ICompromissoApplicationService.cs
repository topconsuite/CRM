using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Compromisso;
using TopSys.TopConWeb.Application.DTOS.Response.Compromisso;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICompromissoApplicationService : IApplicationServiceBase<Compromisso>
    {
        void Adicionar(CompromissoInclusaoRequest compromissoInclusaoRequest, string userRequest);
        void Atualizar(CompromissoAlteracaoRequest compromissoInclusaoRequest, string userRequest);
        CompromissoResponse ObterPorId(int codigo);
        PagedList<CompromissoResponse> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter);
        void Deletar(int codigo, string userRequest);
        Dictionary<string, string> ListarGrupoUsuario();
        void AdicionarGrupo(CompromissoInclusaoRequest[] compromissoInclusaoRequest, string userRequest);
        Dictionary<string, string> UsuariosLigadosAgrupamento(string idAgrupamento);
    }
}
