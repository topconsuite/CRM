using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Lead;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Lead.LeadInseridaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Lead.LeadSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class LeadApplicationService : ApplicationServiceBase<Lead>, ILeadApplicationService
    {
        private readonly LeadService _leadService;
        private readonly LeadContatoService _leadContatoService;
        private readonly IVisitaService _visitaService;
        private readonly IEnderecoService _enderecoService;
        private readonly IOportunidadeService _oportunidadeService;

        public LeadApplicationService(LeadService leadService, LeadContatoService leadContatoService, IVisitaService visitaService, IEnderecoService enderecoService, IOportunidadeService oportunidadeService, IUnitOfWork unityOfWork) : base(leadService, unityOfWork)
        {
            _leadService = leadService;
            _leadContatoService = leadContatoService;
            _visitaService = visitaService;
            _enderecoService = enderecoService;
            _oportunidadeService = oportunidadeService;
        }

        public LeadInseridaResponse Adicionar(string usuario, LeadInclusaoRequest leadRequest)
        {
            var lead = AutoMapper.Mapper.Map(leadRequest, new Lead());

            if (lead.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(lead.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    lead.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    Commit();

                }
            }

            lead.ContatoPrincipal = AutoMapper.Mapper.Map(leadRequest.ContatoPrincipal, new LeadContato());
            lead.ContatoSecundario = AutoMapper.Mapper.Map(leadRequest.ContatoSecundario, new LeadContato());

            if (string.IsNullOrEmpty(lead.ContatoSecundario.Nome) && lead.ContatoSecundario.Funcao == null && lead.ContatoSecundario.Telefone == 0 && lead.ContatoSecundario.Celular == 0)
                lead.ContatoSecundario = null;

            using (var scope = new TransactionScope())
            {
                _leadService.Adicionar(usuario, lead);

                if (lead.VisitaNumero != 0)
                {
                    var visita = _visitaService.ObterPorId(lead.UsinaCodigo, lead.VisitaAno, lead.VisitaNumero, true);

                    if (visita == null)
                    {
                        AssertionConcern.Notify("Visita", "Visita informada para Lead não localizada.");
                        scope.Dispose();
                        return null;
                    }

                    if(visita.LeadNumero > 0)
                    {
                        AssertionConcern.Notify("Visita", "Visita informada já esta vinculada a um Lead.");
                        scope.Dispose();
                        return null;
                    }

                    visita.LeadNumero = lead.Numero;
                    visita.LeadAno = lead.Ano;

                    var log = new VisitaLog()
                    {
                        Usina = visita.UsinaCodigo,
                        Ano = visita.Ano,
                        Numero = visita.Numero,

                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Tipo = "ALTERACAO",
                        Evento = "LEAD GERADO",
                        Complemento = $"Lead {lead.Numero.ToString().PadRight(6, '0')}-{lead.Ano} gerado em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}."
                    };

                    visita.Logs.Add(log);

                }

                Commit();

                scope.Complete();
            }

            return AutoMapper.Mapper.Map(lead, new LeadInseridaResponse());
        }

        public Guid AdicionarAnexo(string usuario, LeadAnexoAdicionarRequest request)
        {
            var id = Guid.NewGuid();

            if (request == null)
            {
                AssertionConcern.Notify("Anexo", "Arquivos maiores que 10 MB não são suportados. Por favor, envie um arquivo com tamanho inferior.");
                return id;
            }

            var leadAnexo = AutoMapper.Mapper.Map(request, new LeadAnexo());

            _leadService.AdicionarAnexo(usuario, id, request.Usina, request.Ano, request.Numero, request.Arquivo, request.Nome);

            return id;
        }

        public ICollection<LeadAnexoResponse> ListarAnexos(int usina, int anoLead, int numeroLead)
        {
            return AutoMapper.Mapper.Map(_leadService.ListarAnexos(usina, anoLead, numeroLead), new List<LeadAnexoResponse>());
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _leadService.ObterAnexo(id);
        }

        public LeadAnexo ObterLeadAnexoPorId(Guid id)
        {
            return _leadService.ObterLeadAnexoPorId(id);
        }

        public void AtualizarDescricaoAnexo(LeadAnexoAtualizarRequest anexo)
        {
            _leadService.AtualizarDescricaoAnexo(AutoMapper.Mapper.Map(anexo, new LeadAnexo()));
        }

        public void RemoverAnexo(Guid id)
        {
            _leadService.RemoverAnexo(id);
        }

        public void Atualizar(string usuario, LeadAlteracaoRequest lead)
        {
            var leadOld = _leadService.ObterPorId(lead.Usina.Codigo, lead.Ano, lead.Numero);

            if (leadOld.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(leadOld.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    leadOld.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    Commit();

                }
            }

            leadOld.ContatoPrincipal = _leadContatoService.ObterPorId(lead.Usina.Codigo, lead.Ano, lead.Numero, 1);
            leadOld.ContatoSecundario = _leadContatoService.ObterPorId(lead.Usina.Codigo, lead.Ano, lead.Numero, 2);

            int sequenciaObraLog = _leadService.ListarFiltrados<LeadLog>(t => t.Usina == lead.Usina.Codigo && t.AnoLead == lead.Ano && t.NumeroLead == lead.Numero).Max(t => t.Sequencia);

            var clienteAnterior = leadOld.Cliente;
            var vendedorAnterior = leadOld.VendedorCodigo;
            var viaCaptacaoAnterior = leadOld.ViaCaptacaoCodigo;
            var faseAnterior = leadOld.FaseCodigo;
            var classificacaoAnterior = leadOld.GetClassificacaoDescricao;
            var proximaEtapaAnterior = leadOld.ProximaEtapa;
            var motivoPerdaAnterior = leadOld.MotivoPerdaCodigo;

            using (var scope = new TransactionScope())
            {
                lead.IdCadastro = leadOld.IdCadastro;
                lead.IdAtualizacao = StringHelper.GetIDD(usuario);
                var leadNew = AutoMapper.Mapper.Map(lead, leadOld);

                leadNew.Vendedor = _leadService.ObterPorId<Vendedor>(leadNew.VendedorCodigo);
                leadNew.ViaCaptacao = _leadService.ObterPorId<CadastroGeral>(leadNew.ViaCaptacaoCodigo);
                leadNew.Fase = _leadService.ObterPorId<LeadFase>(leadNew.FaseCodigo);
                leadNew.MotivoPerda = _leadService.ObterPorId<MotivoPerda>(leadNew.MotivoPerdaCodigo);


                if ((!(leadNew.ContatoSecundario.Nome ?? "").Trim().Equals("") || (leadNew.ContatoSecundario.FuncaoCodigo ?? 0) != 0
                 || leadNew.ContatoSecundario.Ddd != 0 || leadNew.ContatoSecundario.Telefone != 0
                 || leadNew.ContatoSecundario.DddCelular != 0 || leadNew.ContatoSecundario.Celular != 0) && leadNew.ContatoSecundario.NumeroLead == 0)
                {
                    leadNew.ContatoSecundario.Usina = leadOld.ContatoPrincipal.Usina;
                    leadNew.ContatoSecundario.AnoLead = leadOld.ContatoPrincipal.AnoLead;
                    leadNew.ContatoSecundario.NumeroLead = leadOld.ContatoPrincipal.NumeroLead;
                    leadNew.ContatoSecundario.Sequencia = 2;

                    if (leadNew.ContatoSecundario.Nome == null)
                        leadNew.ContatoSecundario.Nome = "";

                    if (leadNew.ContatoSecundario.FuncaoCodigo == null)
                        leadNew.ContatoSecundario.FuncaoCodigo = 0;

                    _leadContatoService.Adicionar<LeadContato>(leadNew.ContatoSecundario);
                    Commit();
                }

                if (clienteAnterior != leadNew.Cliente)
                {
                    sequenciaObraLog++;
                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração do nome do cliente de {clienteAnterior} para {leadNew.Cliente}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (vendedorAnterior != leadNew.VendedorCodigo)
                {
                    var vendedorOld = _leadService.ListarFiltrados<Vendedor>(t => t.Codigo == vendedorAnterior).FirstOrDefault();

                    sequenciaObraLog++;
                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração do vendedor de {(vendedorOld == null ? vendedorAnterior.ToString() : vendedorOld.Nome)} para {leadNew.Vendedor.Nome}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (viaCaptacaoAnterior != leadNew.ViaCaptacaoCodigo)
                {
                    var viaCaptacaoOld = _leadService.ListarFiltrados<CadastroGeral>(t => t.Codigo == viaCaptacaoAnterior).FirstOrDefault();

                    sequenciaObraLog++;

                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração de via de captação de {(viaCaptacaoOld == null ? viaCaptacaoOld.ToString() : viaCaptacaoOld.Descricao)} para {leadNew.ViaCaptacao.Descricao}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (faseAnterior != leadNew.FaseCodigo)
                {
                    var faseOld = _leadService.ListarFiltrados<LeadFase>(t => t.Codigo == faseAnterior).FirstOrDefault();

                    sequenciaObraLog++;

                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração de fase do lead de {(faseOld == null ? faseOld.ToString() : faseOld.Descricao)} para {leadNew.Fase.Descricao}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (classificacaoAnterior != leadNew.GetClassificacaoDescricao)
                {
                    sequenciaObraLog++;

                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração de classificação do lead de {classificacaoAnterior} para {leadNew.GetClassificacaoDescricao}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (proximaEtapaAnterior != leadNew.ProximaEtapa)
                {
                    sequenciaObraLog++;

                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração de próxima etapa do lead de {proximaEtapaAnterior} para {leadNew.ProximaEtapa}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                if (motivoPerdaAnterior != (leadNew?.MotivoPerdaCodigo ?? 0))
                {
                    var motivoPerdaOld = _leadService.ListarFiltrados<MotivoPerda>(t => t.Codigo == motivoPerdaAnterior).FirstOrDefault();

                    sequenciaObraLog++;

                    var log = new LeadLog()
                    {
                        Usina = leadNew.UsinaCodigo,
                        AnoLead = leadNew.Ano,
                        NumeroLead = leadNew.Numero,
                        Sequencia = sequenciaObraLog,
                        DataHoraEvento = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO LEAD",
                        Complemento = $"Alteração de motivo da perda do lead de {(motivoPerdaOld == null ? "" : motivoPerdaOld.Descricao)} para {(leadNew.MotivoPerda == null ? "" : leadNew.MotivoPerda.Descricao)}.",
                        Tipo = ""
                    };

                    _leadService.Adicionar<LeadLog>(log);
                    Commit();
                }

                Commit();
                scope.Complete();
            }
        }

        public PagedList<LeadSimplesResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Lead, bool>> filter)
        {
            var leads = _leadService.ListarEmOrdemDecrescente(pagina, porPagina, filter);

            return AutoMapper.Mapper.Map(leads, new PagedList<LeadSimplesResponse>());
        }

        public IEnumerable<LeadLogResponse> ListarLeadLogsPorId(int idUsina, int ano, int numero)
        {
            var logs = _leadService.ListarFiltrados<LeadLog>(t => t.Usina == idUsina && t.AnoLead == ano && t.NumeroLead == numero);

            return AutoMapper.Mapper.Map(logs, new List<LeadLogResponse>());
        }

        public LeadResponse ObterPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var lead = _leadService.ObterPorUsinaAnoNumero(idUsina, ano, numero);

            return AutoMapper.Mapper.Map(lead, new LeadResponse());
        }

        public PagedList<LeadInteracaoResponse> ListarInteracoes(int pagina, int porPagina, Expression<Func<LeadInteracao, bool>> filter)
        {
            var leadInteracoes = _leadService.ListarInteracoes(pagina, porPagina, filter);

            return AutoMapper.Mapper.Map(leadInteracoes, new PagedList<LeadInteracaoResponse>());
        }

        public void AdicionarInteracao(string usuario, LeadInteracaoAdicionarRequest request)
        {
            var leadInteracao = AutoMapper.Mapper.Map(request, new LeadInteracao());

            leadInteracao.Id = Guid.NewGuid();
            leadInteracao.IdCadastro = StringHelper.GetIDD(usuario);

            _leadService.AdicionarInteracao(usuario, leadInteracao);
        }

        public OportunidadeResponse ObterOportunidadeDeLead(int usina, int ano, int numero)
        {
            const int QUALIFICACAO = 1;

            var lead = _leadService.ObterPorId(usina, ano, numero);

            lead.ContatoPrincipal = _leadContatoService.ObterPorId(lead.UsinaCodigo, lead.Ano, lead.Numero, 1);
            lead.ContatoSecundario = _leadContatoService.ObterPorId(lead.UsinaCodigo, lead.Ano, lead.Numero, 2);

            var newOportunidade = AutoMapper.Mapper.Map(lead, new Oportunidade());

            if (newOportunidade.ContatoSecundario == null)
                newOportunidade.ContatoSecundario = new OportunidadeContato();

            newOportunidade.Fase = _oportunidadeService.ObterPorId<OportunidadeFase>(QUALIFICACAO);
            if(newOportunidade.Fase != null) 
                newOportunidade.FaseCodigo = newOportunidade.Fase.Codigo;

            newOportunidade.Classificacao = lead.Classificacao;
            newOportunidade.EnderecoMunicipio = _oportunidadeService.ObterPorId<Municipio>(newOportunidade.EnderecoMunicipioCodigo);
            newOportunidade.Vendedor = _oportunidadeService.ObterPorId<Vendedor>(newOportunidade.VendedorCodigo);
            newOportunidade.ViaCaptacao = _oportunidadeService.ObterPorId<CadastroGeral>(newOportunidade.ViaCaptacaoCodigo);

            return AutoMapper.Mapper.Map(newOportunidade, new OportunidadeResponse());
        }
    }
}
