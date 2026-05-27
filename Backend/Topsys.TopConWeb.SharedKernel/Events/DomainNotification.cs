using System;
using TopSys.TopConWeb.SharedKernel.Events.Interfaces;

namespace TopSys.TopConWeb.SharedKernel.Events
{
    public class DomainNotification : IDomainEvent
    {
        public string Key { get; private set; }
        public string Message { get; private set; }
        public DateTime DateOccurred { get; private set; }

        public DomainNotification(string key, string value)
        {
            this.Key = key;
            this.Message = value;
            this.DateOccurred = DateTime.Now;
        }
    }
}
