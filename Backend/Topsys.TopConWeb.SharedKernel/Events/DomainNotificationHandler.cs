using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.SharedKernel.Events
{
    public class DomainNotificationHandler : IHandler<DomainNotification>
    {

        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Handle(DomainNotification args)
        {
            _notifications.Add(args);
        }

        public IEnumerable<DomainNotification> Notify()
        {
            return GetValue();
        }

        private List<DomainNotification> GetValue()
        {
            return _notifications;
        }

        public bool HasNotifications()
        {
            return GetValue().Count > 0;
        }

        public void Dispose()
        {
            //evitando que fique sujeira quando as próximas requisições forem realizadas
            //melhorando a performance uma vez que o gargabe collector não tem que ser acionado
            this._notifications = new List<DomainNotification>();
            GC.SuppressFinalize(this);
        }

        public void RemoveNotificationsByKey(string[] keys)
        {
            _notifications = _notifications.Where(t => !(keys.Contains(t.Key))).ToList();
        }
    }
}