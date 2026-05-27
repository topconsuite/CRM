using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class AprovacaoComercialHierarquiaApplicationService : ApplicationServiceBase<AprovacaoComercialHierarquia>, IAprovacaoComercialHierarquiaApplicationService
    {

        private readonly IAprovacaoComercialHierarquiaService _aprovacaoComercialHierarquiaService;
        private readonly IAprovacaoComercialPendenteService _aprovacaoComercialPendenteService;
        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly ICondicaoPagamentoApplicationService _condicaoPagamentoApplicationService;
        private readonly IUsuarioService _usuarioService;
        private readonly IObraService _obraService;

        public AprovacaoComercialHierarquiaApplicationService(
            IAprovacaoComercialHierarquiaService aprovacaoComercialHierarquiaService,
            IAprovacaoComercialPendenteService aprovacaoComercialPendenteService,
            IAprovacaoComercialService aprovacaoComercialService,
            IUsuarioService usuarioService,
            IObraService obraService,
            ICondicaoPagamentoApplicationService condicaoPagamentoApplicationService,
            IUnitOfWork unityOfWork
            ) : base(aprovacaoComercialHierarquiaService, unityOfWork)
        {
            _aprovacaoComercialHierarquiaService = aprovacaoComercialHierarquiaService;
            _aprovacaoComercialPendenteService = aprovacaoComercialPendenteService;
            _aprovacaoComercialService = aprovacaoComercialService;
            _condicaoPagamentoApplicationService = condicaoPagamentoApplicationService;
            _usuarioService = usuarioService;
            _obraService = obraService;
        }

        public int ObterProximoNivelHierarquiaPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId)
        {

            var hierarquias = _aprovacaoComercialHierarquiaService.ListarPorAprovacaoComercialUsina(aprovacaoComercialUsinaId).ToList();

            if (hierarquias.Count == 0)
                return 1;

            var ultimo = hierarquias.Max(x => x.NivelAutoridade);
            ultimo++;

            return ultimo;

        }

        public IEnumerable<AprovacaoComercialHierarquia> ListarNivelHierarquiaPorUsina(Guid aprovacaoComercialUsinaId)
        {

            var result = _aprovacaoComercialHierarquiaService.ListarPorAprovacaoComercialUsina(aprovacaoComercialUsinaId).ToList();

            return result;
        }

        public AprovacaoComercialHierarquiaResponse AdicionarHierarquia(AprovacaoComercialHierarquiaInsercaoRequest aprovacaoComercialHierarquiaInsercaoRequest)
        {

            var hierarquia = AutoMapper.Mapper.Map(aprovacaoComercialHierarquiaInsercaoRequest, new AprovacaoComercialHierarquia());

            hierarquia.CreatedAt = DateTime.Now;
            hierarquia.Id = Guid.NewGuid();

            if (hierarquia.NivelAutoridade < 1)
            {
                AssertionConcern.Notify("NivelAutoridade", "Nivel de autoridade deve ser superior a 0.");
                return null;
            }

            if (hierarquia.QuantidadeAprovacoesNecessarias < 1)
            {
                AssertionConcern.Notify("QuantidadeAprovacoesNecessarias", "Quantidade de aprovações deve ser superior a 0.");
                return null;
            }

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia", "AprovacaoComercialHierarquiaApplicationService.AdicionarHierarquia", "", PayloadHelper.ConvertToJson(hierarquia)));
            _aprovacaoComercialHierarquiaService.Adicionar<AprovacaoComercialHierarquia>(hierarquia);

            Commit();

            return AutoMapper.Mapper.Map<AprovacaoComercialHierarquiaResponse>(hierarquia);

        }

        public void AtualizarHierarquia(AprovacaoComercialHierarquiaAlteracaoRequest aprovacaoComercialHierarquiaAlteracaoRequest)
        {

            var hierarquiaOld = _aprovacaoComercialHierarquiaService.ObterPorId(aprovacaoComercialHierarquiaAlteracaoRequest.Id);

            if (hierarquiaOld == null)
            {
                AssertionConcern.Notify("Hierarquia", "Hierarquia não encontrada para o ID informado.");
                return;
            }

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia", "AprovacaoComercialHierarquiaApplicationService.AtualizarHierarquia", "", 
                PayloadHelper.ConvertToJson(hierarquiaOld, aprovacaoComercialHierarquiaAlteracaoRequest))
                );
            hierarquiaOld = AutoMapper.Mapper.Map(aprovacaoComercialHierarquiaAlteracaoRequest, hierarquiaOld);
            hierarquiaOld.UpdatedAt = DateTime.Now;

            Commit();

        }


        // ------------------------- Condições -----------------------------------------------------------------------------------------

        public void AdicionarCondicoes(AprovacaoComercialHierarquiaCondicaoInsercaoRequest[] condicoes)
        {

            foreach (var condicao in condicoes)
            {

                var novo = condicao.Id == Guid.Empty;

                if (novo)
                {
                    AdicionarCondicao(condicao);
                }
                else
                {
                    AtualizarCondicao(condicao);
                }
            }

            Commit();


        }

        public void AtualizarCondicao(AprovacaoComercialHierarquiaCondicaoInsercaoRequest condicaoRequest, bool commit = false)
        {

            var condicao = _aprovacaoComercialHierarquiaService.ObterCondicaoPorId(condicaoRequest.Id);

            if (condicao == null)
            {
                AdicionarCondicao(condicaoRequest, commit);
                return;
            }

            if(condicaoRequest.PercentualAte != condicao.PercentualAte 
                || condicaoRequest.PercentualDe != condicao.PercentualDe
                || condicaoRequest.ValorDe != condicao.ValorDe
                || condicaoRequest.ValorAte != condicao.ValorAte)
            {
                _aprovacaoComercialService.AdicionarLog(
                    new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia_condicao", "AprovacaoComercialHierarquiaApplicationService.AtualizarCondicao", "",
                        PayloadHelper.ConvertToJson(condicao, condicaoRequest))
                    );
            }
            
            condicao = AutoMapper.Mapper.Map(condicaoRequest, condicao);

            if (commit)
                Commit();

        }

        public void AdicionarCondicao(AprovacaoComercialHierarquiaCondicaoInsercaoRequest condicaoRequest, bool commit = false)
        {

            var verificaCondExiste = _aprovacaoComercialHierarquiaService.ObterCondicaoPorHierarquiaTipoPessoaTipoValor(
                condicaoRequest.AprovacaoComercialHierarquiaId,
                condicaoRequest.TipoPessoaId,
                condicaoRequest.Valor
                );

            if (verificaCondExiste != null)
            {
                condicaoRequest.Id = verificaCondExiste.Id;
                AtualizarCondicao(condicaoRequest);
                return;
            }

            var condicao = AutoMapper.Mapper.Map(condicaoRequest, new AprovacaoComercialHierarquiaCondicao());

            condicao.Id = Guid.NewGuid();

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia_condicao", "AprovacaoComercialHierarquiaApplicationService.AdicionarCondicao", "",
                PayloadHelper.ConvertToJson(condicao))
                );
            _aprovacaoComercialHierarquiaService.Adicionar<AprovacaoComercialHierarquiaCondicao>(condicao);

            if (commit)
                Commit();
        }

        public AprovacaoComercialHierarquiaCondicaoResponse ObterCondicaoPorId(Guid id)
        {
            return AutoMapper.Mapper.Map<AprovacaoComercialHierarquiaCondicaoResponse>(_aprovacaoComercialHierarquiaService.ObterCondicaoPorId(id));
        }

        public AprovacaoComercialHierarquiaCondicaoResponse ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string Valor)
        {
            return AutoMapper.Mapper.Map<AprovacaoComercialHierarquiaCondicaoResponse>(_aprovacaoComercialHierarquiaService.ObterCondicaoPorHierarquiaTipoPessoaTipoValor(hierarquiaId, tipoPessoa, Valor));
        }

        public IEnumerable<AprovacaoComercialHierarquiaCondicaoResponse> ListarCondicaoPorNivelHierarquia(Guid hierarquiaId)
        {
            return AutoMapper.Mapper.Map(_aprovacaoComercialHierarquiaService.ListarCondicaoPorNivelHierarquia(hierarquiaId).ToList(), new List<AprovacaoComercialHierarquiaCondicaoResponse>());
        }


        public IEnumerable<AprovacaoComercialHierarquiaCondicaoResponse> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId)
        {
            return AutoMapper.Mapper.Map(_aprovacaoComercialHierarquiaService.ListarCondicoesPorNivelHierarquiaTipoPessoa(hierarquiaId, pessoaId).ToList(), new List<AprovacaoComercialHierarquiaCondicaoResponse>());
        }

        // ------------------------- Usuário -----------------------------------------------------------------------------------------

        public IEnumerable<AprovacaoComercialHierarquiaUsuarioResponse> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId)
        {
            var result = _aprovacaoComercialHierarquiaService.ListarUsuariosPorNivelHierarquia(hierarquiaId).ToList();

            return AutoMapper.Mapper.Map(result.ToList(), new List<AprovacaoComercialHierarquiaUsuarioResponse>());
        }

        public AprovacaoComercialHierarquiaUsuarioResponse ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId)
        {
            return AutoMapper.Mapper.Map(_aprovacaoComercialHierarquiaService.ObterUsuarioNivelHierarquiaPorUsuario(usuarioId), new AprovacaoComercialHierarquiaUsuarioResponse());
        }

        public IEnumerable<UsuarioAprovacaoComercialResponse> ListarUsuarios()
        {
            return AutoMapper.Mapper.Map(
                _usuarioService.ObterNomeUsuariosVerificados().ToList(),
                new List<UsuarioAprovacaoComercialResponse>());
        }

        public AprovacaoComercialHierarquiaUsuarioResponse AdicionarUsuario(AprovacaoComercialHierarquiaUsuarioInsercaoRequest aprovacaoComercialHierarquiaUsuario)
        {

            var usuario = AutoMapper.Mapper.Map<AprovacaoComercialHierarquiaUsuario>(aprovacaoComercialHierarquiaUsuario);
            usuario.Id = Guid.NewGuid();

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia_usuario", "AprovacaoComercialHierarquiaApplicationService.AdicionarUsuario", "",
                PayloadHelper.ConvertToJson(usuario))
                );
            _aprovacaoComercialHierarquiaService.AdicionarUsuario(usuario);

            Commit();

            return AutoMapper.Mapper.Map<AprovacaoComercialHierarquiaUsuarioResponse>(usuario);
        }

        public void RemoverUsuario(Guid aprovComUsuarioId)
        {

            var usuario = _aprovacaoComercialHierarquiaService.ObterPorId<AprovacaoComercialHierarquiaUsuario>(aprovComUsuarioId);

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia_usuario", "AprovacaoComercialHierarquiaApplicationService.RemoverUsuario", "",
                PayloadHelper.ConvertToJson(usuario))
                );
            _aprovacaoComercialHierarquiaService.RemoverUsuario(aprovComUsuarioId);

            Commit();

        }

        // ------------------------- Tipo Pessoa -----------------------------------------------------------------------------------------

        public IEnumerable<AprovacaoComercialTipoPessoaResponse> ListarTipoPessoa()
        {
            return AutoMapper.Mapper.Map(_aprovacaoComercialHierarquiaService.ListarTipoPessoa(), new List<AprovacaoComercialTipoPessoaResponse>());
        }

        public AprovacaoComercialHierarquiaUsuarioDireitoResponse UsuarioTemDireitoAprovacaoComercialObra(string userId, int obraUsina, int obraNumero, int obraVersao)
        {

            var usinaEntrega = 0;

            var direitos = new AprovacaoComercialHierarquiaUsuarioDireitoResponse()
            {
                UtilizaAprovacaoComercialAlcada = false,
                ObraEstaReprovado = false,

                DireitoBombas = new List<int>(),
                DireitoTracos = new List<int>(),
                DireitoVolume = false,
                DireitoCondicaoPagamento = false
            };

            var ultimaVersao = obraVersao == 0 ? _obraService.ObterUltimaVersaoObra(obraUsina, obraNumero) : obraVersao;

            var reprovadoComercial = false;

            if(ultimaVersao == 0)
            {
                var _obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero);
                usinaEntrega = _obra.UsinaEntregaCodigo;
                reprovadoComercial = _obra.StatusComercial == EObraStatusComercial.Reprovado;
            } 
            else
            {
                var _obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero, ultimaVersao);
                usinaEntrega = _obra.UsinaEntregaCodigo;
                reprovadoComercial = _obra.StatusComercial == EObraStatusComercial.Reprovado;
            }


            if (usinaEntrega == 0)
                AssertionConcern.Notify("UsinaEntrega", "Usina entrega da Obra não encontrada.");

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(usinaEntrega);

            // Não possui aprovação comercial por alçada cadastrado para usina
            if (aprovacaoComercialUsina == null)
                return direitos;

            // Aprovação comercial por alçada esta desativada
            if (!aprovacaoComercialUsina.Ativo)
                return direitos;

            direitos.UtilizaAprovacaoComercialAlcada = aprovacaoComercialUsina.Ativo;
            direitos.ObraEstaReprovado = reprovadoComercial;

            if (reprovadoComercial)
                return direitos;

            var aprovComercialUsuario = _aprovacaoComercialHierarquiaService.ObterUsuarioNivelHierarquiaPorUsuarioUsina(userId, aprovacaoComercialUsina.Id);

            if (aprovComercialUsuario == null)
                return direitos;

            var aprovacaoPendenteNivelHierarquia = _aprovacaoComercialPendenteService.ObterAprovacaoPendentePorObraVersaoNivelHierarquia(obraUsina, obraNumero, ultimaVersao, aprovComercialUsuario.AprovacaoComercialHierarquia.NivelAutoridade);

            if (aprovacaoPendenteNivelHierarquia == null)
                return direitos;

            var tracosAprovados = aprovacaoPendenteNivelHierarquia.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.AprovacaoUsuario.Equals(userId)).Select(x => x.ObraSeq).ToList();

            direitos.DireitoTracos = aprovacaoPendenteNivelHierarquia.Tracos
                .Where(x => x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Aprovado 
                            && x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior
                            && !x.AprovacaoUsuario.Equals(userId))
                .GroupBy(x => x.ObraSeq) 
                .Select(x => x.First())
                .Select(x => x.ObraSeq)
                .Where(x => !tracosAprovados.Contains(x))
                .ToList();

            var bombasAprovadas = aprovacaoPendenteNivelHierarquia.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.AprovacaoUsuario.Equals(userId)).Select(x => x.ObraSeq).ToList();

            direitos.DireitoBombas = aprovacaoPendenteNivelHierarquia.Bombas
                .Where(x => x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Aprovado
                            && x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior
                            && !x.AprovacaoUsuario.Equals(userId))
                .GroupBy(x => x.ObraSeq)
                .Select(x => x.First())
                .Select(x => x.ObraSeq)
                .Where(x => !bombasAprovadas.Contains(x))
                .ToList();

            var usuarioAprovouVolume = aprovacaoPendenteNivelHierarquia.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.AprovacaoUsuario.Equals(userId)).Count();

            if (usuarioAprovouVolume == 0)
                direitos.DireitoVolume = aprovacaoPendenteNivelHierarquia.Volumes
                    .Where(x => x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Aprovado
                            && x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior
                            && !x.AprovacaoUsuario.Equals(userId)).Count() > 0;

            var usuarioAprovouCondicaoPagamento = aprovacaoPendenteNivelHierarquia.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.AprovacaoUsuario.Equals(userId)).Count();
            if (usuarioAprovouCondicaoPagamento == 0)
                direitos.DireitoCondicaoPagamento = aprovacaoPendenteNivelHierarquia.CondicoesPagamento
                    .Where(x => x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Aprovado
                            && x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior
                            && !x.AprovacaoUsuario.Equals(userId)).Count() > 0;

            return direitos;

        }

        public AprovacaoComercialDadosResponse ListarDadosAprovacoesComercialObra(int obraUsina, int obraNumero, int obraVersao)
        {

            var response = new AprovacaoComercialDadosResponse()
            {
                Tracos = new List<AprovacaoComercialDadosItemResponse>(),
                Bombas = new List<AprovacaoComercialDadosItemResponse>(),
                Volumes = new List<AprovacaoComercialDadosItemResponse>(),
                CondicoesPagamento = new List<AprovacaoComercialDadosItemResponse>()
            };

            var ultimaVersao = obraVersao != 0 ? obraVersao : _obraService.ObterUltimaVersaoObra(obraUsina, obraNumero);
            var aprovacaoComercialPendentes = _aprovacaoComercialPendenteService.ListarTodasAprovacoesPorObraVersao(obraUsina, obraNumero, ultimaVersao).ToList();

            if (aprovacaoComercialPendentes.Count() == 0)
                return response;

            var usinaEntrega = 0;

            if (ultimaVersao == 0)
            {
                var _obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero);
                usinaEntrega = _obra.UsinaEntregaCodigo;
            }
            else
            {
                var _obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero, ultimaVersao);
                usinaEntrega = _obra.UsinaEntregaCodigo;
            }

            var hierarquias = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(usinaEntrega);

            var nivelMin = aprovacaoComercialPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacaoComercialPendentes.Max(x => x.NivelHierarquia);

            for (var nivelHierarquia = nivelMin; nivelHierarquia <= nivelMax; nivelHierarquia++)
            {
                var aprovacaoPendente = aprovacaoComercialPendentes.Where(x => x.NivelHierarquia == nivelHierarquia).FirstOrDefault();
                var hierarquia = hierarquias.Where(x => x.NivelAutoridade == nivelHierarquia).FirstOrDefault();

                if (aprovacaoPendente == null)
                    continue;

                var tracos = aprovacaoPendente.Tracos
                            .GroupBy(x => x.ObraSeq)
                            .Select(x => x.First())
                            .Select(x => x.ObraSeq)
                            .ToList();

                foreach(var sequenciaTraco in tracos)
                {

                    var pendentesTraco = aprovacaoPendente.Tracos.Where(x => x.ObraSeq == sequenciaTraco);

                    var aprovados = pendentesTraco.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado);
                    var reprovados = pendentesTraco.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado);


                    var responseTraco = new AprovacaoComercialDadosItemResponse()
                    {
                        NivelAutoridade = aprovacaoPendente.NivelHierarquia,
                        NivelDescricao = hierarquia.Titulo,

                        Sequencia = sequenciaTraco,

                        QuantidadeNotificacoes = pendentesTraco.Count(),
                        QuantidadeNotificacoesAprovadas = aprovados.Count(),
                        QuantidadeNotificacoesReprovadas = reprovados.Count(),
                        Status = aprovacaoPendente.AprovacaoStatus,
                        Aprovadores = aprovados.Count() == 0 ? "" : aprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b),
                        Reprovadores = reprovados.Count() == 0 ? "" : reprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b)
                    };

                    if(responseTraco.QuantidadeNotificacoes > 0)
                        response.Tracos.Add(responseTraco);

                }

                var bombas = aprovacaoPendente.Bombas
                            .GroupBy(x => x.ObraSeq)
                            .Select(x => x.First())
                            .Select(x => x.ObraSeq)
                            .ToList();

                foreach (var sequenciaBomba in bombas)
                {

                    var pendentesBomba = aprovacaoPendente.Bombas.Where(x => x.ObraSeq == sequenciaBomba);

                    var aprovados = pendentesBomba.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado);
                    var reprovados = pendentesBomba.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado);

                    var responseBomba = new AprovacaoComercialDadosItemResponse()
                    {
                        NivelAutoridade = aprovacaoPendente.NivelHierarquia,
                        NivelDescricao = hierarquia.Titulo,

                        Sequencia = sequenciaBomba,

                        QuantidadeNotificacoes = pendentesBomba.Count(),
                        QuantidadeNotificacoesAprovadas = pendentesBomba.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count(),
                        QuantidadeNotificacoesReprovadas = pendentesBomba.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count(),
                        Status = aprovacaoPendente.AprovacaoStatus,
                        Aprovadores = aprovados.Count() == 0 ? "" : aprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b),
                        Reprovadores = reprovados.Count() == 0 ? "" : reprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b)
                    };

                    response.Bombas.Add(responseBomba);

                }

                if(aprovacaoPendente.Volumes.Count() > 0)
                {

                    var pendentesVolume = aprovacaoPendente.Volumes;

                    var aprovados = pendentesVolume.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado);
                    var reprovados = pendentesVolume.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado);

                    var responseVolume = new AprovacaoComercialDadosItemResponse()
                    {
                        NivelAutoridade = aprovacaoPendente.NivelHierarquia,
                        NivelDescricao = hierarquia.Titulo,

                        Sequencia = 0,

                        QuantidadeNotificacoes = pendentesVolume.Count(),
                        QuantidadeNotificacoesAprovadas = pendentesVolume.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count(),
                        QuantidadeNotificacoesReprovadas = pendentesVolume.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count(),

                        Status = aprovacaoPendente.AprovacaoStatus,

                        Aprovadores = aprovados.Count() == 0 ? "" : aprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b),
                        Reprovadores = reprovados.Count() == 0 ? "" : reprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b)
                    };

                    response.Volumes.Add(responseVolume);

                }

                if (aprovacaoPendente.CondicoesPagamento.Count() > 0)
                {

                    var pendentesCondicaoPagamento = aprovacaoPendente.CondicoesPagamento;

                    var aprovados = pendentesCondicaoPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado);
                    var reprovados = pendentesCondicaoPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado);

                    var responseCondicaoPagamento = new AprovacaoComercialDadosItemResponse()
                    {
                        NivelAutoridade = aprovacaoPendente.NivelHierarquia,
                        NivelDescricao = hierarquia.Titulo,

                        Sequencia = 0,

                        QuantidadeNotificacoes = pendentesCondicaoPagamento.Count(),
                        QuantidadeNotificacoesAprovadas = pendentesCondicaoPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count(),
                        QuantidadeNotificacoesReprovadas = pendentesCondicaoPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count(),

                        Status = aprovacaoPendente.AprovacaoStatus,

                        Aprovadores = aprovados.Count() == 0 ? "" : aprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b),
                        Reprovadores = reprovados.Count() == 0 ? "" : reprovados.Select(x => x.AprovacaoUsuario + " - " + x.AprovacaoData.ToString()).Aggregate((a, b) => a + "\n" + b)
                    };

                    response.CondicoesPagamento.Add(responseCondicaoPagamento);

                }



            }

            return response;

        }

        // ------------------------------- Demais Condições -------------------------------


        public AprovacaoComercialCondicaoPagamentoResponse ObterCondicaPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int segmentacaoId)
        {

            var condicaoPagamento = _aprovacaoComercialHierarquiaService.ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(hierarquiaId, tipoPessoaId, segmentacaoId);

            return AutoMapper.Mapper.Map(condicaoPagamento, new AprovacaoComercialCondicaoPagamentoResponse());

        }

        public string AtualizarCondicaoPagamento(AprovacaoComercialCondicaoPagamentoAtualizarRequest[] request)
        {

            var result = new StringBuilder();

            if (request.Length == 0)
                AssertionConcern.Notify("Request", "Lista Demais Condição para atualização esta vazia.");

            foreach(var CondicaoPagamentoNew in request)
            {

                var CondicaoPagamentoOld = _aprovacaoComercialHierarquiaService.ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(CondicaoPagamentoNew.AprovacaoComercialHierarquiaId, CondicaoPagamentoNew.TipoPessoaId, CondicaoPagamentoNew.SegmentacaoId, true);
                var jaExisteCondicaoPagamento = CondicaoPagamentoOld != null;

                if (!jaExisteCondicaoPagamento)
                    CondicaoPagamentoOld = new AprovacaoComercialHierarquiaCondicaoPagamento() 
                    { 
                        Id = Guid.NewGuid(),
                        AprovacaoComercialHierarquiaId = CondicaoPagamentoNew.AprovacaoComercialHierarquiaId,
                        TipoPessoaId = CondicaoPagamentoNew.TipoPessoaId,
                        SegmentacaoId = CondicaoPagamentoNew.SegmentacaoId,
                    };

                if(CondicaoPagamentoOld.MediaDiasDe == CondicaoPagamentoNew.MediaDiasDe || CondicaoPagamentoOld.MediaDiasAte == CondicaoPagamentoNew.MediaDiasAte)
                {
                    var payload = new
                    {
                        novo = !jaExisteCondicaoPagamento,
                        oldObject = CondicaoPagamentoOld,
                        newObject = CondicaoPagamentoNew
                    };
                    _aprovacaoComercialService.AdicionarLog(
                        new AprovacaoComercialLog("con_aprovacao_comercial_hierarquia_condicao_pagamento", "AprovacaoComercialApplicationService.AtualizarCondicaoPagamento", "", PayloadHelper.ConvertToJson(payload))
                        );
                }

                CondicaoPagamentoOld.MediaDiasDe = CondicaoPagamentoNew.MediaDiasDe;
                CondicaoPagamentoOld.MediaDiasAte = CondicaoPagamentoNew.MediaDiasAte;


                if (!jaExisteCondicaoPagamento)
                    _aprovacaoComercialHierarquiaService.Adicionar<AprovacaoComercialHierarquiaCondicaoPagamento>(CondicaoPagamentoOld);

                Commit();

            }

            return result.ToString();

        }

    }
}
