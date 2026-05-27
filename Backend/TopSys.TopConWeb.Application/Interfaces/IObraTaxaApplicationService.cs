using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.ObraTaxa;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IObraTaxaApplicationService
    {
        ICollection<ObraTaxaResponse> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo);

        ICollection<ObraTaxaResponse> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo);

        ICollection<ObraTaxaResponse> ListarTaxaPadraoByIdUsinaSegmento(int usinaEntregaCodigo, int idSegmentacao);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, bool possuiBomba);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero, bool possuiBomba);
    }
}
