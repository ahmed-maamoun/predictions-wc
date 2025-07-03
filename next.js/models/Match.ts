import mongoose, { Schema, Document, Types } from 'mongoose';

export interface IMatch extends Document {
  homeTeam: string;
  awayTeam: string;
  startTime: Date;
  resultHomeScore?: number;
  resultAwayScore?: number;
  predictions: Types.ObjectId[];
}

const MatchSchema: Schema = new Schema({
  homeTeam: { type: String, required: true },
  awayTeam: { type: String, required: true },
  startTime: { type: Date, required: true },
  resultHomeScore: { type: Number },
  resultAwayScore: { type: Number },
  predictions: [{ type: Schema.Types.ObjectId, ref: 'Prediction' }],
});

export default mongoose.models.Match || mongoose.model<IMatch>('Match', MatchSchema); 