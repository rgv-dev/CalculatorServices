using CalculatorService.Server.Interfaces;
using CalculatorService.Server.Models;

namespace CalculatorService.Server.Services
{
    public class ErrorServices : IErrors
    {
        public ErrorServices() 
        { }
        public Error GetErrorMessage(int codeHttp) 
        { 
            var error = new Error();

            switch (codeHttp)
            {
                case 400:

                    error.ErrorMessage = "BadRequest";
                    error.ErrorCode = codeHttp;
                    error.ErrorStatus = "Unable to procces request";

                    break;
                case 500:

                    error.ErrorMessage = "InternalError";
                    error.ErrorCode = codeHttp;
                    error.ErrorStatus = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again";

                    break;
                default:
                    break;
            }
            return error;
        }

    }
}
