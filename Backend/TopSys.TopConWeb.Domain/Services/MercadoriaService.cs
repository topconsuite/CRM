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
    public class MercadoriaService: ServiceBase<Mercadoria>, IMercadoriaService
    {
        private IMercadoriaRepository _mercadoriaRepository;

        public MercadoriaService(IMercadoriaRepository mercadoriaRepository) : base(mercadoriaRepository)
        {
            _mercadoriaRepository = mercadoriaRepository;
        }

        public Mercadoria ObterTracoMercadoria(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            var resistencia = _mercadoriaRepository.ObterPorId<ResistenciaTipo>(idResistenciaTipo);

            var codMercadoria = Mercadoria.GerarCodigoMercadoriaTraco(idUso, idPedra, idSlump, resistencia, mpa, consumo);

            return _mercadoriaRepository.ObterPorId(codMercadoria);

        }

        public bool TracoCriadoPeloTech(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            var resistencia = _mercadoriaRepository.ObterPorId<ResistenciaTipo>(idResistenciaTipo);

            var codMercadoria = Mercadoria.GerarCodigoMercadoriaTraco(idUso, idPedra, idSlump, resistencia, mpa, consumo);

            return _mercadoriaRepository.ObtemTracoCriadoPeloTech(codMercadoria);
        }
    }
}
