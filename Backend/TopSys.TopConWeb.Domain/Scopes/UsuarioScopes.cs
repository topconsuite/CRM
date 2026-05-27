using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class UsuarioScopes
    {
        public static bool AutenticacaoScopeEhValida(this Usuario usuario)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                //TODO: Criar dicionário de mensagens
                //TODO: Testar scopes treinamento 1940-A AULA 03
                AssertionConcern.AssertNotNull(usuario, "usuario", "Usuário e ou senha inválidos")

            );
        }

        public static bool RegistrarUsuarioScopeIsValid(this Usuario usuario)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotEmpty(usuario.Id , "usuario_id", "O E-mail é obrigatório"),
                AssertionConcern.AssertNotEmpty(usuario.Senha, "usuario_senha", "A senha é obrigatória")

            );
        }

        public static bool CadastrarSenhaScopeIsValid(this Usuario usuario, string novaSenha, string confirmacaoNovaSenha)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(usuario, "usuario", "Usuário não cadastrado"),
                AssertionConcern.AssertAreEquals(usuario?.Senha ?? "","", "usuario_senha", "Usuário já possui senha cadastrada"),
                AssertionConcern.AssertAreEquals(novaSenha, confirmacaoNovaSenha, "usuario_senha", "Senhas digitadas diferem"),
                AssertionConcern.AssertNotEmpty(novaSenha, "usuario_senha", "Não foi informado uma senha")
            );
        }
    }
}
