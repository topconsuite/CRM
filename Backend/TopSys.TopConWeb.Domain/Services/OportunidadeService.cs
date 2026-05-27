using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class OportunidadeService : ServiceBase<Oportunidade>, IOportunidadeService
    {

        private readonly IOportunidadeRepository _oportunidadeRepository;

        public OportunidadeService(IOportunidadeRepository oportunidadeRepository): base(oportunidadeRepository)
        {
            _oportunidadeRepository = oportunidadeRepository;
        }

        public void Adicionar(Oportunidade oportunidade)
        {
            _oportunidadeRepository.Adicionar(oportunidade);
        }

        public PagedList<Oportunidade> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Oportunidade, bool>> filter)
        {
            return _oportunidadeRepository.ListarEmOrdemDecrescente(pagina, porPagina, filter);
        }

        public Oportunidade ObterPorId(int usina, int ano, int numero, bool tracking = false)
        {
            return _oportunidadeRepository.ObterPorId(usina, ano, numero, tracking);
        }

        public PagedList<OportunidadeInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<OportunidadeInteracao, bool>> filter)
        {
            return _oportunidadeRepository.ListarInteracoes(pagina, porPagina, filter);
        }

        public void AdicionarInteracao(string usuario, OportunidadeInteracao oportunidadeInteracao)
        {
            _oportunidadeRepository.AdicionarInteracao(usuario, oportunidadeInteracao);
        }
    }
}
