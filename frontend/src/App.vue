<template>
  <div id="app">
    <h1>Prediction Competition</h1>
    <form @submit.prevent="addPerson" style="margin-bottom: 24px; display: flex; gap: 12px; align-items: center;">
      <input v-model="newPersonName" placeholder="New Person Name" required />
      <input v-model.number="newPersonPoints" type="number" min="0" step="0.01" placeholder="Initial Points" required />
      <button type="submit">Add Person</button>
      <span v-if="personMessage" :style="{ color: personMessageColor, marginLeft: '12px' }">{{ personMessage }}</span>
    </form>
    <MatchList :matches="matches" @add="addMatch" @open-prediction="openPredictionModal" @open-result="openResultModal" @show-predictions="openPredictionsModal" />
    <ModalDialog :show="showPredictionModal" @close="showPredictionModal = false">
      <h3>Add Prediction for {{ selectedMatch?.teamA }} vs {{ selectedMatch?.teamB }}</h3>
      <div v-if="matchHasStarted" style="color: #d9534f; margin-bottom: 8px;">Predictions are closed for this match.</div>
      <form v-else @submit.prevent="submitPrediction">
        <select v-model="predictionPersonId" required>
          <option value="" disabled>Select person</option>
          <option v-for="person in availablePredictionPersons" :key="person._id" :value="person._id">{{ person.name }}</option>
        </select>
        <input v-model.number="predictionScoreA" type="number" :placeholder="selectedMatch?.teamA || 'Score A'" required />
        <input v-model.number="predictionScoreB" type="number" :placeholder="selectedMatch?.teamB || 'Score B'" required />
        <button type="submit">Add Prediction</button>
      </form>
      <div v-if="predictionError" style="color: #d9534f; margin-top: 8px;">{{ predictionError }}</div>
    </ModalDialog>
    <ModalDialog :show="showResultModal" @close="showResultModal = false">
      <h3>Enter Result for {{ selectedMatch?.teamA }} vs {{ selectedMatch?.teamB }}</h3>
      <EnterResult :match="selectedMatch" @enter-result="(result) => submitResult(result.scoreA, result.scoreB)" />
    </ModalDialog>
    <ModalDialog :show="showPredictionsModal" @close="showPredictionsModal = false">
      <h3>Predictions for {{ selectedMatch?.teamA }} vs {{ selectedMatch?.teamB }}</h3>
      <table v-if="predictionsForMatch.length">
        <thead>
          <tr>
            <th>Person</th>
            <th>{{ selectedMatch?.homeTeam || 'Team A' }}</th>
            <th>{{ selectedMatch?.awayTeam || 'Team B' }}</th>
            <th>Points</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in predictionsForMatch" :key="p._id">
            <td>{{ getPersonName(p.person) }}</td>
            <td>{{ p.homeScore }}</td>
            <td>{{ p.awayScore }}</td>
            <td>{{ p.predictionPoints }}</td>
          </tr>
        </tbody>
      </table>
      <div v-else>No predictions for this match yet.</div>
    </ModalDialog>
    <div v-if="predictionError" style="color: #d9534f; margin: 12px 0; text-align: center;">{{ predictionError }}</div>
    <StandingsTable :persons="persons" />
  </div>
</template>

<script>
import MatchList from './components/MatchList.vue';
// import PredictionForm from './components/PredictionForm.vue';
import EnterResult from './components/EnterResult.vue';
import StandingsTable from './components/Standings.vue';
import ModalDialog from './components/ModalDialog.vue';

export default {
  components: { MatchList, EnterResult, StandingsTable, ModalDialog },
  data() {
    return {
      matches: [],
      persons: [],
      selectedMatch: null,
      predictionError: '',
      newPersonName: '',
      newPersonPoints: '',
      personMessage: '',
      personMessageColor: 'green',
      showPredictionModal: false,
      showResultModal: false,
      showPredictionsModal: false,
      predictionsForMatch: [],
      predictionPersonId: '',
      predictionScoreA: '',
      predictionScoreB: ''
    };
  },
  methods: {
    async fetchMatches() {
      const rawMatches = await fetch('https://predictions-wc.vercel.app/api/match').then(r => r.json());
      // Normalize fields for frontend
      this.matches = rawMatches.map(m => ({
        ...m,
        teamA: m.homeTeam || m.teamA,
        teamB: m.awayTeam || m.teamB,
        predictions: Array.isArray(m.predictions) ? m.predictions : [],
      }));
    },
    async fetchPersons() {
      this.persons = await fetch('https://predictions-wc.vercel.app/api/person').then(r => r.json());
    },
    async addMatch(match) {
      await fetch('https://predictions-wc.vercel.app/api/match', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({
        homeTeam: match.teamA,
        awayTeam: match.teamB,
        startTime: match.matchDate
      }) });
      this.fetchMatches();
    },
    openPredictionModal(match) {
      // Always use the match from the main matches array to ensure predictions are populated
      const latestMatch = this.matches.find(m => m._id === match._id);
      this.selectedMatch = latestMatch || match;
      this.showPredictionModal = true;
      this.predictionError = '';
      this.predictionPersonId = '';
      this.predictionScoreA = '';
      this.predictionScoreB = '';
    },
    openResultModal(match) {
      this.selectedMatch = match;
      this.showResultModal = true;
    },
    async submitPrediction() {
      this.predictionError = '';
      if (!this.predictionPersonId) {
        this.predictionError = 'Please select a person.';
        return;
      }
      // Check if already predicted (shouldn't happen, but double check)
      const predictions = this.selectedMatch.predictions || [];
      if (predictions.some(p => String(p.person) === String(this.predictionPersonId))) {
        this.predictionError = 'This person already predicted for this match.';
        return;
      }
      const response = await fetch('https://predictions-wc.vercel.app/api/prediction', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          person: this.predictionPersonId,
          match: this.selectedMatch._id,
          homeScore: this.predictionScoreA,
          awayScore: this.predictionScoreB
        })
      });
      if (!response.ok) {
        const msg = await response.text();
        this.predictionError = msg || 'Failed to add prediction.';
        return;
      }
      this.showPredictionModal = false;
      this.predictionPersonId = '';
      this.predictionScoreA = '';
      this.predictionScoreB = '';
      alert('Prediction added!');
      await this.fetchMatches();
      await this.fetchPersons();
    },
    async submitResult(scoreA, scoreB) {
      await fetch(`https://predictions-wc.vercel.app/api/match/result?id=${this.selectedMatch._id}`,
        { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ ScoreA: scoreA, ScoreB: scoreB }) });
      this.showResultModal = false;
      await this.fetchPersons();
      await this.fetchMatches();
      alert('Result entered and points calculated!');
    },
    async addPerson() {
      this.personMessage = '';
      try {
        const response = await fetch('https://predictions-wc.vercel.app/api/person', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ name: this.newPersonName, points: this.newPersonPoints || 0 })
        });
        if (!response.ok) {
          const msg = await response.text();
          this.personMessage = msg || 'Failed to add person.';
          this.personMessageColor = 'red';
          return;
        }
        this.personMessage = 'Person added!';
        this.personMessageColor = 'green';
        this.newPersonName = '';
        this.newPersonPoints = '';
        await this.fetchPersons();
      } catch (e) {
        this.personMessage = 'Error adding person.';
        this.personMessageColor = 'red';
      }
    },
    async openPredictionsModal(match) {
      this.selectedMatch = match;
      this.showPredictionsModal = true;
      // Fetch all predictions and filter by matchId
      const allPredictions = await fetch('https://predictions-wc.vercel.app/api/prediction').then(r => r.json());
      this.predictionsForMatch = allPredictions.filter(p => p.match._id === match._id);
    },
    getPersonName(person) {
      if (!person) return 'Unknown';
      // If person is populated object
      if (typeof person === 'object' && person.name) return person.name;
      // If person is id, find in persons
      const found = this.persons.find(p => p._id === person || p.id === person);
      return found ? found.name : 'Unknown';
    }
  },
  computed: {
    availablePredictionPersons() {
    console.log(this.selectedMatch)
          debugger; // eslint-disable-line no-debugger
          if (!this.selectedMatch) return [];

          const predictedIds = (this.selectedMatch.predictions || []).map(p => {

        if (typeof p.person === 'object' && p.person._id) return String(p.person._id);
        return String(p.person);
      });
    console.log("hi",predictedIds)

      return this.persons.filter(p => !predictedIds.includes(String(p._id)));
    },
    matchHasStarted() {
      if (!this.selectedMatch) return false;
      const now = new Date(Date.now() + 3 * 3600000);
      const matchTime = new Date(this.selectedMatch.startTime);
      return now >= matchTime;
    }
  },
  mounted() {
    this.fetchMatches();
    this.fetchPersons();
  }
};
</script>

<style>
body {
  font-family: 'Segoe UI', Arial, sans-serif;
  background: #f7f7f7;
  margin: 0;
  padding: 0;
}
#app {
  max-width: 900px;
  margin: 40px auto;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 16px rgba(0,0,0,0.08);
  padding: 32px 40px 40px 40px;
}
h1 {
  text-align: center;
  color: #2c3e50;
  margin-bottom: 32px;
}
form {
  display: flex;
  gap: 12px;
  margin-bottom: 18px;
  flex-wrap: wrap;
}
input[type="text"], input[type="number"], input[type="date"] {
  padding: 8px 10px;
  border: 1px solid #d0d0d0;
  border-radius: 6px;
  font-size: 1rem;
  background: #fafafa;
  transition: border 0.2s;
}
input:focus {
  border: 1.5px solid #007bff;
  outline: none;
}
button {
  background: #007bff;
  color: #fff;
  border: none;
  border-radius: 6px;
  padding: 8px 18px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}
button:hover {
  background: #0056b3;
}
ul {
  list-style: none;
  padding: 0;
}
li {
  background: #f2f6fa;
  margin-bottom: 8px;
  padding: 10px 14px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 24px;
  background: #fafcff;
  border-radius: 8px;
  overflow: hidden;
}
th, td {
  padding: 10px 12px;
  text-align: left;
}
th {
  background: #e9f0fa;
  color: #2c3e50;
  font-weight: 600;
}
tbody tr:nth-child(even) {
  background: #f4f8fb;
}
@media (max-width: 600px) {
  #app {
    padding: 10px;
  }
  form {
    flex-direction: column;
    gap: 8px;
  }
  th, td {
    padding: 7px 6px;
  }
}
</style>
