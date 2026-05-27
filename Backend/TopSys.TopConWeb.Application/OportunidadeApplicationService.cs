using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class OportunidadeApplicationService : ApplicationServiceBase<Oportunidade>, IOportunidadeApplicationService
    {

        private readonly IOportunidadeService _oportunidadeService;
        private readonly IVendedorService _vendedorService;
        private readonly ICadastroGeralService _cadastroGeralService;
        private readonly IMotivoPerdaService _motivoPerdaService;
        private readonly IConcorrenteService _concorrenteService;

        public OportunidadeApplicationService(
            IUnitOfWork unitOfWork, 
            IOportunidadeService oportunidadeService,
            IVendedorService vendedorService,
            ICadastroGeralService cadastroGeralService,
            IMotivoPerdaService motivoPerdaService,
            IConcorrenteService concorrenteService) : base(oportunidadeService, unitOfWork)
        {
            _oportunidadeService = oportunidadeService;
            _vendedorService = vendedorService;
            _cadastroGeralService = cadastroGeralService;
            _motivoPerdaService = motivoPerdaService;
            _concorrenteService = concorrenteService;
        }

        public OportunidadeAdicionarResponse Adicionar(OportunidadeAdicionarRequest request)
        {

            var newOportunidade = AutoMapper.Mapper.Map(request, new Oportunidade());
            newOportunidade.UsinaCodigo = 999; // Usina Padrão WEB
            newOportunidade.Data = DateTime.Now;
            newOportunidade.Ano = int.Parse(newOportunidade.Data.ToString("yy"));

            using (var scope = new TransactionScope())
            {

                _oportunidadeService.Adicionar(newOportunidade);

                Commit();

                scope.Complete();

            }

            var response = new OportunidadeAdicionarResponse()
            {
                Ano = newOportunidade.Ano,
                Numero = newOportunidade.Numero,
                Usina = newOportunidade.UsinaCodigo
            };

            return response;

        }

        public void Atualizar(OportunidadeAtualizarRequest request, string usuario)
        {

            var oldOportunidade = _oportunidadeService.ObterPorId(request.UsinaCodigo, request.Ano, request.Numero, true);

            if (oldOportunidade == null)
            {
                AssertionConcern.Notify("Visita", "Visita não encontrada para Usina/Ano/Numero informado.");
                return;
            }

            if(!oldOportunidade.OportunidadeNome.Equals(request.OportunidadeNome, StringComparison.OrdinalIgnoreCase))
            {
                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO NOME OPORTUNIDADE",
                    Complemento = $"Alteração nome da oportunidade de {oldOportunidade.OportunidadeNome} para {request.OportunidadeNome}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (!oldOportunidade.Cliente.Equals(request.Cliente, StringComparison.OrdinalIgnoreCase) 
                || oldOportunidade.IntervenienteCodigo != request.IntervenienteCodigo)
            {
                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO CLIENTE",
                    Complemento = $"Alteração cliente de {oldOportunidade.IntervenienteCodigo}-{oldOportunidade.Cliente}" +
                    $" para {request.IntervenienteCodigo}-{request.Cliente}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.VendedorCodigo != request.VendedorCodigo)
            {

                var newVendedor = _vendedorService.ObterPorId(request.VendedorCodigo);

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO VENDEDOR",
                    Complemento = $"Alteração vendedor de {oldOportunidade.VendedorCodigo}-{oldOportunidade.Vendedor.Nome}" +
                    $" para {request.VendedorCodigo}-{(newVendedor != null ? newVendedor.Nome : "")}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.VendedorCodigo != request.VendedorCodigo)
            {

                var newCaptacao = _cadastroGeralService.ObterPorId(request.ViaCaptacaoCodigo);

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO VIA CAPTAÇÃO",
                    Complemento = $"Alteração via captação de " +
                    $"{(oldOportunidade.ViaCaptacao != null ? oldOportunidade.ViaCaptacao.Descricao : "Nenhum")}" +
                    $" para {(newCaptacao != null ? newCaptacao.Descricao : "Nenhum")}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.FaseCodigo != request.FaseCodigo)
            {

                var newFase = _oportunidadeService.ListarFiltrados<OportunidadeFase>(x => x.Codigo == request.FaseCodigo).FirstOrDefault();

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO FASE",
                    Complemento = $"Alteração fase de " +
                    $"{(oldOportunidade.Fase != null ? oldOportunidade.Fase.Descricao : "Nenhum")}" +
                    $" para {(newFase != null ? newFase.Descricao : "Nenhum")}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.Classificacao != request.Classificacao)
            {

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO CLASSIFICAÇÃO",
                    Complemento = $"Alteração classificacao de {oldOportunidade.Classificacao.ToString()}" +
                    $" para {request.Classificacao.ToString()}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (!oldOportunidade.ProximaEtapa.Equals(request.ProximaEtapa, StringComparison.OrdinalIgnoreCase))
            {
                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO PROX. ETAPA",
                    Complemento = $"Alteração próxima etapa de {oldOportunidade.ProximaEtapa}" +
                    $" para {request.ProximaEtapa}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.PrevisaoFechamento != request.PrevisaoFechamento)
            {
                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO PREV. FECHAMENTO",
                    Complemento = $"Alteração próxima etapa de {(oldOportunidade.PrevisaoFechamento != null ? ((DateTime)oldOportunidade.PrevisaoFechamento).ToString("dd/MM/yyyy") : "Vazio")}" +
                    $" para  {(request.PrevisaoFechamento != null ? ((DateTime)request.PrevisaoFechamento).ToString("dd/MM/yyyy") : "Vazio")}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.MotivoPerdaCodigo != request.MotivoPerdaCodigo)
            {

                var newMotivoPerda = _motivoPerdaService.ObterPorId(request.MotivoPerdaCodigo);

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO MOTIVO PERDA",
                    Complemento = $"Alteração motivo perda de " +
                    $"{(oldOportunidade.MotivoPerda != null ? oldOportunidade.MotivoPerda.Descricao : "Nenhum")}" +
                    $" para {(newMotivoPerda != null ? newMotivoPerda.Descricao : "Nenhum")}."
                };

                oldOportunidade.Logs.Add(log);
            }

            if (oldOportunidade.ConcorrenteCodigo != request.ConcorrenteCodigo)
            {

                var newConcorrente = _concorrenteService.ObterPorId(request.ConcorrenteCodigo);

                var log = new OportunidadeLog()
                {
                    Usina = oldOportunidade.UsinaCodigo,
                    Ano = oldOportunidade.Ano,
                    Numero = oldOportunidade.Numero,
                    Usuario = usuario,
                    DataHoraEvento = DateTime.Now,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO CONCORRENTE",
                    Complemento = $"Alteração concorrente de " +
                    $"{(oldOportunidade.Concorrente != null ? oldOportunidade.Concorrente.Descricao : "Nenhum")}" +
                    $" para {(newConcorrente != null ? newConcorrente.Descricao : "Nenhum")}."
                };

                oldOportunidade.Logs.Add(log);
            }


            oldOportunidade = AutoMapper.Mapper.Map(request, oldOportunidade);

            if (oldOportunidade.ContatoSecundario != null)
            {
                var contato = oldOportunidade.ContatoSecundario;
                var estaPreenchido = string.IsNullOrWhiteSpace(contato.Nome) && contato.Telefone > 0;

                if (!estaPreenchido)
                    oldOportunidade.ContatoSecundario = null;

                if (estaPreenchido && contato.NumeroOportunidade == 0)
                {
                    contato.Usina = oldOportunidade.UsinaCodigo;
                    contato.AnoOportunidade = oldOportunidade.Ano;
                    contato.NumeroOportunidade = oldOportunidade.Numero;
                    contato.Sequencia = 2;

                    this.Adicionar<OportunidadeContato>(contato);
                }

            }

            Commit();

        }

        public PagedList<OportunidadeResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Oportunidade, bool>> filter)
        {
            var pagedList = _oportunidadeService.ListarEmOrdemDecrescente(pagina, porPagina, filter);
            var response = AutoMapper.Mapper.Map(pagedList, new PagedList<OportunidadeResponse>());

            return response;
        }

        public OportunidadeResponse ObterPorId(int usina, int ano, int numero)
        {
            var oportunidade = _oportunidadeService.ObterPorId(usina, ano, numero);
            return AutoMapper.Mapper.Map(oportunidade, new OportunidadeResponse());
        }

        public PropostaDetalhadaResponse ObterPropostaDeOportunidade(int usina, int ano, int numero)
        {

            var oportunidade = _oportunidadeService.ObterPorId(usina, ano, numero);

            if (oportunidade == null)
            {
                AssertionConcern.Notify("Oportunidade", "Oportunidade informada não localizada.");
                return null;
            }

            var newProposta = AutoMapper.Mapper.Map(oportunidade, new Proposta());

            newProposta.Obra = AutoMapper.Mapper.Map(oportunidade, new Obra());

            newProposta.InscricaoEstadual = "ISENTO";
            newProposta.Observacao = oportunidade.ObservacaoInterna;

            newProposta.CelularDdd = oportunidade.DddCelular;
            newProposta.CelularNumero = oportunidade.Celular;
            newProposta.TelefoneDdd = oportunidade.DddTelefone;
            newProposta.TelefoneNumero = oportunidade.Telefone;
            newProposta.Contato = oportunidade.Cliente;
            newProposta.IntervenienteRazao = oportunidade.Cliente;
            newProposta.IntervenienteNome = oportunidade.Cliente;


            newProposta.Obra.ContatoPrincipalCelularDdd = oportunidade.ContatoPrincipal.DddCelular;
            newProposta.Obra.ContatoPrincipalCelularNumero = oportunidade.ContatoPrincipal.Celular;
            newProposta.Obra.ContatoPrincipalFuncao = oportunidade.ContatoPrincipal.Funcao;
            newProposta.Obra.ContatoPrincipalFuncaoCodigo = oportunidade.ContatoPrincipal.FuncaoCodigo;
            newProposta.Obra.ContatoPrincipalNome = oportunidade.ContatoPrincipal.Nome;
            newProposta.Obra.ContatoPrincipalTelefoneDdd = oportunidade.ContatoPrincipal.DddTelefone;
            newProposta.Obra.ContatoPrincipalTelefoneNumero = oportunidade.ContatoPrincipal.Telefone;

            if (oportunidade.ContatoSecundario != null)
            {
                newProposta.Obra.ContatoSecundarioCelularDdd = oportunidade.ContatoSecundario.DddCelular;
                newProposta.Obra.ContatoSecundarioCelularNumero = oportunidade.ContatoSecundario.Celular;
                newProposta.Obra.ContatoSecundarioFuncao = oportunidade.ContatoSecundario.Funcao;
                newProposta.Obra.ContatoSecundarioFuncaoCodigo = oportunidade.ContatoSecundario.FuncaoCodigo;
                newProposta.Obra.ContatoSecundarioNome = oportunidade.ContatoSecundario.Nome;
                newProposta.Obra.ContatoSecundarioTelefoneDdd = oportunidade.ContatoSecundario.DddTelefone;
                newProposta.Obra.ContatoSecundarioTelefoneNumero = oportunidade.ContatoSecundario.Telefone;
            }

            return AutoMapper.Mapper.Map(newProposta, new PropostaDetalhadaResponse());

        }

        public List<OportunidadeFaseDTO> ListarFases()
        {

            var fases = _oportunidadeService.ListarFiltrados<OportunidadeFase>(x => x.Descricao != null);
            return AutoMapper.Mapper.Map(fases, new List<OportunidadeFaseDTO>());

        }
        

        public List<OportunidadeTipoDTO> ListarTiposAtivos()
        {

            var tipos = _oportunidadeService.ListarFiltrados<OportunidadeTipo>(x => x.Ativo);
            return AutoMapper.Mapper.Map(tipos, new List<OportunidadeTipoDTO>());

        }

        public List<ConcorrenteDTO> ListarConcorrentesAtivos()
        {

            var concorrentes = _oportunidadeService.ListarFiltrados<Concorrente>(x => x.Ativo);
            return AutoMapper.Mapper.Map(concorrentes, new List<ConcorrenteDTO>());

        }

        public PagedList<OportunidadeInteracaoResponse> ListarInteracoes(int pagina, int porPagina, Expression<Func<OportunidadeInteracao, bool>> filter)
        {
            var oportunidadeInteracoes = _oportunidadeService.ListarInteracoes(pagina, porPagina, filter);

            return AutoMapper.Mapper.Map(oportunidadeInteracoes, new PagedList<OportunidadeInteracaoResponse>());
        }

        public void AdicionarInteracao(string usuario, OportunidadeInteracaoAdicionarRequest request)
        {
            var oportunidadeInteracao = AutoMapper.Mapper.Map(request, new OportunidadeInteracao());

            oportunidadeInteracao.Id = Guid.NewGuid();
            oportunidadeInteracao.IdCadastro = StringHelper.GetIDD(usuario);

            _oportunidadeService.AdicionarInteracao(usuario, oportunidadeInteracao);
        }
    }
}
