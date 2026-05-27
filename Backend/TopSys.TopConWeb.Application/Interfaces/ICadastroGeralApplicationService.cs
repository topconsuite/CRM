using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICadastroGeralApplicationService : IApplicationServiceBase<CadastroGeral>
    {
        ICollection<CadastroGeralResponse> ListarMotivosBloqueioInterveniente();
        ICollection<CadastroGeralResponse> ListarViasCaptacao();
        ICollection<CadastroGeralResponse> ListarFuncoes();
        ICollection<CadastroGeralResponse> ListarTipoObra();
        ICollection<CadastroGeralResponse> ListarPorteObra();
        ICollection<CadastroGeralResponse> ListarTemposAprovacaoMedicao();

        ResultDTO<CadastroGeralIntegracaoAdicionarResponses> Adicionar(CadastroGeralIntegracaoAdicionarRequest[] request, ECadastroGeralTipo type);
        ResultDTO<CadastroGeralIntegracaoResponse> AtualizarId(int id, CadastroGeralIntegracaoAtualizarRequest request, ECadastroGeralTipo type);
        ResultDTO<CadastroGeralIntegracaoResponse> AtualizarExternalId(string externalId, CadastroGeralIntegracaoAtualizarRequest request, ECadastroGeralTipo type);
        ResultDTO<ICollection<CadastroGeralIntegracaoResponse>> Listar(ECadastroGeralTipo type);
        ResultDTO<CadastroGeralIntegracaoResponse> ObterPorId(int id, ECadastroGeralTipo type);
        ResultDTO<CadastroGeralIntegracaoResponse> ObterPorExternalId(string externalId, ECadastroGeralTipo type);
        ResultDTO<int> DeletarPorId(int id, ECadastroGeralTipo type);
        ResultDTO<int> DeletarPorExternalId(string externalId, ECadastroGeralTipo type);
    }
}
