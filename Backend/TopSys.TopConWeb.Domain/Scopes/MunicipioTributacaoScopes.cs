using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
	public static class MunicipioTributacaoScopes
    {

		public static void MunicipalTaxesToCreateIsValid(this MunicipalTaxes municipalTaxes)
		{
			MunicipalTaxesIsValidValues(municipalTaxes);
            MunicipalTaxesIsValidBusinessRules(municipalTaxes);
        }

		public static void MunicipalTaxesToAlterIsValid(this MunicipalTaxes municipalTaxes)
		{
            MunicipalTaxesIsValidValues(municipalTaxes);
            MunicipalTaxesIsValidBusinessRules(municipalTaxes);
        }

		private static void MunicipalTaxesIsValidBusinessRules(this MunicipalTaxes municipalTaxes)
		{
		
        }

        private static void MunicipalTaxesIsValidValues(this MunicipalTaxes municipalTaxes)
		{
			
        }
	}
}
