using System;
using System.Collections.Generic;

namespace Domain
{
    public class Match
    {
        public int Id { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public DateTime MatchDate { get; set; }
        public int? ScoreA { get; set; }
        public int? ScoreB { get; set; }
        public ICollection<Prediction>? Predictions { get; set; }
    }
} 