<template>
  <div id="main-layout">
      <div>
        <MainHeader></MainHeader>
      </div>
  </div>
</template>

<script>
import MainHeader from '../../common/layout/header';
import MainMenu from '../../common/layout/menu';
import login from '../../pages/login/login';
import { mapGetters } from 'vuex';

export default {
  name: 'app-main-layout',
  components: {
    MainHeader,
    MainMenu,
    login,
  },
  created() {
    this.redirect();
  },
  methods: {
    redirect() {
      if(!this.isLoggedIn) {        
        this.$router.push('/login');       
      }
    }
  },
  computed: {
    ...mapGetters({
      isLoggedIn: 'login/isLoggedIn',
    }),
  },
};
</script>

<style>

</style>
