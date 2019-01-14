
namespace MediatonicApi.Models
{
    public class Animal
    {
        public uint Id { get; set; }
        public string TypeName { get; set; }
        public uint HungerPerSecond { get; set; }
        public uint SadnessPerSecond { get; set; }

        public uint MaxHunger { get; set; }
        public int MinHappiness { get; set; }
        public int MaxHappiness { get; set; }
    }
}
