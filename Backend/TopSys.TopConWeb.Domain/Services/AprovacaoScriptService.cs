using System;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class AprovacaoScriptService : ServiceBase<AprovacaoScript>, IAprovacaoScriptService
    {
        private readonly IAprovacaoScriptRepository _aprovacaoScriptRepository;

        public AprovacaoScriptService(IAprovacaoScriptRepository aprovacaoScriptRepository) : base(aprovacaoScriptRepository)
        {
            _aprovacaoScriptRepository = aprovacaoScriptRepository;
        }

        public void ExecutarAprovacao(string chave)
        {
            AprovacaoScript aprovacaoScript = _aprovacaoScriptRepository.ObterPorId(chave, "A");
            aprovacaoScript.Executar();
            _aprovacaoScriptRepository.ExecutarScript(aprovacaoScript.Script);
        }

        public void ExecutarReprovacao(string chave)
        {
            AprovacaoScript aprovacaoScript = _aprovacaoScriptRepository.ObterPorId(chave, "R");
            aprovacaoScript.Executar();
            _aprovacaoScriptRepository.ExecutarScript(aprovacaoScript.Script);
        }
    }
}
