using System;

namespace TopSys.TopConWeb.Domain.Entities.WebHook
{
    public class WebHookDesktop
    {

        public string Evento { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; }
        public string FilePath { get; set; }
        public string AlertEmails { get; set; }
        public DateTime DtSendAfter { get; set; }
        public string Headers { get; set; }

    }
}
