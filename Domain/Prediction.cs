namespace Domain
{
    public class Prediction
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int MatchId { get; set; }
        public Match? Match { get; set; }
        public int PredictedScoreA { get; set; }
        public int PredictedScoreB { get; set; }
        public decimal? PredictionPoints {  get; set; }
    }
} 