using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Equipamento;
using TopSys.TopConWeb.Application.DTOS.Response.Equipamento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IEquipamentoApplicationService : IApplicationServiceBase<Equipamento>
    {

        ResultDTO<EquipamentoAdicionarResponse> Adicionar(EquipamentoAdicionarRequest[] request);
        ResultDTO<EquipamentoResponse> AtualizarPorID(string codigo, EquipamentoAtualizarRequest request);
        ResultDTO<EquipamentoResponse> AtualizarPorExternalID(string externalID, EquipamentoAtualizarRequest request);
        ResultDTO<List<EquipamentoResponse>> Listar();
        ResultDTO<EquipamentoResponse> ObterPorID(string codigo);
        ResultDTO<EquipamentoResponse> ObterPorExternalID(string codigo);
        ResultDTO<object> DeletarPorID(string codigo);
        ResultDTO<object> DeletarPorExternalID(string externalId);


    }
}
