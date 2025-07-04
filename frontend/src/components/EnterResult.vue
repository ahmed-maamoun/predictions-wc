<template>
  <div>
    <h3>Enter Result for {{ match.homeTeam }} vs {{ match.awayTeam }}</h3>
    <form @submit.prevent="submitResult">
      <label :for="'scoreA-' + match._id">{{ match.homeTeam }}</label>
      <input :id="'scoreA-' + match._id" v-model.number="scoreA" type="number" :placeholder="match.homeTeam || 'Score A'" required />
      <label :for="'scoreB-' + match._id">{{ match.awayTeam }}</label>
      <input :id="'scoreB-' + match._id" v-model.number="scoreB" type="number" :placeholder="match.awayTeam || 'Score B'" required  />
      <button type="submit" >Submit Result</button>
    </form>
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
