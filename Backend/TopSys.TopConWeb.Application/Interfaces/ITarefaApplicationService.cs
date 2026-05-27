using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Tarefa;
using TopSys.TopConWeb.Application.DTOS.Response.Tarefa;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITarefaApplicationService : IApplicationServiceBase<Tarefa>
    {
        void Adicionar(TarefaInclusaoRequest tarefaInclusaoRequest, string userRequest);
        void Atualizar(TarefaAlteracaoRequest tarefaInclusaoRequest, string userRequest);
        TarefaResponse ObterPorId(int codigo);
        PagedList<TarefaResponse> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter);
        void Deletar(int codigo, string userRequest);

        void AdicionarGrupo(TarefaInclusaoRequest[] tarefasInclusaoRequest, string userRequest);
        Dictionary<string, string> UsuariosLigadosAgrupamento(string idAgrupamento);
    }
}
