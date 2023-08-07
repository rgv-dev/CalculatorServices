namespace CalculatorService.Server.Interfaces
{
    public interface IMainOperations
    {
        public List<double> MainOperations { get; set; }
        public double Result { get; set; }
    }
}
