using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.Usuario;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class UsuarioApplicationService : ApplicationServiceBase<Usuario>, IUsuarioApplicationService
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioApplicationService(IUsuarioService usuarioService, IUnitOfWork unitOfWork) 
            : base(usuarioService, unitOfWork)
        {
            _usuarioService = usuarioService;
        }
        
        public AutenticarUsuarioResponse Autenticar(string id, string senha)
        {
            var usuario = _usuarioService.Autenticar(id, senha);

            if (usuario == null)
                return null;

            var AutenticarUsuarioResponse = new AutenticarUsuarioResponse
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Direitos = usuario.Direitos
            };

            return AutenticarUsuarioResponse;
        }

        public IDictionary<string, string> ObterClaimsVendedores(AutenticarUsuarioResponse usuario)
        {
            var _usuario = new Usuario(usuario.UsuarioId, "", usuario.Direitos);

            return _usuarioService.ObterClaimsVendedores(_usuario);
        }

        public RegistrarUsuarioResponse Registrar(RegistrarUsuarioRequest usuarioRequest)
        {

            var usuario = _usuarioService.Registrar(usuarioRequest.IdUsuario, usuarioRequest.Senha, usuarioRequest.SenhaConfirmacao, usuarioRequest.Email);

            Commit();

            if (usuario != null)
                return new RegistrarUsuarioResponse { IdUsuario = usuario.Id };

            return null;

        }

        public float? ObterPercentualDescontoLimitePorId(string id)
        {
            return _usuarioService.ObterPercentualDescontoLimitePorId(id);
        }

        public CadastrarSenhaUsuarioResponse CadastrarSenhaUsuario(CadastrarSenhaUsuarioRequest usuario)
        {
            var response = _usuarioService.CadastrarSenha(usuario.IdUsuario, usuario.Senha, usuario.SenhaConfirmacao);

            if (response != null)
                return new CadastrarSenhaUsuarioResponse { IdUsuario = response.Id };

            return null;
        }

        public void GravarAcessoAplicacao(string usuarioNome, string aplicativo, int programa)
        {
            _usuarioService.GravarAcessoAplicacao(usuarioNome, aplicativo, programa);
        }

        public UsuarioWebFiltroResponse ObterFiltroWebPorId(string usuario, string aplicativo)
        {
            return AutoMapper.Mapper.Map(_usuarioService.ObterFiltroWebPorId(usuario, aplicativo), new UsuarioWebFiltroResponse());
        }

        public void SalvarFiltroWeb(string usuario, UsuarioWebFiltroSalvarRequest request)
        {
            _usuarioService.SalvarFiltroWeb(usuario, request.Aplicativo, request.Json, request.FilterString);
        }

        public Usuario ObterUsuarioPeloEmail(string email)
        {
            return _usuarioService.ListarFiltrados(t => t.Email == email).FirstOrDefault();
        }
    }
}
