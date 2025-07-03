import { NextRequest, NextResponse } from 'next/server';
import dbConnect from '@/utils/db';
import Match from '@/models/Match';
import Prediction from '@/models/Prediction';

export async function GET() {
  await dbConnect();
  const matches = await Match.find().populate({
    path: 'predictions',
    model: Prediction,
  }).exec();
  return NextResponse.json(matches);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  const match = new Match(data);
  await match.save();
  return NextResponse.json(match, { status: 201 });
}

// Add PUT, DELETE as needed for full CRUD 