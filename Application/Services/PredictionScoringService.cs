using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PredictionScoringService
    {
        public int CalculatePoints(int predictedA, int predictedB, int actualA, int actualB)
        {
            if (IsCorrectWinner(predictedA, predictedB, actualA, actualB))
            {
                if (predictedA == actualA && predictedB == actualB)
                    return 5;
                if ((predictedA - predictedB) == (actualA - actualB))
                    return 2;
                if (predictedA == actualA || predictedB == actualB)
                    return 1;
            }
            return 0;
        }

        public decimal CalculateWinnerPoints(int totalPredictors, int correctWinnerCount)
        {
            if (totalPredictors == 0 || correctWinnerCount == 0) return 0;
           decimal points = (decimal)totalPredictors /  (decimal)correctWinnerCount ;
            // Round to nearest quarter (0.25)
            return Math.Round(points * (decimal)4) / (decimal)4;
        }

        public bool IsCorrectWinner(int predictedA, int predictedB, int actualA, int actualB)
        {
            return (predictedA > predictedB && actualA > actualB) ||
                   (predictedA < predictedB && actualA < actualB) ||
                   (predictedA == actualA && predictedB == actualB);
        }
    }
} 