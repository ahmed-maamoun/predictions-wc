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
  const predictiions = await Prediction.find()
          .populate('match')
          .exec();
   const matchesJson = await Promise.all(matches.map(async m => {
    const matchObj = m.toObject();
    if (matchObj.predictions) {
      matchObj.predictions = await Prediction.find({ match: matchObj._id });
    }
    return matchObj;
  }));
  return NextResponse.json(matchesJson);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  const match = new Match(data);
  await match.save();
  return NextResponse.json(match, { status: 201 });
}

// Add PUT, DELETE as needed for full CRUD 