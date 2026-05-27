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
    public class TipoCobrancaService : ServiceBase<TipoCobranca>, ITipoCobrancaService
    {
        private readonly ITipoCobrancaRepository _tipoCobrancaRepository;

        public TipoCobrancaService(ITipoCobrancaRepository tipoCobrancaRepository) : base(tipoCobrancaRepository)
        {
            _tipoCobrancaRepository = tipoCobrancaRepository;
        }

        public IEnumerable<TipoCobranca> ListarPorCondicaoPagamento(int idCondicaoPagamento)
        {
            return _tipoCobrancaRepository.ListarPorCondicaoPagamento(idCondicaoPagamento);
        }

        public bool ChecarSeExistePeloCodigo(int code)
        {
            return _tipoCobrancaRepository.GetByCode(code) != null;
        }
    }
}
