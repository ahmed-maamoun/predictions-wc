import { NextRequest, NextResponse } from 'next/server';
import dbConnect from '@/utils/db';
import Prediction from '@/models/Prediction';
import Match from '@/models/Match';
import Person from '@/models/Person';

export async function GET() {
  await dbConnect();
  const predictions = await Prediction.find()
    .populate('match')
    .populate('person')
    .exec();
  const predictionsJson = predictions.map(p => ({
    ...p.toObject(),
    predictionPoints: p.predictionPoints ? parseFloat(p.predictionPoints.toString()) : 0
  }));
  return NextResponse.json(predictionsJson);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  // Prevent duplicate predictions for the same person and match
  const existing = await Prediction.findOne({ person: data.person, match: data.match });
  if (existing) {
    return NextResponse.json({ error: 'This person already predicted for this match.' }, { status: 400 });
  }
  const prediction = new Prediction(data);
  await prediction.save();
  // Add prediction to the match's predictions array
  await Match.findByIdAndUpdate(prediction.match, { $addToSet: { predictions: prediction._id } });
  const predictionJson = {
    ...prediction.toObject(),
    predictionPoints: prediction.predictionPoints ? parseFloat(prediction.predictionPoints.toString()) : 0
  };
  return NextResponse.json(predictionJson, { status: 201 });
}

export async function PUT(req: NextRequest) {
  await dbConnect();
  const { _id, homeScore, awayScore } = await req.json();
  if (!_id) return NextResponse.json({ error: 'Missing prediction id' }, { status: 400 });
  // Find the prediction and its match
  const prediction = await Prediction.findById(_id);
  if (!prediction) return NextResponse.json({ error: 'Prediction not found' }, { status: 404 });
  const match = await Match.findById(prediction.match);
  if (!match) return NextResponse.json({ error: 'Match not found' }, { status: 404 });
  // Only allow update if match has not started
  if (new Date() >= new Date(match.startTime)) {
    return NextResponse.json({ error: 'Cannot edit prediction after match has started' }, { status: 403 });
  }
  prediction.homeScore = homeScore;
  prediction.awayScore = awayScore;
  await prediction.save();
  return NextResponse.json({ success: true });
}

// Add DELETE as needed for full CRUD 