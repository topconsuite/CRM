namespace TopSys.TopConWeb.SharedKernel
{
    public static class GenericErrors
    {
        public static string InternalEndpointError = "500";
        public static string VersionNotSupported = "505";
        public static string IdNotCorrectFormat = "400";
        public static string DoesNotExistInOurSystem = "80001";
        public static string TypeDoesNotFollowCorrectPattern = "80002";
        public static string AlreadyExistInOurSystem = "80003";
        public static string CantBeDeletedSinceItsInUse = "80004";
        public static string RequiredToFollowTheRule = "80005";
        public static string CannotBeEmpty = "80006";
        public static string MoreCharactersThanAllowed = "80007";
        public static string LessThanZeroMoreDigitsThanAllowed = "80008";
        public static string MoreDigitsThanAllowed = "80009";
        public static string PreconditionFailed = "80010";
    }
}
