import mongoose, { Schema, Document, Types } from 'mongoose';

export interface IPrediction extends Document {
  match: Types.ObjectId;
  person: Types.ObjectId;
  homeScore: number;
  awayScore: number;
  predictionPoints?: number;
}

const PredictionSchema: Schema = new Schema({
  match: { type: Schema.Types.ObjectId, ref: 'Match', required: true },
  person: { type: Schema.Types.ObjectId, ref: 'Person', required: true },
  homeScore: { type: Number, required: true },
  awayScore: { type: Number, required: true },
  predictionPoints: { type: Number },
});

export default mongoose.models.Prediction || mongoose.model<IPrediction>('Prediction', PredictionSchema); 