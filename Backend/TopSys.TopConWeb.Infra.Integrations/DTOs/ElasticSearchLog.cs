using System;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs
{
    public class ElasticSearchLog
    {

        public string Module { get; set; }
        public string ApplicationType { get; set; }
        public string Client { get; set; }
        public string Identifier { get; set; }
        public string Environment { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

    }
}
