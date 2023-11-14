namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLMaterialRow
    {
        public string Name { get; private set; }
        public double Weight { get; private set; }

        public OBNLMaterialRow(string name, double weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}