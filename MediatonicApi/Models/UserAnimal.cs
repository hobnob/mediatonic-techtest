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
                if (Animal == null) {
                    throw new InvalidOperationException("Animal must be loaded before calculating Happiness");
                }

                return Math.Round(
                        Math.Max(
                            MIN_HAPPINESS,
                            happinessAtUpdate - (Animal.SadnessPerSecond * (int) Math.Floor((DateTime.UtcNow - lastHappinessUpdate).TotalSeconds))
                        ), 2
                );
            }
        }

        public decimal Hunger {
            get
            {
                if (Animal == null) {
                    throw new InvalidOperationException("Animal must be loaded before calculating Hunger");
                }

                return Math.Round(
                    Math.Min(
                        MAX_HUNGER,
                        hungerAtUpdate + (Animal.HungerPerSecond * (int) Math.Floor((DateTime.UtcNow - lastHungerUpdate).TotalSeconds))
                    ), 2
                );
            }
        }

        private DateTime lastHungerUpdate = DateTime.UtcNow;
        private DateTime lastHappinessUpdate = DateTime.UtcNow;
        private decimal hungerAtUpdate;
        private decimal happinessAtUpdate;

        private const uint MAX_HUNGER = 1;
        private const int MIN_HAPPINESS = -1;
        private const uint MAX_HAPPINESS = 1;

        public void Feed(decimal foodAmount)
        {
            if (foodAmount <= 0) {
                throw new ArgumentException("Food amount must be higher than zero");
            }

            hungerAtUpdate = Math.Max(Hunger - foodAmount, 0);
            lastHungerUpdate = DateTime.UtcNow;
        }

        public void Stroke(decimal happinessAmount)
        {
            if (happinessAmount <= 0) {
                throw new ArgumentException("Happiness amount must be higher than zero");
            }

            happinessAtUpdate = Math.Min(Happiness + happinessAmount, MAX_HAPPINESS);
            lastHappinessUpdate = DateTime.UtcNow;
        }
    }
}
