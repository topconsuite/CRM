using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IUsuarioWebFiltroRepository
    {

        UsuarioWebFiltro ObterPorId(string usuario, string aplicativo);
        void Salvar(string usuario, string aplicativo, string json, string filterString);
        void Salvar(UsuarioWebFiltro filtro);


    }
}
