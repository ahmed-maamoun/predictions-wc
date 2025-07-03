<template>
  <div>
    <h3>Add Prediction for {{ match.teamA }} vs {{ match.teamB }}</h3>
    <form @submit.prevent="submitPrediction">
      <input v-model="personName" placeholder="Your Name" required />
      <input v-model.number="scoreA" type="number" placeholder="Score A" required />
      <input v-model.number="scoreB" type="number" placeholder="Score B" required />
      <button type="submit">Submit Prediction</button>
    </form>
  </div>
</template>

<script>
export default {
  props: ['match'],
  data() {
    return {
      personName: '',
      scoreA: '',
      scoreB: ''
    };
  },
  methods: {
    submitPrediction() {
      this.$emit('add-prediction', {
        personName: this.personName,
        matchId: this.match.id,
        predictedScoreA: this.scoreA,
        predictedScoreB: this.scoreB
      });
      this.personName = '';
      this.scoreA = '';
      this.scoreB = '';
    }
  }
};
</script> 