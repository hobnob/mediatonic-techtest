using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediatonicApi.Models
{
    public class User
    {
        public uint Id { get; set; }
        public string DisplayName { get; set; }

        [JsonIgnore]
        public ICollection<UserAnimal> Animals { get; set; }
    }
}
