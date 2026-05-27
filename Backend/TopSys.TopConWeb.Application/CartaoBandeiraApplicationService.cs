using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Response.CartaoBandeira;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CartaoBandeiraApplicationService : ApplicationServiceBase<CartaoBandeira>, ICartaoBandeiraApplicationService
    {
        private readonly ICartaoBandeiraService _cartaoBandeiraService;

        public CartaoBandeiraApplicationService(ICartaoBandeiraService cartaoBandeiraService, IUnitOfWork unityOfWork)
            : base(cartaoBandeiraService, unityOfWork)
        {
            _cartaoBandeiraService = cartaoBandeiraService;
        }

        public IEnumerable<CartaoBandeiraResponse> ListarTodosComAgregados()
        {
            return AutoMapper.Mapper.Map(_cartaoBandeiraService.ListarTodosComAgregados(), new List<CartaoBandeiraResponse>());
        }

        public IEnumerable<CartaoBandeiraResponse> ListarTodosComIntegracao()
        {
            return AutoMapper.Mapper.Map(_cartaoBandeiraService.ListarTodosComIntegracao(), new List<CartaoBandeiraResponse>());
        }
    }
}
