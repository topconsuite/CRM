using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IBombaPrecoRepository
    {
        BombaPreco ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime dataBase);
        BombaPrecoTerceiro ObterPorBombistaBombaTipoData(int idBombista, int idBombaTipo, DateTime dataBase);
        float ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao);
        IEnumerable<CadastroGeral> ListarBombaTiposPorUsina(int idUsina);
        IEnumerable<Interveniente> ListarTerceirosPorBombaTipo(int idBombaTipo);
    }
}
