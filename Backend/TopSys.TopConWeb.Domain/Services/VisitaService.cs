using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class VisitaService : ServiceBase<Visita>, IVisitaService
    {

        private readonly IVisitaRepository _visitaRepository;

        public VisitaService(IVisitaRepository visitaRepository): base(visitaRepository)
        {
            _visitaRepository = visitaRepository;
        }

        public void AdicionarAnexo(string usuario, Guid id, int usina, int anoVisita, int numeroVisita, string anexo, string nome)
        {
            _visitaRepository.AdicionarAnexo(usuario, id, usina, anoVisita, numeroVisita, anexo, nome);
        }

        public void Adiconar(Visita visita)
        {
            _visitaRepository.Adicionar(visita);
        }

        public void AtualizarDescricaoAnexo(VisitaAnexo anexo)
        {
            _visitaRepository.AtualizarDescricaoAnexo(anexo);
        }

        public ICollection<VisitaAnexo> ListarAnexos(int usina, int anoVisita, int numeroVisita)
        {
            return _visitaRepository.ListarAnexos(usina, anoVisita, numeroVisita);
        }

        public PagedList<Visita> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Visita, bool>> filter)
        {
            return _visitaRepository.ListarEmOrdemDecrescente(pagina, porPagina, filter);
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _visitaRepository.ObterAnexo(id);
        }

        public VisitaAnexo ObterVisitaAnexoPorId(Guid id)
        {
            return _visitaRepository.ObterVisitaAnexoPorId(id);
        }

        public Visita ObterPorId(int usina, int ano, int numero, bool tracking = false)
        {
            return _visitaRepository.ObterPorId(usina, ano, numero, tracking);
        }

        public void RemoverAnexo(Guid id)
        {
            _visitaRepository.RemoverAnexo(id);
        }

        public PagedList<VisitaHistorico> ListarHistoricoEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<VisitaHistorico, bool>> filter)
        {
            return _visitaRepository.ListarHistoricoEmOrdemDecrescente(pagina, porPagina, filter);
        }

        public void AdicionarHistorico(VisitaHistorico visitaHistorico)
        {
            _visitaRepository.AdicionarHistorico(visitaHistorico);
        }

    }
}
