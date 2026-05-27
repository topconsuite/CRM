using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Constants
{
	public static class GenericResponsesIntegration
	{
		public const string ErrorStatus = "error";
		public const string SuccessStatus = "success";

        public const string TCreatedWithSuccessAction = "created";
        public const string TUpdatedWithSuccessAction = "update";
        public const string TReturnedWithSuccessAction = "returned";

        public const string TDoesNotExistInOurSystemCode = "80001";
		public const string TheTypeOfTDoesNotFollowTheCorrectPatternCode = "80002";
		public const string TAlreadyExistInOurSystemCode = "80003";
		public const string InternalEndpointErrorCode = "99999";

		public static IDictionary<string, string> Message = new Dictionary<string, string>()
		{
			{TDoesNotExistInOurSystemCode, "<T> does not exist in our system" },
			{TheTypeOfTDoesNotFollowTheCorrectPatternCode , "The type of <T> does not follow the correct pattern." },
			{TAlreadyExistInOurSystemCode , "<T> already exist in our system" },
			{TCreatedWithSuccessAction , "<T>(s) successfully created" },
			{TUpdatedWithSuccessAction , "<T> successfully updated" },
			{TReturnedWithSuccessAction , "<T> successfully returned" },
			{InternalEndpointErrorCode , "Internal endpoint error" }
		};
	}

}
