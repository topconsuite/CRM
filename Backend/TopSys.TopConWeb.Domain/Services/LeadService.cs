using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Services
{
    public class LeadService : ServiceBase<Lead>, ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILeadContatoRepository _leadContatoRepository;

        public LeadService(ILeadRepository leadRepository, ILeadContatoRepository leadContatoRepository) : base(leadRepository)
        {
            _leadRepository = leadRepository;
            _leadContatoRepository = leadContatoRepository;
        }

        public void Adicionar(string usuario, Lead lead)
        {
            const int USINA_DEFAULT = 999;

            lead.IdCadastro = StringHelper.GetIDD(usuario);
            lead.UsinaCodigo = USINA_DEFAULT;
            lead.Ano = int.Parse($"{lead.Data.Year}".Substring(2));

            lead.ContatoPrincipal.Usina = lead.UsinaCodigo;
            lead.ContatoPrincipal.AnoLead = lead.Ano;
            lead.ContatoPrincipal.Sequencia = 1;

            if (lead.ContatoSecundario != null)
            {
                lead.ContatoSecundario.Usina = lead.UsinaCodigo;
                lead.ContatoSecundario.AnoLead = lead.Ano;
                lead.ContatoSecundario.Sequencia = 2;
            }

            if (lead.Classificacao == 0)
                lead.Classificacao = Enums.EClassificacaoTemperatura.Frio;

            _leadRepository.Adicionar(lead);

            lead.ContatoPrincipal.NumeroLead = lead.Numero;
            _leadContatoRepository.Adicionar(lead.ContatoPrincipal);

            if (lead.ContatoSecundario != null)
            {
                lead.ContatoSecundario.NumeroLead = lead.Numero;
                _leadContatoRepository.Adicionar(lead.ContatoSecundario); 
            }
        }

        public void AdicionarAnexo(string usuario, Guid id, int usina, int anoLead, int numeroLead, string anexo, string nome)
        {
            _leadRepository.AdicionarAnexo(usuario, id, usina, anoLead, numeroLead, anexo, nome);
        }

        public ICollection<LeadAnexo> ListarAnexos(int usina, int anoLead, int numeroLead)
        {
            return _leadRepository.ListarAnexos(usina, anoLead, numeroLead);
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _leadRepository.ObterAnexo(id);
        }

        public LeadAnexo ObterLeadAnexoPorId(Guid id)
        {
            return _leadRepository.ObterLeadAnexoPorId(id);
        }

        public void AtualizarDescricaoAnexo(LeadAnexo anexo)
        {
            _leadRepository.AtualizarDescricaoAnexo(anexo);
        }

        public void RemoverAnexo(Guid id)
        {
            _leadRepository.RemoverAnexo(id);
        }

        public PagedList<Lead> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Lead, bool>> filter)
        {
            var pagedList = _leadRepository.ListarEmOrdemDecrescente(pagina, porPagina, filter);

            return pagedList;
        }

        public Lead ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false)
        {
            var lead = _leadRepository.ObterPorUsinaAnoNumero(idUsina, ano, numero, tracking);

            return lead;
        }

        public PagedList<LeadInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<LeadInteracao, bool>> filter)
        {
            return _leadRepository.ListarInteracoes(pagina, porPagina, filter);
        }

        public void AdicionarInteracao(string usuario, LeadInteracao leadInteracao)
        {
            _leadRepository.AdicionarInteracao(usuario, leadInteracao);
        }
    }
}
