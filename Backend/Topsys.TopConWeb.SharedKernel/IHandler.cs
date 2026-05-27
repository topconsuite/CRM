using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Events.Interfaces;

namespace TopSys.TopConWeb.SharedKernel
{
    //Interface responsável por manipular os eventos
    //Para manipular um evento este evento tem que ser obrigatóriamente do tipo DomainEvent
    public interface IHandler<T> : IDisposable where T : IDomainEvent
    {
        void Handle(T args);
        IEnumerable<T> Notify();
        bool HasNotifications();
        void RemoveNotificationsByKey(string[] Key);

    }
}
