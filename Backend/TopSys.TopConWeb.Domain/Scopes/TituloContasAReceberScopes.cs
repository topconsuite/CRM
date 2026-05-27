using DocsBr.Validation.IE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class TituloContasAReceberScopes
    {
        public static void AccountsReceivableValidate(this AccountsReceivable tituloContasAReceber)
        {
            bool hasError = false;
            string fieldWithError = "";
           
            if (tituloContasAReceber.Splitting > 999 || tituloContasAReceber.Splitting < 0)
            {
                hasError = true;
                fieldWithError = nameof(tituloContasAReceber.Splitting);
            }

            if (tituloContasAReceber.Sequence > 999 || tituloContasAReceber.Sequence < 1)
            {
                hasError = true;
                fieldWithError = nameof(tituloContasAReceber.Sequence);
            }

            if (tituloContasAReceber.DocumentNumber > 999999999 || tituloContasAReceber.DocumentNumber < 1)
            {
                hasError = true;
                fieldWithError = nameof(tituloContasAReceber.DocumentNumber);
            }

            if (hasError) {
                throw new IntegrationValidatorError(
                               GenericResponsesIntegration.TheTypeOfTDoesNotFollowTheCorrectPatternCode,
                               GenericResponsesIntegration.Message[GenericResponsesIntegration.TheTypeOfTDoesNotFollowTheCorrectPatternCode]
                                   .Replace("<T>", $"{nameof(AccountsReceivable)}({fieldWithError})"
                                   .ToString()
                                   .CamelToSnake())
                           );
            }
            
        }
    }
}
