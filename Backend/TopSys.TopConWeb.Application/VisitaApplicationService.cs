using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Visita;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class VisitaApplicationService : ApplicationServiceBase<Visita>, IVisitaApplicationService
    {

        private readonly IVisitaService _visitaService;
        private readonly IEnderecoService _enderecoService;
        private readonly IVendedorService _vendedorService;
        private readonly IVisitaTipoService _visitaTipoService;

        public VisitaApplicationService(IVisitaService visitaService, 
            IEnderecoService enderecoService,
            IVendedorService vendedorService,
            IVisitaTipoService visitaTipoService,
            IUnitOfWork unitofWork) : base(visitaService, unitofWork)
        {
            _visitaService = visitaService;
            _enderecoService = enderecoService;
            _vendedorService = vendedorService;
            _visitaTipoService = visitaTipoService;
        }

        public VisitaAdicionarResponse Adicionar(VisitaAdicionarRequest request)
        {

            var newVisita = AutoMapper.Mapper.Map(request, new Visita());
            newVisita.UsinaCodigo = 999; // Usina Padrão WEB
            newVisita.Ano = int.Parse(DateTime.Now.ToString("yy"));

            if (newVisita.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(newVisita.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    newVisita.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    Commit();

                }
            }

            using (var scope = new TransactionScope())
            {

                _visitaService.Adicionar(newVisita);

                Commit();

                scope.Complete();

            }

            var response = new VisitaAdicionarResponse()
            {
                Ano = newVisita.Ano,
                Numero = newVisita.Numero,
                Usina = newVisita.UsinaCodigo
            };

            return response;

        }

        public void Atualizar(VisitaAtualizarRequest request, string usuario)
        {

            var oldVisita = _visitaService.ObterPorId(request.UsinaCodigo, request.Ano, request.Numero, true);

            if(oldVisita == null)
            {
                AssertionConcern.Notify("Visita", "Visita não encontrada para Usina/Ano/Numero informado.");
                return;
            }

            if(oldVisita.Data != request.Data || oldVisita.HoraVisita != request.HoraVisita)
            {

                var log = new VisitaLog()
                {
                    Usina = oldVisita.UsinaCodigo,
                    Ano = oldVisita.Ano,
                    Numero = oldVisita.Numero,

                    DataHoraEvento = DateTime.Now,
                    Usuario = usuario,
                    Tipo = "ALTERAÇÃO",
                    Evento = "ALTERAÇÃO DATA",
                    Complemento = $"Alteração data de {oldVisita.Data.ToString("dd/MM/yyyy")} {oldVisita.HoraVisita.ToString()} para {request.Data.ToString("dd/MM/yyyy")} {request.HoraVisita.ToString()}"
                };

                oldVisita.Logs.Add(log);

            }

            if (!oldVisita.Cliente.Equals(request.Cliente, StringComparison.OrdinalIgnoreCase))
            {

                var log = new VisitaLog()
                {
                    Usina = oldVisita.UsinaCodigo,
                    Ano = oldVisita.Ano,
                    Numero = oldVisita.Numero,

                    DataHoraEvento = DateTime.Now,
                    Usuario = usuario,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO NOME DO CLIENTE",
                    Complemento = $"Alteração nome cliente de {oldVisita.Cliente} para {request.Cliente}"
                };

                oldVisita.Logs.Add(log);

            }

            if (oldVisita.VendedorCodigo != request.VendedorCodigo)
            {

                var vendedorNovo = _vendedorService.ObterPorId(request.VendedorCodigo);

                var log = new VisitaLog()
                {
                    Usina = oldVisita.UsinaCodigo,
                    Ano = oldVisita.Ano,
                    Numero = oldVisita.Numero,

                    DataHoraEvento = DateTime.Now,
                    Usuario = usuario,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO VENDEDOR",
                    Complemento = $"Alteração vendedor de {oldVisita.VendedorCodigo}-{oldVisita.Vendedor.Nome} para {request.VendedorCodigo}-{(vendedorNovo != null? vendedorNovo.Nome : "")}"
                };

                oldVisita.Logs.Add(log);

            }

            if (oldVisita.VisitaTipoCodigo != request.VisitaTipoCodigo)
            {

                var newTipoVisita = _visitaTipoService.ObterPorId(request.VisitaTipoCodigo);

                var log = new VisitaLog()
                {
                    Usina = oldVisita.UsinaCodigo,
                    Ano = oldVisita.Ano,
                    Numero = oldVisita.Numero,

                    DataHoraEvento = DateTime.Now,
                    Usuario = usuario,
                    Tipo = "ALTERACAO",
                    Evento = "ALTERAÇÃO TIPO DE VISITA",
                    Complemento = $"Alteração tipo de visita de {oldVisita.TipoVisita.Descricao} para {(newTipoVisita != null ? newTipoVisita.Descricao : request.VisitaTipoCodigo.ToString())}"
                };

                oldVisita.Logs.Add(log);

            }

            if (request.Endereco.Municipio.Codigo != 0)
                request.Endereco.Municipio.Pais = _enderecoService.ObterPorId<Municipio>(request.Endereco.Municipio.Codigo).Pais;

            oldVisita = AutoMapper.Mapper.Map(request, oldVisita);

            if (oldVisita.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(oldVisita.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    oldVisita.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    Commit();

                }
            }

            if (oldVisita.ContatoSecundario != null)
            {
                var contato = oldVisita.ContatoSecundario;
                var estaPreenchido = string.IsNullOrWhiteSpace(contato.Nome) && contato.Telefone > 0;

                if (!estaPreenchido)
                    oldVisita.ContatoSecundario = null;

                if(estaPreenchido && contato.NumeroVisita == 0)
                {
                    contato.Usina = oldVisita.UsinaCodigo;
                    contato.AnoVisita = oldVisita.Ano;
                    contato.NumeroVisita = oldVisita.Numero;
                    contato.Sequencia = 2;

                    this.Adicionar<VisitaContato>(contato);
                }

            }

            Commit();

        }

        public PagedList<VisitaResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Visita, bool>> filter)
        {
            var pagedList = _visitaService.ListarEmOrdemDecrescente(pagina, porPagina, filter);
            var response = AutoMapper.Mapper.Map(pagedList, new PagedList<VisitaResponse>());

            return response;
        }

        public VisitaResponse ObterPorId(int usina, int ano, int numero)
        {
            var visita = _visitaService.ObterPorId(usina, ano, numero);
            return AutoMapper.Mapper.Map(visita, new VisitaResponse());
        }

        public LeadResponse ObterLeadDeVisita(int usina, int ano, int numero)
        {

            var visita = _visitaService.ObterPorId(usina, ano, numero);

            if(visita == null)
            {
                AssertionConcern.Notify("Visita", "Visita informada não localizada.");
                return null;
            }

            var newLead = AutoMapper.Mapper.Map(visita, new Lead());

            if (newLead.ContatoSecundario == null)
                newLead.ContatoSecundario = new LeadContato();

            return AutoMapper.Mapper.Map(newLead, new LeadResponse());


        }

        public Guid AdicionarAnexo(string usuario, VisitaAnexoAdicionarRequest request)
        {

            var id = Guid.NewGuid();

            if(request == null)
            {
                AssertionConcern.Notify("Anexo", "Arquivos maiores que 10 MB não são suportados. Por favor, envie um arquivo com tamanho inferior.");
                return id;
            }

            var visitaAnexo = AutoMapper.Mapper.Map(request, new VisitaAnexo());

            _visitaService.AdicionarAnexo(usuario, id, request.Usina, request.Ano, request.Numero, request.Arquivo, request.Nome);

            return id;

        }

        public ICollection<VisitaAnexoResponse> ListarAnexos(int usina, int anoVisita, int numeroVisita)
        {
            return AutoMapper.Mapper.Map(_visitaService.ListarAnexos(usina, anoVisita, numeroVisita), new List<VisitaAnexoResponse>());
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _visitaService.ObterAnexo(id);
        }

        public VisitaAnexo ObterVisitaAnexoPorId(Guid id)
        {
            return _visitaService.ObterVisitaAnexoPorId(id);
        }

        public void AtualizarDescricaoAnexo(VisitaAnexoAtualizarRequest anexo)
        {
            _visitaService.AtualizarDescricaoAnexo(AutoMapper.Mapper.Map(anexo, new VisitaAnexo()));
        }

        public void RemoverAnexo(Guid id)
        {
            _visitaService.RemoverAnexo(id);
        }

        public void AdicionarHistorico(VisitaHistoricoAdicionarRequest request, string usuario)
        {

            var newHistorico = AutoMapper.Mapper.Map(request, new VisitaHistorico());

            newHistorico.Id = Guid.NewGuid();
            newHistorico.IdCadastro = StringHelper.GetIDD(usuario);

            _visitaService.AdicionarHistorico(newHistorico);

            Commit();

        }

        public PagedList<VisitaHistoricoResponse> ListarHistoricoEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<VisitaHistorico, bool>> filter)
        {
            var pagedList = _visitaService.ListarHistoricoEmOrdemDecrescente(pagina, porPagina, filter);
            var response = AutoMapper.Mapper.Map(pagedList, new PagedList<VisitaHistoricoResponse>());

            return response;
        }

        
    }
}
