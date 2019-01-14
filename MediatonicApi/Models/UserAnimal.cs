using System;

namespace MediatonicApi.Models
{
    public class UserAnimal
    {
        public User User { get; set; }
        public Animal Animal { get; set; }
        public string AnimalName { get; set; }

        public decimal Happiness {
            get
            {
                return (decimal) Math.Round(
                        Math.Max(
                            Animal.MinHappiness,
                            happinessAtUpdate - (Animal.SadnessPerSecond * (int) (DateTime.UtcNow - lastHappinessUpdate).TotalSeconds)
                        ), 2
                );
            }
        }

        public decimal Hunger {
            get
            {
                return (decimal) Math.Round(
                    Math.Min(
                        Animal.MaxHunger,
                        hungerAtUpdate + (Animal.HungerPerSecond * (int) (DateTime.UtcNow - lastHungerUpdate).TotalSeconds)
                    ), 2
                );
            }
        }

        private DateTime lastHungerUpdate = DateTime.UtcNow;
        private DateTime lastHappinessUpdate = DateTime.UtcNow;
        private uint hungerAtUpdate;
        private int happinessAtUpdate;

        public void Feed(uint foodAmount)
        {
            hungerAtUpdate = (uint) Math.Max((int) Hunger - (int) foodAmount, 0);
            lastHungerUpdate = DateTime.UtcNow;
        }

        public void Stroke(uint happinessAmount)
        {
            happinessAtUpdate = Math.Min(happinessAtUpdate + (int)happinessAmount, Animal.MaxHappiness);
            lastHappinessUpdate = DateTime.UtcNow;
        }
    }
}
