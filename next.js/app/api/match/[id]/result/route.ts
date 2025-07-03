import { NextRequest, NextResponse } from 'next/server';
import dbConnect from '@/utils/db';
import Match from '@/models/Match';
import Prediction from '@/models/Prediction';
import Person from '@/models/Person';

function calculatePoints(predictedA: number, predictedB: number, actualA: number, actualB: number): number {
  if (isCorrectWinner(predictedA, predictedB, actualA, actualB)) {
    if (predictedA === actualA && predictedB === actualB) return 5;
    if ((predictedA - predictedB) === (actualA - actualB)) return 2;
    if (predictedA === actualA || predictedB === actualB) return 1;
  }
  return 0;
}

function calculateWinnerPoints(totalPredictors: number, correctWinnerCount: number): number {
  if (totalPredictors === 0 || correctWinnerCount === 0) return 0;
  const points = totalPredictors / correctWinnerCount;
  return Math.round(points * 4) / 4;
}

function isCorrectWinner(predictedA: number, predictedB: number, actualA: number, actualB: number): boolean {
  return (
    (predictedA > predictedB && actualA > actualB) ||
    (predictedA < predictedB && actualA < actualB) ||
    (predictedA === actualA && predictedB === actualB)
  );
}

export async function POST(req: NextRequest, { params }: { params: { id: string } }) {
  await dbConnect();
  const matchId = params.id;
  const { ScoreA, ScoreB } = await req.json();
  // Update match result
  const match = await Match.findByIdAndUpdate(matchId, { resultHomeScore: ScoreA, resultAwayScore: ScoreB }, { new: true });
  if (!match) return NextResponse.json({ error: 'Match not found' }, { status: 404 });
  // Calculate points for all predictions for this match
  const predictions = await Prediction.find({ match: matchId });
  const totalPredictors = predictions.length;
  const correctWinnerCount = predictions.filter(p => isCorrectWinner(p.homeScore, p.awayScore, ScoreA, ScoreB)).length;
  for (const prediction of predictions) {
    let points = calculatePoints(prediction.homeScore, prediction.awayScore, ScoreA, ScoreB);
    if (isCorrectWinner(prediction.homeScore, prediction.awayScore, ScoreA, ScoreB)) {
      points += calculateWinnerPoints(totalPredictors, correctWinnerCount);
    }
    prediction.predictionPoints = points;
    await prediction.save();
    // Update person points
    const person = await Person.findById(prediction.person);
    if (person) {
      person.points = (person.points || 0) + points;
      await person.save();
    }
  }
  return NextResponse.json({ success: true });
} 