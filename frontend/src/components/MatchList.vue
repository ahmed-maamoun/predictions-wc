<template>
  <div>
    <h2>Matches</h2>
    <ul>
      <li v-for="match in orderedMatches" :key="match._id"
          :class="{ 'highlight-missing-result': shouldHighlight(match) }">
        {{ match.homeTeam }} ({{ match.resultHomeScore ?? '-' }}) vs ({{ match.resultAwayScore ?? '-' }}) {{ match.awayTeam }} - {{ formatDate(match.startTime) }}
        <div class="menu-wrapper">
          <button class="menu-btn" @click="toggleMenu(match._id)">⋯</button>
          <div v-if="openMenuId === match._id" class="menu-dropdown">
            <button v-if="!hasStarted(match) && (match.resultHomeScore == null && match.resultAwayScore == null)" @click="$emit('open-prediction', match); closeMenu()">Add Prediction</button>
            <button v-if="canEnterResult(match)" @click="$emit('open-result', match); closeMenu()">Enter Result</button>
            <button v-if="(match.predictions && match.predictions.length > 0)" @click="$emit('show-predictions', match); closeMenu()">Show Predictions</button>
          </div>
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
      matchDate: '',
      openMenuId: null
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
    canEnterResult(match) {
      const now = new Date();
      const matchTime = new Date(match.startTime);
      // Show only if 2 hours have passed since match time and result is not entered
      return (now - matchTime > 2 * 3600000) && (match.resultHomeScore == null && match.resultAwayScore == null);
    },
    shouldHighlight(match) {
      const now = new Date();
      const matchTime = new Date(match.startTime);
      // Highlight if more than 3 hours have passed since match start and no result
      return now - matchTime > 3 * 3600000 && (match.resultHomeScore == null || match.resultAwayScore == null);
    },
    toggleMenu(id) {
      this.openMenuId = this.openMenuId === id ? null : id;
    },
    closeMenu() {
      this.openMenuId = null;
    }
  }
};
</script>

<style scoped>
.highlight-missing-result {
  border: 2px solid #d9534f;
  background: #fff0f0;
}
.menu-wrapper {
  position: relative;
  display: inline-block;
}
.menu-btn {
  background: none;
  border: none;
  font-size: 1.7rem;
  color: #1e7c1e;
  cursor: pointer;
  padding: 0 8px;
  border-radius: 50%;
  transition: background 0.2s;
}
.menu-btn:hover {
  background: #eafbe0;
}
.menu-dropdown {
  position: absolute;
  right: 0;
  top: 32px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 12px rgba(30,124,30,0.13);
  min-width: 170px;
  z-index: 10;
  display: flex;
  flex-direction: column;
  padding: 8px 0;
}
.menu-dropdown button {
  background: none;
  border: none;
  color: #1e7c1e;
  font-size: 1.08rem;
  text-align: left;
  padding: 10px 18px;
  cursor: pointer;
  border-radius: 0;
  transition: background 0.2s;
}
.menu-dropdown button:hover {
  background: #eafbe0;
}
</style> 