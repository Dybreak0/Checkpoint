<template>
  <el-menu
    class="el-menu-side-nav"
    :router="true"
  >
    <el-menu-item index="/home">
      <router-link to="/home">{{ $t("people") }}</router-link>
    </el-menu-item>
  </el-menu>
</template>

<script>
export default {
  name: 'app-menu',

  methods: {
    classFor(path) {
      return { active: this.$route.path.startsWith(path) };
    },
  },
};
</script>

<style>

</style>
