using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class EnderecoHelper
    {
        public static string AbreviarTipoLogradouro(string tipoLogradouro)
        {
            var length = tipoLogradouro.Length;
            tipoLogradouro = tipoLogradouro.ToUpper();
            var vogais = new char[] { 'A', 'E', 'I', 'O', 'U' };

            if (length <= 4) return tipoLogradouro.PadRight(3, 'X').Substring(0, 3);

            var consoantes = String.Join("", tipoLogradouro.Split(vogais));

            if (vogais.Contains(tipoLogradouro.FirstOrDefault())) // INICIA COM VOGAL
            {
                if (consoantes.Length < 2)
                {
                    return tipoLogradouro.Substring(0, 3);
                }
                else
                {
                    return (tipoLogradouro.First() + consoantes).Substring(0, 3);
                }
            }
            else  // INICIA COM CONSOANTE
            {
                if (consoantes.Length < 3)
                {
                    return tipoLogradouro.Substring(0, 3);
                }
                else
                {
                    return consoantes.Substring(0, 3);
                }
            }
        }

        public static string RetornarTipoLogradouro(string logradouro)
        {
            return logradouro.Split(' ')[0].ToUpper();
        }

        public static string RetornarTipoLogradouroAbreviado(string logradouro)
        {
            var tipoLogradouro = RetornarTipoLogradouro(logradouro);
            return AbreviarTipoLogradouro(tipoLogradouro);
        }

        public static string RetornarComTipoLogradouroAbreviado(string logradouro)
        {
            var tipoLogradouro = RetornarTipoLogradouro(logradouro);
            var abreviatura = AbreviarTipoLogradouro(tipoLogradouro);

            return abreviatura + logradouro.Substring(tipoLogradouro.Length);
        }

    }
}
