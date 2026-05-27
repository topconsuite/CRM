using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel.Events;
using TopSys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly IUsuarioWebFiltroRepository _usuarioWebFiltroRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IParametroRepository parametroRepository, IUsuarioWebFiltroRepository usuarioWebFiltroRepository) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _parametroRepository = parametroRepository;
            _usuarioWebFiltroRepository = usuarioWebFiltroRepository;
        }

        public Usuario Autenticar(string id, string senha)
        {

            //TODO:refatorar para criptografar a senha
            //Foi necessário alterar o código abaixo para deixar a senha toda em maiusculo antes de encriptá-la devido ao sistema legado
            senha = StringHelper.EncrypTopSys(senha.ToUpper());

            var usuario = _usuarioRepository.ObterPorIdSenha(id, senha);
            return usuario.AutenticacaoScopeEhValida() ? usuario : null;
        }

        public Usuario CadastrarSenha(string id, string senha, string senhaConfirmacao)
        {
            var usuario = _usuarioRepository.ObterPorId(id);

            if (!usuario.CadastrarSenhaScopeIsValid(senha, senhaConfirmacao))
                return null;

            senha = StringHelper.EncrypTopSys(senha.ToUpper());

            _usuarioRepository.CadastrarSenha(id, senha);
            return new Usuario(id, senha, "");
        }

        public IDictionary<string, string> ObterClaimsVendedores(Usuario usuario)
        {
            return _usuarioRepository.ObterClaimsVendedores(usuario);
        }

        public Usuario Registrar(string id, string senha, string senhaConfirmacao, string email)
        {
            var existeUsuario = _usuarioRepository.ListarFiltrados(t => t.Id == id).FirstOrDefault();

            if (existeUsuario != null)
            {
                DomainEvent.Raise(new DomainNotification("IdDuplicado", "Já existe usuário com esse id"));
                
                return null;
            }
                
            Usuario usuario = new Usuario(id, senha, email);

            if (!usuario.RegistrarUsuarioScopeIsValid())
                return null;

            _usuarioRepository.Adicionar(usuario);

            return usuario;
        }

        public float? ObterPercentualDescontoLimitePorId(string id)
        {
            return _usuarioRepository.ObterPercentualDescontoLimitePorId(id);
        }

        public void GravarAcessoAplicacao(string usuarioNome, string aplicativo, int programa)
        {
            var usuarioListados = _usuarioRepository.ListarFiltrados(t => t.Nome == usuarioNome);
            var usuario = usuarioListados.Where(t => t.Status == EStatusUsuario.Verificado).FirstOrDefault();

            if (usuario == null)
                return;

            var clienteNome = _parametroRepository.ObterParametroN("Telluria", "ClienteNome");
            int.TryParse(_parametroRepository.ObterParametroN("Telluria", "ClienteCodigo"), out var clienteCodigo);

            _usuarioRepository.GravaAcessoAplicacao(new AcessoAplicacao(aplicativo, programa, usuario.Id, usuario.Email, clienteCodigo, clienteNome));
        }

        public IEnumerable<Usuario> ObterNomeUsuariosVerificados()
        {
            return _usuarioRepository.ListarTodos();
        }

        public Usuario ObterPorId(string id)
        {
            return _usuarioRepository.ObterPorId(id);
        }

        public UsuarioWebFiltro ObterFiltroWebPorId(string usuario, string aplicativo)
        {
            return _usuarioWebFiltroRepository.ObterPorId(usuario, aplicativo);
        }

        public void SalvarFiltroWeb(string usuario, string aplicativo, string json, string filterString)
        {
            _usuarioWebFiltroRepository.Salvar(usuario, aplicativo, json, filterString);
        }

        public Dictionary<string, string> ListarUsuariosComGrupos()
        {
            return _usuarioRepository.ListarUsuariosComGrupos();
        }

    }
}
