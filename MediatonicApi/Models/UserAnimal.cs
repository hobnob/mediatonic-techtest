﻿using Newtonsoft.Json;
using System;

namespace MediatonicApi.Models
{
    public class UserAnimal
    {
        /// <summary>
        /// The ID of the user that owns this animal
        /// </summary>
        public uint UserId { get; set; }

        /// <summary>
        /// The ID of the animal that the user owns
        /// </summary>
        public uint AnimalId { get; set; }

        /// <summary>
        /// The User object representing the user that owns thiis animal
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        /// <summary>
        /// The Animal object that the user owns
        /// </summary>
        [JsonIgnore]
        public Animal Animal { get; set; }

        /// <summary>
        /// The current happiness of this animal to 2dp
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the Animal property is null</exception>
        public decimal Happiness {
            get
            {
                if (Animal == null) {
                    throw new InvalidOperationException("Animal must be loaded before calculating Happiness");
                }

                // Calculates happiness for that specific time period to 2dp. This is calculated as the
                // happiness since the last update minus the amount of sadness since the happiness
                // was updated. Won't return less than the minimum happiness
                return Math.Round(
                        Math.Max(
                            MIN_HAPPINESS,
                            happinessAtUpdate - (Animal.SadnessPerSecond * (int) Math.Floor((DateTime.UtcNow - lastHappinessUpdate).TotalSeconds))
                        ), 2
                );
            }
        }

        /// <summary>
        /// The current hunger of this animal to 2dp
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the Animal property is null</exception>
        public decimal Hunger {
            get
            {
                if (Animal == null) {
                    throw new InvalidOperationException("Animal must be loaded before calculating Hunger");
                }

                // Calculates hunger for that specific time period to 2dp. This is calculated as the
                // hunger since the last update plus the amount of hunger since the hunger
                // was updated. Won't return more than the maximum hunger
                return Math.Round(
                    Math.Min(
                        MAX_HUNGER,
                        hungerAtUpdate + (Animal.HungerPerSecond * (int) Math.Floor((DateTime.UtcNow - lastHungerUpdate).TotalSeconds))
                    ), 2
                );
            }
        }

        /// <summary>
        /// The time that hunger was last updated
        /// </summary>
        private DateTime lastHungerUpdate = DateTime.UtcNow;

        /// <summary>
        /// The time that the happiness was last updated
        /// </summary>
        private DateTime lastHappinessUpdate = DateTime.UtcNow;

        /// <summary>
        /// The amount of hunger at the last update
        /// </summary>
        private decimal hungerAtUpdate;

        /// <summary>
        /// The amount of happiness at the last update
        /// </summary>
        private decimal happinessAtUpdate;

        /// <summary>
        /// The maximum level of hunger the animal can feel
        /// </summary>
        private const uint MAX_HUNGER = 1;

        /// <summary>
        /// The minimum level of hunger the animal can feel
        /// </summary>
        private const int MIN_HUNGER = -1;

        /// <summary>
        /// The minimum amount of happiness an animal can feel
        /// </summary>
        private const int MIN_HAPPINESS = -1;

        /// <summary>
        /// The maximum amount of happiness an animal can feel
        /// </summary>
        private const uint MAX_HAPPINESS = 1;

        /// <summary>
        /// Feeds an animal a food product with a certain amount of nutritional value (`foodAmount`)
        /// </summary>
        /// <param name="foodAmount">The nutritional value of that food</param>
        /// <exception cref="ArgumentException">Thrown if the food amount is less than zero</exception>
        public void Feed(decimal foodAmount)
        {
            if (foodAmount <= 0) {
                throw new ArgumentException("Food amount must be higher than zero");
            }

            hungerAtUpdate = Math.Max(Hunger - foodAmount, MIN_HUNGER);
            lastHungerUpdate = DateTime.UtcNow;
        }

        /// <summary>
        /// Strokes an animal for a set amount of happiness (`happinessAmount`)
        /// </summary>
        /// <param name="happinessAmount">The amount of happiness generated by stroking</param>
        /// <exception cref="ArgumentException">Thrown if the happiness amount is less than zero</exception>
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
