<template>
  <div>
    <loading v-if="fullscreenLoading"></loading>
    Oops, unauthorized! Please <router-link class="myLink" to="/login">login</router-link> again with correct credentials.
  </div>
</template>
<script>
import loading from "../../common/layout/progress";
export default {
  data() {
    return {
      fullscreenLoading: false,
    }
  },
  compoments: {
    loading,
  },
};
</script>
<style scoped>
.myLink {
  color: #0600ff;
}
</style>

