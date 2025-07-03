using System.Collections.Generic;

namespace Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Prediction>? Predictions { get; set; }
        public decimal Points { get; set; }
    }
} 