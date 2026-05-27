using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class MunicipioTributacaoService : ServiceBase<Municipio>, IMunicipioTributacaoService
    {
        private readonly IMunicipioTributacaoRepository _municipioTributacaoRepository;

        public MunicipioTributacaoService(IMunicipioTributacaoRepository municipioTributacaoRepository) : base(municipioTributacaoRepository)
        {
            _municipioTributacaoRepository = municipioTributacaoRepository;
        }

        public ICollection<Municipio> Listar(string uf)
        {
            return _municipioTributacaoRepository.ListarMunicipioTributacao(uf);
        }

        public Municipio ObterPorId(int id, bool tracking = false)
        {
            return _municipioTributacaoRepository.ObterPorId(id, tracking);
        }

        public Municipio ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _municipioTributacaoRepository.ObterPorExternalId(externalId, tracking);
        }

        public Municipio ObterPorIbgeCode(int ibgeCode, bool tracking = false)
        {
            return _municipioTributacaoRepository.ObterPorIbgeCode(ibgeCode, tracking);
        }

        public Municipio ObterPorMunicipioUf(string municipio, string uf, bool tracking = false)
        {
            return _municipioTributacaoRepository.ObterPorMunicipioUf(municipio, uf, tracking);
        }

        public bool VerificaSeExisteInterveniente(int codigoInterveniente)
        {
            return _municipioTributacaoRepository.IsOnTable(codigoInterveniente.ToString(), "cod", "ger_interv");
        }

        public int ObterProximoCodigo()
        {
            return _municipioTributacaoRepository.ObterProximoCodigo();
        }
    }
}
