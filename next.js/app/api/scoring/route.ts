import { NextRequest, NextResponse } from 'next/server';

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

export async function GET(req: NextRequest) {
  const { searchParams } = new URL(req.url);
  const action = searchParams.get('action');
  if (action === 'calculate-points') {
    const predictedA = Number(searchParams.get('predictedA'));
    const predictedB = Number(searchParams.get('predictedB'));
    const actualA = Number(searchParams.get('actualA'));
    const actualB = Number(searchParams.get('actualB'));
    const points = calculatePoints(predictedA, predictedB, actualA, actualB);
    return NextResponse.json(points);
  }
  if (action === 'calculate-winner-points') {
    const totalPredictors = Number(searchParams.get('totalPredictors'));
    const correctWinnerCount = Number(searchParams.get('correctWinnerCount'));
    const points = calculateWinnerPoints(totalPredictors, correctWinnerCount);
    return NextResponse.json(points);
  }
  if (action === 'is-correct-winner') {
    const predictedA = Number(searchParams.get('predictedA'));
    const predictedB = Number(searchParams.get('predictedB'));
    const actualA = Number(searchParams.get('actualA'));
    const actualB = Number(searchParams.get('actualB'));
    const isWinner = isCorrectWinner(predictedA, predictedB, actualA, actualB);
    return NextResponse.json(isWinner);
  }
  return NextResponse.json({ error: 'Invalid action' }, { status: 400 });
} 