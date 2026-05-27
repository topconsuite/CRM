using System.Globalization;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;

namespace TopSys.TopConWeb.Domain.Helpers
{
    public class HeaderProvider : IHeaderProvider
    {
        public CultureInfo AcceptLanguage { get; private set; } = new CultureInfo("en-US");

        public CultureInfo GetAcceptLanguage()
        {
            return AcceptLanguage;
        }

        public void SetAcceptLanguage(CultureInfo acceptLanguage)
        {
            AcceptLanguage = acceptLanguage ?? new CultureInfo("pt-BR");
        }
    }
}
