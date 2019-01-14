using System;

namespace MediatonicApi.Models
{
    public class UserAnimal
    {
        public User User { get; set; }
        public Animal Animal { get; set; }
        public string AnimalName { get; set; }
        public DateTime LastHungerUpdate { get; set; }
        public DateTime LastHappinessUpdate { get; set; }
        public uint HungerAtUpdate { get; set; }
        public int HappinessAtUpdate { get; set; }

        public double Happiness {
            get
            {
                return Math.Max(
                    Animal.MaxHappiness,
                    HappinessAtUpdate - (Animal.SadnessPerSecond * (DateTime.UtcNow - LastHappinessUpdate).TotalSeconds)
                );
            }
        }

        public double Hunger {
            get
            {
                return Math.Min(
                    Animal.MaxHunger,
                    HungerAtUpdate + (Animal.HungerPerSecond * (DateTime.UtcNow - LastHungerUpdate).TotalSeconds)
                );
            }
        }

        public void Feed(uint foodAmount)
        {
            HungerAtUpdate = (uint) Math.Max((int) Hunger - (int) foodAmount, 0);
            LastHungerUpdate = DateTime.UtcNow;
        }

        public void Stroke(uint happinessAmount)
        {
            HappinessAtUpdate = Math.Min(HappinessAtUpdate + (int)happinessAmount, Animal.MaxHappiness);
            LastHappinessUpdate = DateTime.UtcNow;
        }
    }
}
