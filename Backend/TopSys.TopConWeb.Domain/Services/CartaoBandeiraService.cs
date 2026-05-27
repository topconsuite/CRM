using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CartaoBandeiraService : ServiceBase<CartaoBandeira>, ICartaoBandeiraService
    {
        private readonly ICartaoBandeiraRepository _cartaoBandeiraRepository;

        public CartaoBandeiraService(ICartaoBandeiraRepository cartaoBandeiraRepository) : base(cartaoBandeiraRepository)
        {
            _cartaoBandeiraRepository = cartaoBandeiraRepository;
        }

        public IEnumerable<CartaoBandeira> ListarTodosComAgregados()
        {
            return _cartaoBandeiraRepository.ListarTodosComAgregados();
        }

        public IEnumerable<CartaoBandeira> ListarTodosComIntegracao()
        {
            return _cartaoBandeiraRepository.ListarTodosComIntegracao();
        }
    }
}
