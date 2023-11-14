namespace EcoCentre.Models.Domain.Dashboard
{
    public class EcoCenterEntry
    {
        public int Clients { get; set; }
        public int Visits { get; set; }
        public int OBNLVisits { get; set; }
        public double OBNLWeight { get; set; }
        public string Name { get; set; }
    }
}