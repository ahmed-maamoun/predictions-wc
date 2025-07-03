<template>
  <div>
    <h2>Matches</h2>
    <ul>
      <li v-for="match in orderedMatches" :key="match._id"
          :class="{ 'highlight-missing-result': shouldHighlight(match) }">
        {{ match.homeTeam }} ({{ match.resultHomeScore ?? '-' }}) vs ({{ match.resultAwayScore ?? '-' }}) {{ match.awayTeam }} - {{ formatDate(match.startTime) }}
        <div>
          <button v-if="!hasStarted(match) && (match.resultHomeScore == null && match.resultAwayScore == null)" @click="$emit('open-prediction', match)">Add Prediction</button>
          <button v-if="match.resultHomeScore == null" @click="$emit('open-result', match)">Enter Result</button>
          <button @click="$emit('show-predictions', match)">Show Predictions</button>
        </div>
      </li>
    </ul>
    <h3>Add Match</h3>
    <form @submit.prevent="addMatch">
      <input v-model="teamA" placeholder="Team A" required />
      <input v-model="teamB" placeholder="Team B" required />
      <input v-model="matchDate" type="datetime-local" required />
      <button type="submit">Add</button>
    </form>
  </div>
</template>

<script>
export default {
  props: ['matches'],
  data() {
    return {
      teamA: '',
      teamB: '',
      matchDate: ''
    };
  },
  computed: {
    orderedMatches() {
      const now = new Date();
      const notStarted = this.matches.filter(m => new Date(m.startTime) > now)
        .sort((a, b) => new Date(a.startTime) - new Date(b.startTime));
      const started = this.matches.filter(m => new Date(m.startTime) <= now)
        .sort((a, b) => new Date(b.startTime) - new Date(a.startTime));
      return [...notStarted, ...started];
    }
  },
  methods: {
    addMatch() {
      // Convert local datetime to UTC ISO string (no manual offset)
      const utcDate = new Date(this.matchDate).toISOString();
      this.$emit('add', {
        teamA: this.teamA,
        teamB: this.teamB,
        matchDate: utcDate
      });
      this.teamA = '';
      this.teamB = '';
      this.matchDate = '';
    },
    formatDate(dateStr) {
      // Convert UTC date string to local time for display, with AM/PM
      const date = new Date(dateStr);
      return date.toLocaleString('en-GB', {
        year: 'numeric',
        month: 'short',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true
      });
    },
    hasStarted(match) {
      const now = new Date();
      const matchTime = new Date(match.startTime);
      return now >= matchTime;
    },
    shouldHighlight(match) {
      const now = new Date();
      const matchTime = new Date(match.startTime);
      // Highlight if more than 3 hours have passed since match start and no result
      return now - matchTime > 3 * 3600000 && (match.resultHomeScore == null || match.resultAwayScore == null);
    }
  }
};
</script>

<style scoped>
.highlight-missing-result {
  border: 2px solid #d9534f;
  background: #fff0f0;
}
</style> 