using CalculatorService.Server.Models;

namespace CalculatorService.Server.Interfaces
{
    public interface IErrors
    {
        public Error GetErrorMessage(int codeHttp);
    }
}
