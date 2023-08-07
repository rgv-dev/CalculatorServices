using CalculatorService.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CalculatorService.Server.Models
{
    public class SquareRoot : IMainOperations
    {
        [JsonPropertyName("Numbers")]
        public List<double> MainOperations { get; set; }
        [JsonIgnore]
        public double Result { get; set; }
    }
}
