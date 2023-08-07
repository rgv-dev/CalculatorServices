
using CalculatorService.Server.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

namespace CalculatorService.Server.Models
{
    public class Add : IMainOperations
    {
        //public List<double> Addends { get; set; }
        [JsonRequired]
        [JsonPropertyName("Addends")]
        public List<double> MainOperations { get; set; }
        [JsonIgnore]
        public double Result { get; set; }
    }
}
