using CalculatorService.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CalculatorService.Server.Models
{
    public class Division : ISubOperations
    {

        [JsonPropertyName("Divisor")]
        public List<double> SubOperations { get; set; }     
        [JsonPropertyName("Dividend")]
        public List<double> MainOperations { get; set; }
        [JsonIgnore]
        public double Result { get; set; }
        [JsonIgnore]
        public int Quotient { get; set; }
        [JsonIgnore]
        public int Remainder { get; set; }
    }
}
