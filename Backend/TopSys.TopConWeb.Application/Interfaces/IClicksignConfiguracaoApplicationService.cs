using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.ClicksignConfiguracao;
using TopSys.TopConWeb.Application.DTOS.Response.ClicksignConfiguracao;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IClicksignConfiguracaoApplicationService : IApplicationServiceBase<ClicksignConfiguracao>
    {
        IEnumerable<ClicksignConfiguracaoResponse> ListarTodos();

        ClicksignConfiguracaoResponse ObterPorId(int id);

        void Salvar(ClicksignConfiguracaoRequest request);

        void Remover(int id);

        IEnumerable<UsinaResponse> ListarUsinasPorConfiguracao(int configuracaoId);

        void VincularUsina(int configuracaoId, int usinaId);

        void DesvincularUsina(int configuracaoId, int usinaId);

        /// <summary>
        /// Retorna o sha256-secret vinculado à configuração ClickSign de uma Usina específica.
        /// Retorna null se a Usina não possuir configuração própria ou se o secret não estiver preenchido.
        /// </summary>
        string ObterSha256SecretPorUsina(int usinaId);
    }
}
