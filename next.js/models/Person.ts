import mongoose, { Schema, Document } from 'mongoose';

export interface IPerson extends Document {
  name: string;
  points: number;
}

const PersonSchema: Schema = new Schema({
  name: { type: String, required: true },
  points: { type: Number, default: 0 },
});

export default mongoose.models.Person || mongoose.model<IPerson>('Person', PersonSchema); 