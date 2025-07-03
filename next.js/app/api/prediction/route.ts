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
  return NextResponse.json(predictions);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  const prediction = new Prediction(data);
  await prediction.save();
  return NextResponse.json(prediction, { status: 201 });
}

// Add PUT, DELETE as needed for full CRUD 