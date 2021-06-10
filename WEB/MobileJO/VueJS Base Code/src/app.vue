<template>
  <v-app id="app">
    <transition name="fade" mode="out-in">
      <router-view/>
    </transition>
  </v-app>
</template>

<script>
export default {
  name: 'app',
};
</script>

<style>
</style>
