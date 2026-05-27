using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.BombaPreco;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IBombaPrecoApplicationService
    {
        BombaPrecoResponse ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime dataBase);
        BombaPrecoTerceiroResponse ObterPorBombistaBombaTipoData(int idBombista, int idBombaTipo, DateTime dataBase);
        float ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao);
        IEnumerable<CadastroGeralDTO> ListarBombaTiposPorUsina(int idUsina);
        IEnumerable<IntervenienteDTO> ListarTerceirosPorBombaTipo(int idBombaTipo);
    }
}
