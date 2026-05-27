using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.AssinaturaEletronica.Clicksign
{
    public class ClicksignEvento
    {
        public ClicksignEventoDTO Event { get; set; }
        public DocumentDTO Document { get; set; }
    }

    public class ClicksignEventoDTO
    {
        public string Name { get; set; }

        /// <summary>Preenchido nos eventos sem data específica, ex: auto_close.</summary>
        public DateTime Occurred_at { get; set; }

        /// <summary>Preenchido nos eventos com data específica, ex: deadline (contém reached_at).</summary>
        public DeadlineDataDTO Data { get; set; }
    }

    /// <summary>Payload de data do evento deadline.</summary>
    public class DeadlineDataDTO
    {
        public DateTime Reached_at { get; set; }
    }

    public class DocumentDTO
    {
        public Guid Key { get; set; }
    }
}
