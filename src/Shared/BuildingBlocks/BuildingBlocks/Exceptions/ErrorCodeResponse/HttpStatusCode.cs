namespace BuildingBlocks.Exceptions.ErrorCodeResponse
{
    public class HttpStatusCode
    {
        // Success codes
        public const int ErrCodeSuccess = 20001;

        // Client errors (4xx)
        public const int ErrCodeParamInvalid = 20003;
        public const int ErrInvalidToken = 30001;
        public const int ErrNotFound = 30004;
        public const int ErrBadRequest = 40001;
        public const int ErrUnauthorized = 40002;
        public const int ErrForbidden = 40003;
        public const int ErrValidationError = 40004;
        public const int ErrTooManyRequests = 40005;

        // Server errors (5xx)
        public const int ErrInternalServerError = 50001;
        public const int ErrServiceUnavailable = 50002;
        public const int ErrGatewayTimeout = 50003;
        public const int ErrDatabaseError = 50004;
        
        // Define messages for each code
        public static readonly Dictionary<int, string> Messages = new Dictionary<int, string>
        {
            // Success messages
            { ErrCodeSuccess, "Success" },
            
            // Client error messages
            { ErrCodeParamInvalid, "Parameter invalid" },
            { ErrInvalidToken, "Invalid token" },
            { ErrNotFound, "Resource not found" },
            { ErrBadRequest, "Bad request" },
            { ErrUnauthorized, "Unauthorized access" },
            { ErrForbidden, "Forbidden access" },
            { ErrValidationError, "Validation failed" },
            { ErrTooManyRequests, "Too many requests, please try again later" },

            // Server error messages
            { ErrInternalServerError, "Internal server error" },
            { ErrServiceUnavailable, "Service is currently unavailable" },
            { ErrGatewayTimeout, "Gateway timeout" },
            { ErrDatabaseError, "Database error occurred" }
        };
    }
}