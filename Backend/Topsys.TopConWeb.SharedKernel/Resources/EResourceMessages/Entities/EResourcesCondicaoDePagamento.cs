using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos da Condição de Pagamento.
    /// </summary>
    public enum EResourcesCondicaoDePagamento
    {
        /// <summary>
        /// 7.1.1 Number of number_of_payments different from the days_to_?th_payment entered
        /// </summary>
        CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31
        
    }
        
    public static class EResourcesCondicaoDePagamentoExtenions
    {
        public static string GetResourceMessage(this EResourcesCondicaoDePagamento erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }
        
        public static string GetMessageCode(this EResourcesCondicaoDePagamento error)
        {
            switch (error)
            {
                case EResourcesCondicaoDePagamento.CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31:
                    return "TCON375f335f31";
                default:
                    return "";
            }
        }
    }
}