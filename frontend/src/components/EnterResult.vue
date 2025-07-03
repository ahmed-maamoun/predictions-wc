<template>
  <div>
    <h3>Enter Result for {{ match.homeTeam }} vs {{ match.awayTeam }}</h3>
    <form @submit.prevent="submitResult">
      <label :for="'scoreA-' + match._id">{{ match.homeTeam }}</label>
      <input :id="'scoreA-' + match._id" v-model.number="scoreA" type="number" :placeholder="match.homeTeam || 'Score A'" required :disabled="hasStarted" />
      <label :for="'scoreB-' + match._id">{{ match.awayTeam }}</label>
      <input :id="'scoreB-' + match._id" v-model.number="scoreB" type="number" :placeholder="match.awayTeam || 'Score B'" required :disabled="hasStarted" />
      <button type="submit" :disabled="hasStarted">Submit Result</button>
    </form>
    <div v-if="hasStarted" style="color: #d9534f; margin-top: 8px;">Result entry is disabled after match start.</div>
  </div>
</template>

<script>
export default {
  props: ['match'],
  data() {
    return {
      scoreA: '',
      scoreB: ''
    };
  },
  computed: {
    hasStarted() {
      if (!this.match || !this.match.startTime) return false;
      // Current time in GMT+3
      const now = new Date(Date.now() + 3 * 3600000);
      const matchTime = new Date(this.match.startTime);
      return now >= matchTime;
    }
  },
  methods: {
    submitResult() {
      this.$emit('enter-result', {
        matchId: this.match._id,
        scoreA: this.scoreA,
        scoreB: this.scoreB
      });
      this.scoreA = '';
      this.scoreB = '';
    }
  }
};
</script> 