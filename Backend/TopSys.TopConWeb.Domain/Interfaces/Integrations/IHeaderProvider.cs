using System.Globalization;

namespace TopSys.TopConWeb.Domain.Interfaces.Integrations
{
    public interface IHeaderProvider
    {
        CultureInfo GetAcceptLanguage();
        void SetAcceptLanguage(CultureInfo acceptLanguage);
    }
}
