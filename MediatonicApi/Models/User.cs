using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediatonicApi.Models
{
    public class User
    {
        /// <summary>
        /// The identifier of the user
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// The display name for the user
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A collection of animals that the user owns
        /// </summary>
        [JsonIgnore]
        public ICollection<UserAnimal> Animals { get; set; }
    }
}
