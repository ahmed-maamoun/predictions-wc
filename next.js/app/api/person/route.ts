import { NextRequest, NextResponse } from 'next/server';
import dbConnect from '@/utils/db';
import Person from '@/models/Person';

export async function GET() {
  await dbConnect();
  const persons = await Person.find().exec();
  const personsJson = persons.map(p => ({
    ...p.toObject(),
    points: p.points ? parseFloat(p.points.toString()) : 0
  }));
  return NextResponse.json(personsJson);
}

export async function POST(req: NextRequest) {
  await dbConnect();
  const data = await req.json();
  const person = new Person(data);
  await person.save();
  const personJson = {
    ...person.toObject(),
    points: person.points ? parseFloat(person.points.toString()) : 0
  };
  return NextResponse.json(personJson, { status: 201 });
}

// Add PUT, DELETE as needed for full CRUD 