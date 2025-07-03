import { NextRequest, NextResponse } from 'next/server';
import dbConnect from '@/utils/db';
import Person from '@/models/Person';

export async function GET() {
  await dbConnect();
  const persons = await Person.find().exec();
  return NextResponse.json(persons);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  const person = new Person(data);
  await person.save();
  return NextResponse.json(person, { status: 201 });
}

// Add PUT, DELETE as needed for full CRUD 