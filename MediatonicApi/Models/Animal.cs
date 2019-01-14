
namespace MediatonicApi.Models
{
    public class Animal
    {
        public uint Id { get; set; }
        public string TypeName { get; set; }
        public double HungerPerSecond { get; set; }
        public double SadnessPerSecond { get; set; }

        public uint MaxHunger { get; set; }
        public int MinHappiness { get; set; }
        public int MaxHappiness { get; set; }
    }
}
