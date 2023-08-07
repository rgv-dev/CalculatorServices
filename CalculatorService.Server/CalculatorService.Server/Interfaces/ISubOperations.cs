namespace CalculatorService.Server.Interfaces
{
    public interface ISubOperations : IMainOperations
    {
        public List<double> SubOperations { get; set; }
    }
}
