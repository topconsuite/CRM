using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Services
{
    public class IdentityHelperService
    {

        public IPrincipal User { get => Thread.CurrentPrincipal; }

        public string GetUserName()
        {
            return User.Identity.Name;
        }

    }
}
