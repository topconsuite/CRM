using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.SharedKernel.Helpers
{
    public static class StringHelper
    {
        //TODO: Implementar o método de encriptação da topsys

        public static string EncrypTopSys(string senhaTextoPuro)
        {
            const string CHAVE = "061127";
            int p = 0;

            StringBuilder senhaEncriptada = new StringBuilder();

            for (int i = 0; i < senhaTextoPuro.Length; i++)
            {
                p++;
                if (p > CHAVE.Length) p = 1;
                int j = Encoding.ASCII.GetBytes(CHAVE.Substring(p - 1, 1))[0] | 128;
                int n = Encoding.GetEncoding(1252).GetBytes(new char[] { senhaTextoPuro[i] })[0];
                senhaEncriptada.Append(ChrVB6(decode(n, j)));
            }
            return senhaEncriptada.ToString();
        }

        public static string EncryptMD5(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            value += "|54be1d80-b6d0-45c0-b8d7-13b3c798729f";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(value));
            System.Text.StringBuilder sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            return sbString.ToString();
        }

        private static int decode(int n, int j)
        {
            n = n ^ j;
            if (n < 31)
            {
                n = decode(128 + n, j);
            }
            else if (n > 127 && n < 159)
            {
                n = decode(n - 128, j);
            }

            return n;
        }

        public static char ChrVB6(int code)
        {
            return Encoding.GetEncoding(1252).GetChars(new byte[] { (byte)code })[0];
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string GetIDD(string idUsuario)
        {
            var d = DateTime.Now;

            var day = d.Day.ToString().PadLeft(2, '0');
            var month = d.Month.ToString().PadLeft(2, '0');
            var year = d.Year.ToString().Substring(2);

            return $"{idUsuario} {day}/{month}/{year}";
        }


        public static bool EmailIsValid(string email)
        {
            var regexEmail = new Regex(@"^[-a-zA-Z0-9][-_.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|live|haus|energy|casa|online|solar|top|inc|tech|[a-zA-Z]{2})$");

            return regexEmail.IsMatch(email);
        }

        public static string RemoverCaracteresNaoNumericos(this string source)
        {
            return Regex.Replace(source, "[^0-9.]", "");
        }

        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

        public class EnumExtensions
        {
            public static T ParseEnum<T>(string value)
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
        }

        public static string RemoverAcentos(string textoOriginal)
        {
            if (string.IsNullOrEmpty(textoOriginal))
                return textoOriginal;

            ReadOnlySpan<char> textoNormalizado = textoOriginal.Normalize(NormalizationForm.FormD).AsSpan();
            var textoSemAcento = new StringBuilder(textoOriginal.Length);

            foreach (var caractere in textoNormalizado)
            {
                var categoriaUnicode = CharUnicodeInfo.GetUnicodeCategory(caractere);
                if (categoriaUnicode != UnicodeCategory.NonSpacingMark)
                    textoSemAcento.Append(caractere);
            }

            return textoSemAcento.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
