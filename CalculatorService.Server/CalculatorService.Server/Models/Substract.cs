using CalculatorService.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CalculatorService.Server.Models
{
    public class Substract : ISubOperations
    {
        [JsonPropertyName("Minuend")]
        public List<double> MainOperations { get; set; }
        [JsonPropertyName("Subtrahend")]
        public List<double> SubOperations { get; set; }
        [JsonIgnore]
        public double Result { get; set; }
    }
}
