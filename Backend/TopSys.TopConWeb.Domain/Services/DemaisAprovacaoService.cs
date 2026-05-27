using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class DemaisAprovacaoService : ServiceBase<DemaisAprovacao>, IDemaisAprovacaoService
    {
        private readonly IDemaisAprovacaoRepository _demaisAprovacaoRepository;
        private readonly IAprovacaoScriptService _aprovacaoScriptService;

        public DemaisAprovacaoService(IDemaisAprovacaoRepository aprovacaoComercialRepository, IAprovacaoScriptService aprovacaoScriptService) : base(aprovacaoComercialRepository)
        {
            _demaisAprovacaoRepository = aprovacaoComercialRepository;
            _aprovacaoScriptService = aprovacaoScriptService;
        }

        public void AtualizarAprovacoes(string usuario, ICollection<DemaisAprovacao> demaisAprovacoes)
        {
            DemaisAprovacao _demaisAprovacao;

            foreach (var demaisAprovacao in demaisAprovacoes)
                switch (demaisAprovacao.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        _demaisAprovacao = _demaisAprovacaoRepository.ObterPorId(demaisAprovacao.Chave);
                        _demaisAprovacao.Executar(usuario);
                        _aprovacaoScriptService.ExecutarAprovacao(_demaisAprovacao.Chave);
                        break;
                    case EStatusAprovacao.Reprovado:
                        _demaisAprovacao = _demaisAprovacaoRepository.ObterPorId(demaisAprovacao.Chave);
                        _demaisAprovacao.Executar(usuario);
                        _aprovacaoScriptService.ExecutarReprovacao(_demaisAprovacao.Chave);
                        break;
                }
        }       

        public ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int usinaCodigo, int obraCodigo, string usuario)
        {
            var demaisAprovacoes =  _demaisAprovacaoRepository.BuscarDemaisAprovacaoByIdObra(usinaCodigo, obraCodigo);

            foreach (var demaisAprovacao in demaisAprovacoes)
                demaisAprovacao.AtualizaStatusAprovacao(usuario);

            return demaisAprovacoes;
        }

        public ICollection<DemaisAprovacao> BuscarDemaisAprovacaoByIdObra(int numVersao, int usinaCodigo, int obraCodigo, string usuario)
        {
            var demaisAprovacoes = _demaisAprovacaoRepository.BuscarDemaisAprovacaoByIdObra(numVersao, usinaCodigo, obraCodigo);

            foreach (var demaisAprovacao in demaisAprovacoes)
                demaisAprovacao.AtualizaStatusAprovacao(usuario);

            return demaisAprovacoes;
        }

        public void RemoverAprovacoes(string id)
        {
            _demaisAprovacaoRepository.RemoverAprovacoes(id);
        }
    }
}
