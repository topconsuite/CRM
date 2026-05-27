using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Domain.Enums;
using static TopSys.TopConWeb.SharedKernel.Helpers.StringHelper;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Usuario
    {
        public string Id { get; private set; }

        public string Nome { get; private set; }

        public string Senha { get; private set; }
        public string Email { get; set; }
        public string StatusString
        {
            get { return Status.ToString(); }
            private set { Status = EnumExtensions.ParseEnum<EStatusUsuario>(value); }
        }

        public EStatusUsuario Status { get; set; }


        public IDictionary<string,string> Direitos { get; private set; }

        protected Usuario() { }

        public Usuario(string id, string senha, string email)
        {
            this.Id = id;
            this.Nome = id;
            this.Senha = StringHelper.EncrypTopSys(senha);
            this.Email = email;
            this.Status = EStatusUsuario.Nao_Verificado;
        }

        public Usuario(string id, string senha, IDictionary<string, string> direitos)
        {
            this.Id = id;
            this.Nome = id;
            this.Senha = StringHelper.EncrypTopSys(senha);
            this.Direitos = direitos;
        }
    }
}
