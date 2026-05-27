using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Constants.CondicaoPagamento
{
    public static class CondicaoPagamentoCondicaoDaCobrancaCod
    {
        private const char Emission = 'E';
		private const char OutOfDate = 'D';
		private const char OutsideMonth = 'M';
		private const char FortnightOut = 'Q';
		private const char FromInvoice = 'F';
		private const char OffWeek = 'S';

        public static List<char> Values = new List<char>() { 
			Emission,
			OutOfDate, 
			OutsideMonth, 
			FortnightOut, 
			FromInvoice, 
			OffWeek 
		};
    }
}
