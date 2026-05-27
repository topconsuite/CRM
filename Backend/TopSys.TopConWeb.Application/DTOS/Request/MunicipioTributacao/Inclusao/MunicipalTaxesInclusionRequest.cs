using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao.Inclusao
{
	public class MunicipalTaxesInclusionRequest
    {
		[Required]
		public IEnumerable<MunicipalTaxesInclusion> MunicipalTaxes { get; set; }
	}
}
