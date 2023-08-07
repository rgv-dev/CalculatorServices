using CalculatorService.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CalculatorService.Server.Models
{
    public class Multiply : IMainOperations
    {
        [JsonRequired]
        [JsonPropertyName("Factors")]
        public List<double> MainOperations { get; set; }
        [JsonIgnore]
        public double Result { get; set; }
    }
}
