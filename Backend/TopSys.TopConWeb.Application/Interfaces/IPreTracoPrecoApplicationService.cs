using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.PreTracoPreco;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IPreTracoPrecoApplicationService : IApplicationServiceBase<PreTracoPreco>
    {

        PagedList<PreTracoPrecoResponse> ListarAguardandoCienciaPorPagina(int pagina, int porPagina, int segmentacao, Expression<Func<PreTracoPreco, bool>> filter);

        PreTracoPrecoResponse ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistencia, float mpa, int consumo);

        PreTracoPrecoResponse ObterPorId(string id);

        void Atualizar(PreTracoPrecoAlteracaoRequest preTracoPreco, string usuario);

        void AprovarTodos(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario);

        void Reprovar(PreTracoPrecoAlteracaoRequest preTracoPreco, string usuario);

        void ReprovarTodos(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario);

        void AtualizarLote(IEnumerable<PreTracoPrecoAlteracaoRequest> preTracosPrecos, string usuario, ETipoAlteracaoLoteTabelaVenda tipoAlteracao, float valorAlteracao);

    }
}
