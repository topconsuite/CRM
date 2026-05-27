using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Constants.CondicaoPagamento
{
	public static class CondicaoPagamentoDiaUltimoVencimento
	{
		public static List<ECondicaoPagamentoDiaUltimoVencimento> Values = new List<ECondicaoPagamentoDiaUltimoVencimento>() 
		{
			ECondicaoPagamentoDiaUltimoVencimento.MaintainExpiration,
			ECondicaoPagamentoDiaUltimoVencimento.Anticipate,
			ECondicaoPagamentoDiaUltimoVencimento.Extend
		};
	}
}
