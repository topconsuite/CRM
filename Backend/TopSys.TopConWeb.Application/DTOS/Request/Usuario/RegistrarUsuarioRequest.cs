using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Usuario
{
    public class RegistrarUsuarioRequest
    {
        public string IdUsuario { get; set; }

        public string Senha { get; set; }

        public string SenhaConfirmacao { get; set; }

        public string Email { get; set; }

        public RegistrarUsuarioRequest(string email)
        {
            Email = email;
            Senha = GenerateRandomPassword();
            SenhaConfirmacao = Senha;
            IdUsuario = GenerateUniqueId(email);

        }

        private string GenerateUniqueId(string email)
        {
            var emailPrefix = email.Split('@')[0];

            var sanitizedId = new string(emailPrefix.Where(char.IsLetterOrDigit).ToArray());

            int baseLength = 8;
            if (sanitizedId.Length > baseLength)
            {
                sanitizedId = sanitizedId.Substring(0, baseLength);
            }

            if (string.IsNullOrEmpty(sanitizedId))
            {
                sanitizedId = "user";
            }

            return sanitizedId.ToUpper();
        }

        private string GenerateRandomPassword()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[16];
                rng.GetBytes(randomBytes);
                string password = Convert.ToBase64String(randomBytes)
                                         .Replace("+", "a")
                                         .Replace("/", "b")
                                         .Substring(0, 10);
                return password;
            }
        }

    }
}
