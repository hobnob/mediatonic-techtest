using System;

namespace MediatonicApi.Models
{
    public class UserAnimal
    {
        public uint UserId { get; set; }
        public uint AnimalId { get; set; }
        public string AnimalName { get; set; }
        public DateTime LastHungerUpdate { get; set; }
        public DateTime LastHappinessUpdate { get; set; }
        public uint HungerAtUpdate { get; set; }
        public int HappinessAtUpdate { get; set; }

        public int Happiness { get; }
        public uint Hunger { get; }
    }
}
