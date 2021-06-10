<template>
   <div class="child-body">
       <v-btn @click="getPersonList()" type="primary">{{ $t("people") }}</v-btn>
           <v-btn @click="getDetailsList()" type="primary">{{ $t("details") }}</v-btn>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'app-person-list',

  computed: {
    ...mapGetters({
      loading: 'person/loading',
    }),
  },

  created() {
    this.$router.push('/home');
  },

  beforeRouteLeave(to, from, next) {
    this.$store.dispatch('person/clear');
    next();
  },

  methods: {
    getPersonList() {
      this.$router.push('/person/person');
    },
    getDetailsList() {
      this.$router.push('/person/details');
    },
  },
};
</script>

<style>

</style>
