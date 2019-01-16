
namespace MediatonicApi.Models
{
    public class Animal
    {
        /// <summary>
        /// The Identifier for this animal
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// The type name for the animal (cat, dog, dragon etc.)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// How hungry the animal gets every second
        /// </summary>
        public decimal HungerPerSecond { get; set; }

        /// <summary>
        /// How sad the animal gets each second
        /// </summary>
        public decimal SadnessPerSecond { get; set; }
    }
}
