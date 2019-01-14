using System.Collections.Generic;

namespace MediatonicApi.Models
{
    public class User
    {
        public uint Id { get; set; }
        public string DisplayName { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
