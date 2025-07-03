<template>
  <div>
    <h2>Standings</h2>
    <table>
      <thead>
        <tr>
          <th>Rank</th>
          <th>Name</th>
          <th>Points</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(person, idx) in sortedPersons" :key="person.id">
          <td>{{ idx + 1 }}</td>
          <td>{{ person.name }}</td>
          <td>{{ person.points }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: 'StandingsTable',
  props: ['persons'],
  computed: {
    sortedPersons() {
      return [...this.persons].sort((a, b) => b.points - a.points);
    }
  }
};
</script> 