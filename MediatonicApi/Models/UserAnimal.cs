using System;

namespace MediatonicApi.Models
{
    public class UserAnimal
    {
        public uint UserId { get; set; }
        public User User { get; set; }

        public uint AnimalId { get; set; }
        public Animal Animal { get; set; }

        public decimal Happiness {
            get
            {
                return Math.Round(
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
                return Math.Round(
                    Math.Min(
                        Animal.MaxHunger,
                        hungerAtUpdate + (Animal.HungerPerSecond * (int) (DateTime.UtcNow - lastHungerUpdate).TotalSeconds)
                    ), 2
                );
            }
        }

        private DateTime lastHungerUpdate = DateTime.UtcNow;
        private DateTime lastHappinessUpdate = DateTime.UtcNow;
        private decimal hungerAtUpdate;
        private decimal happinessAtUpdate;

        public void Feed(uint foodAmount)
        {
            hungerAtUpdate = Math.Max(Hunger - foodAmount, 0);
            lastHungerUpdate = DateTime.UtcNow;
        }

        public void Stroke(uint happinessAmount)
        {
            happinessAtUpdate = Math.Min(Happiness + happinessAmount, Animal.MaxHappiness);
            lastHappinessUpdate = DateTime.UtcNow;
        }
    }
}
