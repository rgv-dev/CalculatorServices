using CalculatorService.Server.Interfaces;
using CalculatorService.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace CalculatorService.Server.Services
{
    public class CaculatorServices : ICalculator
    {
        private static readonly Dictionary<string, List<TrackingHistory>> _requestJournal = new Dictionary<string, List<TrackingHistory>>();

        public CaculatorServices() {}

        public async Task<double> GetAddResult(Add add)
        {
            double result = 0;

            await Task.Run(() => { add.MainOperations.ForEach(s => result += s); });

            return result;
        }
        public async Task<double> GetSubstractResult(Substract sub)
        {
            double minuend = 0;
            double subtrahend = 0;

            await Task.Run(() => {

                sub.MainOperations.ForEach(s => minuend += s);
                sub.SubOperations.ForEach(s =>  subtrahend += s);

            });
            return minuend - subtrahend;
           
        }
        public async Task<double> GetMultiplyResult(Multiply multiply)
        {
            double result = 1; 

            await Task.Run(() => { multiply.MainOperations.ForEach(s => result *= s); });

            return result;
        }

        public async Task<Division> GetDivideResult(Division div)
        {
            int quotient = 0;
            int remainder = 0;
            double dividend = 0;
            double divisor = 0;

            await Task.Run(() => {

                div.MainOperations.ForEach(s => dividend += s);
                div.SubOperations.ForEach(s => divisor += s);

            });
            div.Result = dividend / divisor;
            div.Quotient = Math.DivRem(((int)dividend), ((int)divisor), out remainder);
            div.Remainder = remainder;

            return div;
        }

        public async Task<double> GetSquareResult(SquareRoot sqrt)
        {
            double result = 0;
            sqrt.MainOperations.ForEach(s => result += s);
            await Task.Run(() => { result = Math.Sqrt(result); });

            return result;
        }

        public async Task<IReadOnlyList<TrackingHistory>> GetAllTrackingsId(string trackingId)
        {
            var result = new List<TrackingHistory>();

            await Task.Run(() => { result = _requestJournal[trackingId]; });

            return result;
        }

        public async Task SaveTrackingId(string trackingId, IMainOperations mainOperations,ISubOperations subOperations)
        {
            TrackingHistory trackingHistory;
            if (!string.IsNullOrEmpty(trackingId))
            {
                await Task.Run(() =>
                {
                    lock (_requestJournal)
                    {
                        if (!_requestJournal.ContainsKey(trackingId))
                        {
                            _requestJournal[trackingId] = new List<TrackingHistory>();
                        }

                        trackingHistory = new TrackingHistory();
                        trackingHistory.Date = DateTime.Now.ToString();

                        switch (mainOperations)
                        {
                            case Add:
                                trackingHistory.Operation = "Add";
                                trackingHistory.Calculation = $"Addition: {string.Join(" + ", mainOperations.MainOperations)} = {mainOperations.Result}";

                                _requestJournal[trackingId].Add(trackingHistory);
                                break;

                            case Multiply:
                                trackingHistory.Operation = "Multipliy";
                                trackingHistory.Calculation = $"Multipliy: {string.Join("*", mainOperations.MainOperations)} = {mainOperations.Result}";

                                _requestJournal[trackingId].Add(trackingHistory);
                                break;

                            case Substract:
                                trackingHistory.Operation = "Substract";
                                trackingHistory.Calculation = $"Substract: ({string.Join(" + ", mainOperations.MainOperations)}) - ({string.Join(" + ", subOperations.SubOperations)}) = {mainOperations.Result}";

                                _requestJournal[trackingId].Add(trackingHistory);
                                break;

                            case Division:
                                trackingHistory.Operation = "Division";
                                trackingHistory.Calculation = $"Division: ({string.Join(" + ", mainOperations.MainOperations)}) / ({string.Join(" + ", subOperations.SubOperations)}) = {mainOperations.Result}";

                                _requestJournal[trackingId].Add(trackingHistory);
                                break;
                            case SquareRoot:
                                trackingHistory.Operation = "SquareRoot";                                
                                trackingHistory.Calculation = $"Square Root: sqrt({string.Join(" + ", mainOperations.MainOperations)}) = {mainOperations.Result}";

                                _requestJournal[trackingId].Add(trackingHistory);
                                break;
                        }
                    }

                });
            }
        }
    }
}
