export class Constants {
   
   public static readonly VERSION = '2017.08.29/001';
   
   public static readonly API_VERSION = '2017.08.18/001';
   
   public static readonly HTTP_STATUS_CODE = {
    Success: {
      OK: 200,
      Created: 201,
      Accepted: 202,
      PartialInformation: 203,
      NoResponse: 204
    },
    ClientError: {
      BadRequest: 400,
      Unauthorized: 401,
      PaymentRequired: 402,
      Forbidden: 403,
      NotFound: 404
    },
    ServerError: {
      InternalError: 500,
      NotImplemented: 501,
      ServiceTemporarilyOverloaded: 502,
      GatewayTimeout: 503
    },
    Redirection: {
      Moved: 301,
      Found: 302,
      Method: 303,
      NotModified: 304
    }
  }
  
}