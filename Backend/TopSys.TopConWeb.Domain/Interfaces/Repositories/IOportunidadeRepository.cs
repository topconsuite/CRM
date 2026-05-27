using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IOportunidadeRepository : IRepositoryBase<Oportunidade>
    {

        new void Adicionar(Oportunidade oportunidade);
        Oportunidade ObterPorId(int usina, int ano, int numero, bool tracking = false);
        PagedList<Oportunidade> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Oportunidade, bool>> filter);
        PagedList<OportunidadeInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<OportunidadeInteracao, bool>> filter);
        void AdicionarInteracao(string usuario, OportunidadeInteracao oportunidadeInteracao);
    }
}
