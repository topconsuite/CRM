using System.Linq;
using System.Text.RegularExpressions;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class EquipamentoScopes
    {
        public static bool ValidarCampoSimNao(string value) => (value.Equals("S") || value.Equals("N"));

        public static bool ValidarTipoProprietario(string tipoProprietario)
        {

            var values = new string[] { "", "0", "1", "2" };

            return values.Contains(tipoProprietario);

        }
        
        public static bool ValidarTipoRodado(string tipoRodado)
        {

            var values = new string[] { "", "0", "1", "2", "3", "4", "5", "6" };

            return values.Contains(tipoRodado);

        }

        public static bool ValidarTipoCarroceria(string tipoCarroceria)
        {

            var values = new string[] { "", "0", "1", "2", "3", "4", "5" };

            return values.Contains(tipoCarroceria);

        }

        public static bool ValidarPlaca(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) { return false; }

            if (placa.Length > 8) { return false; }

            placa = placa.Replace("-", "").Trim();

            /*
             *  Verifica se o caractere da posição 4 é uma letra, se sim, aplica a validação para o formato de placa do Mercosul,
             *  senão, aplica a validação do formato de placa padrão.
             */
            if (char.IsLetter(placa, 4))
            {
                /*
                 *  Verifica se a placa está no formato: três letras, um número, uma letra e dois números.
                 */
                var padraoMercosul = new Regex("[a-zA-Z]{3}[0-9]{1}[a-zA-Z]{1}[0-9]{2}");
                return padraoMercosul.IsMatch(placa);
            }
            else
            {
                // Verifica se os 3 primeiros caracteres são letras e se os 4 últimos são números.
                var padraoNormal = new Regex("[a-zA-Z]{3}[0-9]{4}");
                return padraoNormal.IsMatch(placa);
            }
        }

    }
}
