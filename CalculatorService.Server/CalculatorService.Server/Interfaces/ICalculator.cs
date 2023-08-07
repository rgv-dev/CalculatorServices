using CalculatorService.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static CalculatorService.Server.Services.CaculatorServices;

namespace CalculatorService.Server.Interfaces
{
    public interface ICalculator
    {
        public Task<double> GetAddResult(Add add);
        public Task<double> GetSubstractResult(Substract sub);
        public Task<double> GetMultiplyResult(Multiply mult);
        public Task<Division> GetDivideResult(Division div);
        public Task<double> GetSquareResult(SquareRoot sqrt);
        public Task SaveTrackingId(string trackingId, IMainOperations mainOperations,ISubOperations subOperations);
        public Task<IReadOnlyList<TrackingHistory>> GetAllTrackingsId(string trackingId);
    }
}
