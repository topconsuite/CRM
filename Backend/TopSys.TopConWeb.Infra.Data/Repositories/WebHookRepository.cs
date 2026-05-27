using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities.WebHook;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class WebHookRepository : IWebHookRepository
    {

        private readonly AppDataContext _context;
        private readonly ParametroRepository _parametroRepository;

        public WebHookRepository(AppDataContext context, ParametroRepository parametroRepository)
        {
            _context = context;
            _parametroRepository = parametroRepository;
        }

        public void Adicionar(WebHookDesktop webHook)
        {

            var sql = new StringBuilder();

            if(string.IsNullOrEmpty(webHook.Url))
                webHook.Url = _parametroRepository.ObterParametroIntegracoes("webhook", webHook.Evento);

            if (string.IsNullOrEmpty(webHook.Url))
                return;

            sql.AppendLine($"INSERT INTO `topsys`.`con_webhooks` SET");
            sql.AppendLine($"   evento = @{nameof(webHook.Evento)}");
            sql.AppendLine($",  url = @{nameof(webHook.Url)}");
            sql.AppendLine($",  payload = @{nameof(webHook.Payload)}");
            sql.AppendLine($",  filePath = {(string.IsNullOrEmpty(webHook.FilePath) ? "NULL" : $"@{nameof(webHook.FilePath)}")}");
            sql.AppendLine($",  alertEmails = @{nameof(webHook.AlertEmails)}");
            sql.AppendLine($",  dtSendAfter = @{nameof(webHook.DtSendAfter)}");
            sql.AppendLine($",  headers = @{nameof(webHook.Headers)}");

            _context.Connection.Execute(sql.ToString(), webHook);

        }

    }
}
