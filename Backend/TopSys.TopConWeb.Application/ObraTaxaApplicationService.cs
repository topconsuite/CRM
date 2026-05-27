using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.ObraTaxa;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class ObraTaxaApplicationService : IObraTaxaApplicationService
    {
        private IObraTaxaService _obraTaxaService;

        public ObraTaxaApplicationService(IObraTaxaService obraTaxaService, IUnitOfWork unityOfWork)
        {
            _obraTaxaService = obraTaxaService;
        }

        public ICollection<ObraTaxaResponse> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo)
        {
            return AutoMapper.Mapper.Map(_obraTaxaService.ListarByIdObra(usinaEntregaCodigo, obraCodigo), new List<ObraTaxaResponse>());
        }

        public ICollection<ObraTaxaResponse> ListarTaxaPadraoByIdUsinaSegmento(int usinaEntregaCodigo, int idSegmentacao)
        {
            return AutoMapper.Mapper.Map(_obraTaxaService.ListarTaxaPadraoByIdUsinaSegmento(usinaEntregaCodigo, idSegmentacao), new List<ObraTaxaResponse>());
        }

        public ICollection<ObraTaxaResponse> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo)
        {
            return AutoMapper.Mapper.Map(_obraTaxaService.ListarTaxaPadraoByIdUsina(usinaEntregaCodigo), new List<ObraTaxaResponse>());
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina)
        {
            return _obraTaxaService.ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, idUsina);
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero)
        {
            return _obraTaxaService.ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, idUsina, obraNumero);
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, bool possuiBomba)
        {
            return _obraTaxaService.ObterValorAdicionalPorKmRodado(distanciaUsina, volumeTotal, volumePorCarga, idUsina, possuiBomba);
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero, bool possuiBomba)
        {
            return _obraTaxaService.ObterValorAdicionalPorKmRodado(distanciaUsina, volumeTotal, volumePorCarga, idUsina, obraNumero, possuiBomba);
        }
    }
}
